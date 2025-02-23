using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class LeakablePipe : Hazard {
    
    [Export] private Node3D _pipeLeak;
    [Export] private Node3D _pipeFixed;
    [Export] private int _fixTime = 150;
    [Export] private AudioStreamPlayer3D _leakSound;
    
    [Export] private bool _isHazard = true;

    public override void _Ready() {
        if (_isHazard) {
            Type = Pickupable.PickupType.Blowtorch;
            _main = (GameManager) GetTree().Root.GetChild(0);
            MakeInteractable();
        }
        else {
            MakeUninteractable();
        }
    }
    
    private int _fixProgress;
    private GameManager _main;
    
    protected override void ActivateSpecific() {
        if(_fixProgress < _fixTime) {
            _fixProgress++;
        } else {
            MakeUninteractable();
        }
    }

    protected override void MakeInteractableSpecific()
    {
        IsSolved = false;
        _fixProgress = 0;
        _pipeFixed.Visible = false;
        _pipeLeak.Visible = true;
        _leakSound.Play();
        //_main._currentScene.UpdateIntegrityPercentage(); //osef psk on le suprime anyway ???? 
    }

    protected override void MakeUninteractableSpecific()
    {
        IsSolved = true;
        _pipeFixed.Visible = true;
        _pipeLeak.Visible = false;
        _leakSound.Stop();
        _main._currentScene.UpdateIntegrityPercentage();
    }
}