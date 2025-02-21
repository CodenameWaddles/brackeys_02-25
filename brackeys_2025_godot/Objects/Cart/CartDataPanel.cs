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
	
	public DataMode _dataMode { get; private set; } = DataMode.Dual;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}
	
	public void SwitchDataMode() {
		if(_dataMode == DataMode.Dual) {
			_dataMode = DataMode.Single;
			_animationPlayer.Play("switch");
		} else {
			_dataMode = DataMode.Dual;
			_animationPlayer.Play("switch_back");
		}
	}
}
