using System;
using UnityEngine;

namespace Zuma.Curve
{
    [Serializable]
    public struct BezierCurvePoint
    {
        /// <summary>
        /// �����
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// ���Ƶ� ��������γ�����
        /// </summary>
        public Vector3 tangent;
    }
}
