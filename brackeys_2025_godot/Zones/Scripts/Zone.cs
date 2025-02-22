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
    [Export] public CartDataPanel.DataMode ZoneDataMode;

    private float IntegrityPercentage;
    private float InitialIntegrityPercentage;
    private float IntegrityPercentageToComplete;
    private float IntegritySteps;
    private bool dataMatched;
    private Timer EndTimer;
    
    public bool IsTimed { get; private set; }
    public bool IsComplete { get; private set; }
    public Cart Cart { get; set; }

    public override void _Ready()
    {
        IsComplete = false;
        if (Cart.CartDataPanel._dataMode != ZoneDataMode)
        {
            Cart.CartDataPanel.SwitchDataMode();
        }
        
        if (IsTimed)
        {
            ZoneTimer.Start();
            EndTimer = new Timer();
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
            Cart.CartDataPanel.TurnOnGreenLight();
            nbOfHazardSolved += 1;
        }
        else {
            Cart.CartDataPanel.TurnOffGreenLight();
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

    public void _on_zone_timer_timeout()
    {
        EndTimer.Timeout += _cart_leave_without_player;
        EndTimer.Start(10);
    }

    private void _cart_leave_without_player()
    {
        if (Cart._playerIsInCart)
        {
            Cart._movePlayerWithCart = true;
            Cart.Start();
        }
        else
        {
            Cart._movePlayerWithCart = false;
            Cart.Start();
            
            EndTimer = new Timer();
            EndTimer.Timeout += _restart_zone;
            EndTimer.Start(10);
        }
        
        
    }

    private void _restart_zone()
    {
        GameManager gm = (GameManager)GetTree().Root.GetChild(0);
        gm._failedCurrentScene();
    }
    
    
}