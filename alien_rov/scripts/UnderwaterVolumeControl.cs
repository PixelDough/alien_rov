using Godot;
using System;

[Tool]
public partial class UnderwaterVolumeControl : Node
{
    [Export] private WorldEnvironment _worldEnvironment;
    [Export] private DirectionalLight3D _sunLight;
    [Export] private Gradient _fogDepthGradient;
    [Export] private Curve _fogDepthCurve;
    [Export] private Curve _ambientLightCurve;
    [Export] private bool _testInEditor;

    public override void _Ready()
    {
        base._Ready();
        _worldEnvironment.Environment.VolumetricFogDensity = 0f;
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        RenderingServer.GlobalShaderParameterSet("fog_color", _worldEnvironment.Environment.FogLightColor);

        Vector3 camPos = GetViewport().GetCamera3D().GlobalPosition;
        if (Engine.IsEditorHint())
        {
            if (!_testInEditor) return;
            camPos = EditorInterface.Singleton.GetEditorViewport3D().GetCamera3D().Position;
        }
        float belowYPos = Mathf.Min(0f, camPos.Y);

        if (camPos.Y >= 0.1)
        {
            _worldEnvironment.Environment.BackgroundEnergyMultiplier = 1.0f;
            _worldEnvironment.Environment.FogDepthEnd = 400;
            _worldEnvironment.Environment.VolumetricFogDensity = 0f;
            _worldEnvironment.Environment.FogSkyAffect = 0f;
        }
        else
        {
            _worldEnvironment.Environment.BackgroundEnergyMultiplier = _ambientLightCurve?.Sample(belowYPos) ?? 1.0f;
            _worldEnvironment.Environment.FogDepthEnd = 300;
            _worldEnvironment.Environment.VolumetricFogDensity = 0.015f;
            _worldEnvironment.Environment.FogSkyAffect = 1f;
            _worldEnvironment.Environment.FogSunScatter = _worldEnvironment.Environment.BackgroundEnergyMultiplier;
            if (_fogDepthCurve is not null && _fogDepthGradient is not null)
            {
                Color fogColor = _fogDepthGradient.Sample(_fogDepthCurve.Sample(belowYPos));
                _worldEnvironment.Environment.FogLightColor = fogColor;
                _worldEnvironment.Environment.VolumetricFogAlbedo = fogColor;
            }
        }
    }
}
