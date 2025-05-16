using System.Collections.Generic;
using System.Linq;
using System.Threading;
using PathCreation;
using Unity.VisualScripting;
using UnityEngine;
using Cysharp.Threading.Tasks;
using static UnityEditor.Progress;
using UnityEditor;
using System;


public class BallChainController : MonoBehaviour
{
    [Header("是否在测试，打包记得关掉")]
    public bool isTesting = false;
    public static BallChainController Instance { get; private set; }   
    public PathCreator pathCreator;
    public BallChainConfig _ballChainConfig;
    public GameObject zumaBall;
    public GameObject playerBall;
    public List<Color> ballColors = new List<Color>();
    public int playerBallCount = 10;
    public List<Ball> pool;
    
  
    public bool _needGizmos = false;
    private List<Color> _colorItems = new List<Color>();
    private List<Color> _colors = new List<Color>();
    private int _colorCount;
    private CancellationTokenSource _startBallSpawning;
    private BallProvider _ballProvider;
    private BallProvider _playerBalls;
    private ChainTracker _chainTracker ;
    private AttachingBallChainHandler _attachingBallChainHandler;
    private float _wholeDistance;


    
    public List<Ball> ActiveItems => _chainTracker.Balls.ToList();

    private void Awake()
    {
        
        _colors = EnemyManager.Instance.colors;
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        _wholeDistance = pathCreator.path.length;
    }
    private void OnEnable()
    {
        _chainTracker = new ChainTracker(_ballChainConfig);

        _ballProvider = new BallProvider(zumaBall , this, _ballChainConfig, 0);
        _playerBalls = new BallProvider(playerBall, this, _ballChainConfig, playerBallCount);
        
        _ballProvider.CreatePoolBall();
        _playerBalls.CreatePoolBall();
        
        _attachingBallChainHandler = new AttachingBallChainHandler(pathCreator, _ballChainConfig, _chainTracker, _ballProvider);
        if (isTesting)
            StartBallSpawning(ballColors);
        else
            StartBallSpawning(_colors);



    }
    private void OnDisable()
    {
        if(_ballProvider!= null)
            _ballProvider.CleanupPool();
        else
        {
            Debug.LogError(" _ballProvider is null");
        }
        _playerBalls.CleanupPool();
        StopBallSpawning();

    }

    private void OnDestroy()
    {
        _ballProvider.CleanupPool();
        _playerBalls.CleanupPool();
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
        pool = _ballProvider.GetPool();
    }

    public void StartBallSpawning(List<Color> colorItems)
    {
        if (pathCreator == null)
            return;

        _startBallSpawning?.Cancel();
        _startBallSpawning = new CancellationTokenSource();

        _colorItems = colorItems;
        _colorCount = _colorItems.Count;
        //BoostSpeedAsync(_startBallSpawning.Token).Forget();
        SpawnInitialBallsAsync(_startBallSpawning.Token).Forget();
    }

    public void StopBallSpawning()
    {
        _startBallSpawning?.Cancel();
        pathCreator = null;

        _chainTracker.ClearUp();
        _colorItems.Clear();

        


    }  

    private async UniTaskVoid SpawnInitialBallsAsync(CancellationToken token)
    {
        while (!token.IsCancellationRequested)
        {
            // 1. 动态计算生成间隔（基于当前速度）
            float currentSpeed = GetCurrentSpeed();
            float actualInterval = _ballChainConfig.SpacingBalls / currentSpeed;
            actualInterval = Mathf.Max(actualInterval, 0.05f); // 最小间隔保护

            // 2. 计算生成位置（基于链表头部移动距离 + 当前链长 * 间距）
            // 计算实际生成位置（路径起点 + 链头距离的循环偏移）\
            float spawnDistance = 0;
            



            int index = UnityEngine.Random.Range(0, _colorCount);
            var color = _colorItems[index];

            // 3. 安全生成
            if (spawnDistance >= 0 && spawnDistance <= _wholeDistance)
            {
                Vector3 spawnPosition = pathCreator.path.GetPointAtDistance(spawnDistance);
                Ball newBall = _ballProvider.GetBall(spawnPosition, Quaternion.identity);
                newBall.SetColor(color);
                _chainTracker.AddBallLast(newBall);
                Debug.Log("生成祖玛球体");
            }
            else
            {
                Debug.LogError($"非法生成位置: {spawnDistance}，实际总长：{_wholeDistance}");
            }



            // 4. 等待动态计算的间隔
            await UniTask.Delay(
                (int)(actualInterval * 1000),
                cancellationToken: token
            );
        }
    }

   

    private void MoveBalls()
    {
        if (!_chainTracker.Balls.Any()) return;

        // 更新链头距离
        float currentSpeed = GetCurrentSpeed();
        _chainTracker.AddChainHeadDistance(currentSpeed * Time.deltaTime);

        // 移动所有球
        foreach (var ball in _chainTracker.Balls)
        {
            MoveBall(ball);
        }

        // 检测并处理到达终点的球
        HandleBallsReachingEnd();
    }

    private void MoveBall(Ball ball)
    {
        float targetDistance = _chainTracker.GetTargetDistanceForBall(ball);
        targetDistance = Mathf.Clamp(targetDistance, 0, _wholeDistance - 0.2f);

        Vector3 targetPos = pathCreator.path.GetPointAtDistance(targetDistance);
        ball.transform.position = Vector3.Lerp(
            ball.transform.position,
            targetPos,
            Time.deltaTime / _ballChainConfig.DurationMovingOffset
        );
    }



    private void HandleBallsReachingEnd()
    {
        var ballsToRemove = new List<Ball>();
        foreach (var ball in _chainTracker.Balls)
        {
            float currentDist = pathCreator.path.GetClosestDistanceAlongPath(ball.transform.position);
            if (currentDist >= _wholeDistance - _ballChainConfig.EndOffset)
            {
                ballsToRemove.Add(ball);
            }
        }

        foreach (var ball in ballsToRemove)
        {
            _chainTracker.RemoveBall(ball);
            _ballProvider.ReturnBall(ball);
        }
    }

    private float GetCurrentSpeed()
    {
        float currentSpeed = _ballChainConfig.MoveSpeed;
        return currentSpeed;
    }
    
    public bool TryAttachBall(Ball ball)
    {
        return _attachingBallChainHandler.TryAttachBall(ball);
    }

    public BallProvider GetPlayerBalls()
    {
        if (_playerBalls == null)
        {
            Debug.LogWarning("_playerBalls is null");
        }
        return _playerBalls;
    }


    public GameObject Create(GameObject obj, Vector3 positon, Quaternion rotation)
    {
        return Instantiate(obj, positon, rotation);
    }
    


    public Ball GetShootBall(Vector3 p ,Quaternion r)
    {
        return _playerBalls.GetBall(p , r);
    }

    
}
