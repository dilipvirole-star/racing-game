using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace RacingGame.Vehicle
{
    /// <summary>
    /// Handles all player input for vehicle control.
    /// Uses the New Input System.
    /// </summary>
    public class InputHandler : MonoBehaviour
    {
        private Vector2 _moveInput;
        private float _throttle;
        private float _brake;
        private bool _handbrake;
        private bool _nitro;

#if ENABLE_INPUT_SYSTEM
        private PlayerInput _playerInput;
        private InputAction _moveAction;
        private InputAction _throttleAction;
        private InputAction _brakeAction;
        private InputAction _handbrakeAction;
        private InputAction _nitroAction;
#endif

        private void Awake()
        {
#if ENABLE_INPUT_SYSTEM
            _playerInput = GetComponent<PlayerInput>();
            if (_playerInput == null)
            {
                _playerInput = gameObject.AddComponent<PlayerInput>();
            }
#endif
        }

        private void Update()
        {
#if ENABLE_INPUT_SYSTEM
            _moveInput = new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
            );
            _throttle = Input.GetAxis("Vertical");
            _brake = Input.GetKey(KeyCode.Space) ? 1f : 0f;
            _handbrake = Input.GetKey(KeyCode.LeftShift);
            _nitro = Input.GetKey(KeyCode.E);
#else
            GatherLegacyInput();
#endif
        }

        private void GatherLegacyInput()
        {
            _moveInput.x = Input.GetAxis("Horizontal");
            _moveInput.y = Input.GetAxis("Vertical");
            _throttle = Input.GetAxis("Vertical");
            _brake = Input.GetKey(KeyCode.Space) ? 1f : 0f;
            _handbrake = Input.GetKey(KeyCode.LeftShift);
            _nitro = Input.GetKey(KeyCode.E);
        }

        public Vector2 GetMoveInput() => _moveInput;
        public float GetThrottle() => _throttle;
        public float GetBrake() => _brake;
        public bool GetHandbrake() => _handbrake;
        public bool GetNitro() => _nitro;
    }
}
