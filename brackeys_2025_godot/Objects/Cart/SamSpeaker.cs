using Godot;
using System;
using Godot.Collections;

public partial class SamSpeaker : Node3D
{
	[Export] private Array<AudioStreamPlayer3D> _audioPlayers;
	[Export] private Array<AudioStreamPlayer3D> _stabilityAudios;
	
	private Array<AudioStreamPlayer3D> _audioClips = new Array<AudioStreamPlayer3D>();
	
	//singleton
	private static SamSpeaker _instance;
	public static SamSpeaker Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new SamSpeaker();
			}
			return _instance;
		}
	}
	
	public override void _Ready()
	{
		_instance = this;
	}

	public void AddZoneSound(int index) {
		_audioClips.Add(_audioPlayers[index]);
	}
	public void AddStabilitySound(int index) {
		_audioClips.Add(_stabilityAudios[index]);
	}
	
	public async void PlaySounds() {
		foreach (var clip in _audioClips) {
			clip.Play();
			await ToSignal(GetTree().CreateTimer(clip.Stream.GetLength() + 0.2f), "timeout");
		}
		_audioClips.Clear();
	}
}
