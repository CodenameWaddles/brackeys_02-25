using Godot;
using System;

public partial class Light : OmniLight3D
{
	[Export] private Node3D _bulbNode;
	[Export] private AudioStreamPlayer3D _audioPlayer;
	[Export] private AudioStream _flickerSound;
	[Export] private Vector3 _onColor = new(1, 1, 0.2f);
	[Export] private bool _flickering = true;
	[Export] private bool _shadow = true;

	public bool _on = true;
	
	private Vector3 _offColor = new(0.05f, 0.05f, 0.05f);
	private const int _maxFlickers = 5;
	private MeshInstance3D _mesh;
	
	public override void _Ready() {
		_mesh = (MeshInstance3D) _bulbNode.FindChild("Industrial lantern A_1");
		_audioPlayer.Stream = _flickerSound;
		ShadowEnabled = _shadow;
		if (!_on) {
			LightEnergy = 0;
			_mesh?.SetInstanceShaderParameter("color", _offColor);
		}
	}

	public override void _Process(double delta) {
		if(_flickering && _on) {
			RandomNumberGenerator rng = new RandomNumberGenerator();
			if(rng.Randf() < LightManager.Instance.FlickeringFrequency) {
				_audioPlayer.VolumeDb = LightManager.Instance.FlickeringVolume;
				Flicker(false);
			}
		}
		else if(!_on){
			LightEnergy = 0;
			_mesh?.SetInstanceShaderParameter("color", _offColor);
		}
	}

	private void Activate() {
		LightEnergy = 1;
		_audioPlayer.Play();
		_mesh?.SetInstanceShaderParameter("color", _onColor);
	}
	
	private void Deactivate() {
		LightEnergy = 0;
		_audioPlayer.Play();
		_mesh?.SetInstanceShaderParameter("color", _offColor);
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
