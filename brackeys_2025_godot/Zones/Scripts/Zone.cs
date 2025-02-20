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

    private float IntegrityPercentage;
    private float InitialIntegrityPercentage;
    private float IntegrityPercentageToComplete;
    private float IntegritySteps;
    private bool dataMatched;
    
    public bool IsTimed { get; private set; }
    public bool IsComplete { get; private set; }
    public Cart Cart { get; set; }

    public override void _Ready()
    {
        IsComplete = false;
        if (IsTimed)
        {
            ZoneTimer.Start();
        }

        RandomNumberGenerator rng = new RandomNumberGenerator();
        InitialIntegrityPercentage = rng.RandfRange(50, 100);
        IntegrityPercentageToComplete = rng.RandfRange(0, 30);

        IntegritySteps = (InitialIntegrityPercentage - IntegrityPercentageToComplete) / (ZoneHazards.Count + 1); // +1 pour les datas
        IntegrityPercentageToComplete += IntegritySteps / 2; //petit offset pour que ce soit plus lisible
        IntegrityPercentage = InitialIntegrityPercentage;
        
        //print to debug
        GD.Print("Integrity percentage : " + IntegrityPercentage + " / " + IntegrityPercentageToComplete);

    }
    
    public override void _Process(double delta)
    {
        IsComplete = (IntegrityPercentage <= IntegrityPercentageToComplete);
    }

    public void UpdateIntegrityPercentage()
    {
        //set le integrity percentage
        int nbOfHazardSolved = 0;
        foreach (var hazard in ZoneHazards)
        {
            if (hazard.IsSolved) nbOfHazardSolved++;
        }
    
        if (MatchData())
        {
            GD.Print("data matched");
            nbOfHazardSolved += 1;
        }
        
        IntegrityPercentage = InitialIntegrityPercentage - nbOfHazardSolved * IntegritySteps;

        GD.Print("nb of hazard solved : " + nbOfHazardSolved + " / " + (ZoneHazards.Count + 1) +  ", Integrity percentage : " + IntegrityPercentage + " / " + IntegrityPercentageToComplete);
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