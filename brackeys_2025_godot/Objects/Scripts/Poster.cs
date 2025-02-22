using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class Poster : Hazard
{
	[Export] private Node3D _posterFlat;
	[Export] private Node3D _posterFolded;
	[Export] private AudioStreamPlayer3D _tapeSound;
	
	public override void _Ready()
	{
		Type = Pickupable.PickupType.Tape;
		MakeInteractable();
	}

	public override void _Process(double delta)
	{
	}

	protected override void MakeInteractableSpecific() {
		IsSolved = false;
		_posterFlat.Visible = false;
		_posterFolded.Visible = true;
	}
	
	protected override void MakeUninteractableSpecific() {
		IsSolved = true;
		_posterFlat.Visible = true;
		_posterFolded.Visible = false;
	}

	protected override void ActivateSpecific() {
		_tapeSound.Play();
		MakeUninteractable();
	}
}
