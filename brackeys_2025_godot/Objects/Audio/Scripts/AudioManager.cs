using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public partial class AudioManager : Node
{
	[Export] private Array<AudioStreamPlayer3D> _soundscapes;
	[Export] public AudioStreamPlayer3D _alarm;
	[Export] private Timer _timer;
	[Export] private Vector2 _timerMaxAndMin;
	
	private Array<float> _bangingFrequencies = new Array<float> { 0.002f, 0.005f };
	public float BangingFrequency { private set; get; }
	private int _frequencyIndex = 0;

	public bool IsEnabled { private set; get; }
	private AudioStreamPlayer3D _playingSound;
	
	// singleton
	private static AudioManager _instance;
	public static AudioManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new AudioManager();
			}
			return _instance;
		}
	}
	
	public override void _Ready()
	{
		_instance = this;
		BangingFrequency = _bangingFrequencies[0];
	}
	
	public void PlayRandomSoundscape()
	{
		Array<AudioStreamPlayer3D> soundPool = _soundscapes.Duplicate();
		soundPool.Remove(_playingSound);
		Random r = new Random();
		int index = r.Next(soundPool.Count);
		soundPool[index].Play();
		_playingSound = soundPool[index];
	}

	public void Enable()
	{
		RandomNumberGenerator rng = new RandomNumberGenerator();
		_timer.Start(rng.RandfRange(_timerMaxAndMin.X, _timerMaxAndMin.Y));
		IsEnabled = true;
	}

	public void Disable()
	{
		_playingSound = null;
		_timer.Stop();
		IsEnabled = false;
	}
	
	private void _on_timer_timeout()
	{
		PlayRandomSoundscape();
		RandomNumberGenerator rng = new RandomNumberGenerator();
		_timer.Start(rng.RandfRange(_timerMaxAndMin.X, _timerMaxAndMin.Y) + _playingSound.Stream.GetLength());
	}
	
	public void NextBangingFrequency()
	{
		_frequencyIndex = (_frequencyIndex + 1) % _bangingFrequencies.Count;
		BangingFrequency = _bangingFrequencies[_frequencyIndex];
	}
	
}
