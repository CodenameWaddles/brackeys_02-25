using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public partial class TrashDeposit : InteractableNeedObject
{

	[Export] private Dictionary<Node3D, bool> _trashSlots;
	
	private playerInteraction _playerInteraction;
	
	public override void _Ready()
	{
		_playerInteraction = (playerInteraction)GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("Camera");

	}

	protected override void ActivateSpecific()
	{
		if (_playerInteraction.held.Type == Pickupable.PickupType.Trash)
		{
			//
			foreach (var slot in _trashSlots)
			{
				if (slot.Value == false) // slot free
				{
					_playerInteraction.DropItem();
					break;
				}
			}
		}
	}
}
