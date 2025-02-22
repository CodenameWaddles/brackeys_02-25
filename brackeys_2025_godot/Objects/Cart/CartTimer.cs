using Godot;
using System;

public partial class CartTimer : Node3D
{
	[Export] private Viewport _textViewport;
	[Export] private MeshInstance3D _screenQuad;
	[Export] private RichTextLabel _timerText;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Get the ViewportTexture
		ViewportTexture viewportTexture = _textViewport.GetTexture();

		// Assign the texture to the shader material
		_screenQuad.MaterialOverride = new StandardMaterial3D();
		StandardMaterial3D shaderMat = (StandardMaterial3D)_screenQuad.MaterialOverride;
		shaderMat.AlbedoTexture = viewportTexture;
	}
	
	public void Reset()
	{
		_timerText.Text = "";
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public void UpdateTimerText(float time)
	{
		int rounded = Mathf.RoundToInt(time);
		String text = "\n ";
		if(rounded < 10) {
			text += "0";
		}
		if(rounded < 100) {
			text += "0";
		}
		text += rounded.ToString();
		_timerText.Text = text;
	}
}
