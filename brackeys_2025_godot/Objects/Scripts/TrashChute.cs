using Godot;
using System;
using Godot.Collections;

public partial class TrashChute : Node3D
{
	[Export] private TrashButton _trashButton;
	[Export] private Array<Trashbag> _trashbags;
	[Export] private Node3D _trashEndPoint;
	[Export] private float _trashSpeed = 0.01f;
	
	private bool _trashIsSpawning = false;
	private float _trashFallingT = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_trashButton.TrashButtonPressed += _on_TrashButtonPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (_trashIsSpawning) {
			_trashIsSpawning = false;
			foreach (Trashbag bag in _trashbags) {
				if(bag.GlobalPosition.Y > _trashEndPoint.GlobalPosition.Y) {
					bag.Position = new Vector3(bag.Position.X, bag.Position.Y - _trashSpeed, bag.Position.Z);
					_trashIsSpawning = true; //keep stuff falling
				}
			}
		}
	}
	
	private void _on_TrashButtonPressed(int trashAmount)
	{
		_trashIsSpawning = true;
		GD.Print("TrashChute received " + trashAmount + " trash");
	}
}
