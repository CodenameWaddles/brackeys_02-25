using Godot;
using System;
using Godot.Collections;

public partial class TrashChute : Node3D
{
	[Export] private TrashButton _trashButton;
	[Export] private Array<Trashbag> _trashbags;
	[Export] private Node3D _trashEndPoint;
	[Export] private float _trashSpeed = 0.01f;
	[Export] private AudioStreamPlayer3D _trashOpenSound;
	
	private bool _trashIsSpawning = false;
	private float _trashFallingT = 0;
	private Dictionary<Trashbag, bool> _trashbagSounds = new Dictionary<Trashbag, bool>();
	
	public override void _Ready()
	{
		foreach (var bag in _trashbags) {
			_trashbagSounds.Add(bag, false);
		}
		_trashButton.TrashButtonPressed += _on_TrashButtonPressed;
	}
	
	public override void _Process(double delta)
	{
		if (_trashIsSpawning) {
			_trashIsSpawning = false;
			foreach (Trashbag bag in _trashbags) {
				if(bag.GlobalPosition.Y > _trashEndPoint.GlobalPosition.Y) {
					bag.Position = new Vector3(bag.Position.X, bag.Position.Y - _trashSpeed, bag.Position.Z);
					_trashIsSpawning = true; //keep stuff falling
				}else if(!_trashbagSounds[bag]){
					bag.PlayGroundSound();
					_trashbagSounds[bag] = true;
				}
			}
		}
	}
	
	private void _on_TrashButtonPressed(int trashAmount)
	{
		_trashIsSpawning = true;
		_trashOpenSound.Play();
		GD.Print("TrashChute received " + trashAmount + " trash");
	}
}
