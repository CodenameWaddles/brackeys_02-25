using Godot;
using System;

public partial class BangingNoise : AudioStreamPlayer3D
{
	public override void _Process(double delta)
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		if(rng.Randf() < AudioManager.Instance.BangingFrequency) {
			Play();
		}
	}
}
