using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class Poster : Hazard
{
	[Export] private Node3D _posterFlat;
	[Export] private Node3D _posterFolded;
	[Export] private AudioStreamPlayer3D _tapeSound;
	[Export] private bool _isHazard = true;

	public override void _Ready() {
		Type = Pickupable.PickupType.Tape;
		if (_isHazard) {
			MakeInteractable();
		}
		else {
			MakeUninteractable();
		}
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
		GameManager.Instance._currentScene.UpdateIntegrityPercentage();
	}
}
