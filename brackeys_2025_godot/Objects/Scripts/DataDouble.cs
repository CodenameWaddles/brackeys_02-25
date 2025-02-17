using Godot;
using System;
using Godot.Collections;

public partial class DataDouble : DataDisplay {
    
    [Export] private int _defaultValue = 2;
    [Export] private Node3D _screen;
    private ShaderMaterial _material;
    
    public override void _Ready() {
        Data = new Array<int>();
        Data.Add(_defaultValue);
        Data.Add(_defaultValue);

        MeshInstance3D mesh = (MeshInstance3D)_screen.GetChild(0);
        
        _material = (ShaderMaterial)mesh.GetSurfaceOverrideMaterial(0);
    }
    
    public override void _Process(double delta) {
        _material.SetShaderParameter("amplitude", _amplitudeTable[Data[0]]);
        _material.SetShaderParameter("period", _periodTable[Data[1]]);
    }
}