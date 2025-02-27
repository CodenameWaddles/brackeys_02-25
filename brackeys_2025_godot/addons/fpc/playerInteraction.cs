using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class playerInteraction : Node3D {
    
    [Export] public Node3D _hand; //LAISSER PUBLIC
    [Export] public HeadLight _headLight;

    [Export] private Control _UI;
    
    [Export] private PackedScene _defaultReticle;
    [Export] private PackedScene _interactReticle;
    
    [Export] private AudioStreamPlayer3D _pickUpSound;
    [Export] private AudioStreamPlayer3D _dropSound;

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
        _hovering = null;
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
            if(_hovering.IsInteractable)
                ChangeReticle(_interactReticle);
            if (Input.IsActionPressed("interact")) {
                _main?._currentScene?.UpdateIntegrityPercentage();
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
            if((_hovering.IsInteractable && !pickupable.IsPickedUp && held == null) ||
               (_hovering.IsInteractable && held.Type == pickupable.Type && pickupable.IsPickedUp))
                ChangeReticle(_interactReticle);
            if (Input.IsActionJustPressed("interact")) {
                if(held == null) {
                    if (!pickupable.IsPickedUp) {
                        pickupable.Activate();
                        PickUpItem(pickupable);
                        if (pickupable.Type != Pickupable.PickupType.Trash)
                        {
                            _pickUpSound.Play();
                        }
                    }
                }
                else {
                    if(held == pickupable && pickupable.IsPickedUp) {
                        pickupable.Activate();
                        DropItem();
                        _dropSound.Play();
                    }
                }
            }
        }
        // interact with interactable need object
        else if (_hovering is InteractableNeedObject interactableNeedObject)
        {
            if (_hovering.IsInteractable && interactableNeedObject.Type == held?.Type)
                ChangeReticle(_interactReticle);
            if (Input.IsActionPressed("interact"))
            {
                if (interactableNeedObject.Type == held?.Type)
                {
                    interactableNeedObject.Activate();
                }

                if (interactableNeedObject.Type == Pickupable.PickupType.Blowtorch &&
                    held.Type == Pickupable.PickupType.Blowtorch)
                {
                    if (interactableNeedObject is LeakablePipe or BreakPipe)
                    {
                        Blowtorch blowtorch = (Blowtorch)held;
                        Hazard hazard = (Hazard)interactableNeedObject;
                        if (!hazard.IsSolved)
                        {
                            blowtorch.isBeingUsed = true;
                        }
                        else
                        {
                            blowtorch.isBeingUsed = false;
                        }
                    }
                   
                }
            }
            else
            {
                if(interactableNeedObject.Type == held?.Type && held.Type == Pickupable.PickupType.Blowtorch)
                {
                    Blowtorch blowtorch = (Blowtorch)held;
                    blowtorch.isBeingUsed = false;
                }
            }
        }
        // hovering interactable
        else if (_hovering is Interactable interactable) {
            if(_hovering.IsInteractable)
                ChangeReticle(_interactReticle);
            if (Input.IsActionJustPressed("interact")) {
                _hovering.Activate();
            }
        }
        // reset ticks
        else {
            ChangeReticle(_defaultReticle);
            _interactStep = 0;
            _interactTime = 0;
            
            if(held != null && held.Type == Pickupable.PickupType.Blowtorch)
            {
                Blowtorch blowtorch = (Blowtorch)held;
                blowtorch.isBeingUsed = false;
            }
            
        }
    }
    
    private void PickUpItem(Pickupable item) {
        held = item;
        Node3D newItem = (Node3D) item.Item.Duplicate();
        newItem.Position = _hand.Position;
        newItem.Visible = true;
        switch (held.Type)
        {
            case Pickupable.PickupType.Trash:
                newItem.Scale = new Vector3(0.5f, 0.5f, 0.5f);
                break;
            case Pickupable.PickupType.Blowtorch:
                newItem.Scale = new Vector3(1.5f, 1.5f, 1.5f);
                newItem.RotationDegrees = new Vector3(0, 90, 0);
                newItem.TranslateObjectLocal(new Vector3(-0.2f, 0, -0.1f));
                break;
        }

        
        if (held.Type == Pickupable.PickupType.Trash)
        {
            
        }
        _hand.AddChild(newItem);
    }
    
    public void DropItem() {
        held = null;
        _hand.GetChild(0).QueueFree();
    }
    
    private void ChangeReticle(PackedScene reticle) {
        _UI.GetChild(0).QueueFree();
        Control newReticle = (Control)reticle.Instantiate();
        _UI.AddChild(newReticle);
    }
}