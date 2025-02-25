using Godot;
using System;
using System.Linq;
using Godot.Collections;
using Array = Godot.Collections.Array;

public partial class TrashDeposit : InteractableNeedObject
{

	[Export] private Array<Node3D> _trashSlotsArray = new Array<Node3D>();
	[Export] private PackedScene _trashPrefab;
	[Export] private RigidBody3D _rigidBody;
	
	private playerInteraction _playerInteraction;
	private Array<Trashbag> trashbagsHeld = new Array<Trashbag>();
	
	public int TrashCount { get; private set; } = 0;
	
	public override void _Ready()
	{
		Type = Pickupable.PickupType.Trash;
		MakeInteractable();
		_playerInteraction = (playerInteraction)GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("Camera");
		for (int i = 0; i < _trashSlotsArray.Count; i++)
		{
			trashbagsHeld.Add(null);
		}
	}

	protected override void ActivateSpecific()
	{
		if (_playerInteraction.held.Type == Pickupable.PickupType.Trash)
		{
			for(int i = 0; i < _trashSlotsArray.Count; i++)
			{
				if (trashbagsHeld[i] == null)
				{
					//on suprime l'ancien bag
					Trashbag temp = (Trashbag)_playerInteraction.held;
					temp.IsSolved = true;
					temp.MakeUninteractable();
					_playerInteraction.DropItem();
					
					//on instancie une poubelle dans le wagon
					Node3D item = (Node3D) _trashPrefab.Instantiate();
					AddChild(item);
					item.Position = _trashSlotsArray[i].Position;
					Trashbag trashbag = (Trashbag)item; //on cast en trashbag pour set les propriétés qui vont avec
					trashbag.IsInCart = true;
					trashbag.IsSolved = true;
					trashbagsHeld[i] = trashbag;
					GameManager gm = (GameManager)GetTree().Root.GetChild(0);
					trashbag.Reparent(gm.Cart); //on l'atache au wagon
					TrashCount++;
					return; //vide
				}
			}
		}
	}

	public void RemoveBag(Trashbag trashbag)
	{
		for (int i = 0; i < _trashSlotsArray.Count; i++)
		{
			if (trashbagsHeld[i] == trashbag)
			{
				trashbagsHeld[i] = null;
				TrashCount--;
				return;
			}
		}
		
	}

	public void SetTrashAsIssues()
	{
		GameManager gm = (GameManager)GetTree().Root.GetChild(0);
		Zone zone = gm._currentScene;
		foreach (var bag in trashbagsHeld)
		{
			zone.ZoneHazards.Add(bag);
			bag.IsSolved = false;
		}
	}

	public void Reset() //jsp.
	{
		for (int i = 0; i < _trashSlotsArray.Count; i++)
		{
			if(trashbagsHeld[i] != null)
			{
				trashbagsHeld[i].QueueFree();
				trashbagsHeld[i] = null;
			}
		}
		TrashCount = 0;
	}
	
}
