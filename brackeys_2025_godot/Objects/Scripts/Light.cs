using Godot;
using System;

public partial class Light : OmniLight3D
{
	[Export] private Node3D _bulbNode;
	[Export] private AudioStreamPlayer3D _audioPlayer;
	[Export] private AudioStream _flickerSound;
	[Export] private Vector3 _onColor = new(1, 1, 0.2f);
	
	private ShaderMaterial _shaderMaterial;
	private Vector3 _offColor = new(0.05f, 0.05f, 0.05f);
	private const int _maxFlickers = 5;
	
	public override void _Process(double delta) {
		if(Input.IsActionJustPressed("ui_accept")) {
			Flicker(false);
		}
	}
	
	public override void _Ready() {
		MeshInstance3D child = (MeshInstance3D) _bulbNode.FindChild("Industrial lantern A_1");
		_shaderMaterial = (ShaderMaterial) child.MaterialOverride;
		_audioPlayer.Stream = _flickerSound;
	}
	
	private void Activate() {
		LightEnergy = 1;
		_audioPlayer.Play();
		if(_shaderMaterial == null) return;
		_shaderMaterial.SetShaderParameter("color", _onColor);
	}
	
	private void Deactivate() {
		LightEnergy = 0;
		_audioPlayer.Play();
		if(_shaderMaterial == null) return;
		_shaderMaterial.SetShaderParameter("color", _offColor);
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
