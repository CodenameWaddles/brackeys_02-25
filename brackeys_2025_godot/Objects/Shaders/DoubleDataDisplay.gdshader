shader_type spatial;

instance uniform float amplitude = 0.3;
instance uniform float period = 10.0;
instance uniform float speed = 3.0;
instance uniform float noise = 1.0;
const float spotX = 0.9;
const float spotSize = 0.02;
const float n = 6.0;
const float threshold = 0.003;
const float barSize = 0.003;

float function(float x){
	return (amplitude * sin(period*x + speed*TIME)) + 0.5
	+ (noise * 0.02*sin(150.0*x)*sin(period*x + speed*TIME));
}

void fragment() {
	vec3 color = vec3(0, 0, 0);

	float value = function(UV.x);
	vec2 point = vec2(spotX, function(spotX));
	float dist = distance(UV.xy, point);

	for(int i = 1; i <= int(n); i++){
		if((UV.x + 1.0/(2.0 * n) > float(i)/n - barSize && UV.x + 1.0/(2.0 * n) < float(i)/n + barSize)){
			color = vec3(0, 0.1, 0);
		}
		if((UV.y + 1.0/(2.0 * n) > float(i)/n - barSize && UV.y + 1.0/(2.0 * n) < float(i)/n + barSize)){
			color = vec3(0, 0.1, 0);
		}
	}
	if((UV.y > value - threshold && UV.y < value + threshold) && UV.x < spotX){
		color = vec3(0, 1, 0);
	}
	if(dist < spotSize){
		color = vec3(0, 1.0, 0);
	}
	ALBEDO.rgb = color;
}

//void light() {
	// Called for every pixel for every light affecting the material.
	// Uncomment to replace the default light processing function with this one.
//}

//const float threshold = 0.003;
//const float amplitude = 0.1;
//const float period = 20.0;
//const float speed = 1.0;
//
//float function(float x){
	//return (amplitude * sin(period * x) + 0.5);
//}
//
//void vertex() {
//
//}
//
//void fragment() {
	//vec3 color = vec3(0, 0, 0);
	//
	//if(UV.y < function(UV.x) + threshold && UV.y >  function(UV.x) - threshold){
		//color = vec3(0, 1, 1);
	//}
	//
	//ALBEDO.rgb = color;
//}
