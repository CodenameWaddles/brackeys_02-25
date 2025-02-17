using Godot;
using System;
using Godot.Collections;

public partial class DataSingle : DataDisplay {
    
    [Export] private int _defaultValue;
    [Export] Label3D _label;
    
    public override void _Ready() {
        Data = new Array<int>();
        Data.Add(_defaultValue);
    }
    
    public override void _Process(double delta) {
        _label.Text = Data[0].ToString();
    }
}