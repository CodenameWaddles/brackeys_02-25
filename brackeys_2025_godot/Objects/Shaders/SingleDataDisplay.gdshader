shader_type spatial;

instance uniform int data = 3;
instance uniform float speed = 3.0;
const float n = 6.0;
const float threshold = 0.007;
const float barSize = 0.003;
const float dataSize = 0.12;

bool function(float d){
	bool res = false;
	if(d < dataSize * float(data)){
		res = true;
	}
	for(int i = 0; i <= data; i++){
		if(d > dataSize * float(i) - threshold && d < dataSize * float(i) + threshold){
			res = false;
		}
	}
	return res;
}

void fragment() {
	vec3 color = vec3(0, 0, 0);

	//float value = function(UV.x);
	//vec2 point = vec2(spotX, function(spotX));
	//float dist = distance(UV.xy, point);

	float dist = distance(UV, vec2(0.5, 0.5));

	for(int i = 1; i <= int(n); i++){
		if((UV.x + 1.0/(2.0 * n) > float(i)/n - barSize && UV.x + 1.0/(2.0 * n) < float(i)/n + barSize)){
			color = vec3(0, 0.1, 0);
		}
		if((UV.y + 1.0/(2.0 * n) > float(i)/n - barSize && UV.y + 1.0/(2.0 * n) < float(i)/n + barSize)){
			color = vec3(0, 0.1, 0);
		}
	}

	color += float(function(dist)) * vec3(0, 0.4, 0);

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
