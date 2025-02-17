using Godot;
using System;

public partial class Wheel : InteractableNeedObject {
    
    [Export] private Node3D _wheelFixed;
    [Export] private Node3D _wheelRusty;
    [Export] private int _fixTime = 150;
    
    private int _fixProgress;
    
    public override void _Ready() {
        Type = Pickupable.PickupType.Oil;
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
        _wheelFixed.Visible = false;
        _wheelRusty.Visible = true;
    }
    
    protected override void MakeUninteractableSpecific() {
        _wheelFixed.Visible = true;
        _wheelRusty.Visible = false;
    }
}