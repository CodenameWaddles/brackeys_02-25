using Godot;
using System;

public partial class HeadLight : SpotLight3D
{
	[Export] private AudioStreamPlayer3D _flickerSound;
	
	private const int _maxFlickers = 4;
	
	private void Activate() {
		_flickerSound.Play();
		LightEnergy = 1;
	}
	
	private void Deactivate() {
		_flickerSound.Play();
		LightEnergy = 0;
	}
	
	public override void _Process(double delta) {
		
	}

	public async void Flicker(bool turnOff) {
		RandomNumberGenerator rng = new RandomNumberGenerator();
		int nb = rng.RandiRange(2, _maxFlickers);
		for(int i = 0; i < nb; i++) {
			Deactivate();
			await ToSignal(GetTree().CreateTimer(rng.RandfRange(0.05f, 0.3f)), "timeout");
			Activate();
			await ToSignal(GetTree().CreateTimer(rng.RandfRange(0.05f, 0.3f)), "timeout");
		}
		if(turnOff) {
			Deactivate();
		}
	}
}
