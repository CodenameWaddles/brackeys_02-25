using Godot;
using System;

public partial class playerInteraction : Node3D {

    [Export] public Node3D _hand; //LAISSER PUBLIC

    public Pickupable held { get; private set; } = null;
    private Interactable _hovering;
    
    private bool _justInteracted = false;
    
    // Time to hold until interaction loops
    private double _interactTime = 0;
    private const double interactTimeMax = 0.5;
    
    // Step between interactions while holding interact button
    private double _interactStep = 0;
    private const double interactStepMax = 0.07;

    private GameManager _main;
    
    [Export] RayCast3D _rayCast;

    public override void _Ready()
    {
        _main = (GameManager) GetTree().Root.GetChild(0);
    }

    public override void _PhysicsProcess(double delta) {
        if (_rayCast.IsColliding()) {
            var target = (Node)_rayCast.GetCollider();
            if(target == null) return;
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
        if(_hovering is InputButton button && held == null) {
            if (Input.IsActionPressed("interact")) {
                _main._currentScene.UpdateIntegrityPercentage();
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
                if(held == null) {
                    if (!pickupable.IsPickedUp) {
                        pickupable.Activate();
                        PickUpItem(pickupable);
                    }
                    
                }
                else {
                    if(held == pickupable && pickupable.IsPickedUp) {
                        pickupable.Activate();
                        DropItem();
                    }
                }
            }
        }
        // interact with interactable need object
        else if (_hovering is InteractableNeedObject interactableNeedObject) {
            if (Input.IsActionPressed("interact"))
            {
                if (held == null) return;
                if (interactableNeedObject.Type == held.Type) {
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
        held = item;
        Node3D newItem = (Node3D) item.Item.Duplicate();
        newItem.Position = _hand.Position;
        newItem.Visible = true;
        _hand.AddChild(newItem);
        Node3D player = (Node3D)GetParent();
    }
    
    public void DropItem() {
        held = null;
        _hand.GetChild(0).QueueFree();
    }
}