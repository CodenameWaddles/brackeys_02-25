using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class Sand : Hazard
{

	[Export] private Node3D _visuals;
	[Export] private Node3D _collider;
	
	private GameManager _main;
	
	public override void _Ready()
	{
		Type = Pickupable.PickupType.Shovel;
		_main = (GameManager) GetTree().Root.GetChild(0);
		MakeInteractable();
	}

	protected override void ActivateSpecific()
	{
		
		//					/!\ SUSSY CODE /!\				uwu ça marche
		playerInteraction player = (playerInteraction)GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("Camera");
			
		Node3D shovel = (Node3D)player._hand.GetChild(0);
		AnimationPlayer animationPlayer = null;
		foreach (var child in shovel.GetChildren())
		{
			if (child is AnimationPlayer)
			{
				animationPlayer = (AnimationPlayer) child;
			}
		}
		animationPlayer.Play("use");
	
		MakeUninteractable();

	}

	protected override void MakeUninteractableSpecific()
	{
		IsSolved = true;
		_collider.Visible = false;
		_visuals.Visible = false;
		_main._currentScene.UpdateIntegrityPercentage();
	}

	protected override void MakeInteractableSpecific()
	{
		IsSolved = false;
		_collider.Visible = true;
		_visuals.Visible = true;
		//_main._currentScene.UpdateIntegrityPercentage(); //useless psk suprimé ? et genr dans zone on reset on update après avoir reac les hazards
	}
}
