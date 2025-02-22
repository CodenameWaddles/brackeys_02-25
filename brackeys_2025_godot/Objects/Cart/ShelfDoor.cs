using Godot;
using System;

public partial class ShelfDoor : Node3D
{
	
	[Export] private AnimationPlayer _animationPlayer;
	
	public void OpenDoor() {
		_animationPlayer.Play("open");
	}
	
	public void CloseDoor() {
		_animationPlayer.Play("close");
	}
}
