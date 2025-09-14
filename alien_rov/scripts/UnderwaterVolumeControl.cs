using Godot;
using System;

[Tool]
public partial class UnderwaterVolumeControl : Node
{
    [Export] private WorldEnvironment _worldEnvironment;

    public override void _Ready()
    {
        base._Ready();
        _worldEnvironment.Environment.VolumetricFogDensity = 0f;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        Vector3 camPos = GetViewport().GetCamera3D().Position;
        if (Engine.IsEditorHint())
        {
            camPos = EditorInterface.Singleton.GetEditorViewport3D().GetCamera3D().Position;
        }
        if (camPos.Y >= 0.1)
        {
            _worldEnvironment.Environment.VolumetricFogDensity = 0f;
        }
        else
        {
            _worldEnvironment.Environment.VolumetricFogDensity = 0.035f;
            _worldEnvironment.Environment.VolumetricFogAlbedo = Color.FromString("#2d97ff", Colors.White);
        }
    }
}
