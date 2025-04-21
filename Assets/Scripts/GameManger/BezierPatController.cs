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
    public int segmentPerCurve; //�����������߷ֳɶ��ٸ����Զ�
    public bool isShowDrawLine = true;

    private void Awake()
    {
        InitializeGenerateList();

        if (targetBallPrefab != null)
        {
            //�ڼ���õ�λ��������
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

    //    // ���Ʊ���������

    //    DrawBezierCurve();

    //}

    /// <summary>
    /// ���Ʊ���������
    /// </summary>
    private void DrawBezierCurve()
    {
        

        Gizmos.color = Color.blue; // ���û�����ɫ

        foreach (var point in m_ballGeneratePointsList)
        {
            int index = m_ballGeneratePointsList.IndexOf(point);

            // ����ÿ����
            Gizmos.DrawSphere(point, 0.05f);

            // �����߶�
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
            if (child != transform) // ����Ƿ��Ǹ������Transform
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
    /// ���㱴��������
    /// </summary>
    /// <param name="controlPoints">��Ҫ�γɱ��������ߵĵ㼯</param>
    /// <param name="t">���Բ�ֵ����</param>
    /// <returns>Vector3 �����������ڲ��� t ���ĵ�</returns>
    public Vector3 BezierPoint(List<Vector3> controlPoints, float t)
    {
        int n = controlPoints.Count - 1; // ���������ߵĴ���
        Vector3[] tempPoints = new Vector3[controlPoints.Count];

        // ���ƿ��Ƶ㵽��ʱ����
        for (int i = 0; i <= n; i++)
        {
            tempPoints[i] = controlPoints[i];
        }

        // ʹ��De Casteljau�㷨���㱴���������ϵĵ�
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
