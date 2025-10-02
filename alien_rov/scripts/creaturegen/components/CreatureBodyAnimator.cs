using Godot;
using System;

[GlobalClass]
public partial class CreatureBodyAnimator : Node
{
    [Export] public BodyTypeVisualsSpawner VisualsSpawner;

    public override void _Process(double delta)
    {
        base._Process(delta);

        // VisualsSpawner.CreatureCreatureBody.AnimationPlayer.Play("jfkldsajfdksla");
    }
}
