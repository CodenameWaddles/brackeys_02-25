using Godot;
using System;
using Godot.Collections;

public partial class ConsoleScreen : Interactable
{
	[Export] private Viewport _textViewport;
	[Export] private MeshInstance3D _screenQuad;
	[Export] private ScreenText _screenText;
	[Export] private MeshInstance3D _greenLightMesh;
	[Export] private OmniLight3D _greenLight;
	
	[Export] private AudioStreamPlayer3D _notificationSound;
	
	private Array<String> _messages = new Array<String>();
	
	private bool _isMakingNoise = false;
	
	public override void _Ready()
	{
		// Get the ViewportTexture
		ViewportTexture viewportTexture = _textViewport.GetTexture();

		// Assign the texture to the shader material
		ShaderMaterial shaderMat = (ShaderMaterial)_screenQuad.GetSurfaceOverrideMaterial(0);
		shaderMat.SetShaderParameter("screen_texture", viewportTexture);
		
		//MakeInteractable();
	}

	protected override void ActivateSpecific() {
		var count = _messages.Count;
		String message = "";
		for(int i = 0; i < count; i++) {
			message += _messages[0];
			_messages.RemoveAt(0);
			if(count>0 && i < count-1) {
				message += "\n \n> ";
			}
		}
		_screenText.TypeText(message, 0.075f);
		SamSpeaker.Instance.PlaySounds();
		MakeUninteractable();
	}

	public void AddMessage(string message) {
		MakeInteractable();
		_messages.Add(message);
		if(_messages.Count == 1) {
			_screenText.NewMessage();
		}
		if (!_isMakingNoise) {
			MakeNoise();
		}
	}
	
	protected override void MakeInteractableSpecific() {
		_greenLightMesh.SetInstanceShaderParameter("color", new Vector3(0.05f, 1, 0.05f));
		_greenLight.LightEnergy = 1;
	}
	
	protected override void MakeUninteractableSpecific() {
		_greenLightMesh.SetInstanceShaderParameter("color", new Vector3(0.05f, 0.05f, 0.05f));
		_greenLight.LightEnergy = 0;
		_isMakingNoise = false;
	}
	
	private async void MakeNoise() {
		_isMakingNoise = true;
		RandomNumberGenerator rng = new RandomNumberGenerator();
		while(_isMakingNoise) {
			_notificationSound.Play();
			await ToSignal(GetTree().CreateTimer(2f), "timeout");
		}
	}
}
