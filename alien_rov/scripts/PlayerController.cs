using Godot;
using System;

public partial class PlayerController : Node
{
    [Export] private RovBehavior _rovBehavior;

    private Vector3 _inputMove;
    private Vector2 _inputLook;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionPressed("move_forward"))
            _inputMove += Vector3.Forward;
        if (Input.IsActionPressed("move_backward"))
            _inputMove += Vector3.Back;
        if (Input.IsActionPressed("move_left"))
            _inputMove += Vector3.Left;
        if (Input.IsActionPressed("move_right"))
            _inputMove += Vector3.Right;
        if (Input.IsActionPressed("move_up"))
            _inputMove += Vector3.Up;
        if (Input.IsActionPressed("move_down"))
            _inputMove += Vector3.Down;
        if (Input.IsActionPressed("look_right"))
            _inputLook += Vector2.Right;
        if (Input.IsActionPressed("look_left"))
            _inputLook += Vector2.Left;
        if (Input.IsActionPressed("look_up"))
            _inputLook += Vector2.Up;
        if (Input.IsActionPressed("look_down"))
            _inputLook += Vector2.Down;

        _rovBehavior.inputMove = _inputMove;
        _rovBehavior.inputLook = _inputLook;

        _inputMove = Vector3.Zero;
        _inputLook = Vector2.Zero;
    }
}
