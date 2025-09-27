using Godot;
using System;
using Parallas;

public partial class RovBehavior : Node
{
    [Export] private RigidBody3D _rigidBody3D;
    [Export] private Camera3D _camera3D;

    private const float OutOfWaterThreshold = 0.4f;

    public Vector3 inputMove;
    public Vector2 inputLook;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Quaternion camRotationFlattened = new Quaternion(Vector3.Up, _rigidBody3D.GlobalRotation.Y - Mathf.Pi);
        Vector3 strafeCamRelative = camRotationFlattened * inputMove;
        Vector3 strafeClamped = strafeCamRelative.LimitLength(1);

        float depthPosition = _rigidBody3D.GlobalPosition.Y;
        if (depthPosition > OutOfWaterThreshold)
        {
            _rigidBody3D.SetGravityScale(MathUtil.InverseLerp01(0f, 1f, depthPosition) * 3f);
        }
        else if (depthPosition > 0.0)
        {
            _rigidBody3D.SetGravityScale(-1f);
        }
        else
        {
            _rigidBody3D.SetGravityScale(0f);
        }

        float velY = _rigidBody3D.LinearVelocity.Y;
        float velYDelta = velY * (float)delta;
        if (depthPosition > 0.0f && depthPosition + velYDelta < 0.0f)
        {
            _rigidBody3D.ApplyImpulse(Vector3.Up * Mathf.Abs(velYDelta) * 50f);
        }

        // Strafe Movement 3D
        if (strafeClamped.LengthSquared() > 0.1f && depthPosition < OutOfWaterThreshold + 0.3f)
        {
            float engineForce = 8f;
            _rigidBody3D.ApplyForce(strafeClamped * engineForce, -_rigidBody3D.Transform.Basis.X);
            _rigidBody3D.ApplyForce(strafeClamped * engineForce, _rigidBody3D.Transform.Basis.X);
        }

        // Look Angle
        Vector2 lookAxial = new Vector2();
        if (Mathf.Abs(inputLook.X) > 0.1) lookAxial.X = -inputLook.X;
        if (Mathf.Abs(inputLook.Y) > 0.1) lookAxial.Y = inputLook.Y;
        if (depthPosition > OutOfWaterThreshold + 0.1f) lookAxial = Vector2.Zero;

        float twistForce = 0.7f;
        float yawForce = 3f;
        _rigidBody3D.ApplyTorque(_rigidBody3D.Transform.Basis.Z * (-lookAxial.X * twistForce));
        _rigidBody3D.ApplyTorque(Vector3.Up * (lookAxial.X * yawForce));

        // Auto-uprighting
        float pitchAngleMax = 60f;
        Vector3 targetUpAngle = new Quaternion(_rigidBody3D.Transform.Basis.X, Mathf.DegToRad(lookAxial.Y * pitchAngleMax)) * Vector3.Up;

        float springStrength = 5;
        float damperStrength = 2;
        var springTorque = springStrength * _rigidBody3D.Transform.Basis.Y.Cross(targetUpAngle);
        var dampTorque = damperStrength * -_rigidBody3D.AngularVelocity;
        float uprightingModifier = 1.5f;
        _rigidBody3D.ApplyTorque(springTorque * uprightingModifier + dampTorque);
    }
}
