using Godot;
using System;

public partial class FinalZone : Node3D {
	[Export] private StaticBody3D _colliderBehind;
	[Export] private uint _collisionLayerBefore;
	[Export] private uint _collisionLayerAfter;
	[Export] private AudioStreamPlayer3D _noiseBehind;
	[Export] private AudioStreamPlayer3D _growlSound;
	[Export] private FogVolume _fog;
	[Export] private Light _escapeLight;
	[Export] private PackedScene _blackScreen;
	
	private Node3D _player;
	private HeadLight _headlight;
	private bool _noiseBehindPlayed = false;
	private bool _headLightCut = false;
	private bool _escapeLightCut = false;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_player = (Node3D) GameManager.Instance._player.GetNode("Head");
		_headlight = (HeadLight) _player.GetNode("Camera").GetNode("HeadLight");
		_colliderBehind.CollisionLayer = _collisionLayerBefore;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		var d = _player.GlobalPosition.DistanceTo(_escapeLight.GlobalPosition);
		if (d < 100) {
			if (_fog.Material is FogMaterial fogMaterial) {
				var inv = d / 100;
				fogMaterial.Density = Mathf.Lerp(0.1f, 1f, 1 - inv);
				float col = Mathf.Lerp(0.1f, 0.34f, inv);
				fogMaterial.Albedo = new Color(col, col, col, 1);
			}
		}
	}

	public void _on_area_3d_body_entered_change_collider(Node3D body) {
		if (body.IsInGroup("Player")) {
			AudioManager.Instance._timerMaxAndMin = new Vector2(0.5f, 1f);
			_colliderBehind.CollisionLayer = _collisionLayerAfter;
		}
	}

	public void _on_noise_behind_body_entered(Node3D body) {
		if (body.IsInGroup("Player") && !_noiseBehindPlayed) {
			_noiseBehind.Play();
			WaitPlayerTurnBack();
			_noiseBehindPlayed = true;
		}
	}

	public void _on_head_light_cut_area_body_entered(Node3D body) {
		if (body.IsInGroup("Player") && !_headLightCut) {
			_headlight.Flicker(true);
			_headLightCut = true;
		}
	}
	
	public void _on_escape_light_cut_area_body_entered(Node3D body) {
		if (body.IsInGroup("Player") && !_escapeLightCut) {
			_escapeLight.Flicker(true);
			_escapeLightCut = true;
			CreateBlackScreen();
		}
	}
	
	private async void WaitPlayerTurnBack() {
		while (true) {
			if(Mathf.Abs(_player.RotationDegrees.Y%360) is < 25 or > 335) {
				CutCartLamp();
				break;
			}
			await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
		}
	}

	private void CutCartLamp() {
		GameManager.Instance.Cart.CartLight.Flicker(true);
	}
	
	private async void CreateBlackScreen() {
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		CallDeferred("BlackScreen");
	}

	private void BlackScreen() {
		var black = (Node3D)_blackScreen.Instantiate();
		_player.AddChild(black);
		black.Position = new Vector3(0, 0, 0);
		black.Scale = new Vector3(80, 80, 80);
		BlackScreenFade(black);
	}
	
	private async void BlackScreenFade(Node3D black) {
		while (true) {
			black.Scale -= new Vector3(0.3f, 0.3f, 0.3f);
			if (black.Scale.X <= 0.2f) {
				break;
			}
			await ToSignal(GetTree().CreateTimer(0.01f), "timeout");
		}
		GameManager.Instance.DisablePlayer();
		AudioManager.Instance.EndSounds();
		await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
		_growlSound.Play();
		await ToSignal(GetTree().CreateTimer(6f), "timeout");
		GameManager.Instance.EndGame();
	}
}
