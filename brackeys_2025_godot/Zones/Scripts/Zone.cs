using Godot;
using System;
using System.Linq;
using Brackeys_2025_Godot.Objects.Scripts;
using Godot.Collections;
using Array = System.Array;

public partial class Zone : Node3D
{
    [Export] private String roomname;
    [Export] public InstabilityLevel _instabilityLevel;
    [Export] public Array<Interactable> ZoneHazards { get; private set; }
    [Export] public bool IsTimed { get; private set; }
    [Export] public Timer ZoneTimer { get; private set; }
    [Export] private Area3D DataArea;
    [Export] public DataSingle DisplayDataSingleInRoom { get; private set; }
    [Export] public DataDouble DisplayDataDoubleInRoom { get; private set; }
    [Export] public CartDataPanel.DataMode ZoneDataMode;

    private bool _wentToGetData;
    private BurningPlace _burningPlace;
    private int _cartTrash;

    public enum InstabilityLevel {
        Low,
        Medium,
        High,
        Extreme,
    }

    private float IntegrityPercentage;
    private float InitialIntegrityPercentage;
    private float IntegrityPercentageToComplete;
    private float IntegritySteps;
    private bool dataMatched;
    private Timer EndTimer;

    public bool IsComplete { get; set; }
    public Cart Cart { get; set; }

    public override void _Ready()
    {
        IsComplete = false;
        _wentToGetData = false;

        DataArea.BodyEntered += _onDataAreaBodyEntered;
        
        if (IsTimed)
        {
            EndTimer = new Timer();
            AddChild(EndTimer);
        }
        else
        {
            Cart?.ResetCartTimer();
        }
        
        SetRandomData();
    }
    
    public override void _Process(double delta)
    {
        /*
        if (Input.IsActionJustPressed("test"))
        {
            IsComplete = true; //pr test
        }
        */

        if (IsTimed && Cart._state.Equals(Cart.State.Stopped))
        {
            Cart.UpdateCartTimer((float)ZoneTimer.TimeLeft);
        }
        
        if (!IsComplete) {
            IsComplete = (IntegrityPercentage <= IntegrityPercentageToComplete);
            Cart.StabilityMeter.SetStability(1 - (IntegrityPercentage / 100));
        }
    }

    public void SetupIntegrity()
    {
        if(Cart != null)
        {
            Cart.ParkedSignal += StartTimer;
            _cartTrash = Cart.TrashDeposit.TrashCount;
        }

        Vector2 integrityFrequency = new Vector2();
        switch (_instabilityLevel)
        {
            case InstabilityLevel.Low : integrityFrequency = new Vector2(20, 40);
                break;
            case InstabilityLevel.Medium : integrityFrequency = new Vector2(40, 60);
                break;
            case InstabilityLevel.High : integrityFrequency = new Vector2(60, 80);
                break;
            case InstabilityLevel.Extreme : integrityFrequency = new Vector2(95, 100);
                break;
            default: integrityFrequency = new Vector2(0, 100); 
                break;
        }
        
        RandomNumberGenerator rng = new RandomNumberGenerator();
        InitialIntegrityPercentage = rng.RandfRange(integrityFrequency.X, integrityFrequency.Y);
        IntegrityPercentageToComplete = rng.RandfRange(0, 20);
        
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
        if (!_wentToGetData) return false;
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
        ZoneTimer.Stop();
        EndTimer.Timeout += _cart_leave_without_player;
        EndTimer.Start(10);
        if(Cart._state.Equals(Cart.State.Stopped))
        {
            Cart._alarm.Play();
        }
    }

    private void _cart_leave_without_player()
    {
        EndTimer.Stop();
        if (Cart._playerIsInCart)
        {
            if (Cart._state.Equals(Cart.State.Stopped)) {
                Cart._movePlayerWithCart = true;
                Cart.Start();
            }
            
        }
        else
        {
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
        Cart.TrashDeposit.Reset();
        GameManager.Instance._failedCurrentScene();
    }

    public void _sendRoomMessage()
    {
        Cart.ConsoleScreen.AddMessage("Location : " + roomname + "\nInstability : " + Mathf.RoundToInt(IntegrityPercentage) + "%");
        SamSpeaker.Instance.AddZoneSound(GameManager.Instance._currentSceneIndex%5);
        if (IntegrityPercentage < 40) {
            SamSpeaker.Instance.AddStabilitySound(0);
        }
        else if (IntegrityPercentage < 60) {
            SamSpeaker.Instance.AddStabilitySound(1);
        }
        else if (IntegrityPercentage < 80) {
            SamSpeaker.Instance.AddStabilitySound(2);
        }
        else
        {
            SamSpeaker.Instance.AddStabilitySound(2); // rajouter extreme
        }
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
        if (GameManager.Instance._currentSceneIndex is >= 4 and <= 12)
        {
            if(ZoneHazards.Count > 0) Cart.ConsoleScreen.AddMessage("Issues detected : " + ZoneHazards.Count);
        }
        if (GameManager.Instance.ByePassActivated) IsComplete = true;
        if (!IsTimed) return;
        ZoneTimer.Start();
        //ZoneTimer.Timeout += _on_zone_timer_timeout;
    }
    
    public void DisposeTimer()
    {
        Cart.ParkedSignal -= StartTimer;
        DataArea.BodyEntered -= _onDataAreaBodyEntered;
        if (!IsTimed) return;
        ZoneTimer.Stop();
    }

    public void _onDataAreaBodyEntered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            if (!_wentToGetData)
            {
                _wentToGetData = true;
            } 
        }
    }
   
}