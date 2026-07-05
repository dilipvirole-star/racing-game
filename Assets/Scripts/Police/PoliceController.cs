using UnityEngine;

namespace RacingGame.Police
{
    /// <summary>
    /// Police AI controller managing pursuit and enforcement behaviors.
    /// </summary>
    public class PoliceController : MonoBehaviour
    {
        [SerializeField] private float _pursuitSpeed = 50f;
        [SerializeField] private float _detectionRange = 200f;
        [SerializeField] private float _minFollowDistance = 30f;

        private Rigidbody _rigidbody;
        private Transform _targetTransform;
        private bool _isPursuing = false;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (_targetTransform == null)
            {
                _isPursuing = false;
                return;
            }

            float distanceToTarget = Vector3.Distance(transform.position, _targetTransform.position);

            if (distanceToTarget > _detectionRange)
            {
                _isPursuing = false;
                return;
            }

            _isPursuing = true;
            PursuitBehavior();
        }

        private void PursuitBehavior()
        {
            Vector3 direction = (_targetTransform.position - transform.position).normalized;
            _rigidbody.linearVelocity = direction * _pursuitSpeed;
        }

        public void SetTarget(Transform target)
        {
            _targetTransform = target;
        }

        public bool IsPursuing => _isPursuing;
    }
}
