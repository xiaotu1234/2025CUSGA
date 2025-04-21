using UnityEngine;
using Zuma.Curve;

namespace Zuma.Curve
{
    public class SimpleBezierCurvePathAlonger : MonoBehaviour
    {
        public enum UpdateMode
        {
            FixedUpdate,
            Update,
            LateUpdate,
        }

        [SerializeField] private SimpleBezierCurvePath path;

        [SerializeField] private float speed = .1f;

        [SerializeField] private UpdateMode updateMode = UpdateMode.Update;

        private float normalized = 0f;

        private Vector3 lastPosition;

        private void FixedUpdate()
        {
            if (updateMode == UpdateMode.FixedUpdate && path != null)
                MoveAlongPath();
        }

        private void Update()
        {
            if (updateMode == UpdateMode.Update && path != null)
                MoveAlongPath();
        }

        private void LateUpdate()
        {
            if (updateMode == UpdateMode.LateUpdate && path != null)
                MoveAlongPath();
        }

        private void MoveAlongPath()
        {
            float t = normalized + speed * Time.deltaTime;
            float max = path.Points.Count - 1 < 1 ? 0 : (path.Loop ? path.Points.Count : path.Points.Count - 1);
            normalized = (path.Loop && max > 0) ? ((t %= max) + (t < 0 ? max : 0)) : Mathf.Clamp(t, 0, max);
            transform.position = path.EvaluatePosition(normalized);
            Vector3 forward = transform.position - lastPosition;
            transform.forward = forward != Vector3.zero ? forward : transform.forward;
            lastPosition = transform.position;
        }
    }
}
