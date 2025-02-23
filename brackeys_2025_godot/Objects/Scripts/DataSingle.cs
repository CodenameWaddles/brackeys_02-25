using Godot;
using System;
using Godot.Collections;

public partial class DataSingle : DataDisplay {
    
    [Export] private int _defaultValue;
    [Export] private MeshInstance3D _screen;
    
    public override void _Ready() {
        Data = new Array<int>();
        Data.Add(_defaultValue);
    }
    
    public override void _Process(double delta) {
        _screen.SetInstanceShaderParameter("data", Data[0]);
    }
    
    public void SetRandomData(int exclude) {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        Data[0] = rng.RandiRange(_minimumValue, _maximumValue);
        while (Data[0] == exclude)
        {
            Data[0] = rng.RandiRange(_minimumValue, _maximumValue);
        }

        GD.Print("excluding : " + exclude);
        GD.Print("Data[0] : " + Data[0]);
    }

    public void Reset()
    {
        Data[0] = _defaultValue;
    }
}