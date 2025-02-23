using Godot;
using System;

public partial class TrashDoors : Node3D
{
	[Export] private AnimationPlayer _openAnimation;
	
	public void OpenDoors() {
		_openAnimation.Play("open");
	}
}
