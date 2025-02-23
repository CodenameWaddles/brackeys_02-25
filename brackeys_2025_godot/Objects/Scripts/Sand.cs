using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class Sand : Hazard
{

	[Export] private Node3D _visuals;
	[Export] private CollisionShape3D _collider;
	[Export] private AudioStreamPlayer3D _shovelUseSound;
	
	private GameManager _main;
	
	public override void _Ready()
	{
		Type = Pickupable.PickupType.Shovel;
		_main = (GameManager) GetTree().Root.GetChild(0);
		MakeInteractable();
	}

	protected override void ActivateSpecific()
	{
		
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
		_shovelUseSound.Play();
	
		MakeUninteractable();

	}

	protected override void MakeUninteractableSpecific()
	{
		IsSolved = true;
		_collider.Disabled = true;
		_visuals.Visible = false;
		_main._currentScene.UpdateIntegrityPercentage();
	}

	protected override void MakeInteractableSpecific()
	{
		IsSolved = false;
		_collider.Disabled = false;
		_visuals.Visible = true;
	}
}
