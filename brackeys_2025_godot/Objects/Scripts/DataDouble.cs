using Godot;
using System;
using Godot.Collections;

public partial class DataDouble : DataDisplay {
    
    [Export] private int _defaultValue = 2;
    [Export] private Node3D _screen;
    private ShaderMaterial _material;
    private MeshInstance3D _mesh;
    
    public override void _Ready() {
        Data = new Array<int>();
        Data.Add(_defaultValue);
        Data.Add(_defaultValue);

        _mesh = (MeshInstance3D)_screen.GetChild(0);
    }
    
    public override void _Process(double delta) {
        //GD.Print(_amplitudeTable[Data[0]]);
        _mesh.SetInstanceShaderParameter("amplitude", _amplitudeTable[Data[0]]);
        _mesh.SetInstanceShaderParameter("period", _periodTable[Data[1]]);
    }

    public void SetRandomData(int exclude = -1) {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        Data[0] = rng.RandiRange(_minimumValue, _maximumValue);
        while (Data[0] == exclude)
        {
            Data[0] = rng.RandiRange(_minimumValue, _maximumValue);
        }
        Data[1] = rng.RandiRange(_minimumValue, _maximumValue); //que pour data 0
    }
    
    public void Reset()
    {
        Data[0] = _defaultValue;
        Data[1] = _defaultValue;
    }
}