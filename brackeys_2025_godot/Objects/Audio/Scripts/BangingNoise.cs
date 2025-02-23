using Godot;
using System;

public partial class BangingNoise : AudioStreamPlayer3D
{
	
	private bool _justPlayed = false;
	
	public override void _Process(double delta)
	{
		if(_justPlayed) return;
		RandomNumberGenerator rng = new RandomNumberGenerator();
		if(rng.Randf() < AudioManager.Instance.BangingFrequency) {
			PlayNoise();
		}
	}
	
	private async void PlayNoise()
	{
		_justPlayed = true;
		Play();
		await ToSignal(GetTree().CreateTimer(0.7f), "timeout");
		_justPlayed = false;
	}
}
