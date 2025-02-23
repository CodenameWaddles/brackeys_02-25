using Godot;
using System;

public partial class ScreenText : RichTextLabel
{
	[Export] private AudioStreamPlayer3D _typingSound;
	
	public override void _Ready() {
		for(int i = 0; i < 11; i++) {
			Text += "\n";
		}
	}

	public async void TypeText(string text, float speed) {
		Text += " l";
		foreach (char c in text)
		{
			Text = Text.Insert(Text.Length - 2, c.ToString());
			_typingSound.Play();
			await ToSignal(GetTree().CreateTimer(speed), "timeout");
		}
		Text = Text.Remove(Text.Length - 1);
	}

	public void NewMessage() {
		Text += "\n";
		Text += "\n";
		Text += "> ";
	}
}
