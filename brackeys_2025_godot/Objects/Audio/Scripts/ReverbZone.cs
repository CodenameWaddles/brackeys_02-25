using Godot;
using System;

public partial class ReverbZone : Node3D
{
	[Export] private bool useDefaultReverb;
	[Export] private AudioEffectReverb _reverb = new AudioEffectReverb();
	[Export] private Area3D _area;

	public override void _Ready()
	{
		_area.BodyEntered += _on_area_entered;
	}

	private void _on_area_entered(Node3D body)
	{

		if (!body.IsInGroup("Player")) return;
		
		if (useDefaultReverb)
		{
			AudioManager.Instance.resetBusReverb();
		}
		else
		{
			AudioManager.Instance.setBusReverb(_reverb);
		}
	}
	
}
