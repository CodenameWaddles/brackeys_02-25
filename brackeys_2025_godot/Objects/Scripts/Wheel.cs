using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class Wheel : Hazard {
    
    [Export] private Node3D _wheelFixed;
    [Export] private Node3D _wheelRusty;
    [Export] private int _fixTime = 150;
    
    private int _fixProgress;
    
    public override void _Ready() {
        MakeInteractable();
    }
    
    protected override void ActivateSpecific() {
        if(_fixProgress < _fixTime) {
            _fixProgress++;
        } else {
            MakeUninteractable();
        }
    }

    protected override void MakeInteractableSpecific()
    {
        IsSolved = true;
        _fixProgress = 0;
        _wheelFixed.Visible = false;
        _wheelRusty.Visible = true;
    }
    
    protected override void MakeUninteractableSpecific()
    {
        IsSolved = false;
        _wheelFixed.Visible = true;
        _wheelRusty.Visible = false;
    }
}