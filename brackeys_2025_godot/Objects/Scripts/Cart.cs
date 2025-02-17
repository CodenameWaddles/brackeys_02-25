using Godot;
using System;
using Godot.Collections;

public partial class Cart : Node {
    
    [Export] public Array<DataSingle> DataSingles { get; private set; }
    [Export] public DataDouble DataDouble { get; private set; }
    
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