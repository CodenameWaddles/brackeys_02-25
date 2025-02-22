using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public partial class AudioManager : Node
{
	[Export] private Array<AudioStreamPlayer3D> _soundscapes;
	[Export] private Timer _timer;
	[Export] private Vector2 _timerMaxAndMin;

	public bool IsEnabled { private set; get; }
	private AudioStreamPlayer3D _playingSound;

	public override void _Ready()
	{
		Enable();
	}

	public void PlayRandomSoundscape()
	{
		Array<AudioStreamPlayer3D> soundPool = _soundscapes;
		soundPool.Remove(_playingSound);
		Random r = new Random();
		int index = r.Next(soundPool.Count);
		GD.Print("sound pool size : " + soundPool.Count);
		GD.Print("index picked : " + index);
		
		soundPool[index].Play();
		_playingSound = soundPool[index];
	}

	public void Enable()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		_timer.Start(rng.RandfRange(_timerMaxAndMin.X, _timerMaxAndMin.Y));
		IsEnabled = true;
		GD.Print("enabled");
	}

	public void Disable()
	{
		_playingSound = null;
		_timer.Stop();
		IsEnabled = false;
		GD.Print("disabled");
	}

	private void _on_timer_timeout()
	{
		PlayRandomSoundscape();
		RandomNumberGenerator rng = new RandomNumberGenerator();
		_timer.Start(rng.RandfRange(_timerMaxAndMin.X, _timerMaxAndMin.Y) + _playingSound.Stream.GetLength());
	}
	
}
