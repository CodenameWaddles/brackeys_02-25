using Godot;
using System;

public partial class ConsoleScreen : Interactable
{
	[Export] private Viewport _textViewport;
	[Export] private MeshInstance3D _screenQuad;
	[Export] private ScreenText _screenText;
	
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
		_screenText.TypeText("Welcome to the retro terminal...", 0.1f);
	}
}
