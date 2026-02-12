shader_type spatial;

// Lightmap
uniform sampler2D Light_Map;

//[texture-headers]

uniform float Tile_Size = 1.0;
uniform int World_Size = 256;

// World Position X Z
varying vec2 world_xz;

// R, G, B, A -> Self, Down, Down and Right, Right
varying vec4 tile_alpha_corners;  
// R, G, B, A -> Layer1, Layer2, Alpha, None
varying vec4 tile_layer_info;


// Helper function
vec3 get_tile_color(int tile_id, vec2 uv) {
	//[tile_colors]
	return vec3(1.0, 0.0, 1.0); // Magenta for error
}


void vertex() {
    world_xz = (vec4(VERTEX, 1.0)).xz;

	// Vertex store Alpha colors as CUSTOM0
    tile_alpha_corners = CUSTOM0;
    // Vertex store Tile Data as CUSTOM1
	tile_layer_info = CUSTOM1;
}

void fragment() {
    vec2 tile_coord = floor(world_xz / Tile_Size);
    vec2 tile_uv = fract(world_xz / Tile_Size);

    vec2 index_uv = tile_coord / float(World_Size);
    // vec2 index_uv_flip_y = vec2(index_uv.x, 1.0 - index_uv.y);

    int base_tile_id    = int(tile_layer_info.r * 255.0 + 0.5);
    int overlay_tile_id = int(tile_layer_info.g * 255.0 + 0.5);
    float overlay_alpha = tile_layer_info.b;

    // Revive tile light from light map
    vec3 tile_light = texture(Light_Map, index_uv).rgb;

    // Base tile
    vec3 base_color = get_tile_color(base_tile_id, tile_uv);

    // Bilinear interpolation
    float a1 = tile_alpha_corners.r; // Self
    float a2 = tile_alpha_corners.g; // Down
    float a3 = tile_alpha_corners.b; // Down and Right
    float a4 = tile_alpha_corners.a; // Right

    float blend_alpha = mix(
        mix(a1, a2, tile_uv.x), // Mix Self and Down (Left side)
        mix(a4, a3, tile_uv.x), // Mix Right and Down + Right (Right side)
        tile_uv.y
    ); // Mix 

    bool layer2Valid = overlay_tile_id < 254;

    vec3 blended;
    blended = get_tile_color(base_tile_id, tile_uv);
    if (blend_alpha > 0.0 && layer2Valid) {
        vec3 overlay = get_tile_color(overlay_tile_id, tile_uv);
        blended = mix(blended, overlay, blend_alpha);
    }

    ALBEDO = blended * tile_light;
    METALLIC = 1.0;
}
//void light() {
//	// Called for every pixel for every light affecting the material.
//	// Uncomment to replace the default light processing function with this one.
//}
