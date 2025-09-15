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

        Quaternion camRotationFlattened = new Quaternion(Vector3.Up, _rigidBody3D.GlobalRotation.Y - Mathf.Pi);
        Vector3 strafeCamRelative = camRotationFlattened * inputMove;
        Vector3 strafeClamped = strafeCamRelative.LimitLength(1);

        // Strafe Movement 3D
        if (strafeClamped.LengthSquared() > 0.1f)
        {
            float engineForce = 8f;
            _rigidBody3D.ApplyForce(strafeClamped * engineForce, -_rigidBody3D.Transform.Basis.X);
            _rigidBody3D.ApplyForce(strafeClamped * engineForce, _rigidBody3D.Transform.Basis.X);
        }

        // Look Angle
        Vector2 lookAxial = new Vector2();
        if (Mathf.Abs(inputLook.X) > 0.1) lookAxial.X = -inputLook.X;
        if (Mathf.Abs(inputLook.Y) > 0.1) lookAxial.Y = inputLook.Y;

        float twistForce = 0.7f;
        float yawForce = 3f;
        _rigidBody3D.ApplyTorque(_rigidBody3D.Transform.Basis.Z * (-lookAxial.X * twistForce));
        _rigidBody3D.ApplyTorque(Vector3.Up * (lookAxial.X * yawForce));

        // Auto-uprighting
        Vector3 targetUpAngle = Vector3.Up;
        float pitchAngleMax = 30f;
        targetUpAngle = new Quaternion(_rigidBody3D.Transform.Basis.X, lookAxial.Y * pitchAngleMax) * Vector3.Up;

        float springStrength = 5;
        float damperStrength = 2;
        var springTorque = springStrength * _rigidBody3D.Transform.Basis.Y.Cross(targetUpAngle);
        var dampTorque = damperStrength * -_rigidBody3D.AngularVelocity;
        float uprightingModifier = 1f;
        if (Mathf.Abs(inputLook.Y) > 0.1) uprightingModifier = 0.5f;
        _rigidBody3D.ApplyTorque(springTorque * uprightingModifier + dampTorque);
    }
}
