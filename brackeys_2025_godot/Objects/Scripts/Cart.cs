using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;
using Godot.Collections;

public partial class Cart : Node3D {
    
    public enum State
    {
        Moving,
        Stopped,
        Starting,
        Stopping
    }
    
    [Export] private CollisionShape3D _doorCollider;
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private CharacterBody3D _player;
    [Export] private AnimationTree _lampAnimation;

    [Export] public DataSingle InputDataSingleOnWagon { get; private set; }
    [Export] public DataDouble InputDataDoubleOnWagon { get; private set; }
    [Export] public CartDataPanel CartDataPanel;
    [Export] public PressButton PressButton { get; private set; }
    [Export] public float InitialSpeed { get; private set; } = 10;
    [Export] public float StartStopTime { get; private set; } = 1;
    
    [Signal]
    public delegate void ArrivedSignalEventHandler();
    
    public State _state = State.Stopped;
    public bool allowedToMove = true; //temp
    private float LerpWeight => 0.1f / StartStopTime;
    private float _speed;
    private bool _movePlayerWithCart = true;
    
    public override void _Ready()
    {
        InputDataDoubleOnWagon = CartDataPanel.DataDouble;
        InputDataSingleOnWagon = CartDataPanel.DataSingle;
        _speed = InitialSpeed;
        PressButton.ButtonPressed += Start;
    }

    public override void _PhysicsProcess(double delta)
    {
        // Check if the cart is allowed to move
        if (!allowedToMove)
        {
            PressButton.MakeUninteractable();
        }
        else
        {
            PressButton.MakeInteractable();
        }

        
        // Smoothly start the cart
        if (_state == State.Starting)
        {
            //tant que l'anim de close door est en train de jouer on augmente pas la vitesse
            if (!_animationPlayer.GetCurrentAnimation().Contains("close_door"))
            {
                _speed = Mathf.Lerp(_speed, InitialSpeed, LerpWeight);
                _lampAnimation.Set("parameters/conditions/stopCart", false);
                _lampAnimation.Set("parameters/conditions/startCart", true);
            }
            if (_speed > InitialSpeed - 0.1)
            {
                _speed = InitialSpeed;
                _state = State.Moving;
            }
        }
        // Smoothly stop the cart
        else if (_state == State.Stopping)
        {
            _speed = Mathf.Lerp(_speed, 0, LerpWeight);
            if (_speed < 0.1)
            {
                _speed = 0;
                _state = State.Stopped;
                _doorCollider.Disabled = true;
                if (_movePlayerWithCart)
                {
                    _player.Reparent(GetTree().Root.GetNode("Main"));
                }
                _animationPlayer.Play("open_door");
                
            }
        }
        
        // Move the cart forward along the Z axis
        if (_state != State.Stopped)
        {
            Position = new Vector3(Position.X, Position.Y, Position.Z + (float) (_speed * delta));
        }
        
    }
    
    
    public void Stop() //refaire pr faire vrai smooth bruh
    {
        _state = State.Stopping;
        _lampAnimation.Set("parameters/conditions/stopCart", true);
        _lampAnimation.Set("parameters/conditions/startCart", false);
    }
    
    public void Start() {
        _state = State.Starting;
        _doorCollider.Disabled = false;
        if (_movePlayerWithCart)
        {
            _player.Reparent(this);
        }
        _animationPlayer.Play("close_door");
    }
    
    public void _on_area_entered(Node3D area) {
        if (area.IsInGroup("Parking"))
        {
            Stop();
            allowedToMove = false;
        }   
        else if(area.IsInGroup("End"))
        {
            //puisque chaque end collider ne sert qu'une fois on le queue free ici
            area.QueueFree();
            EmitSignal(SignalName.ArrivedSignal);
        }
    }
    
    /*
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
 */
    
}