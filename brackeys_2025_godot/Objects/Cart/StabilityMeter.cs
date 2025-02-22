using Godot;
using System;

public partial class StabilityMeter : Node3D
{
	
	[Export] private Node3D _indicator;
	[Export] private Node3D _max;
	[Export] private Node3D _min;
	
	public float Stability { get; private set; } = 0.5f;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ui_up")) {
			Stability += 0.1f;
			if (Stability > 1) Stability = 1;
			SetStability(Stability);
		}

		if (Input.IsActionJustPressed("ui_down")) {
			Stability -= 0.1f;
			if (Stability < 0) Stability = 0;
			SetStability(Stability);
		}
	}
	
	public void SetStability(float value) {
		_indicator.SetPosition(_min.Position.Lerp(_max.Position, value));
	}
}
