using Godot;
using System;

public abstract partial class Pickupable : Interactable {
    
    public enum PickupType {
        Blowtorch,
        Tape,
        Shovel,
        None
    }

    [Export] public Node3D Item { get; protected set; }

    public PickupType Type { get; protected set; }

    public bool IsPickedUp { get; protected set; } = false;
    
    protected override void ActivateSpecific() {
        if(!IsPickedUp) {
            IsPickedUp = true;
            Item.Visible = false;
        }
        else {
            IsPickedUp = false;
            Item.Visible = true;
        }
    }

    public override void _Process(double delta) {
        
    }
}