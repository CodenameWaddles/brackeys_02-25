using Godot;
using System;

public partial class Blowtorch : Pickupable {
    
    public override void _Ready() {
        _isInteractable = true;
        Type = PickupType.Blowtorch;
    }
    
}