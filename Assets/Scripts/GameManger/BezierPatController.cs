using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierPatController : MonoBehaviour
{
    public GameObject targetBallPrefab;
    public List<GameObject> controlPointsGameObject;
    private List<Vector3> m_controlPoints;
    private List<Vector3> m_ballGeneratePointsList;
    public int segmentPerCurve; //将贝塞尔曲线分成多少个线性段
    public bool isShowDrawLine = true;

    private void Awake()
    {
        InitializeGenerateList();

        if (targetBallPrefab != null)
        {
            //在计算好的位置生成球
            foreach (Vector3 position in m_ballGeneratePointsList)
            {
                GameObject targetBall = Instantiate(targetBallPrefab, position, Quaternion.identity);
            }
        }
    }

    //private void OnDrawGizmos()
    //{
    //    InitializeGenerateList();
    //    if (m_controlPoints == null || !isShowDrawLine)
    //        return;

    //    // 绘制贝塞尔曲线

    //    DrawBezierCurve();

    //}

    /// <summary>
    /// 绘制贝塞尔曲线
    /// </summary>
    private void DrawBezierCurve()
    {
        

        Gizmos.color = Color.blue; // 设置绘制颜色

        foreach (var point in m_ballGeneratePointsList)
        {
            int index = m_ballGeneratePointsList.IndexOf(point);

            // 绘制每个点
            Gizmos.DrawSphere(point, 0.05f);

            // 绘制线段
            if (index > 0)
            {
                Vector3 prevPoint = m_ballGeneratePointsList[index - 1];
                Gizmos.DrawLine(prevPoint, point);
            }
        }
    }



    private void InitializeGenerateList()
    {
        List<Vector3> m_ballGeneratePointsList = new List<Vector3>();
        Transform[] allTransforms = GetComponentsInChildren<Transform>();
        foreach (Transform child in allTransforms)
        {
            if (child != transform) // 检查是否是父物体的Transform
            {
                m_controlPoints.Add(child.position);
            }
        }


        for (int j = 0; j <= segmentPerCurve; j++)
        {
            float t = (float)j / segmentPerCurve;
            m_ballGeneratePointsList.Add(BezierPoint(m_controlPoints, t));
        }
    }

    /// <summary>
    /// 计算贝塞尔曲线
    /// </summary>
    /// <param name="controlPoints">需要形成贝塞尔曲线的点集</param>
    /// <param name="t">线性插值参数</param>
    /// <returns>Vector3 贝塞尔曲线在参数 t 处的点</returns>
    public Vector3 BezierPoint(List<Vector3> controlPoints, float t)
    {
        int n = controlPoints.Count - 1; // 贝塞尔曲线的次数
        Vector3[] tempPoints = new Vector3[controlPoints.Count];

        // 复制控制点到临时数组
        for (int i = 0; i <= n; i++)
        {
            tempPoints[i] = controlPoints[i];
        }

        // 使用De Casteljau算法计算贝塞尔曲线上的点
        for (int k = 1; k <= n; k++)
        {
            for (int i = 0; i <= n - k; i++)
            {
                tempPoints[i] = (1 - t) * tempPoints[i] + t * tempPoints[i + 1];
            }
        }

        return tempPoints[0];
    }
}
