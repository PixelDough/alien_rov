using Godot;
using System;

[GlobalClass]
public partial class DescriptorTrait : CreatureGenResources
{
    [Export] public String Name { get; private set; }

}
