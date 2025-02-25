using Godot;
using System;
using System.Threading.Tasks;

public partial class IntroBlackScreen : CanvasLayer
{
	[Export] private Label _label;
	[Export] private AudioStreamPlayer _typingSound;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TypeIntroText();
	}

	public async void TypeIntroText() {
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		TypeText("Forgotten Abyss", 0.12f);
	}
	
	public async void TypeText(string text, float speed) {
		_label.Text += "l";
		foreach (char c in text)
		{
			_label.Text = _label.Text.Insert(_label.Text.Length - 1, c.ToString());
			_typingSound.Play();
			await ToSignal(GetTree().CreateTimer(speed), "timeout");
		}
		_label.Text = _label.Text.Remove(_label.Text.Length - 1);
	}

	public async Task DeleteText(float speed) {
		_label.Text += "l";
		while (_label.Text.Length > 2) {
			_label.Text = _label.Text.Remove(_label.Text.Length - 2, 1);
			_typingSound.Play();
			await ToSignal(GetTree().CreateTimer(speed), "timeout");
		}
		_label.Text = _label.Text.Remove(0, 1);
		await ToSignal(GetTree().CreateTimer(speed), "timeout");
		_label.Text = "";
	}
}
