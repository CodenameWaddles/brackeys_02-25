using Godot;
using System;

public abstract partial class InteractableNeedObject : Interactable {

    [Export] public Pickupable.PickupType Type { get; protected set; }
    
}