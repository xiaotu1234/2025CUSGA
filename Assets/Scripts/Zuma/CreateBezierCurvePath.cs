using UnityEngine;
using System.Collections.Generic;
using Zuma.Curve;
using System.ComponentModel;



#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Zuma.Curve
{
    /// <summary>
    /// ����������·��
    /// </summary>
    public class CreateBezierCurvePath : MonoBehaviour
    {
        [SerializeField] private BezierCurve _curve;

        public bool Loop { get { return _curve.loop; } }

        public List<BezierCurvePoint> Points { get { return _curve.points; } }

        [HideInInspector]
        public List<Vector3>ballPointList = new List<Vector3>();
        [HideInInspector]
        public int segments { get { return _curve.segments; } }
        [HideInInspector]
        [ReadOnly(true)]
        public float distance;

        /// <summary>
        /// ���ݹ�һ��λ��ֵ��ȡ��Ӧ�ı����������ϵĵ�
        /// </summary>
        /// <param name="t">��һ��λ��ֵ [0,1]</param>
        /// <returns></returns>
        public Vector3 EvaluatePosition(float t)
        {
            return _curve.EvaluatePosition(t);
        }

        private void Awake()
        {
            InitialBallPoint();
        }

        private void InitialBallPoint()
        {
            float step = 1f / _curve.segments;
            Vector3 lastPos = transform.TransformPoint(_curve.EvaluatePosition(0f));
            Vector3 firstPos = transform.TransformPoint(_curve.EvaluatePosition(step));
            distance = Vector3.Distance(lastPos, firstPos);
            ballPointList.Add(lastPos);
            float end = (_curve.points.Count - 1 < 1 ? 0 : (_curve.loop ? _curve.points.Count : _curve.points.Count - 1)) + step * .5f;
            for (float t = step; t <= end; t += step)
            {
                //����λ��
                Vector3 p = transform.TransformPoint(_curve.EvaluatePosition(t));
                ballPointList.Add(p);
                //��¼
                lastPos = p;
            }
}


#if UNITY_EDITOR
        /// <summary>
        /// ·����ɫ(Gizmos)
        /// </summary>
        public Color pathColor = Color.green;

        private void OnDrawGizmos()
        {
            if (_curve.points.Count == 0) return;
            //������ɫ
            Color cacheColor = Gizmos.color;
            //·��������ɫ
            Gizmos.color = pathColor;
            //����
            float step = 1f / _curve.segments;
            //�����ϸ������
            Vector3 lastPos = transform.TransformPoint(_curve.EvaluatePosition(0f));
            float end = (_curve.points.Count - 1 < 1 ? 0 : (_curve.loop ? _curve.points.Count : _curve.points.Count - 1)) + step * .5f;
            for (float t = step; t <= end; t += step)
            {
                //����λ��
                Vector3 p = transform.TransformPoint(_curve.EvaluatePosition(t));
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
    [CustomEditor(typeof(CreateBezierCurvePath))]
    public class SimpleBezierCurvePathEditor : Editor
    {
        private CreateBezierCurvePath path;
        private const float sphereHandleCapSize = .2f;

        private void OnEnable()
        {
            path = target as CreateBezierCurvePath;
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

        //����������λ�û���
        private void DrawZumaBalls()
        {
            foreach (var point in path.ballPointList)
            {
                
            }
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

