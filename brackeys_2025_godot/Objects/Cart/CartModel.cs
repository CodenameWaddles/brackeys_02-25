using Godot;
using System;

public partial class CartModel : Node3D
{
	[Export] private AnimationPlayer _animationPlayer;

	public void PlayCrashAnimation() {
		_animationPlayer.Play("crash");
	}
}
