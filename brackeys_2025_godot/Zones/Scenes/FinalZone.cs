using Godot;
using System;

public partial class FinalZone : Node3D {
	[Export] private StaticBody3D _colliderBehind;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_area_entered(Node3D area) {
		_colliderBehind.CollisionLayer =
	}
}
