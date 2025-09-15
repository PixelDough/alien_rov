using Godot;
using System;

public partial class RovBehavior : Node
{
    [Export] private RigidBody3D _rigidBody3D;
    [Export] private Camera3D _camera3D;

    public Vector3 inputMove;
    public Vector2 inputLook;

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        Vector3 strafeCamRelative = _camera3D.Quaternion * inputMove;
        Vector3 strafeClamped = strafeCamRelative.LimitLength(1);

        // Strafe Movement 3D
        if (strafeClamped.LengthSquared() > 0.1f)
        {
            float engineForce = 8f;
            _rigidBody3D.ApplyForce(_rigidBody3D.Quaternion * strafeClamped * engineForce, -_rigidBody3D.Transform.Basis.X);
            _rigidBody3D.ApplyForce(_rigidBody3D.Quaternion * strafeClamped * engineForce, _rigidBody3D.Transform.Basis.X);
        }

        Vector3 targetUpAngle = Vector3.Up;

        // Look Angle
        Vector2 lookAxial = new Vector2();
        if (Mathf.Abs(inputLook.X) > 0.1) lookAxial.X = -inputLook.X;
        if (Mathf.Abs(inputLook.Y) > 0.1) lookAxial.Y = inputLook.Y;
        float pitchAngleMax = 30f;
        targetUpAngle = new Quaternion(_rigidBody3D.Transform.Basis.X, lookAxial.Y * pitchAngleMax) * Vector3.Up;

        float twistForce = 0.7f;
        float yawForce = 3f;
        _rigidBody3D.ApplyTorque(_rigidBody3D.Transform.Basis.Z * (-lookAxial.X * twistForce));
        _rigidBody3D.ApplyTorque(Vector3.Up * (lookAxial.X * yawForce));

        // Auto-uprighting
        float springStrength = 5;
        float damperStrength = 2;
        var springTorque = springStrength * _rigidBody3D.Transform.Basis.Y.Cross(targetUpAngle);
        var dampTorque = damperStrength * -_rigidBody3D.AngularVelocity;
        _rigidBody3D.ApplyTorque(springTorque * 0.5f + dampTorque);
    }
}
