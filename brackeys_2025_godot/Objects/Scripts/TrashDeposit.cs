using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public partial class TrashDeposit : InteractableNeedObject
{

	[Export] private Array<Node3D> _trashSlotsArray;
	[Export] private PackedScene _trashPrefab;
	
	private Dictionary<Vector3, bool> _trashSlots = new Dictionary<Vector3, bool>();
	private playerInteraction _playerInteraction;
	
	public override void _Ready()
	{
		Type = Pickupable.PickupType.Trash;
		MakeInteractable();
		_playerInteraction = (playerInteraction)GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("Camera");
		SetupTrahSlots();
		GD.Print(_trashSlots.Values);

	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public void SetupTrahSlots()
	{
		foreach (var trash in _trashSlotsArray)
		{
			_trashSlots[trash.Position] = false;
		}
		
	}

	protected override void ActivateSpecific()
	{
		if (_playerInteraction.held.Type == Pickupable.PickupType.Trash)
		{
			foreach (var slot in _trashSlots)
			{
				if (slot.Value == false) // slot free
				{
					_playerInteraction.DropItem();
					Node3D trashbag = (Node3D) _trashPrefab.Instantiate();
					
					AddChild(trashbag);
					trashbag.Position = slot.Key;
					_trashSlots[slot.Key] = true;
					break;
				}
				return; //plein
			}
		}
		GD.Print(_trashSlots.Values);
	}

	public void RemoveBag()
	{
		Vector3 previous_slot = Vector3.Zero;
		foreach (var slot in _trashSlots)
		{
			if (slot.Value == false)
			{
				if (previous_slot != Vector3.Zero)//le previous était plein
				{
					_trashSlots.Remove(previous_slot);
				}
				else
				{
					_trashSlots.Remove(slot.Key);
				}
				break;
			}
		}
		GD.Print(_trashSlots.Values);
	}
	
}
