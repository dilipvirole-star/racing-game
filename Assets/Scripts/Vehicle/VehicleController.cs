using UnityEngine;
using RacingGame.Physics;

namespace RacingGame.Vehicle
{
    /// <summary>
    /// Core vehicle controller managing movement, input, and physics.
    /// </summary>
    public class VehicleController : MonoBehaviour
    {
        [SerializeField] private VehicleStats _vehicleStats;
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private WheelCollider[] _wheels;
        [SerializeField] private Transform[] _wheelMeshes;

        private VehiclePhysicsEngine _physicsEngine;
        private InputHandler _inputHandler;
        private float _steerInput;
        private float _throttleInput;
        private float _brakeInput;
        private bool _isHandbrakeActive;

        private void Awake()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _inputHandler = GetComponent<InputHandler>();
            _physicsEngine = new VehiclePhysicsEngine(_rigidbody, _wheels, _vehicleStats);

            if (_wheels.Length != _wheelMeshes.Length)
            {
                Debug.LogError("Wheel colliders and wheel meshes count mismatch!");
            }
        }

        private void Update()
        {
            GatherInput();
            UpdateWheelVisuals();
        }

        private void FixedUpdate()
        {
            _physicsEngine.Update(_steerInput, _throttleInput, _brakeInput, _isHandbrakeActive);
        }

        private void GatherInput()
        {
            _steerInput = Input.GetAxis("Horizontal");
            _throttleInput = Input.GetAxis("Vertical");
            _brakeInput = Input.GetKey(KeyCode.Space) ? 1f : 0f;
            _isHandbrakeActive = Input.GetKey(KeyCode.LeftShift);
        }

        private void UpdateWheelVisuals()
        {
            for (int i = 0; i < _wheels.Length; i++)
            {
                WheelCollider wheel = _wheels[i];
                Transform wheelMesh = _wheelMeshes[i];

                wheel.GetWorldPose(out Vector3 position, out Quaternion rotation);
                wheelMesh.position = position;
                wheelMesh.rotation = rotation;
            }
        }

        public float GetSpeed() => _rigidbody.linearVelocity.magnitude;
        public float GetRPM() => _physicsEngine.GetCurrentRPM();
        public float GetHealth() => _vehicleStats.CurrentHealth;
    }

    [System.Serializable]
    public class VehicleStats
    {
        [SerializeField] public float MaxHealth = 100f;
        [SerializeField] public float CurrentHealth = 100f;
        [SerializeField] public float TopSpeed = 300f;
        [SerializeField] public float Acceleration = 100f;
        [SerializeField] public float MaxTorque = 5000f;
        [SerializeField] public float BrakePower = 10000f;
        [SerializeField] public float SteerSensitivity = 1f;
        [SerializeField] public float Mass = 1500f;
        [SerializeField] public float DragCoefficient = 0.3f;
    }
}
