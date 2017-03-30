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
    
//    float x = 1.0f, y = 2.0f, z = 3.0f, w = 4.0f;
//    float3 floatData = float3(x, y, z);
//    
//    float fval = 3.0f;
//    float4x4 matrix = float4x4(fval);
//    
    
//    texture2d<float, uint,half>; //texture<T,access a = access::sample>
//    texturecube_array;//texture T:<float, int ,uint,short,half,ushort> depth texture type:T<float>
//    texture2d_ms;//multisampled texture, only the read qualifier is supported; depth texture qualifier only sample or read
//    depth2d;
//    depth2d_array;
//    depth2d_ms;
//    depthcube;
//    depthcube_array<float>;
//    atomic_int
    
    
    ColorInOut out;
    float4 pos = float4(float3(vertex_arr[vid].position),1.0);
    out.position = constUniform.modelview_projection_matrix * pos;
    out.color = vertex_arr[vid].color;
    out.uv = vertex_arr[vid].uv;
    return out;
}

//void foo (texture2d<float> imgA [[texture(0)]], texture2d<float, access::read> imgB [[texture(2)]], texture2d<float, access::read> imgC [[texture(2)]]){
//    constexpr sampler defaultSampler; //默认取样器
//}

fragment half4 passThroughFragment(ColorInOut in [[stage_in]], texture2d<float> texas [[ texture(0) ]], sampler smp [[sampler(3)]]){
//    half4 color = half4(in.color[0], in.color[1], in.color[2], in.color[3]);
    constexpr sampler defaultSampler;
//    constexpr sampler s(coord::normalized, address::clamp_to_zero, filter::linear, compare_func::less, max_anisotropy(10), lod_clamp(0.0f, MAXFLOAT));
    
    float4 rgba = texas.sample(defaultSampler, in.uv).rgba;
//    return half4(in.color[0] * rgba.r, in.color[1] * rgba.g, in.color[2] * rgba.b, in.color[3] * rgba.a);
    return half4(rgba.r, rgba.g, rgba.b, rgba.a);
}


kernel void my_kernel(const array<texture2d<float>, 10> src [[texture(0)]], texture2d<float, access::write> dst [[texture(10)]]){
    
    for(int i = 0; i < (int)src.size(); i++){
        if(is_null_texture(src[i])){
            break;
        }
//        process_image(src[i], dst);
    }
}

float4 foo2(array_ref<texture2d<float>> src){
    float4 clr(0.0f);
    for(int i = 0; i < src.size(); i++){
//        clr += process_texture(src[i]);
    }
   uint result = 10;
   uint result2 = 20;
    ptrdiff_t pointer1 = result2 - result;
    return clr;
}
