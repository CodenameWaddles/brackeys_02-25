using Godot;
using System;

public abstract partial class Pickupable : Interactable {
    
    public enum PickupType {
        Blowtorch,
        Oil,
        Shovel,
        None
    }

    [Export] public MeshInstance3D ItemMesh { get; protected set; }

    public PickupType Type { get; protected set; }

    public bool IsPickedUp { get; protected set; } = false;
    
    protected override void ActivateSpecific() {
        if(!IsPickedUp) {
            IsPickedUp = true;
            ItemMesh.Visible = false;
        }
        else {
            IsPickedUp = false;
            ItemMesh.Visible = true;
        }
    }

    public override void _Process(double delta) {
        
    }
}