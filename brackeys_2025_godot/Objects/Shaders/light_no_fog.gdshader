shader_type spatial;
render_mode unshaded, fog_disabled;

uniform vec3 color = vec3(1, 1, 0);

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	// Called for every pixel the material is visible on.
	ALBEDO.rgb = color;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}
