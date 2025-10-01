
global uniform vec3 fog_color;

vec3 rgb2hsl(vec3 color) {
    float max_color = max(max(color.r, color.g), color.b);
    float min_color = min(min(color.r, color.g), color.b);
    float delta = max_color - min_color;

    float lightness = (max_color + min_color) / 2.0;
    float hue = 0.0;
    float sat = 0.0;  // Rinomina la saturazione locale in 'sat'

    if (delta != 0.0) {
        sat = (lightness < 0.5) ? (delta / (max_color + min_color)) : (delta / (2.0 - max_color - min_color));
        
        if (max_color == color.r) {
            hue = (color.g - color.b) / delta + (color.g < color.b ? 6.0 : 0.0);
        } else if (max_color == color.g) {
            hue = (color.b - color.r) / delta + 2.0;
        } else {
            hue = (color.r - color.g) / delta + 4.0;
        }

        hue /= 6.0;
    }

    return vec3(hue, sat, lightness);
}

vec3 light_absorption(
	vec3 albedo,
	vec4 world_pos,
	float light_attenuation,
	vec3 light_color,
	bool light_is_directional
) {
	float directional_light_influence = light_is_directional ? 1.0 : 0.0;
	if (light_is_directional) {
		light_attenuation = mix(light_attenuation, 1.0, 0.5);
	}
	float absorption_amount = 0.001 + (directional_light_influence * 0.015);
	light_attenuation *= rgb2hsl(light_color / PI).z;
	light_attenuation *= 
		mix(1.0, 1.0 + clamp(world_pos.y * absorption_amount, -1, 0), directional_light_influence);
	float depth = world_pos.w;
	float depth_modifier = clamp(depth * absorption_amount, 0, 1);
	float light_attenuation_depth = light_attenuation * mix(1, 0, depth_modifier);
	vec3 light_color_blended = light_attenuation * light_color / PI;
	//float r_new = mix(light_color.r, fog_color.r, 1.0-pow(light_attenuation_depth, 3.2));
	//float g_new = mix(light_color.g, fog_color.g, 1.0-pow(light_attenuation_depth, 2));
	//float b_new = mix(light_color.b, fog_color.b, 1.0-pow(light_attenuation_depth, 1));
	float r_new = light_color.r * pow(light_attenuation_depth, 3.2);
	float g_new = light_color.g * pow(light_attenuation_depth, 2);
	float b_new = light_color.b * pow(light_attenuation_depth, 1);
	vec3 final_color = vec3(r_new, g_new, b_new);
	final_color = mix(final_color, light_attenuation * light_color, max(sign(world_pos.y), 0.0));
	return final_color / PI;
}
