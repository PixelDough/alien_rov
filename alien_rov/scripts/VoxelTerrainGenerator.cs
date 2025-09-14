using Godot;
using System;
using alienrov.scripts;

public partial class VoxelTerrainGenerator : Node
{
    [Export] private Node _terrainNode;

    private readonly VoxelTool _voxelTool = new VoxelTool();

    public override void _Ready()
    {
        base._Ready();

        _voxelTool.GetVoxelTool(_terrainNode);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        _voxelTool.SetChannel(VoxelTool.ChannelId.Sdf);
        if (!_voxelTool.IsAreaEditable(new Aabb(Vector3.Zero, Vector3.One * 10)))
        {
            GD.Print("Area is not editable!");
            return;
        }
        _voxelTool.SetMode(VoxelTool.Mode.Add);
        _voxelTool.DoSphere(Vector3I.Zero, 2);
        _voxelTool.DoBox(Vector3I.One * 6, Vector3I.One * 8);

        // GodotObject model = (GodotObject)Godot.ClassDB.Instantiate("VoxelBlockyModelCube");
        // model.Call("set_tile", Godot.ClassDB.ClassGetIntegerConstant("VoxelBlockyModel", "SIDE_NEGATIVE_X"),
        //     new Vector2I(1, 1));
        // model.Set("atlas_size_in_tiles", new Vector2I(8, 8));
    }


}
