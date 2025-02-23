using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class BlowtorchableDoor : Hazard {
    
    [Export] private Node3D _doorFixed;
    [Export] private Node3D _doorDamaged;
    [Export] private int _fixTime = 200;
    
    [Export] private bool _isHazard = true;

    //private bool fixing;

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
        _doorFixed.Visible = false;
        _doorDamaged.Visible = true;
    }

    protected override void MakeUninteractableSpecific()
    {
        IsSolved = true;
        _doorFixed.Visible = true;
        _doorDamaged.Visible = false;
        _main._currentScene.UpdateIntegrityPercentage();
    }
}
