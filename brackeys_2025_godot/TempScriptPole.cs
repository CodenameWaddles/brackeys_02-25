using Godot;
using System;

public partial class TempScriptPole : RigidBody3D
{

	public override void _Ready()
	{
		
	}
	
	public override void _Process(double delta) {
		if (Input.IsActionPressed("ui_left")) {
			MoveAndCollide(new Vector3(0.02f, 0, 0));
		}
		if (Input.IsActionPressed("ui_right")) {
			MoveAndCollide(new Vector3(-0.02f, 0, 0));
		}
	}
}
