using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class Trashbag : Pickupable
{
	[Export] public RigidBody3D RigidBody;
	[Export] private AudioStreamPlayer3D _trashGroundSound;

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
				trashDeposit.RemoveBag(this);
				IsInCart = false;
				Reparent(gm);
			}
			IsPickedUp = true;
			Item.Visible = false;
		}
		else {
			IsPickedUp = false;
			Item.Visible = true;
		}
	}
	
	public void PlayGroundSound() {
		_trashGroundSound.Play();
	}
	
}
