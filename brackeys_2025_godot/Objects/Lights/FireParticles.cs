using Godot;
using System;

public partial class FireParticles : Node3D
{
	[Export] private OmniLight3D _light;

	private float _random;
	
	public override void _Ready() {
		_random = (float)GD.RandRange(0.5f, 2.0f);
		_light.LightEnergy = 1f;
	}

	public override void _Process(double delta) {
		_light.LightEnergy = 1f + 0.2f * Mathf.Sin((Time.GetTicksMsec() / _random) * 0.05f);
	}
}
