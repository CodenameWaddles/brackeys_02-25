using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class BreakPipe : Hazard
{
	  
	[Export] private Node3D _pipeLeak;
	[Export] private Node3D _pipeFixed;
	[Export] private AudioStreamPlayer3D _tooBreakSound;
	[Export] private AudioStreamPlayer3D _leakSound;
	[Export] private int _fixTime = 150;
    
	private int _fixProgress;
	private GameManager _main;
	private int _fixAttempt = 0;
    
	public override void _Ready() {
		Type = Pickupable.PickupType.Blowtorch;
		_main = (GameManager) GetTree().Root.GetChild(0);
		MakeInteractable();
	}
    
	protected override void ActivateSpecific() {
		switch (_fixAttempt) {
			case 0:
				if(_fixProgress < _fixTime) {
					_fixProgress++;
				} else {
					_fixAttempt++;
					BreakBlowtorch();
				}
				break;
			case 1:
				MakeUninteractable();
				break;
		}
		
	}

	protected override void MakeInteractableSpecific()
	{
		IsSolved = false;
		_fixProgress = 0;
		_pipeFixed.Visible = false;
		_pipeLeak.Visible = true;
		_leakSound.Play();
	}

	protected override void MakeUninteractableSpecific()
	{
		IsSolved = true;
		_pipeFixed.Visible = true;
		_pipeLeak.Visible = false;
		_leakSound.Stop();
		_main._currentScene.UpdateIntegrityPercentage();
	}
	
	private void BreakBlowtorch() {
		playerInteraction player = (playerInteraction)GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("Camera");
		player.held.QueueFree();
		player.DropItem();
		_tooBreakSound.Play();
		GameManager.Instance.BreakBlowtorchEvent();
		Type = Pickupable.PickupType.Tape;
	}
}
