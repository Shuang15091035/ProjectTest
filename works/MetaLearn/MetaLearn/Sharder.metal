//
//  Sharder.metal
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/24.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#include <metal_stdlib>
#include <simd/simd.h>
#include "AAPLType.h"

using namespace metal;

typedef struct {
    packed_float3 position;
    packed_float4 color;
    packed_float2 uv;
} Vertex_t;

struct ColorInOut {
    float4 position [[position]];
    float4 color;
    float2 uv;
};

vertex ColorInOut passThroughVertex(device Vertex_t *vertex_arr[[buffer(0)]], constant AAPL::Uniforms &constUniform[[buffer(1)]], unsigned int vid [[vertex_id]]){
    
    ColorInOut out;
    float4 pos = float4(float3(vertex_arr[vid].position),1.0);
    out.position = constUniform.modelview_projection_matrix * pos;
    out.color = vertex_arr[vid].color;
    out.uv = vertex_arr[vid].uv;
    return out;
}

fragment half4 passThroughFragment(ColorInOut in [[stage_in]], texture2d<float> texas [[ texture(0) ]]){
//    half4 color = half4(in.color[0], in.color[1], in.color[2], in.color[3]);
    constexpr sampler defaultSampler;
    float4 rgba = texas.sample(defaultSampler, in.uv).rgba;
//    return half4(in.color[0] * rgba.r, in.color[1] * rgba.g, in.color[2] * rgba.b, in.color[3] * rgba.a);
    return half4(rgba.r, rgba.g, rgba.b, rgba.a);
}
