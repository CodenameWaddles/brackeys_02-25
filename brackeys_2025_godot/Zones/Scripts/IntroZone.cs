using Godot;
using System;

public partial class IntroZone : Node3D
{
	[Export] CanvasLayer introScreen;
	[Export] Timer blackScreenTimer;
	[Export] Timer introTimer;
	[Export] private Timer timeBeforeFirstMessage;

	public override void _Ready()
	{
		introScreen.Visible = true;
		blackScreenTimer.Start();
	}

	public override void _Process(double delta)
	{
		if(!blackScreenTimer.IsStopped()) return;
		
		if (!introTimer.IsStopped())
		{
			Sprite2D sprite = (Sprite2D) introScreen.GetChild(0);
			sprite.SetModulate(new Color(1, 1, 1, (float)(introTimer.TimeLeft/introTimer.WaitTime)));
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
