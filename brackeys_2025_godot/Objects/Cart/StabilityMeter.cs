using Godot;
using System;

public partial class StabilityMeter : Node3D
{
	
	[Export] private Node3D _indicator;
	[Export] private Node3D _max;
	[Export] private Node3D _min;
	
	public float Stability { get; private set; } = 0.5f;
	
	private float _stability = 0.5f;
	
	public void SetStability(float value) {
		_stability = Mathf.Clamp(value, 0, 1);
	}
	
	public override void _Process(double delta) {
		_indicator.SetPosition(_indicator.Position.Lerp(_min.Position.Lerp(_max.Position, _stability), 0.05f));
		//_indicator.SetPosition(_min.Position.Lerp(_max.Position, _stability));
	}
}
