using System;
using UnityEngine;

namespace Zuma.Curve
{
    [Serializable]
    public struct BezierCurvePoint
    {
        /// <summary>
        /// 坐标点
        /// </summary>
        public Vector3 position;

        /// <summary>
        /// 控制点 与坐标点形成切线
        /// </summary>
        public Vector3 tangent;
    }
}
