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
    
    private int _currentCycle = 0;
    private Zone _currentScene;
    private int _currentSceneIndex = 0;

    public override void _Ready()
    {
        Cart.ArrivedSignal += _cartArrived;
        _loadScene(0);
        _player.Reparent(Cart);
    }

    public override void _Process(double delta)
    {
        Cart.allowedToMove = _currentScene.IsComplete;
    }

    private void _loadScene(int sceneIndex)
    {
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
            _loadScene(_currentSceneIndex + 1);
        }
    }
}
