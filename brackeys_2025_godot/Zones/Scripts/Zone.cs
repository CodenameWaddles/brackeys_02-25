using Godot;
using System;
using Godot.Collections;

public partial class Zone : Node3D {
    
    [Export] private Array<Interactable> _interactables;
    [Export] public DataSingle DataSingle { get; private set; }
    [Export] public DataDouble DataDouble { get; private set; }
    
    public void ActivateRandomLeak() {
        var random = new RandomNumberGenerator();
        int index = random.RandiRange(0, _interactables.Count - 1);
        
        while(!(_interactables[index] is LeakablePipe)) {
            index = random.RandiRange(0, _interactables.Count - 1);
        }
        _interactables[index].Activate();
    }
    
    public void SetRandomPipeDisplayData() {
        var random = new RandomNumberGenerator();
        int value = random.RandiRange(DataSingle._minimumValue, DataSingle._maximumValue);
        DataSingle.SetData(0, value);
    }
}