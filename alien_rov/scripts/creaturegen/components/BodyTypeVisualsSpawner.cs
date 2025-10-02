using Godot;
using System;

[GlobalClass]
public partial class BodyTypeVisualsSpawner : Node3D
{
    [Export] private BodyTypeComponent _bodyTypeComponent;
    private CreatureBody _creatureBody;
    public CreatureBody CreatureCreatureBody => _creatureBody;

    public override void _Ready()
    {
        base._Ready();
        UpdateCreatureBody(_bodyTypeComponent.BodyType);
        _bodyTypeComponent.BodyTypeChanged += UpdateCreatureBody;
    }

    public void UpdateCreatureBody(BodyType bodyType)
    {
        var newBodyVisuals = bodyType.BodyVisuals.Instantiate<CreatureBody>();
        if (IsInstanceValid(_creatureBody)) _creatureBody.QueueFree();
        AddChild(newBodyVisuals);
        _creatureBody = newBodyVisuals;
    }
}
