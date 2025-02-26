using Godot;
using System;

public partial class FailedRoomScreen : CanvasLayer
{
	[Export] private Label _label;
	[Export] private AudioStreamPlayer _typingSound;
	
	public async void TypeText() {
		String text = " You were forgotten \n in the depths";
		_label.Text = "";
		_label.Text += " l";
		foreach (char c in text)
		{
			_label.Text = _label.Text.Insert(_label.Text.Length - 2, c.ToString());
			_typingSound.Play();
			await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		}
		_label.Text = _label.Text.Remove(_label.Text.Length - 1);
		await ToSignal(GetTree().CreateTimer(2f), "timeout");
		_label.Text += "l";
		while (_label.Text.Length > 2) {
			_label.Text = _label.Text.Remove(_label.Text.Length - 2, 1);
			_typingSound.Play();
			await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		}
		_label.Text = _label.Text.Remove(0, 1);
		await ToSignal(GetTree().CreateTimer(0.05f), "timeout");
		_label.Text = "";
	}
}
