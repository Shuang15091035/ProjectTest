//
//  AAPLType.h
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/20.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#ifndef AAPLType_h
#define AAPLType_h

#import <simd/simd.h>

#ifdef __cplusplus

namespace AAPL
{
    struct Uniforms
    {
        simd::float4x4 modelview_projection_matrix;
        simd::float4x4 normal_matrix;
        simd::float4   ambient_color;
        simd::float4   diffuse_color;
        int            multiplier;
    } __attribute__ ((aligned (256)));
}

#endif

#endif /* AAPLType_h */
