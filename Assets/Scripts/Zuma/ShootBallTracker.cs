using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootBallTracker: MonoBehaviour
{
    private float _thresholdToPoint = 0.35f;

    public Ball _ball;
    private Coroutine _intersectionTrackerCoroutine;
    public BallChainController _controller;
    private BallProvider _ballProvider;

    private void OnEnable()
    {
        _controller = BallChainController.Instance;
        _ball = GetComponent<Ball>();
        _ballProvider = _controller.GetPlayerBalls();

    }
    public void StartTracker(List<Vector3> insertPosition)
    {
        _intersectionTrackerCoroutine ??= StartCoroutine(IntersectionTrackerRoutine(insertPosition));
    }

    public void StopTracker()
    {
        if (_intersectionTrackerCoroutine != null)
        {
            StopCoroutine(_intersectionTrackerCoroutine);
            _intersectionTrackerCoroutine = null;
        }
    }
    private IEnumerator IntersectionTrackerRoutine(List<Vector3> intersectionPoints)
    {
        intersectionPoints = intersectionPoints.OrderBy(point => Vector3.Distance(transform.position, point))
            .ToList();

        if (intersectionPoints.Count == 0)
        {
            Debug.LogError("No intersection points found.");
            while (true)
            {
                Invoke(nameof(ReturnBall), 5f); // —”≥Ÿ2√Î
                yield return null;
                yield break;
            }
        }

        int currentIndex = 0;

        while (true)
        {
            if (currentIndex < intersectionPoints.Count)
            {
                Vector3 targetPoint = intersectionPoints[currentIndex];
                float distanceToPoint = Vector3.Distance(transform.position, targetPoint);

                if (distanceToPoint <= _thresholdToPoint)
                {
                    currentIndex++;
                    _controller.TryAttachBall(_ball);
                }
            }

            else 
            {
                Invoke(nameof(ReturnBall), 1f); // —”≥Ÿ2√Î
                yield break;
            }

            yield return null;
        }
    }

    private void ReturnBall(Ball ball)
    {
        _ballProvider.ReturnBall(ball);
    }

}
