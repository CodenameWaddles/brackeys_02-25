using Godot;
using System;

public partial class CartDataPanel : Node3D
{
	public enum DataMode {
		Dual,
		Single
	}
	
	[Export] public DataDouble DataDouble;
	[Export] public DataSingle DataSingle;
	[Export] private AnimationPlayer _animationPlayer;
	[Export] private MeshInstance3D _greenLightMeshDouble;
	[Export] private OmniLight3D _greenLightDouble;
	[Export] private MeshInstance3D _greenLightMeshSingle;
	[Export] private OmniLight3D _greenLightSingle;
	
	public DataMode _dataMode { get; private set; } = DataMode.Dual;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public void SetDataMode(DataMode dataMode) {
		if (_dataMode == dataMode) return;
		SwitchDataMode();
	}
	
	private void SwitchDataMode() {
		if(_dataMode == DataMode.Dual) {
			_dataMode = DataMode.Single;
			_animationPlayer.Play("switch");
		} else {
			_dataMode = DataMode.Dual;
			_animationPlayer.Play("switch_back");
		}
	}
	
	public void TurnOnGreenLight() {
		if(_dataMode == DataMode.Dual) {
			_greenLightMeshDouble.SetInstanceShaderParameter("color", new Vector3(0.05f, 1, 0.05f));
			_greenLightDouble.LightEnergy = 1;
		} else if(_dataMode == DataMode.Single) {
			_greenLightMeshSingle.SetInstanceShaderParameter("color", new Vector3(0.05f, 1, 0.05f));
			_greenLightSingle.LightEnergy = 1;
		}
	}
	
	public void TurnOffGreenLight() {
		_greenLightMeshDouble.SetInstanceShaderParameter("color", new Vector3(0.05f, 0.05f, 0.05f));
		_greenLightDouble.LightEnergy = 0;
		_greenLightMeshSingle.SetInstanceShaderParameter("color", new Vector3(0.05f, 0.05f, 0.05f));
		_greenLightSingle.LightEnergy = 0;
	}
}
