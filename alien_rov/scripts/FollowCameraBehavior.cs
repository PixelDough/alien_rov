using Godot;
using System;
using Parallas;

public partial class FollowCameraBehavior : Node
{
    [Export] private Node3D targetNode;
    public override void _Process(double delta)
    {
        base._Process(delta);

        var camPos = GetViewport().GetCamera3D().GlobalPosition;
        targetNode.GlobalPosition = MathUtil.ProjectOnPlane(camPos, Vector3.Up);
    }
}
