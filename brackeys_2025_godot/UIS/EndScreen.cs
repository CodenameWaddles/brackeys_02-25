using Godot;
using System;
using System.Threading.Tasks;
using Godot.Collections;

public partial class EndScreen : CanvasLayer {
	
	[Export] private RichTextLabel _creditsLabel;
	[Export] private AudioStreamPlayer _typingSound;
	[Export] private AudioStreamPlayer _music;
	
	private Array<String> _credits = new() {
		"> Forgotten abyss",
		"\n \n> Made for Brackeys Game Jam 2025.1",
		"\n \n> Game Design",
		"\n \n> Justin Lépine",
		"\n \n> Programming",
		"\n \n> Rémi Ducourtieux and Clovie Thouvenot Oudart",
		"\n \n> Art",
		"\n \n> Gwenn Tran Son Tay",
		"\n \n> Music and sound",
		"\n \n> Térence Mathias",
		"\n \n> Thank you for playing",
	};
	
	public async void StartCredits() {
		_music.Play();
		foreach (String message in _credits) {
			await TypeText(message, 0.05f);
			await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
		}
		
	}
	
	public async Task TypeText(string text, float speed) {
		_creditsLabel.Text += " l";
		foreach (char c in text)
		{
			_creditsLabel.Text = _creditsLabel.Text.Insert(_creditsLabel.Text.Length - 2, c.ToString());
			_typingSound.Play();
			await ToSignal(GetTree().CreateTimer(speed), "timeout");
		}
		_creditsLabel.Text = _creditsLabel.Text.Remove(_creditsLabel.Text.Length - 1);
		
	}
	
	public void NewMessage() {
		_creditsLabel.Text += "\n";
		_creditsLabel.Text += "\n";
		_creditsLabel.Text += "> ";
	}
}
