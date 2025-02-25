using Godot;
using System;

public partial class IntroZone : Node3D
{
	[Export] CanvasLayer introScreen;
	[Export] Timer blackScreenTimer;
	[Export] Timer introTimer;
	[Export] private Timer timeBeforeFirstMessage;
	[Export] private float gameVolume = -4;

	public override void _Ready()
	{
		introScreen.Visible = true;
		blackScreenTimer.Start();
		AudioServer.SetBusVolumeDb(0, -35);
	}

	public override void _Process(double delta)
	{
		if (!blackScreenTimer.IsStopped())
		{
			//ajust volume until game volume reached
			if (AudioServer.GetBusVolumeDb(0) < gameVolume)
			{
				AudioServer.SetBusVolumeDb(0, AudioServer.GetBusVolumeDb(0) + 0.1f);
			}
			return;
		}
		else
		{
			AudioServer.SetBusVolumeDb(0, gameVolume);
		}
		
		if (!introTimer.IsStopped())
		{
			ColorRect bg = (ColorRect) introScreen.GetChild(0);
			bg.SetModulate(new Color(1, 1, 1, (float)(introTimer.TimeLeft/introTimer.WaitTime)));
		}
		else if(introScreen.Visible)
		{
			AnimationPlayer anim = introScreen.GetNode<AnimationPlayer>("AnimationPlayer");
			if(!anim.IsPlaying())
			{
				introScreen.Visible = false;
				if (timeBeforeFirstMessage.IsStopped())
				{
					timeBeforeFirstMessage.Start();
				}
			}
		}
	}

	public void OnBlackScreenTimerTimeout()
	{
		blackScreenTimer.Stop();
		introTimer.Start();
	}
	
	public void OnIntroTimerTimeout()
	{
		introTimer.Stop();
		AnimationPlayer anim = introScreen.GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("flicker");
		GD.Print("anim playing : " + anim.CurrentAnimation);
	}
	
	public void OnTimeBeforeFirstMessageTimeout()
	{
		GameManager.Instance.SendMessage(-1);
	}
}
