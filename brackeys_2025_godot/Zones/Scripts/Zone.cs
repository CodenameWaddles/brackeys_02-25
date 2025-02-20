using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;
using Godot.Collections;
using Array = System.Array;

public partial class Zone : Node3D
{
    [Export] public Array<Hazard> ZoneHazards { get; private set; }
    [Export] public Timer ZoneTimer { get; private set; }
    [Export] public DataSingle DisplayDataSingleInRoom { get; private set; }
    [Export] public DataDouble DisplayDataDoubleInRoom { get; private set; }

    private float Integrity;
    public int IntegrityPercentageToComplete { get; private set; }
    private float IntegritySteps;
    
    public bool IsTimed { get; private set; }
    public bool IsComplete { get; private set; }
    public Cart Cart { get; set; }

    public override void _Ready()
    {
        IsComplete = false;
        Integrity = 100;
        if (IsTimed)
        {
            ZoneTimer.Start();
        }
    }

    public override void _Process(double delta)
    {
        IsComplete = MatchData();
    }

    public bool MatchData()
    {
        if (DisplayDataSingleInRoom != null && DisplayDataDoubleInRoom != null)
        {
            //y'a les deux
            if (DisplayDataSingleInRoom.isEqual(Cart.InputDataSingleOnWagon) && DisplayDataDoubleInRoom.isEqual(Cart.InputDataDoubleOnWagon)) return true;
            
        }
        else if(DisplayDataSingleInRoom != null)
        {
            //y'a que dadasingle
            if (DisplayDataSingleInRoom.isEqual(Cart.InputDataSingleOnWagon)) 
                return true;
        }
        else if(DisplayDataDoubleInRoom != null)
        {
            //y'a que datadouble
            if (DisplayDataDoubleInRoom.isEqual(Cart.InputDataDoubleOnWagon)) return true;
        }
        else
        {
            //tout est null
            return true; //salle complete
        }

        return false;
    }
    
    

}