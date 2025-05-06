using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PathCreation;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static UnityEditor.Progress;
using UnityEditor;


public class BallChainController : MonoBehaviour
{
    public PathCreator pathCreator;
    public BallChainConfig _ballChainConfig;
    public GameObject ball;
    public List<Color> BallColors = new List<Color>();
    public bool _needGizmos = false;

    private Ball currentBall = null;
    private bool _isBoosting = true;
    private List<Color> _colorItems = new();
    private int _countItems = 0;
    private CancellationTokenSource _startBallSpawning;
    private BallProvider _ballProvider;
    private ChainTracker _chainTracker = new ChainTracker();
    private AttachingBallChainHandler _attachingBallChainHandler;

    public List<Ball> ActiveItems => _chainTracker.Balls.ToList();

    private void OnEnable()
    {
        _ballProvider = new BallProvider(ball , this, _ballChainConfig);
        _ballProvider.CreatePoolBall();
        StartBallSpawning(BallColors);
    }
    private void OnDisable()
    {
        _ballProvider.CleanupPool();
        StopBallSpawning();

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (!_needGizmos)
            return;
        float radius = _ballChainConfig.ZumaBallRadius;
        float ballSpace = _ballChainConfig.SpacingBalls;
        VertexPath path = pathCreator.path;
        float currentDistance = 0;
        while (currentDistance <= path.length - 0.1f)
        {
            Vector3 position = path.GetPointAtDistance(currentDistance);
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(position, radius);
            currentDistance += ballSpace;
        }
    }
#endif

    public void Update()
    {
        MoveBalls();
    }

    public void StartBallSpawning(List<Color> colorItems)
    {
        if (pathCreator == null)
            return;

        _startBallSpawning?.Cancel();
        _startBallSpawning = new CancellationTokenSource();

        _colorItems = colorItems;

        BoostSpeedAsync(_startBallSpawning.Token).Forget();
        SpawnInitialBallsAsync(_startBallSpawning.Token).Forget();
    }

    public void StopBallSpawning()
    {
        _startBallSpawning?.Cancel();
        pathCreator = null;

        _chainTracker.ClearBalls();
        _colorItems.Clear();

        _countItems = 0;
        _chainTracker.ResetDistanceTravelled();

        _isBoosting = true;
    }

    //public void TryAttachBall(Ball newBall)
    //{
    //    _attachingBallChainHandler.TryAttachBall(newBall);
    //}  

    private async UniTaskVoid SpawnInitialBallsAsync(CancellationToken token)
    {
        while (true) 
        {
            if (token.IsCancellationRequested)
                return;
            int i = _chainTracker.GetCount();
            var color = _colorItems.FirstOrDefault();

            float spawnDistance = i * _ballChainConfig.SpacingBalls;
            Ball newBall = _ballProvider.GetBall(pathCreator.path.GetPointAtDistance(spawnDistance),Quaternion.identity);
            if (currentBall != null)
                currentBall.backBall = newBall;
            newBall.frontBall = currentBall;
            newBall.SetColor(color);
            _colorItems.Remove(color);
            AddBall(newBall);
            newBall.SetIndex(i);
            currentBall = newBall;
            await UniTask.Delay((int)(_ballChainConfig.DurationSpawnBall * 1000), cancellationToken: token);
        }
    }

    private async UniTaskVoid BoostSpeedAsync(CancellationToken token)
    {
        float elapsedTime = 0f;
        float startSpeed = _ballChainConfig.MoveSpeedMultiplier;
        float endSpeed = _ballChainConfig.MoveSpeed;

        _ballChainConfig.MoveSpeed = startSpeed;

        while (elapsedTime < _ballChainConfig.BoostDuration)
        {
            elapsedTime += Time.deltaTime / 2;
            _ballChainConfig.MoveSpeed = Mathf.Lerp(startSpeed, endSpeed, elapsedTime);
            await UniTask.Yield(cancellationToken: token);
        }

        _isBoosting = false;
    }

    private void MoveBalls()
    {
        if (_chainTracker.Balls.Count == 0)
            return;

        var currentSpeed = GetCurrentSpeed();
        _chainTracker.AddDistanceTravelled(currentSpeed * Time.deltaTime);

        MoveFistBall();
        HandleRemoveBallNearEndOfPath(CurrentBalls[0]);

        for (int i = 1; i < CurrentBalls.Count; i++)
        {
            MoveBall(CurrentBalls[i], i);

            if (HandleRemoveBallNearEndOfPath(CurrentBalls[i]))
                i--;
        }
    }

    private void MoveBall(Ball ball, int index)
    {
        float targetDistance = Mathf.Max(_chainTracker.DistanceTravelled - (index * _ballChainConfig.SpacingBalls), 0);
        Vector3 targetPosition = pathCreator.path.GetPointAtDistance(targetDistance);
        float currentSpeed = Time.deltaTime / _ballChainConfig.DurationMovingOffset;
        ball.transform.position = Vector3.Lerp(ball.transform.position, targetPosition, currentSpeed);
    }

    private void MoveFistBall()
    {
        float targetDistance = _chainTracker.DistanceTravelled;
        Vector3 targetPosition = pathCreator.path.GetPointAtDistance(targetDistance);
        CurrentBalls[0].transform.position = Vector3.Lerp(CurrentBalls[0].transform.position, targetPosition,
            Time.deltaTime / _ballChainConfig.DurationMovingOffset);
    }

    private bool HandleRemoveBallNearEndOfPath(Ball ball)
    {
        var currentPosition = ball.transform.position;
        Ball currentBall = ball;
        if (pathCreator.path.GetClosestDistanceAlongPath(currentPosition) >= pathCreator.path.length - 0.1f)
        {
            while (currentBall != null)
            {
                currentBall.backBall.index -= 1;
                currentBall = currentBall.backBall;
            }
            



            ball.Deactivate();
            _chainTracker.RemoveBall(ball);
            return true;
        }
        return false;
    }

    private float GetCurrentSpeed()
    {
        float currentSpeed = _isBoosting
            ? _ballChainConfig.MoveSpeed * _ballChainConfig.MoveSpeedMultiplier
            : _ballChainConfig.MoveSpeed;
        return currentSpeed;
    }

    private void AddBall(Ball ball)
    {
        if (CurrentBalls.Count == 0)
        {
            ball.transform.position = pathCreator.path.GetPointAtDistance(_chainTracker.DistanceTravelled);
        }
        else
        {
            Vector3 lastBallPosition = CurrentBalls[^1].transform.position;
            ball.transform.position = lastBallPosition;
        }

        _chainTracker.AddBall(ball);
    }

    private void ReIndexBalls()
    {
        for (int i = 0; i < _chainTracker.Balls.Count; i++)
        {
            _chainTracker.Balls[i].SetIndex(i);
        }
    }

    public GameObject Create(GameObject obj, Vector3 positon, Quaternion rotation)
    {
        return Instantiate(obj, positon, rotation);
    }


    private List<Ball> CurrentBalls => _chainTracker.Balls;

    
}
