using Godot;
using System;
using System.Collections;
using Godot.Collections;

public partial class GameManager : Node
{
    
    private Zone _currentScene;
    private int _currentSceneIndex = 0;
    [Export] private Array<PackedScene> _scenes = new Array<PackedScene>();
    [Export] private Vector3 _scenePosition = new Vector3(0, 0, 0); //remplacer tmtc
    [Export] private Cart Cart;

    public override void _Ready()
    {
        Cart.ArrivedSignal += _cartArrived;
        _loadScene(0);
    }

    public override void _Process(double delta)
    {
        if (_currentScene.IsComplete && !Cart.allowedToMove)
        {
            Cart.allowedToMove = true;
        }
        
        if(Input.IsActionJustPressed("test"))
        {
            if (Cart._state == Cart.State.Moving)
            {
                Cart.Stop();
            }
            else if (Cart._state == Cart.State.Stopped)
            {
                Cart.Start();
            }
        }
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

        GD.Print("temp scene path : " + tempScene.GetPath());
        
        _currentScene = GetNode<Zone>(tempScene.GetPath());
        _currentScene.Cart = Cart;
        Cart._state = Cart.State.Moving;
        
        GD.Print("loaded scene : " + _currentScene.Name + ", index : " + _currentSceneIndex);
    }
    
    private void _cartArrived()
    {
        if(_currentSceneIndex == _scenes.Count - 1)
            _loadScene(0);
        else
            _loadScene(_currentSceneIndex + 1);
    }
}
