using Godot;
using System;

public partial class FlashlightToggle : Node
{
    [Export] private Light3D _light;

    public override void _Process(double delta)
    {
        base._Process(delta);

        if (Input.IsActionJustPressed("toggle_light"))
        {
            _light?.SetVisible(!_light.Visible);
        }
    }
}
