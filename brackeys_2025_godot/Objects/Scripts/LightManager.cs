using Godot;
using System;
using Godot.Collections;

public partial class LightManager : Node
{
	// singleton stuff
	private static LightManager _instance;
	public static LightManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new LightManager();
			}
			return _instance;
		}
	}
	
	private Array<float> _flickeringFrequencies = new Array<float> {0.0001f, 0.0005f, 0.001f, 0.005f, 0.01f};
	private int index = 0;
	
	public float FlickeringFrequency { get; private set; }
	
	public override void _Ready()
	{
		FlickeringFrequency = _flickeringFrequencies[0];
	}
	
	public void NextFrequency() {
		FlickeringFrequency = _flickeringFrequencies[index];
		index = (index + 1) % _flickeringFrequencies.Count;
	}
}
