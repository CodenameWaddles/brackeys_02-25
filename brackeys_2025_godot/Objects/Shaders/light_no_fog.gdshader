shader_type spatial;
render_mode unshaded, fog_disabled;

instance uniform vec3 color = vec3(1, 1, 0.2);

const float fogMultiplier = 10.0;

varying float depth;

void vertex() {
	depth = distance(NODE_POSITION_WORLD, CAMERA_POSITION_WORLD);
}

void fragment() {
	// Called for every pixel the material is visible on.
	//ALBEDO.rgb = vec3(depth/10.0);
	ALBEDO.rgb = clamp((color*10.0)/depth, vec3(0.0, 0.0, 0.0), color);
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
