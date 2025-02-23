using Godot;
using System;
using Godot.Collections;

public partial class CartMap : Node3D
{
	[Export] private Array<MeshInstance3D> _mapLightMeshes;
	[Export] private Array<OmniLight3D> _mapLights;

	public override void _Ready() {
		DeactivateMapLights();
	}

	public void ActivateMapLight(int index) {
		_mapLightMeshes[index].SetInstanceShaderParameter("color", new Vector3(0.9f, 0.65f, 0.16f));
		_mapLights[index].LightEnergy = 1;
	}
	
	public void DeactivateMapLights() {
		for(int index = 0; index < _mapLightMeshes.Count; index++) {
			_mapLightMeshes[index].SetInstanceShaderParameter("color", new Vector3(0.05f, 0.05f, 0.05f));
			_mapLights[index].LightEnergy = 0;
		}
	}
}
