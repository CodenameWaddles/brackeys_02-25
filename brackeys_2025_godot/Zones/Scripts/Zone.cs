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
            nbOfHazardSolved += 1;
        }
        
        IntegrityPercentage = InitialIntegrityPercentage - nbOfHazardSolved * IntegritySteps;
    }

    public bool MatchData()
    {
        switch(Cart.CartDataPanel._dataMode)
        {
            case CartDataPanel.DataMode.Dual:
                return DisplayDataDoubleInRoom.isEqual(Cart.InputDataDoubleOnWagon);
            case CartDataPanel.DataMode.Single:
                return DisplayDataSingleInRoom.isEqual(Cart.InputDataSingleOnWagon);
            default:
                return true;
        }
    }
    
    

}