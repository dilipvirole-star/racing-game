using UnityEngine;

namespace RacingGame.Physics
{
    /// <summary>
    /// Advanced vehicle physics engine handling wheel forces, suspension, and drivetrain.
    /// </summary>
    public class VehiclePhysicsEngine
    {
        private Rigidbody _rigidbody;
        private WheelCollider[] _wheels;
        private Vehicle.VehicleStats _stats;

        private float _currentRPM = 0f;
        private float _currentGear = 1f;
        private const float RPM_MAX = 7000f;
        private const float RPM_IDLE = 800f;

        public VehiclePhysicsEngine(Rigidbody rigidbody, WheelCollider[] wheels, Vehicle.VehicleStats stats)
        {
            _rigidbody = rigidbody;
            _wheels = wheels;
            _stats = stats;
        }

        public void Update(float steer, float throttle, float brake, bool handbrake)
        {
            ApplySteering(steer);
            ApplyThrottle(throttle);
            ApplyBraking(brake, handbrake);
            UpdateWheelFriction();
            UpdateEngineRPM(throttle);
        }

        private void ApplySteering(float steer)
        {
            float steerAngle = steer * 45f * _stats.SteerSensitivity;

            if (_wheels.Length >= 2)
            {
                _wheels[0].steerAngle = steerAngle;
                _wheels[1].steerAngle = steerAngle;
            }
        }

        private void ApplyThrottle(float throttle)
        {
            float motorForce = throttle * _stats.MaxTorque;

            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheels[i].motorTorque = motorForce;
            }
        }

        private void ApplyBraking(float brake, bool handbrake)
        {
            float brakeTorque = brake * _stats.BrakePower;
            if (handbrake)
                brakeTorque += _stats.BrakePower * 0.5f;

            for (int i = 0; i < _wheels.Length; i++)
            {
                _wheels[i].brakeTorque = brakeTorque;
            }
        }

        private void UpdateWheelFriction()
        {
            foreach (var wheel in _wheels)
            {
                WheelFrictionCurve forwardFriction = wheel.forwardFriction;
                WheelFrictionCurve sidewaysFriction = wheel.sidewaysFriction;

                forwardFriction.extremumValue = 1.2f;
                forwardFriction.extremumSlip = 0.1f;
                sidewaysFriction.extremumValue = 1.2f;
                sidewaysFriction.extremumSlip = 0.1f;

                wheel.forwardFriction = forwardFriction;
                wheel.sidewaysFriction = sidewaysFriction;
            }
        }

        private void UpdateEngineRPM(float throttle)
        {
            float speed = _rigidbody.linearVelocity.magnitude;
            float speedRatio = Mathf.Clamp01(speed / _stats.TopSpeed);

            if (throttle > 0)
            {
                _currentRPM = Mathf.Lerp(_currentRPM, RPM_MAX, Time.fixedDeltaTime * 0.5f);
            }
            else
            {
                _currentRPM = Mathf.Lerp(_currentRPM, RPM_IDLE, Time.fixedDeltaTime * 0.3f);
            }
        }

        public float GetCurrentRPM() => _currentRPM;
        public float GetSpeed() => _rigidbody.linearVelocity.magnitude;
    }
}
