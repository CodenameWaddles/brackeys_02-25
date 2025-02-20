using Godot;
using System;

public partial class ScreenText : RichTextLabel
{
	public override void _Ready() {
		
	}

	public async void TypeText(string text, float speed) {
		Text += " l";
		foreach (char c in text)
		{
			Text = Text.Insert(Text.Length - 2, c.ToString());
			await ToSignal(GetTree().CreateTimer(speed), "timeout");
		}
		Text = Text.Remove(Text.Length - 1);
		AddBar();
	}

	private void AddBar() {
		Text += "\n";
		Text += "\n";
		Text += "> ";
	}
}
