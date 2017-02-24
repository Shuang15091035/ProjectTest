//
//  Sharder.metal
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/24.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#include <metal_stdlib>
#include <metal_matrix>

using namespace metal;


struct VertexIn{
    float4 posit;
    float4 color;
};

struct VertexOut{
    float4 position [[position]];
    float4 color ;
};

typedef struct{
    matrix_float4x4 rotation_matrix;
} Uniforms;


vertex VertexOut passThroughVertex(device VertexIn *position [[buffer(0)]],uint vid [[vertex_id]]){
    VertexOut outVertex;
    outVertex.position = position[vid].posit;
    outVertex.color = position[vid].color;
    return outVertex;
}

fragment half4 passThroughFragment(VertexOut in [[stage_in]]){
    return half4(in.color);
}
