using UnityEngine;
using System.Collections.Generic;
using Zuma.Curve;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Zuma.Curve
{
    /// <summary>
    /// 贝塞尔曲线路径
    /// </summary>
    public class SimpleBezierCurvePath : MonoBehaviour
    {
        [SerializeField] private BezierCurve curve;

        public bool Loop { get { return curve.loop; } }

        public List<BezierCurvePoint> Points { get { return curve.points; } }

        /// <summary>
        /// 根据归一化位置值获取对应的贝塞尔曲线上的点
        /// </summary>
        /// <param name="t">归一化位置值 [0,1]</param>
        /// <returns></returns>
        public Vector3 EvaluatePosition(float t)
        {
            return curve.EvaluatePosition(t);
        }

#if UNITY_EDITOR
        /// <summary>
        /// 路径颜色(Gizmos)
        /// </summary>
        public Color pathColor = Color.green;

        private void OnDrawGizmos()
        {
            if (curve.points.Count == 0) return;
            //缓存颜色
            Color cacheColor = Gizmos.color;
            //路径绘制颜色
            Gizmos.color = pathColor;
            //步长
            float step = 1f / curve.segments;
            //缓存上个坐标点
            Vector3 lastPos = transform.TransformPoint(curve.EvaluatePosition(0f));
            float end = (curve.points.Count - 1 < 1 ? 0 : (curve.loop ? curve.points.Count : curve.points.Count - 1)) + step * .5f;
            for (float t = step; t <= end; t += step)
            {
                //计算位置
                Vector3 p = transform.TransformPoint(curve.EvaluatePosition(t));
                //绘制曲线
                Gizmos.DrawLine(lastPos, p);
                //记录
                lastPos = p;
            }
            //恢复颜色
            Gizmos.color = cacheColor;
        }
#endif
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(SimpleBezierCurvePath))]
    public class SimpleBezierCurvePathEditor : Editor
    {
        private SimpleBezierCurvePath path;
        private const float sphereHandleCapSize = .2f;

        private void OnEnable()
        {
            path = target as SimpleBezierCurvePath;
        }

        private void OnSceneGUI()
        {
            //路径点集合为空
            if (path.Points == null || path.Points.Count == 0) return;
            //当前选中工具非移动工具
            if (Tools.current != Tool.Move) return;
            //颜色缓存
            Color cacheColor = Handles.color;
            Handles.color = Color.yellow;
            //遍历路径点集合
            for (int i = 0; i < path.Points.Count; i++)
            {
                DrawPositionHandle(i);
                DrawTangentHandle(i);

                BezierCurvePoint point = path.Points[i];
                //局部转全局坐标 路径点、控制点 
                Vector3 position = path.transform.TransformPoint(point.position);
                Vector3 controlPoint = path.transform.TransformPoint(point.position + point.tangent);
                //绘制切线
                Handles.DrawDottedLine(position, controlPoint, 1f);
            }
            //恢复颜色
            Handles.color = cacheColor;
        }

        //路径点操作柄绘制
        private void DrawPositionHandle(int index)
        {
            BezierCurvePoint point = path.Points[index];
            //局部转全局坐标
            Vector3 position = path.transform.TransformPoint(point.position);
            //操作柄的旋转类型
            Quaternion rotation = Tools.pivotRotation == PivotRotation.Local
                ? path.transform.rotation : Quaternion.identity;
            //操作柄的大小
            float size = HandleUtility.GetHandleSize(position) * sphereHandleCapSize;
            //在该路径点绘制一个球形
            Handles.color = Color.white;
            Handles.SphereHandleCap(0, position, rotation, size, EventType.Repaint);
            Handles.Label(position, string.Format("Point{0}", index));
            //检测变更
            EditorGUI.BeginChangeCheck();
            //坐标操作柄
            position = Handles.PositionHandle(position, rotation);
            //变更检测结束 如果发生变更 更新路径点
            if (EditorGUI.EndChangeCheck())
            {
                //记录操作
                Undo.RecordObject(path, "Position Changed");
                //全局转局部坐标
                point.position = path.transform.InverseTransformPoint(position);
                //更新路径点
                path.Points[index] = point;
            }
        }

        //控制点操作柄绘制
        private void DrawTangentHandle(int index)
        {
            BezierCurvePoint point = path.Points[index];
            //局部转全局坐标
            Vector3 cp = path.transform.TransformPoint(point.position + point.tangent);
            //操作柄的旋转类型
            Quaternion rotation = Tools.pivotRotation == PivotRotation.Local
                ? path.transform.rotation : Quaternion.identity;
            //操作柄的大小
            float size = HandleUtility.GetHandleSize(cp) * sphereHandleCapSize;
            //在该控制点绘制一个球形
            Handles.color = Color.yellow;
            Handles.SphereHandleCap(0, cp, rotation, size, EventType.Repaint);
            //检测变更
            EditorGUI.BeginChangeCheck();
            //坐标操作柄
            cp = Handles.PositionHandle(cp, rotation);
            //变更检测结束 如果发生变更 更新路径点
            if (EditorGUI.EndChangeCheck())
            {
                //记录操作
                Undo.RecordObject(path, "Control Point Changed");
                //全局转局部坐标
                point.tangent = path.transform.InverseTransformPoint(cp) - point.position;
                //更新路径点
                path.Points[index] = point;
            }
        }
    }
#endif
}

