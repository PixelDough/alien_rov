shader_type spatial;
render_mode cull_disabled;

#include "res://shaders/underwater_shading.gdshaderinc"

uniform sampler2D gradient_texture : source_color;

varying vec3 world_pos;
varying vec3 camera_pos;

void vertex() {
	world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	camera_pos = (INV_VIEW_MATRIX * vec4(vec3(0.0), 1.0)).xyz;
}

void fragment() {
	ROUGHNESS = 1.0;
	METALLIC = 0.0;
	ALBEDO = texture(gradient_texture, UV).rgb;
}

void light() {
	vec3 absorption_color = light_absorption(
		ALBEDO,
		vec4(world_pos, distance(world_pos, camera_pos)),
		clamp(dot(NORMAL, LIGHT), 0.0, 1.0) * ATTENUATION,
		LIGHT_COLOR,
		LIGHT_IS_DIRECTIONAL
	);
    DIFFUSE_LIGHT += absorption_color;
}
