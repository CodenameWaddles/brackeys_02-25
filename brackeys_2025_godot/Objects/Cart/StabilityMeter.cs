using Godot;
using System;

public partial class StabilityMeter : Node3D
{
	
	[Export] private Node3D _indicator;
	[Export] private Node3D _max;
	[Export] private Node3D _min;
	
	public float Stability { get; private set; } = 0.5f;
	
	public void SetStability(float value) {
		_indicator.SetPosition(_min.Position.Lerp(_max.Position, value));
	}
}
