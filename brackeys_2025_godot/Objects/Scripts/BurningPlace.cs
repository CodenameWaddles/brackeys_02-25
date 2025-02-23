using Godot;
public partial class BurningPlace : InteractableNeedObject
{

    [Export] private Zone _zone;
    [Export] private AudioStreamPlayer3D ambiantSound;
    [Export] private AudioStreamPlayer3D burnSound;
    
    public int TrashCount { get; private set; } = 0;
    
    public override void _Ready()
    {
        Type = Pickupable.PickupType.Trash;
        MakeInteractable();
        _zone.setBurningPlace(this);
        ambiantSound.Play();
    }

    public override void _Process(double delta)
    {
        if (!ambiantSound.IsPlaying())
        {
            ambiantSound.Play();
        }
    }

    protected override void ActivateSpecific()
    {
        playerInteraction _playerInteraction = (playerInteraction) GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("Camera");
        Trashbag held = (Trashbag)_playerInteraction.held;
        held.IsSolved = true;
        held.MakeUninteractable();
        GameManager gm = (GameManager)GetTree().Root.GetChild(0);
        _playerInteraction.DropItem();
        TrashCount++;
        gm._currentScene.UpdateIntegrityPercentage();
        burnSound.Play();
    }
}
