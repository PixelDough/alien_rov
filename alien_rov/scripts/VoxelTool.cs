using System;
using Godot;

namespace alienrov.scripts;

public partial class VoxelTool
{
    // https://voxel-tools.readthedocs.io/en/latest/api/VoxelTool/
    private GodotObject _voxelTool;

    public enum Mode
    {
        Add, Remove, Set, TexturePaint
    }

    public enum ChannelId
    {
        Type,
        Sdf,
        Color,
        Indices,
        Weights,
        Data5,
        Data6,
        Data7,
        MAX_CHANNELS,
    }

    public void GetVoxelTool(Node voxelTerrain)
    {
        _voxelTool = (GodotObject)voxelTerrain.Call("get_voxel_tool");
    }

    public bool IsAreaEditable(Aabb area)
    {
        return (bool)_voxelTool.Call("is_area_editable", area);
    }

    public void SetChannel(ChannelId channel)
    {
        _voxelTool.Set("channel", (int)channel);
    }

    public void SetMode(Mode mode)
    {
        _voxelTool.Set("mode", (int)mode);
    }

    public void DoBox(Vector3I begin, Vector3I end)
    {
        _voxelTool.Call("do_box", begin, end);
    }

    public void DoSphere(Vector3I center, int radius)
    {
        _voxelTool.Call("do_sphere", center, radius);
    }

    public void DoPoint(Vector3I pos)
    {
        _voxelTool.Call("do_point", pos);
    }
}
