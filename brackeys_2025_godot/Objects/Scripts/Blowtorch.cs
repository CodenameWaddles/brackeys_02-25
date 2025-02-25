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
        GameManager gm = (GameManager)GetTree().Root.GetChild(0);
        playerInteraction player = (playerInteraction) gm._player.FindChild("Head").FindChild("Camera");
        if(player.held == null) return;
        if(player.held.Type != PickupType.Blowtorch) return;

        GpuParticles3D particles =
            (GpuParticles3D)gm._player.FindChild("Head").FindChild("Hand").GetChild(0).FindChild("blowtorch_particles");
        
        if (isBeingUsed)
        {
            
            if (!useSound.IsPlaying())
            {
                useSound.Play();
            }
            if (!particles.IsEmitting())
            {
                   particles.Emitting = true; 
            }
        }
        else
        {
            if (useSound.IsPlaying())
            {
                useSound.Stop();
            }
            if (particles.IsEmitting())
            {
                particles.Emitting = false;
            }
        }

        //particles.Emitting = true;

    }
}