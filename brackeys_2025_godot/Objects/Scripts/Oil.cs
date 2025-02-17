using Godot;
using System;

public partial class Oil : Pickupable {
    
    public override void _Ready() {
        _isInteractable = true;
        Type = PickupType.Oil;
    }
    
}