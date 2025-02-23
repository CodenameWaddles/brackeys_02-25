using Godot;
using System;

public partial class Blowtorch : Pickupable
{

    [Export] private AudioStreamPlayer3D useSound;
    
    public bool isBeingUsed;
    
    public override void _Ready() {
        _isInteractable = true;
        Type = PickupType.Blowtorch;
    }

    public override void _Process(double delta)
    {
        if (isBeingUsed)
        {
            if (!useSound.IsPlaying())
            {
                useSound.Play();
            }
        }
        else
        {
            if (useSound.IsPlaying())
            {
                useSound.Stop();
            }
        }
    }
}