using Godot;
using System;

public partial class Shovel : Pickupable
{
    public bool isFull = false;
    
    public override void _Ready() {
        _isInteractable = true;
        Type = PickupType.Shovel;
    }
    
    
    
}
