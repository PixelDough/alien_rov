using Godot;
using System;

[GlobalClass]
public partial class BodyTypeComponent : Node
{
    [Signal] public delegate void BodyTypeChangedEventHandler(BodyType bodyType);

    [Export] public BodyType BodyType;
}
