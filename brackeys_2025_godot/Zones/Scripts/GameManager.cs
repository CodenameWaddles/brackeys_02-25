using Godot;
using System;
using System.Collections;
using Godot.Collections;

public partial class GameManager : Node
{
    
    private Node3D _currentScene;
    private int _currentSceneIndex = 0;
    [Export] private Array<PackedScene> _scenes = new Array<PackedScene>();
    [Export] private Vector3 _scenePosition = new Vector3(0, 0, 0); //remplacer tmtc
    [Export] private Cart _cart;

    public override void _Ready()
    {
        _cart.ArrivedSignal += _cartArrived;
        _loadScene(0);
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionPressed("test"))
        {
            _cart.move = !_cart.move;
        }
    }

    private void _loadScene(int sceneIndex)
    {
        if(_currentScene != null)
            _currentScene.QueueFree();
        
        _currentScene = (Node3D) _scenes[sceneIndex].Instantiate();
        AddChild(_currentScene);
        _currentScene.Position = _scenePosition;
        GD.Print("loading scene : " + sceneIndex);
        
        _cart.move = true;
    }
    
    private void _cartArrived()
    {
        GD.Print("current scene index : " + _currentSceneIndex);
        GD.Print("scenes count : " + _scenes.Count);
        if(_currentSceneIndex == _scenes.Count - 1)
            _loadScene(0);
        else
            _loadScene(_currentSceneIndex + 1);
    }
    
}
