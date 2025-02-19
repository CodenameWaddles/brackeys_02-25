using Godot;

namespace Brackeys_2025_Godot.Objects.Scripts;

public partial class PressButton : Interactable
{

    [Export] private CollisionShape3D _shape;
    [Export] private AnimationPlayer _animationPlayer;
    
    [Signal]
    public delegate void ButtonPressedEventHandler();
    
    public override void _Ready() {
        MakeInteractable();
    }

    protected override void ActivateSpecific()
    {
        EmitSignal(SignalName.ButtonPressed);
        _animationPlayer.Play("press");
    }
    
    protected override void MakeInteractableSpecific() {
        // Make button interactable
        _shape.Disabled = false;
    }
    
    protected override void MakeUninteractableSpecific() {
        // Make button uninteractable
        _shape.Disabled = true;
    }
    
}