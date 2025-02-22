using Godot;
using System;
using System.Collections;
using Godot.Collections;

public partial class GameManager : Node
{
    [Export] private Array<PackedScene> _scenes = new Array<PackedScene>();
    [Export] private Vector3 _scenePosition = new Vector3(0, 0, 0); //remplacer tmtc
    [Export] private Cart Cart;
    [Export] private CharacterBody3D _player;
    [Export] public AudioManager _audioManager;
    [Export] private Timer _resetRoomTimer;
    [Export] private CanvasLayer _roomFailedScreen;
    
    //tunnels
    [Export] private Vector3 _tunelPosition1;
    [Export] private Vector3 _tunelPosition2;
    [Export] private Node3D _tunelPreZone;
    [Export] private Node3D _tunelPostZone;
    [Export] private Node3D _resetPoint;
    [Export] private PackedScene _tunelPrefab;
    
    private int _currentCycle = 0;
    public Zone _currentScene;
    private int _currentSceneIndex = 0;

    public override void _Ready()
    {
        _roomFailedScreen.Visible = false;
        Cart.ArrivedSignal += _cartArrived;
        _player.Reparent(Cart);
        _audioManager.Disable();
        _loadScene(0);
    }

    public override void _Process(double delta)
    {
        Cart.allowedToMove = _currentScene.IsComplete;
    }

    private void _loadScene(int sceneIndex)
    {
        
        //swap tunels and tp cart&player
        _tunelPreZone.QueueFree(); //on suprime le pre tunnel
        Cart.Reparent(_tunelPostZone); //on attache le cart au post tunel
        _tunelPostZone.Position = _tunelPosition1; //on tp le post tunel a la pre position
        _tunelPreZone = _tunelPostZone; //on dit que le pre tunel = le post tunnel
        Cart.Reparent(this); //on ratache le cart au main
        
        Node3D tempTunnel = (Node3D)_tunelPrefab.Instantiate(); //on instancie un nouveau post tunel
        this.AddChild(tempTunnel);
        tempTunnel.Position = _tunelPosition2; //on le place au bon endroit
        _tunelPostZone = tempTunnel; //on dit que le post tunel = ce nouveau tunel

        //load new scene
        if (_currentScene != null)
        {
            _currentScene.QueueFree();
        }
        
        Node3D tempScene = (Node3D) _scenes[sceneIndex].Instantiate();
        AddChild(tempScene);
        tempScene.Position = _scenePosition;
        _currentSceneIndex = sceneIndex;
        
        _currentScene = GetNode<Zone>(tempScene.GetPath());
        _currentScene.Cart = Cart;
        Cart._state = Cart.State.Moving;
        
        Cart.CartDataPanel.SetDataMode(_currentScene.ZoneDataMode);
        
        GD.Print("loaded scene : " + _currentScene.Name + ", index : " + _currentSceneIndex);
    }
    
    private void _cartArrived()
    {
        if (_currentSceneIndex == _scenes.Count - 1)
        {
            _loadScene(0);
            _currentCycle++;
        }
        else
        {
            //_loadScene(_currentSceneIndex + 1);
            CallDeferred("_loadScene", _currentSceneIndex + 1);
        }
    }

    public void _failedCurrentScene()
    {
        _resetRoomTimer.Start();
        _roomFailedScreen.Visible = true;
    }

    public void _on_reset_timer_timeout()
    {

        Cart.GlobalPosition = _resetPoint.Position;
        _player.GlobalPosition = _resetPoint.Position + new Vector3(0, 0.2f, 0);
        _player.Rotation = new Vector3(0, 0, 0);
        _player.Reparent(Cart);
        _currentSceneIndex--;
        Cart._playerIsInCart = true;
        Cart._movePlayerWithCart = true;
        
        Cart.CartDataPanel.DataSingle.Reset();
        Cart.CartDataPanel.DataDouble.Reset();

        playerInteraction playerInteraction = (playerInteraction)_player.FindChild("Head").FindChild("Camera");
        if(playerInteraction.held != null)
        {
            playerInteraction.held.Activate();
            playerInteraction.DropItem();
        }
        
        _roomFailedScreen.Visible = false;

    }
    
    
}
