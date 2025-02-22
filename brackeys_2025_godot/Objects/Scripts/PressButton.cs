using Godot;

namespace Brackeys_2025_Godot.Objects.Scripts;

public partial class PressButton : Interactable
{

    [Export] private CollisionShape3D _shape;
    [Export] private AnimationPlayer _animationPlayer;
    [Export] private MeshInstance3D _greenLightMesh;
    [Export] private OmniLight3D _greenLight;
    [Export] private MeshInstance3D _redLightMesh;
    [Export] private OmniLight3D _redLight;
    
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
        //_shape.Disabled = false;
        ActivateGreenLight();
    }
    
    protected override void MakeUninteractableSpecific() {
        // Make button uninteractable
        //_shape.Disabled = true;
        ActivateRedLight();
    }
    
    public void ActivateGreenLight() {
        _greenLightMesh.SetInstanceShaderParameter("color", new Vector3(0.05f, 1, 0.05f));
        _greenLight.LightEnergy = 1;
        _redLightMesh.SetInstanceShaderParameter("color", new Vector3(0.05f, 0.05f, 0.05f));
        _redLight.LightEnergy = 0;
    }
    
    public void ActivateRedLight() {
        _greenLightMesh.SetInstanceShaderParameter("color", new Vector3(0.05f, 0.05f, 0.05f));
        _greenLight.LightEnergy = 0;
        _redLightMesh.SetInstanceShaderParameter("color", new Vector3(1, 0.05f, 0.05f));
        _redLight.LightEnergy = 1;
    }
    
}