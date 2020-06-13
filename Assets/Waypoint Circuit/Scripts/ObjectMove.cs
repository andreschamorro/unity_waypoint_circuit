using System;
using UnityEngine;

namespace Waypoint
{
    public class ObjectMove : MonoBehaviour
    {
        #region FIELDS

        [Header("Settings")]
        [SerializeField]
        [Range(1.0f, 100.0f)]
        private float _speedMove = 10.0f;

        [SerializeField]
        [Range(1.0f, 100.0f)]
        private float _speedRotate = 50.0f;

        [SerializeField]
        private Vector3 _offset = new Vector3(0.0f, 0.5f, 0.0f);

        [SerializeField]
        private float _rayCastDistance = 1.0f;

        private bool _stop = false;
        private Vector3 _position;
        private Quaternion _rotation;

        public event Action OnEndOfRoad;

        #endregion

        #region PROPERTIES

        private Vector3 Position
        {
            get
            {
                _position = transform.position - _offset;
                return _position;
            }

            set
            {
                _position = value;
                transform.position = _position + _offset;
            }
        }

        private Quaternion Rotation
        {
            get
            {
                _rotation = transform.rotation;
                return _rotation;
            }

            set
            {
                _rotation = value;
                transform.rotation = _rotation;
            }
        }

        #endregion

        #region UNITY_METHODS

        private void Update()
        {
            UpdateRayCast();
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag("Obstacle")) return;

            var obstacle = other.GetComponent<Obstacle>();
            obstacle.PathIsBusy = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Obstacle")) return;

            var obstacle = other.GetComponent<Obstacle>();
            obstacle.PathIsBusy = true;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawRay(transform.position, transform.forward * (_rayCastDistance + transform.localScale.z));
        }

        #endregion

        #region PUBLIC_METHODS

        public void Move(Vector3 currentPoint)
        {
            if (_stop) return;

            var targetLookRotation = currentPoint - Position;

            if (targetLookRotation != Vector3.zero)
            {
                Rotation = Quaternion.Lerp(
                    transform.rotation,
                    Quaternion.LookRotation(currentPoint - Position, Vector3.up),
                    _speedRotate * Time.deltaTime);
            }

            Position = Vector3.MoveTowards(
                Position,
                currentPoint,
                _speedMove * Time.deltaTime);

            if (Vector3.Distance(Position, currentPoint) <= 0.1f)
            {
                OnEndOfRoad?.Invoke();
            }
        }

        #endregion

        #region PRIVATE_METHODS

        private void UpdateRayCast()
        {
            Physics.Raycast(transform.position, transform.forward, out var hit, _rayCastDistance);

            if (hit.collider != null && hit.collider.CompareTag("ObjectMove"))
            {
                _stop = true;
            }
            else
            {
                _stop = false;
            }
        }

        #endregion
    }
}