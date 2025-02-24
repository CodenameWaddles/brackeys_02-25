using Godot;
using System;

public partial class TrashButton : Interactable
{
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private int trashAmount = 1;
	
	[Signal]
	public delegate void TrashButtonPressedEventHandler(int trashAmount);
	
	public override void _Ready() {
		MakeInteractable();
	}

	protected override void ActivateSpecific() {
		if (GameManager.Instance._currentSceneIndex == 12)
		{
			GameManager.Instance.SendMessage(12);
			GameManager.Instance.ByePassActivated = true;
			AnimationPlayer anim = (AnimationPlayer)GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("HeadbobAnimation");
			anim.Play("screen_shake");
		}
		
		EmitSignal(SignalName.TrashButtonPressed, trashAmount);
		_animationPlayer.Play("press");
		MakeUninteractable();
	}
}
