using System.Collections;
using UnityEngine;

namespace Assets.Prototype1.Scripts.Camera
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField] private Transform _target;      // Assign your player here
        [SerializeField] private float _smoothSpeed = 0.125f;
        [SerializeField] private Vector2 _clampX = new Vector2(-45f, 45f);
        [SerializeField] private Vector2 _clampZ = new Vector2(-55f, 55f);
        [SerializeField] private float _fixedY = 20f;

        private Vector3 _velocity = Vector3.zero;

        void LateUpdate()
        {
            if (_target == null) return;

            Vector3 desiredPosition = new Vector3(
                Mathf.Clamp(_target.position.x, _clampX.x, _clampX.y),
                _fixedY,
                Mathf.Clamp(_target.position.z, _clampZ.x, _clampZ.y)
            );

            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref _velocity, _smoothSpeed);

            transform.position = smoothedPosition;
        }
    }
}