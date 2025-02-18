using Godot;
using System;
using Godot.Collections;

public partial class Cart : Node {
    
    [Export] public Array<DataSingle> DataSingles { get; private set; }
    [Export] public DataDouble DataDouble { get; private set; }
    [Export] public int _speed { get; private set; } = 1;

    [Signal]
    public delegate void ArrivedSignalEventHandler();
    
    private PathFollow3D _pathFollow3D;
    public bool move = true; //temp
    
    public override void _Ready()
    {
        _pathFollow3D = (PathFollow3D) GetParent();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (move && _pathFollow3D.ProgressRatio < 1)
        {
            _pathFollow3D.ProgressRatio += (float) (_speed * 0.1 * delta);
        }
        
        if(_pathFollow3D.ProgressRatio >= 0.9) {
            Godot.GD.Print("arrived");
            EmitSignal(SignalName.ArrivedSignal);
            _pathFollow3D.ProgressRatio = 0;
        }
    }


    public void SetWeakWaveData() {
        var random = new RandomNumberGenerator();
        int a = random.RandiRange(DataDouble._minimumValue, DataDouble._maximumValue/2);
        int b = random.RandiRange(DataDouble._minimumValue, DataDouble._maximumValue/2);
        DataDouble.SetData(0, a);
        DataDouble.SetData(1, b);
    }
    
    public void SetStrongWaveData() {
        var random = new RandomNumberGenerator();
        int a = random.RandiRange(DataDouble._maximumValue/2, DataDouble._maximumValue);
        int b = random.RandiRange(DataDouble._maximumValue/2, DataDouble._maximumValue);
        DataDouble.SetData(0, a);
        DataDouble.SetData(1, b);
    }
}