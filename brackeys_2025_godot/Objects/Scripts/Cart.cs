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
    
    //audio
    [Export] private AudioStreamPlayer3D _wheels;
    [Export] private AudioStreamPlayer3D _brakes;
    [Export] private AudioStreamPlayer3D _doorClosing;
    [Export] private AudioStreamPlayer3D _doorOpening;
    [Export] private AudioStreamPlayer3D _bell;
    [Export] public AudioStreamPlayer3D _alarm;
    [Export] private AudioStreamPlayer3D _music;

    

    [Export] public CartDataPanel CartDataPanel;
    [Export] public PressButton PressButton { get; private set; }
    [Export] public float InitialSpeed { get; private set; } = 10;
    [Export] public float StartStopTime { get; private set; } = 1;
    
    [Signal]
    public delegate void ArrivedSignalEventHandler();
    
    public DataSingle InputDataSingleOnWagon { get; private set; }
    public DataDouble InputDataDoubleOnWagon { get; private set; }
    
    public State _state = State.Stopped;
    public bool allowedToMove = true; //temp
    private float LerpWeight => 0.1f / StartStopTime;
    private float _speed;
    public bool _movePlayerWithCart = true;
    public bool _playerIsInCart;
    private float _musicTime = 0.0f;
    
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
            PressButton.MakeUninteractable();
            _wheels.VolumeDb = Mathf.Lerp(_wheels.VolumeDb, -30, LerpWeight);
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
            _wheels.VolumeDb = Mathf.Lerp(_wheels.VolumeDb, -50, LerpWeight);
            if (_speed < 0.1)
            {
                _speed = 0;
                _state = State.Stopped;
                _doorCollider.Disabled = true;
                if (_movePlayerWithCart)
                {
                    _player.Reparent(GetTree().Root.GetNode("Main"));
                }
                
                _musicTime = _music.GetPlaybackPosition();
                _music.Stop();
                _wheels.Stop();
                _animationPlayer.Play("open_door");
                _bell.Play();
                _doorOpening.Play();
                GameManager main = (GameManager)GetTree().Root.GetChild(0);
                main._audioManager.Enable();
            }
        }
        
        //play music if moving
        if (_state == State.Moving)
        {
            PressButton.MakeUninteractable();   
            if (!_music.IsPlaying())
            {
                _music.Play(_musicTime);
            }
        }
        
        // Move the cart forward along the Z axis
        if (_state != State.Stopped)
        {
            Position = new Vector3(Position.X, Position.Y, Position.Z + (float) (_speed * delta));
        }

        if (_state != State.Stopped && _state != State.Stopping)
        {
            if (!_wheels.IsPlaying())
            {
                _wheels.Play();
            }
        }
        
    }
    
    
    public void Stop()
    {
        _brakes.Play();
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
        _doorClosing.Play();
        GameManager main = (GameManager)GetTree().Root.GetChild(0);
        main._audioManager.Disable();
    }
    
    public void _on_area_entered(Node3D area) {
        if (area.IsInGroup("Parking"))
        {
            Stop();
            allowedToMove = false;
        }   
        else if(area.IsInGroup("End"))
        {
            //si y'a pas le joueur il continue a l'infini jusqu'au restart de la zone
            if (!_playerIsInCart) return;
            //puisque chaque end collider ne sert qu'une fois on le queue free ici
            area.QueueFree();
            EmitSignal(SignalName.ArrivedSignal);
        }
    }

    public void _on_player_detection_body_entered(Node3D body)
    {
        if (body.IsInGroup("Player"))
        {
            _playerIsInCart = true;
        }
    }
    public void _on_player_detection_body_exited(Node body)
    {
        if (body.IsInGroup("Player"))
        {
            _playerIsInCart = false;
        }
    }
    
}