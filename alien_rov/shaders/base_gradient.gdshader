shader_type spatial;
render_mode cull_disabled;

uniform sampler2D gradient_texture : source_color;

void vertex() {
	// Called for every vertex the material is visible on.
}

void fragment() {
	ROUGHNESS = 1.0;
	METALLIC = 0.0;
	ALBEDO = texture(gradient_texture, UV).rgb;
}

//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
