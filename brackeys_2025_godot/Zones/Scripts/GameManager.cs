using Godot;
using System;
using System.Collections;
using System.Linq;
using Godot.Collections;

public partial class GameManager : Node
{
    private enum GameState
    {
        Intro,
        Game,
        End
    }
    
    [Export] private int startIndex = 0;
    
    [Export] private Array<PackedScene> _scenes = new Array<PackedScene>();
    [Export] private Vector3 _scenePosition = new Vector3(0, 0, 0); //remplacer tmtc
    [Export] private PackedScene _introScenePrefab;
    [Export] private PackedScene _finalScene;
    [Export] public Cart Cart;
    [Export] public CharacterBody3D _player { get; private set; }
    [Export] public AudioManager _audioManager;
    [Export] private Timer _resetRoomTimer;
    [Export] private CanvasLayer _roomFailedScreen;
    [Export] private EndScreen _endScreen;
    
    //tunnels
    [Export] private Vector3 _tunelPosition1;
    [Export] private Vector3 _tunelPosition2;
    [Export] private Node3D _tunelPreZone;
    [Export] private Node3D _tunelPostZone;
    [Export] private Node3D _resetPoint;
    [Export] private PackedScene _tunelPrefab;

    private Node3D _introScene;
    public bool ByePassActivated;
    private int _currentCycle = 0;
    public Zone _currentScene;
    public int _currentSceneIndex = 0;
    private Dictionary<int, String> _messages;
    private GameState _gameState = GameState.Game;
    
    // singleton
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    public override void _Ready() {
        _instance = this;
        _messages = new Dictionary<int, string>();
        SetupMessages();
        _currentSceneIndex = startIndex;
        
        _roomFailedScreen.Visible = false;
        _endScreen.Visible = false;
        Cart.ArrivedSignal += _cartArrived;
        _player.Reparent(Cart);
        _audioManager.Disable();

        if (startIndex > 0)
        {
            _loadScene(startIndex);
        }
        else
        {
            LoadIntroScene();
        }

        ByePassActivated = false;
        
        if (startIndex >= 6) //testing purposes. startIndex should be 0
        {
            Cart.OpenTools();
            //Cart.NextMusic();
            LightManager.Instance.NextFrequency();
            LightManager.Instance.NextFrequency();
            AudioManager.Instance.NextBangingFrequency();
            AudioManager.Instance._alarm.Play();
        }
    }

    public override void _Process(double delta)
    {
        switch (_gameState) {
            case GameState.Intro:
                break;
            case GameState.Game:
                if(_currentScene != null)
                    Cart.allowedToMove = _currentScene.IsComplete;
                break;
            case GameState.End:
                break;
        }

        if (Input.IsActionJustPressed("exit")) {
            GD.Print("exit");
            GetTree().Root.PropagateNotification((int)NotificationWMCloseRequest);
            GetTree().Quit();
        }
    }

    private void _loadScene(int sceneIndex)
    {
        if (_currentSceneIndex == _scenes.Count - 1) { //final scene
            GD.Print("final scene");
            _tunelPrefab = _finalScene;
            _tunelPosition2 = new Vector3(0, 0, 0);
            
        }
        //swap tunels and tp cart&player
        _tunelPreZone.QueueFree(); //on suprime le pre tunnel
        Cart.Reparent(_tunelPostZone); //on attache le cart au post tunel
        _tunelPostZone.Position = _tunelPosition1; //on tp le post tunel a la pre position
        _tunelPreZone = _tunelPostZone; //on dit que le pre tunel = le post tunnel
        Cart.Reparent(this); //on ratache le cart au main

        Node3D tempTunnel = (Node3D)_tunelPrefab.Instantiate(); //on instancie un nouveau post tunel
        AddChild(tempTunnel);
        tempTunnel.Position = _tunelPosition2; //on le place au bon endroit
        _tunelPostZone = tempTunnel; //on dit que le post tunel = ce nouveau tunel

        //load new scene
        if (_currentScene != null) {
            _currentScene.DisposeTimer();
            _currentScene.QueueFree();
        }

        Node3D tempScene = (Node3D)_scenes[sceneIndex].Instantiate();
        AddChild(tempScene);
        tempScene.Position = _scenePosition;
        _currentSceneIndex = sceneIndex;
        _currentScene = GetNode<Zone>(tempScene.GetPath());
        _currentScene.Cart = Cart;
        _currentScene.SetupIntegrity();

        if (_currentSceneIndex < 15) //cut après que porte ouverte découverte
        {
            _currentScene._sendRoomMessage();
        }
        switch (_currentSceneIndex)
        {
            case 0:
                if (_introScene != null)
                {
                    _introScene.QueueFree();
                }
                break;
            case 2:
                SendMessage(_currentSceneIndex); //trash
                LightManager.Instance.NextFrequency();
                break;
            case 3:
                SendMessage(_currentSceneIndex); //burn
                break;
            case 4:
                SendMessage(_currentSceneIndex); //tools
                Cart.OpenTools();
                break;
            case 6:
                SendMessage(_currentSceneIndex); //timer
                Cart.NextMusic();
                LightManager.Instance.NextFrequency();
                AudioManager.Instance.NextBangingFrequency();
                break;
            case 7:
                break;
            case 8:
                break;
            case 9:
                SendMessage(_currentSceneIndex); //door
                LightManager.Instance.NextFrequency();
                break;
            case 10:
                _audioManager._alarm.Play();
                SendMessage(_currentSceneIndex); //breach detected
                break;
            case 11:
                AudioManager.Instance.NextBangingFrequency();
                break;
            case 12:
                LightManager.Instance.NextFrequency();
                break;
            case 13:
                break;
            case 14:
                SendMessage(_currentSceneIndex);
                break;
        }

        Cart._state = Cart.State.Moving;

        Cart.CartDataPanel.SetDataMode(_currentScene.ZoneDataMode);
        Cart.CartMap.DeactivateMapLights();
        Cart.CartMap.ActivateMapLight(_currentSceneIndex % 5);

        GD.Print("loaded scene : " + _currentScene.Name + ", index : " + _currentSceneIndex);
    }
    
    private void _cartArrived()
    {
        if (_currentSceneIndex == _scenes.Count - 1) //final scene
        {
            //_loadScene(0);
            //GD.Print("final scene");
            //CallDeferred("LoadFinalScene");
            //_currentCycle++;
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
        _player.GlobalPosition = _resetPoint.Position + new Vector3(0, 0.1f, 0);
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

    public void SendMessage(int index)
    {
        Cart.ConsoleScreen.AddMessage(_messages[index]);
    }

    private void SetupMessages()
    {
        _messages.Add(-1, "Big tests coming up today. Make sure to input every data sample from the facility's screens in the cart's terminal.");
        _messages.Add(2, "Lot of trash today. There should be enough room for it under the cart's shelf.");
        _messages.Add(3, "Burn everything properly.");
        _messages.Add(4, "Issues detected. Use tools to maintain structural integrity.");
        _messages.Add(6, "Tests are going fast. Cart will leave automatically at the end of the timer.");
        _messages.Add(9, "That door needs to be fixed. Use any means necessary.");
        _messages.Add(10, "Breach detected. Relief Team on their way.");// alarme
        _messages.Add(12, "Relief team nearly there. Bypassing protocol activated."); // salle poubelle
        _messages.Add(14,"Contact lost with the relief team. Get out."); //salle porte ouverte
        
    }
    
    public void BreakBlowtorchEvent()
    {
        Cart.ConsoleScreen.AddMessage("If your blowtorch breaks, tape will do fine. I think.");
    }
    
    private void LoadFinalScene()
    {
        _tunelPreZone.QueueFree(); //on suprime le pre tunnel
        Cart.Reparent(_tunelPostZone); //on attache le cart au post tunel
        _tunelPostZone.Position = _tunelPosition1; //on tp le post tunel a la pre position
        _tunelPreZone = _tunelPostZone; //on dit que le pre tunel = le post tunnel
        Cart.Reparent(this); //on ratache le cart au main
        
        if (_currentScene != null) {
            _currentScene.DisposeTimer();
            _currentScene.QueueFree();
        }
        
        var finalScene = _finalScene.Instantiate();
        AddChild(finalScene);
        Cart._state = Cart.State.Moving;
        
        Cart.DeactivateColliders();
    }
    
    private void LoadIntroScene()
    {
        _tunelPreZone.QueueFree(); //on suprime le pre tunnel
        Cart.Reparent(_tunelPostZone); //on attache le cart au post tunel
        _tunelPostZone.Position = _tunelPosition1; //on tp le post tunel a la pre position
        _tunelPreZone = _tunelPostZone; //on dit que le pre tunel = le post tunnel
        Cart.Reparent(this); //on ratache le cart au main
        
        Node3D tempTunnel = (Node3D)_tunelPrefab.Instantiate(); //on instancie un nouveau post tunel
        AddChild(tempTunnel);
        tempTunnel.Position = _tunelPosition2; //on le place au bon endroit
        _tunelPostZone = tempTunnel; //on dit que le post tunel = ce nouveau tunel
        
        Node3D introScene = (Node3D) _introScenePrefab.Instantiate();
        AddChild(introScene);
        _introScene = introScene;
        Cart._state = Cart.State.Moving;

        _currentSceneIndex = -1;
    }

    public void EndGame() {
        _endScreen.Visible = true;
        _gameState = GameState.End;
        _endScreen.StartCredits();
    }

    public void DisablePlayer() {
        _player.AxisLockLinearX = true;
        _player.AxisLockLinearY = true;
        _player.AxisLockLinearZ = true;
        _player.DisableMode = CollisionObject3D.DisableModeEnum.MakeStatic;
    }
}
