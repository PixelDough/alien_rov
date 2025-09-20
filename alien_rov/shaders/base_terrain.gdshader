shader_type spatial;
//render_mode ambient_light_disabled;

#include "res://shaders/underwater_shading.gdshaderinc"

varying vec3 world_pos;
varying vec3 camera_pos;

// Terrain Stuff

// This uniform is assigned internally by the voxel engine.
// Layout: 00000000 00000000 0000000 00xxyyzz
// Where:
// - xx are respectively -x and x transitions,
// - yy are respectively -y and y transitions,
// - zz are respectively -z and z transitions,
uniform int u_transition_mask;

float get_transvoxel_secondary_factor(int idata) {
    int transition_mask = u_transition_mask & 0xff;

    int cell_border_mask = (idata >> 0) & 63; // Which sides the cell is touching
    int vertex_border_mask = (idata >> 8) & 63; // Which sides the vertex is touching
    // If the vertex is near a side where there is a low-resolution neighbor,
    // move it to secondary position
    int m = transition_mask & cell_border_mask;
    float t = float(m != 0);
    // If the vertex lies on one or more sides, and at least one side has no low-resolution neighbor,
    // don't move the vertex.
    t *= float((vertex_border_mask & ~transition_mask) == 0);

    return t;
}

vec3 get_transvoxel_position(vec3 vertex_pos, vec4 fdata) {
    int idata = floatBitsToInt(fdata.a);

    // Move vertices to smooth transitions
    float secondary_factor = get_transvoxel_secondary_factor(idata);
    vec3 secondary_position = fdata.xyz;
    vec3 pos = mix(vertex_pos, secondary_position, secondary_factor);

    // If the mesh combines transitions and the vertex belongs to a transition,
    // when that transition isn't active we change the position of the vertices so
    // all triangles will be degenerate and won't be visible.
    // This is an alternative to rendering them separately,
    // which has less draw calls and less mesh resources to create in Godot.
    // Ideally I would tweak the index buffer like LOD does but Godot does not
    // expose anything to use it that way.
    int itransition = (idata >> 16) & 0xff; // Is the vertex on a transition mesh?
    float transition_cull = float(itransition == 0 || (itransition & u_transition_mask) != 0);
    pos *= transition_cull;

    return pos;
}
// End of Terrain Stuff

void vertex() {
    VERTEX = get_transvoxel_position(VERTEX, CUSTOM0);
	world_pos = (MODEL_MATRIX * vec4(VERTEX, 1.0)).xyz;
	camera_pos = (INV_VIEW_MATRIX * vec4(vec3(0.0), 1.0)).xyz;
    //...
}

void fragment() {
	ALBEDO = vec3(1.0, 1.0, 1.0);
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