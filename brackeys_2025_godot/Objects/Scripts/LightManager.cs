using Godot;
using System;

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
	
	public float FlickeringFrequency { get; private set; } = 0.0001f;
	
}
