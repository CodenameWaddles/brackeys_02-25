using Godot;
using System;
using Godot.Collections;
using Array = Godot.Collections.Array;

public partial class AudioManager : Node
{
	[Export] private Array<AudioStreamPlayer3D> _soundscapes;
	[Export] public AudioStreamPlayer3D _alarm;
	[Export] private AudioStreamPlayer3D _ambiance;
	[Export] private Timer _timer;
	[Export] public Vector2 _timerMaxAndMin;
	
	private Array<float> _bangingFrequencies = new Array<float> { 0.002f, 0.005f };
	public float BangingFrequency { private set; get; }
	private int _frequencyIndex = 0;
	
	public bool IsEnabled { private set; get; }
	private AudioStreamPlayer3D _playingSound;
	private float _defaultRoomSize;
	private float _targetRoomSize;
	private bool _ambiancePlaying = true;
	
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
		AudioEffectReverb reverb = (AudioEffectReverb) AudioServer.GetBusEffect(0, 0);
		_defaultRoomSize = reverb.RoomSize;
		_targetRoomSize = _defaultRoomSize;
		BangingFrequency = _bangingFrequencies[0];
		PlayAmbiance();
	}

	public override void _Process(double delta)
	{
		AudioEffectReverb reverb = (AudioEffectReverb) AudioServer.GetBusEffect(0, 0);
		float currentRoomSize = (float) reverb.RoomSize;

		if (Mathf.Abs(currentRoomSize - _targetRoomSize) < 0.05f)
		{
			return;
		}
		else if (currentRoomSize - _targetRoomSize < 0.05f)
		{
			GD.Print("room size: " + currentRoomSize + " target room size: " + _targetRoomSize + ". increasing");
			reverb = new AudioEffectReverb();
			reverb.RoomSize = currentRoomSize + 0.1f * (float) delta;
			AudioServer.RemoveBusEffect(0,0);
			AudioServer.AddBusEffect(0, reverb);
		}else if (currentRoomSize - _targetRoomSize > 0.05f)
		{
			GD.Print("room size: " + currentRoomSize + " target room size: " + _targetRoomSize + ". deacreasing");
			reverb = new AudioEffectReverb();
			reverb.RoomSize = currentRoomSize - 0.1f * (float) delta;
			AudioServer.RemoveBusEffect(0,0);
			AudioServer.AddBusEffect(0, reverb);
		}
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
	
	public async void DoorBreak()
	{
		_alarm.Play();
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		GameManager.Instance.SendMessage(10); //breach detected
	}
	
	private void _on_timer_timeout()
	{
		if (IsEnabled) {
			PlayRandomSoundscape();
			RandomNumberGenerator rng = new RandomNumberGenerator();
			_timer.Start(rng.RandfRange(_timerMaxAndMin.X, _timerMaxAndMin.Y) + _playingSound.Stream.GetLength());
		}
	}
	
	public void NextBangingFrequency()
	{
		_frequencyIndex = (_frequencyIndex + 1) % _bangingFrequencies.Count;
		BangingFrequency = _bangingFrequencies[_frequencyIndex];
	}

	private async void PlayAmbiance() {
		while (_ambiancePlaying) {
			_ambiance.Play();
			await ToSignal(GetTree().CreateTimer(_ambiance.Stream.GetLength()), "timeout");
		}
	}

	public void EndSounds() {
		_ambiance.Stop();
		_alarm.Stop();
		_ambiancePlaying = false;
		IsEnabled = false;
	}

	public void setBusReverb(float roomSize)
	{
		_targetRoomSize = roomSize;
	}

	public void resetBusReverb()
	{
		_targetRoomSize = _defaultRoomSize;
	}
	
}
