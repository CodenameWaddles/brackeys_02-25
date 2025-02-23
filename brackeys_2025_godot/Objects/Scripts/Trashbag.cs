using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class Trashbag : Pickupable
{

	public bool IsSolved;
	public bool IsInCart;

	public override void _Ready()
	{
		_isInteractable = true;
		Type = PickupType.Trash;
	}
	
	protected override void ActivateSpecific() {
		if(!IsPickedUp) {
			if (IsInCart)
			{
				GameManager gm = (GameManager) GetTree().Root.GetChild(0);
				TrashDeposit trashDeposit = gm.Cart.TrashDeposit;
				trashDeposit.RemoveBag();
			}
			IsPickedUp = true;
			Item.Visible = false;
		}
		else {
			IsPickedUp = false;
			Item.Visible = true;
		}
	}
	
}
