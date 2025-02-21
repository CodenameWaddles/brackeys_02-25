using Godot;
using System;

public partial class HeadLight : SpotLight3D
{
	private const int _maxFlickers = 5;
	
	private void Activate() {
		LightEnergy = 1;
	}
	
	private void Deactivate() {
		LightEnergy = 0;
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
