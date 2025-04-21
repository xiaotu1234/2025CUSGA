using UnityEngine;
using System.Collections.Generic;
using Zuma.Curve;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Zuma.Curve
{
    /// <summary>
    /// ����������·��
    /// </summary>
    public class SimpleBezierCurvePath : MonoBehaviour
    {
        [SerializeField] private BezierCurve curve;

        public bool Loop { get { return curve.loop; } }

        public List<BezierCurvePoint> Points { get { return curve.points; } }

        /// <summary>
        /// ���ݹ�һ��λ��ֵ��ȡ��Ӧ�ı����������ϵĵ�
        /// </summary>
        /// <param name="t">��һ��λ��ֵ [0,1]</param>
        /// <returns></returns>
        public Vector3 EvaluatePosition(float t)
        {
            return curve.EvaluatePosition(t);
        }

#if UNITY_EDITOR
        /// <summary>
        /// ·����ɫ(Gizmos)
        /// </summary>
        public Color pathColor = Color.green;

        private void OnDrawGizmos()
        {
            if (curve.points.Count == 0) return;
            //������ɫ
            Color cacheColor = Gizmos.color;
            //·��������ɫ
            Gizmos.color = pathColor;
            //����
            float step = 1f / curve.segments;
            //�����ϸ������
            Vector3 lastPos = transform.TransformPoint(curve.EvaluatePosition(0f));
            float end = (curve.points.Count - 1 < 1 ? 0 : (curve.loop ? curve.points.Count : curve.points.Count - 1)) + step * .5f;
            for (float t = step; t <= end; t += step)
            {
                //����λ��
                Vector3 p = transform.TransformPoint(curve.EvaluatePosition(t));
                //��������
                Gizmos.DrawLine(lastPos, p);
                //��¼
                lastPos = p;
            }
            //�ָ���ɫ
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
            //·���㼯��Ϊ��
            if (path.Points == null || path.Points.Count == 0) return;
            //��ǰѡ�й��߷��ƶ�����
            if (Tools.current != Tool.Move) return;
            //��ɫ����
            Color cacheColor = Handles.color;
            Handles.color = Color.yellow;
            //����·���㼯��
            for (int i = 0; i < path.Points.Count; i++)
            {
                DrawPositionHandle(i);
                DrawTangentHandle(i);

                BezierCurvePoint point = path.Points[i];
                //�ֲ�תȫ������ ·���㡢���Ƶ� 
                Vector3 position = path.transform.TransformPoint(point.position);
                Vector3 controlPoint = path.transform.TransformPoint(point.position + point.tangent);
                //��������
                Handles.DrawDottedLine(position, controlPoint, 1f);
            }
            //�ָ���ɫ
            Handles.color = cacheColor;
        }

        //·�������������
        private void DrawPositionHandle(int index)
        {
            BezierCurvePoint point = path.Points[index];
            //�ֲ�תȫ������
            Vector3 position = path.transform.TransformPoint(point.position);
            //����������ת����
            Quaternion rotation = Tools.pivotRotation == PivotRotation.Local
                ? path.transform.rotation : Quaternion.identity;
            //�������Ĵ�С
            float size = HandleUtility.GetHandleSize(position) * sphereHandleCapSize;
            //�ڸ�·�������һ������
            Handles.color = Color.white;
            Handles.SphereHandleCap(0, position, rotation, size, EventType.Repaint);
            Handles.Label(position, string.Format("Point{0}", index));
            //�����
            EditorGUI.BeginChangeCheck();
            //���������
            position = Handles.PositionHandle(position, rotation);
            //��������� ���������� ����·����
            if (EditorGUI.EndChangeCheck())
            {
                //��¼����
                Undo.RecordObject(path, "Position Changed");
                //ȫ��ת�ֲ�����
                point.position = path.transform.InverseTransformPoint(position);
                //����·����
                path.Points[index] = point;
            }
        }

        //���Ƶ����������
        private void DrawTangentHandle(int index)
        {
            BezierCurvePoint point = path.Points[index];
            //�ֲ�תȫ������
            Vector3 cp = path.transform.TransformPoint(point.position + point.tangent);
            //����������ת����
            Quaternion rotation = Tools.pivotRotation == PivotRotation.Local
                ? path.transform.rotation : Quaternion.identity;
            //�������Ĵ�С
            float size = HandleUtility.GetHandleSize(cp) * sphereHandleCapSize;
            //�ڸÿ��Ƶ����һ������
            Handles.color = Color.yellow;
            Handles.SphereHandleCap(0, cp, rotation, size, EventType.Repaint);
            //�����
            EditorGUI.BeginChangeCheck();
            //���������
            cp = Handles.PositionHandle(cp, rotation);
            //��������� ���������� ����·����
            if (EditorGUI.EndChangeCheck())
            {
                //��¼����
                Undo.RecordObject(path, "Control Point Changed");
                //ȫ��ת�ֲ�����
                point.tangent = path.transform.InverseTransformPoint(cp) - point.position;
                //����·����
                path.Points[index] = point;
            }
        }
    }
#endif
}

