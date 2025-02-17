using Godot;
using System;

public partial class LeakablePipe : InteractableNeedObject {
    
    [Export] private Node3D _pipeFixed;
    [Export] private Node3D _pipeLeak;
    [Export] private int _fixTime = 150;
    
    private int _fixProgress;
    
    public override void _Ready() {
        Type = Pickupable.PickupType.Blowtorch;
        MakeInteractable();
    }
    
    protected override void ActivateSpecific() {
        if(_fixProgress < _fixTime) {
            _fixProgress++;
        } else {
            MakeUninteractable();
        }
    }

    protected override void MakeInteractableSpecific() {
        _fixProgress = 0;
        _pipeFixed.Visible = false;
        _pipeLeak.Visible = true;
    }

    protected override void MakeUninteractableSpecific() {
        _pipeFixed.Visible = true;
        _pipeLeak.Visible = false;
    }
}