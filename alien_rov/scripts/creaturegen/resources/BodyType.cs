using Godot;
using System;
using Godot.Collections;

[GlobalClass]
public partial class BodyType : CreatureGenResources
{
    public enum ClassificationTerms
    {
        kala, // fish
        akesi, // herptile (reptile/amphibian)
        ijo // "thing"
    }

    [Export] public PackedScene BodyVisuals;
    [Export] public ClassificationTerms ClassificationTerm = ClassificationTerms.kala;
    [Export] public DescriptorTrait DescriptorTrait;

    [ExportGroup("Custom Name")]
    [Export(PropertyHint.GroupEnable)] public bool CustomNameEnabled = false;
    [Export] public String CustomName = "";
}
