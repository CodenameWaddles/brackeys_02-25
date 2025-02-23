using Godot;
using System;

public partial class WallLight : Node3D {
	[Export] private bool _shadow = true;
	[Export] private Light _light;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_light.ShadowEnabled = _shadow;
	}
}
