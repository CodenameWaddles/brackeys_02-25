using Godot;
using System;
using Godot.Collections;

public partial class ConsoleScreen : Interactable
{
	[Export] private Viewport _textViewport;
	[Export] private MeshInstance3D _screenQuad;
	[Export] private ScreenText _screenText;
	
	private Array<String> _messages = new Array<String>();
	
	public override void _Ready()
	{
		// Get the ViewportTexture
		ViewportTexture viewportTexture = _textViewport.GetTexture();

		// Assign the texture to the shader material
		ShaderMaterial shaderMat = (ShaderMaterial)_screenQuad.GetSurfaceOverrideMaterial(0);
		shaderMat.SetShaderParameter("screen_texture", viewportTexture);
		
		MakeInteractable();
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
		_screenText.TypeText(message, 0.1f);

		MakeUninteractable();
	}

	public void AddMessage(string message) {
		MakeInteractable();
		_messages.Add(message);
		if(_messages.Count == 1) {
			_screenText.NewMessage();
		}
	}
}
