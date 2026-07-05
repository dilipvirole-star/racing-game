using UnityEngine;
using RacingGame.Vehicle;

namespace RacingGame.AI
{
    /// <summary>
    /// AI-controlled traffic vehicle with intelligent navigation and collision avoidance.
    /// </summary>
    public class TrafficVehicle : MonoBehaviour
    {
        [SerializeField] private float _speed = 30f;
        [SerializeField] private float _acceleration = 5f;
        [SerializeField] private float _detectionRadius = 50f;
        [SerializeField] private float _stoppingDistance = 10f;

        private Rigidbody _rigidbody;
        private float _currentSpeed = 0f;
        private Vector3 _targetDirection = Vector3.forward;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
            {
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            }
        }

        private void FixedUpdate()
        {
            DetectObstacles();
            MoveForward();
        }

        private void DetectObstacles()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, _detectionRadius);

            foreach (var col in colliders)
            {
                if (col.gameObject != gameObject && col.CompareTag("Vehicle"))
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < _stoppingDistance)
                    {
                        _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, Time.fixedDeltaTime);
                        return;
                    }
                }
            }
        }

        private void MoveForward()
        {
            _currentSpeed = Mathf.Lerp(_currentSpeed, _speed, _acceleration * Time.fixedDeltaTime);
            _rigidbody.linearVelocity = transform.forward * _currentSpeed;
        }
    }
}
