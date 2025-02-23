using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class BlowtorchableDoor : Hazard {
    
    [Export] private Node3D _doorFixed;
    [Export] private Node3D _doorDamaged;
    [Export] private int _fixTime = 200;
    
    [Export] private bool _isHazard = true;
    [Export] private AudioStreamPlayer3D _tapeSound;

    //private bool fixing;

    public override void _Ready() {
        if (_isHazard) {
            //Type = Pickupable.PickupType.Blowtorch;
            _main = (GameManager) GetTree().Root.GetChild(0);
            MakeInteractable();
        }
        else {
            MakeUninteractable();
        }
    }

    private int _fixProgress;
    private GameManager _main;
    
    protected override void ActivateSpecific() {
        switch (Type)
        {
            case Pickupable.PickupType.Blowtorch:
                playerInteraction player = (playerInteraction)GetTree().Root.GetChild(0).FindChild("Character", true).FindChild("Head").FindChild("Camera");
                Blowtorch blowtorch = (Blowtorch)player.held;
                if(_fixProgress < _fixTime) {
                    _fixProgress++;
                    blowtorch.isBeingUsed = true;
                } else
                {
                    blowtorch.isBeingUsed = false;
                    MakeUninteractable();
                }
                break;
            case Pickupable.PickupType.Tape:
                if(_fixProgress < _fixTime) {
                    _fixProgress++;
                } else
                {
                    MakeUninteractable();
                }
                break;
        }
    }

    protected override void MakeInteractableSpecific()
    {
        IsSolved = false;
        _fixProgress = 0;
        _doorFixed.Visible = false;
        _doorDamaged.Visible = true;
    }

    protected override void MakeUninteractableSpecific()
    {
        if (Type == Pickupable.PickupType.Tape)
        {
            _tapeSound.Play();
        }
        IsSolved = true;
        _doorFixed.Visible = true;
        _doorDamaged.Visible = false;
        _main._currentScene.UpdateIntegrityPercentage();
    }
}
