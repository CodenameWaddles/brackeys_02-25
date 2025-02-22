using Godot;
using System;

public partial class Tape : Pickupable {
    
    public override void _Ready() {
        _isInteractable = true;
        Type = PickupType.Tape;
    }
    
}