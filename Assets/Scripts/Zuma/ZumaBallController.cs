using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using UnityEngine;
using Zuma.Curve;

public class ZumaBallController : MonoBehaviour
{
    private List<Vector3> m_ballPointList = new List<Vector3>();

    [ReadOnly(true)]
    [Header("祖玛球半径等于两个生成点之间的距离")]
    public float radius;
    public GameObject ball;
    // Start is called before the first frame update
    private void Start()
    {
        CreateBezierCurvePath createPath = GetComponent<CreateBezierCurvePath>();
        m_ballPointList = createPath.BallPointList;
        radius = createPath.distance;
        GameObject lastBall = null;
       
        foreach (Vector3 position in m_ballPointList)
        {
            GameObject currentBall = Instantiate(ball, position,Quaternion.identity);
            ZumaBall currentBallInfo = currentBall.GetComponent<ZumaBall>();
            currentBallInfo.radius = radius;
            if (lastBall != null)
                lastBall.GetComponent<ZumaBall>().previousBall = currentBall;
            currentBallInfo.nextBall = lastBall;
            lastBall = currentBall;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
