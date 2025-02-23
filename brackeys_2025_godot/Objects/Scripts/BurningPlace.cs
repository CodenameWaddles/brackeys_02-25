using Godot;
public partial class BurningPlace : InteractableNeedObject
{

    [Export] private Zone _zone;
    
    public override void _Ready()
    {
        Type = Pickupable.PickupType.Trash;
        MakeInteractable();
        
        TrashDeposit deposit = (TrashDeposit) _zone.Cart.FindChild("TrashDepositInCart");
        GD.Print("deposit : " + deposit);
        deposit.SetTrashAsIssues();
    }

    protected override void ActivateSpecific()
    {
        playerInteraction _playerInteraction = (playerInteraction) GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("Camera");
        Trashbag held = (Trashbag)_playerInteraction.held;
        held.IsSolved = true;
        held.MakeUninteractable();
        GameManager gm = (GameManager)GetTree().Root.GetChild(0);
        gm._currentScene.UpdateIntegrityPercentage();
        _playerInteraction.DropItem();
    }
}
