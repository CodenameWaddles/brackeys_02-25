using Godot;
using System;

public partial class AlarmLight : Node3D
{
	
	[Export] private Node3D _bulbNode;
	[Export] private AudioStreamPlayer3D _audioPlayer;
	[Export] private AudioStream _flickerSound;
	[Export] private Vector3 _onColor = new(1, 0.05f, 0.05f);
	[Export] private bool _flickering = true;
	[Export] private bool _shadow = true;
	[Export] private SpotLight3D _spotLight;
	[Export] private SpotLight3D _spotLight2;
	[Export] private Node3D _spotLightNode;
	
	private MeshInstance3D _mesh;

	[Export] public bool _on = true;
	
	[Export] private Vector3 _offColor = new(0.05f, 0.05f, 0.05f);
	private const int _maxFlickers = 5;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_mesh = (MeshInstance3D)_bulbNode?.FindChild("Industrial lantern A_1");
		_mesh?.SetInstanceShaderParameter("color", _onColor);
		_spotLight.LightColor = new Color(_onColor.X, _onColor.Y, _onColor.Z);
		_spotLight2.LightColor = new Color(_onColor.X, _onColor.Y, _onColor.Z);
		if (!_on) {
			_spotLight.LightEnergy = 0;
			_spotLight2.LightEnergy = 0;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// rotate the light
		_spotLightNode.RotateZ(Mathf.Pi * (float)delta);
	}
}
