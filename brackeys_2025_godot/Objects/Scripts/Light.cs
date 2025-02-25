using Godot;
using System;
using Godot.Collections;

public partial class Light : OmniLight3D
{
	[Export] private Node3D _bulbNode;
	[Export] private AudioStreamPlayer3D _audioPlayer;
	[Export] private AudioStream _flickerSound;
	[Export] private Vector3 _onColor = new(1, 1, 0.2f);
	[Export] private bool _flickering = true;
	[Export] private bool _shadow = true;
	[Export] private bool _isEscapeLight = false;

	public bool _on = true;
	
	[Export] private Vector3 _offColor = new(0.05f, 0.05f, 0.05f);
	private const int _maxFlickers = 5;
	private Array<MeshInstance3D> _mesh = new();
	
	public override void _Ready() {
		if (_isEscapeLight) {
			_mesh.Add((MeshInstance3D)_bulbNode?.FindChild("arrowFront"));
			_mesh.Add((MeshInstance3D)_bulbNode?.FindChild("arrowBack"));
		}
		else {
			_mesh.Add((MeshInstance3D)_bulbNode?.FindChild("Industrial lantern A_1"));
		}
		
		_audioPlayer.Stream = _flickerSound;
		ShadowEnabled = _shadow;
		if (!_on) {
			LightEnergy = 0;
			foreach (MeshInstance3D mesh in _mesh) {
				mesh.SetInstanceShaderParameter("color", _offColor);
			}
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
			foreach (MeshInstance3D mesh in _mesh) {
				mesh.SetInstanceShaderParameter("color", _offColor);
			}
		}
	}

	private void Activate() {
		LightEnergy = 1;
		_audioPlayer.Play();
		foreach (MeshInstance3D mesh in _mesh) {
			mesh.SetInstanceShaderParameter("color", _onColor);
		}

		if (_isEscapeLight) {
			foreach (MeshInstance3D mesh in _mesh) {
				mesh.Visible = true;
			}
		}
	}
	
	private void Deactivate() {
		LightEnergy = 0;
		_audioPlayer.Play();
		foreach (MeshInstance3D mesh in _mesh) {
			mesh.SetInstanceShaderParameter("color", _offColor);
		}
		if (_isEscapeLight) {
			foreach (MeshInstance3D mesh in _mesh) {
				mesh.Visible = false;
			}
		}
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
		if(_isEscapeLight) {
			await ToSignal(GetTree().CreateTimer(0.4f), "timeout");
		}
		if(turnOff) {
			Deactivate();
		}
	}
}
