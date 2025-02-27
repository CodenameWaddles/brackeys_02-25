using Godot;
using System;
using Brackeys_2025_Godot.Objects.Scripts;

public partial class LeakablePipe : Hazard {
    
    [Export] private Node3D _pipeLeak;
    [Export] private Node3D _pipeFixed;
    [Export] private int _fixTime = 150;
    [Export] private AudioStreamPlayer3D _leakSound;
    [Export] private AudioStreamPlayer3D _tapeSound;
    
    [Export] private bool _isHazard = true;

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

    public override void _Process(double delta)
    {
        if (!IsSolved && !_leakSound.IsPlaying())
        {
            _leakSound.Play();
        }else if (IsSolved && _leakSound.IsPlaying())
        {
            _leakSound.Stop();
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
                } else
                {
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
        _pipeFixed.Visible = false;
        _pipeLeak.Visible = true;
        //_main._currentScene.UpdateIntegrityPercentage(); //osef psk on le suprime anyway ???? 
    }

    protected override void MakeUninteractableSpecific()
    {
        if (Type == Pickupable.PickupType.Tape)
        {
            _tapeSound.Play();
        }
        IsSolved = true;
        _pipeFixed.Visible = true;
        _pipeLeak.Visible = false;
        _main._currentScene.UpdateIntegrityPercentage();
    }
}