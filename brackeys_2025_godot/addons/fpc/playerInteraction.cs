using Godot;
using System;

public partial class playerInteraction : Node {

    [Export] private Node3D _hand;

    public Pickupable.PickupType held { get; private set; } = Pickupable.PickupType.None;
    private Interactable _hovering;
    
    private bool _justInteracted = false;
    
    // Time to hold until interaction loops
    private double _interactTime = 0;
    private const double interactTimeMax = 0.5;
    
    // Step between interactions while holding interact button
    private double _interactStep = 0;
    private const double interactStepMax = 0.07;
    
    [Export] RayCast3D _rayCast;

    public override void _Ready() {
        
    }

    public override void _PhysicsProcess(double delta) {
        if (_rayCast.IsColliding()) {
            var target = (Node)_rayCast.GetCollider();
            target = target.GetParent();
            if (target is Interactable interactable) {
                _hovering = interactable;
            }
        }
        else {
            _hovering = null;
        }
    }
    
    public override void _Process(double delta) {
        // interact with input button
        if(_hovering is InputButton button && held == Pickupable.PickupType.None) {
            if (Input.IsActionPressed("interact")) {
                if (!_justInteracted) {
                    button.Activate();
                    _justInteracted = true;
                }
                if(_interactTime >= interactTimeMax) {
                    if(_interactStep >= interactStepMax) {
                        button.Activate();
                        _interactStep = 0;
                    } else {
                        _interactStep += delta;
                    }
                }
            } else {
                _justInteracted = false;
                _interactTime = 0;
            }
            if(_justInteracted && _interactTime < interactTimeMax) {
                _interactTime += delta;
            }
        }
        // interact with pickupable
        else if (_hovering is Pickupable pickupable) {
            if (Input.IsActionJustPressed("interact")) {
                if(held == Pickupable.PickupType.None) {
                    if (!pickupable.IsPickedUp) {
                        pickupable.Activate();
                        PickUpItem(pickupable);
                    }
                    
                }
                else {
                    if(held == pickupable.Type && pickupable.IsPickedUp) {
                        pickupable.Activate();
                        DropItem();
                    }
                }
            }
        }
        // interact with interactable need object
        else if (_hovering is InteractableNeedObject interactableNeedObject) {
            if (Input.IsActionPressed("interact")) {
                if (interactableNeedObject.Type == held) {
                    interactableNeedObject.Activate();
                }
            }
        }
        // hovering interactable
        else if (_hovering is Interactable interactable) {
            if (Input.IsActionJustPressed("interact")) {
                _hovering.Activate();
            }
        }
        // reset ticks
        else {
            _interactStep = 0;
            _interactTime = 0;
        }
    }
    
    private void PickUpItem(Pickupable item) {
        held = item.Type;
        MeshInstance3D itemMesh = (MeshInstance3D) item.ItemMesh.Duplicate();
        itemMesh.Visible = true;
        _hand.AddChild(itemMesh);
    }
    
    private void DropItem() {
        held = Pickupable.PickupType.None;
        _hand.GetChild(0).QueueFree();
    }
}