using Godot;
using System;
using Godot.Collections;

public partial class Zone : Node3D
{
    
    [Export] private Array<Interactable> _interactables;
    [Export] public DataSingle DataSingle { get; private set; }
    [Export] public DataDouble DataDouble { get; private set; }
    
    public bool IsComplete { get; private set; }
    public Cart Cart { get; set; }

    public override void _Ready()
    {
        IsComplete = false;
    }

    public override void _Process(double delta)
    {
        IsComplete = MatchData();
    }

    public bool MatchData()
    {
        if (DataSingle != null && DataDouble != null)
        {
            //y'a les deux
            if (DataSingle.isEqual(Cart.DataSingle) && DataDouble.isEqual(Cart.DataDouble)) return true;
            
        }
        else if(DataSingle != null)
        {
            //y'a que dadasingle
            if (DataSingle.isEqual(Cart.DataSingle)) 
                return true;
        }
        else if(DataDouble != null)
        {
            //y'a que datadouble
            if (DataDouble.isEqual(Cart.DataDouble)) return true;
        }
        else
        {
            //tout est null
            return true; //salle complete
        }

        return false;
    }
    
    
    /*
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
    */
}