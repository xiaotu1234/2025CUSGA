using System;
using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

namespace Zuma.Curve
{

    /// <summary>
    /// ����������
    /// </summary>
    [Serializable]
    public class BezierCurve
    {
        /// <summary>
        /// ����
        /// </summary>
        [Range(1, 100)] public int segments = 10;

        /// <summary>
        /// �Ƿ�ѭ��
        /// </summary>
        public bool loop;

        /// <summary>
        /// �㼯��
        /// </summary>
        public List<BezierCurvePoint> points = new List<BezierCurvePoint>(2)
        {
            new BezierCurvePoint() { position = Vector3.back * 5f, tangent = Vector3.back * 5f + Vector3.left * 3f },
            new BezierCurvePoint() { position = Vector3.forward * 5f, tangent = Vector3.forward * 5f + Vector3.right * 3f }
        };


        



        /// <summary>
        /// ���ݹ�һ��λ��ֵ��ȡ��Ӧ�ı����������ϵĵ�
        /// </summary>
        /// <param name="t">��һ��λ��ֵ [0,1]</param>
        /// <returns></returns>
        public Vector3 EvaluatePosition(float t)
        {
            Vector3 retVal = Vector3.zero;
            if (points.Count > 0)
            {
                float max = points.Count - 1 < 1 ? 0 : (loop ? points.Count : points.Count - 1);
                float standardized = (loop && max > 0) ? ((t %= max) + (t < 0 ? max : 0)) : Mathf.Clamp(t, 0, max);
                int rounded = Mathf.RoundToInt(standardized);
                int i1, i2;
                if (Mathf.Abs(standardized - rounded) < Mathf.Epsilon)
                    i1 = i2 = (rounded == points.Count) ? 0 : rounded;
                else
                {
                    i1 = Mathf.FloorToInt(standardized);
                    if (i1 >= points.Count)
                    {
                        standardized -= max;
                        i1 = 0;
                    }
                    i2 = Mathf.CeilToInt(standardized);
                    i2 = i2 >= points.Count ? 0 : i2;
                }
                retVal = i1 == i2 ? points[i1].position : BezierCurveUtility.Bezier3(points[i1].position,
                    points[i1].position + points[i1].tangent, points[i2].position
                    - points[i2].tangent, points[i2].position, standardized - i1);
            }
            return retVal;
        }

    public static class BezierCurveUtility
        {
            /// <summary>
            /// �������α����������ϵĵ�
            /// </summary>
            /// <param name="p0">���</param>
            /// <param name="p1">�������߷��򣨿��Ƶ�1��</param>
            /// <param name="p2">�յ�����߷��򣨿��Ƶ�2��</param>
            /// <param name="p3">�յ�</param>
            /// <param name="t">��������ΧΪ [0, 1]</param>
            /// <returns>�����ϵĵ��λ��</returns>
            public static Vector3 Bezier3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
            {
                // �������α��������ߵĹ�ʽ
                float u = 1 - t;
                float tt = t * t;
                float uu = u * u;
                float uuu = uu * u;
                float ttt = tt * t;

                // ��ֵ��ʽ
                Vector3 p =
                    (uuu * p0) + // (1 - t)^3 * P0
                    (3 * uu * t * p1) + // 3 * (1 - t)^2 * t * P1
                    (3 * u * tt * p2) + // 3 * (1 - t) * t^2 * P2
                    (ttt * p3); // t^3 * P3

                return p;
            }
        }
    }
}


