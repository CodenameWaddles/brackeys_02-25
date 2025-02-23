using Godot;
using System;
using System.Linq;
using Brackeys_2025_Godot.Objects.Scripts;
using Godot.Collections;
using Array = System.Array;

public partial class Zone : Node3D
{
    [Export] private String roomname;
    [Export] public Array<Interactable> ZoneHazards { get; private set; }
    [Export] public Timer ZoneTimer { get; private set; }
    [Export] public bool IsTimed { get; private set; }
    [Export] public DataSingle DisplayDataSingleInRoom { get; private set; }
    [Export] public DataDouble DisplayDataDoubleInRoom { get; private set; }
    [Export] public CartDataPanel.DataMode ZoneDataMode;

    private BurningPlace _burningPlace;

    private int _cartTrash;
    
    private float IntegrityPercentage;
    private float InitialIntegrityPercentage;
    private float IntegrityPercentageToComplete;
    private float IntegritySteps;
    private bool dataMatched;
    private Timer EndTimer;
    
    public bool IsComplete { get; private set; }
    public Cart Cart { get; set; }

    public override void _Ready()
    {
        IsComplete = false;
        
        
        if (IsTimed)
        {
            EndTimer = new Timer();
            AddChild(EndTimer);
            ZoneTimer = (Timer) FindChild("Timer");
        }
        else
        {
            Cart?.ResetCartTimer();
        }
        
        SetRandomData();

    }
    
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("test"))
        {
            IsComplete = true; //pr test
        }
        
        //IsComplete = (IntegrityPercentage <= IntegrityPercentageToComplete);
        Cart.StabilityMeter.SetStability(1 - (IntegrityPercentage / 100));
        if (IsTimed)
        {
            Cart.UpdateCartTimer((float)ZoneTimer.TimeLeft);
            if (IsComplete && !ZoneTimer.IsStopped())
            {
                ZoneTimer.Stop();
            }
        }
    }

    public void SetupIntegrity()
    {
        if(Cart != null)
        {
            Cart.ParkedSignal += StartTimer;
            _cartTrash = Cart.TrashDeposit.TrashCount;
        }
        RandomNumberGenerator rng = new RandomNumberGenerator();
        InitialIntegrityPercentage = rng.RandfRange(40, 70);
        IntegrityPercentageToComplete = rng.RandfRange(0, 30);
        
        IntegritySteps = (InitialIntegrityPercentage - IntegrityPercentageToComplete) / (ZoneHazards.Count + 1 + _cartTrash); // +1 pour les datas
        IntegrityPercentageToComplete += IntegritySteps / 2; //petit offset pour que ce soit plus lisible
        IntegrityPercentage = InitialIntegrityPercentage;
    }

    public void UpdateIntegrityPercentage()
    {
        //set le integrity percentage
        int nbOfHazardSolved = 0;
        if(_burningPlace != null)
        {
            nbOfHazardSolved+= _burningPlace.TrashCount;
        }
        foreach (var item in ZoneHazards)
        {
            if (item is Hazard)
            {
                Hazard hazard = (Hazard)item;
                if (hazard.IsSolved) nbOfHazardSolved++;
            }
            else if (item is Trashbag)
            {
                Trashbag trashbag = (Trashbag)item;
                if (trashbag.IsSolved) nbOfHazardSolved++;
            }
        }

        //GD.Print("nb of hazard solved (excluding data) : " + nbOfHazardSolved + "/" + ZoneHazards.Count() + " + " + _cartTrash + " trash");
    
        if (MatchData())
        {
            Cart.CartDataPanel.TurnOnGreenLight();
            nbOfHazardSolved += 1;
        }
        else {
            Cart.CartDataPanel.TurnOffGreenLight();
        }
        
        IntegrityPercentage = InitialIntegrityPercentage - nbOfHazardSolved * IntegritySteps;
        Cart.StabilityMeter.SetStability(1 - (IntegrityPercentage / 100));
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
        if (!IsTimed)
        {
            ZoneTimer.Stop();
            return;
        }
        
        ZoneTimer.Stop();
        EndTimer.Timeout += _cart_leave_without_player;
        EndTimer.Start(10);
        Cart._alarm.Play();
        GD.Print("playing alarm");
    }

    private void _cart_leave_without_player()
    {
        EndTimer.Stop();
        if (Cart._playerIsInCart)
        {
            Cart._movePlayerWithCart = true;
            Cart.Start();
            GD.Print("player in cart, continuing");
            
        }
        else
        {
            GD.Print("player not in cart, restarting scene in 5 sec");
            Cart._movePlayerWithCart = false;
            Cart.Start();
            
            EndTimer.Timeout -= _cart_leave_without_player;
            EndTimer.QueueFree();
            EndTimer = new Timer();
            AddChild(EndTimer);
            EndTimer.Timeout += _restart_zone;
            EndTimer.Start(7);
        }
        
        
    }

    private void _restart_zone()
    {
        EndTimer.Stop();
        GameManager gm = (GameManager)GetTree().Root.GetChild(0);
        gm._failedCurrentScene();
    }

    public void _sendRoomMessage()
    {
        Cart.ConsoleScreen.AddMessage("Location : " + roomname + "\nInstability : " + Mathf.RoundToInt(IntegrityPercentage) + "%");
    }
    
    public void setBurningPlace(BurningPlace burningPlace)
    {
        _burningPlace = burningPlace;
    }

    private void SetRandomData() {
        switch (ZoneDataMode) {
            case CartDataPanel.DataMode.Dual:
                DisplayDataDoubleInRoom.SetRandomData(); //passer en arg la valeur du cart (marche pas bruh)
                break;
            case CartDataPanel.DataMode.Single:
                DisplayDataSingleInRoom.SetRandomData();
                break;
        }
    }

    private void StartTimer()
    {
        if (!IsTimed) return;
        GD.Print("zone timer : " + ZoneTimer);
        ZoneTimer.Start(ZoneTimer.WaitTime);
    }
   
}