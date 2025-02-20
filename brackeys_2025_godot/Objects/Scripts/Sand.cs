using Godot;
using System;

public partial class Sand : InteractableNeedObject
{
	
	public override void _Ready()
	{
		Type = Pickupable.PickupType.Shovel;
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
	
		MakeUninteractable();
		QueueFree();

	}
}
