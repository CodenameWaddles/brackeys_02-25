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
		EmitSignal(SignalName.TrashButtonPressed, trashAmount);
		_animationPlayer.Play("press");
		MakeUninteractable();
	}
}
