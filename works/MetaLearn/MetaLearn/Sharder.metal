//
//  Sharder.metal
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/24.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#include <metal_stdlib>
#include <metal_graphics>
#include <metal_matrix>
#include <metal_geometric>
#include <metal_math>
#include <metal_texture>
#include <simd/simd.h>

using namespace metal;

constant float3 light_position = float3(0.0, 1.0, -1.0);

typedef struct
{
    packed_float3 position;
    packed_float3 normal;
} vertex_t;

struct ColorInOut {
    float4 position [[position]];
    half4 color;
};

struct Uniforms
{
    simd::float4x4 modelview_projection_matrix;
    simd::float4x4 normal_matrix;
    simd::float4   ambient_color;
    simd::float4   diffuse_color;
    int            multiplier;
} __attribute__ ((aligned (256)));

// vertex shader function
vertex ColorInOut passThroughVertex(device vertex_t* vertex_array [[ buffer(0) ]],
                                  constant Uniforms& constants [[ buffer(1) ]],
                                  unsigned int vid [[ vertex_id ]])
{
    ColorInOut out;
    
    float4 in_position = float4(float3(vertex_array[vid].position), 1.0);
    out.position = constants.modelview_projection_matrix * in_position;
    
    float3 normal = vertex_array[vid].normal;
    float4 eye_normal = normalize(constants.normal_matrix * float4(normal, 0.0));
    float n_dot_l = dot(eye_normal.rgb, normalize(light_position));
    n_dot_l = fmax(0.0, n_dot_l);
    
    out.color = half4(constants.ambient_color + constants.diffuse_color * n_dot_l);
    
    return out;
}

// fragment shader function
fragment half4 passThroughFragment(ColorInOut in [[stage_in]], texture2d<float> texas[[texture(0)]])
{
    return in.color;
};
