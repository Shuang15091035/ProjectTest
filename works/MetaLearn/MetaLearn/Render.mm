//
//  Render.m
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/20.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "Render.h"
#import <string.h>
#import <simd/simd.h>

typedef struct{
    matrix_float4x4 rotation_matrix;
} Uniforms;

static float KVertexDataBuffer[] =

{
    
    1, -1, 0.0,1.0,      1.0, 0.0, 0.0, 1.0,
    
    -1, -1, 0.0, 1.0,      0.0, 1.0, 0.0, 1.0,
    
    -1, 1, 0.0, 1.0,      0.0, 0.0, 1.0, 1.0,
    
    0.5, 0.5, 0.0, 1.0,      1.0, 1.0, 0.0, 1.0,
    
    0.5, -0.5, 0.0, 1.0,      1.0, 0.0, 0.0, 1.0,
    
    -0.5, 0.5, 0.0, 1.0,      0.0, 0.0, 1.0, 1.0,
    
};

static const matrix_float4x4 modeMatrix = {};
static const matrix_float4x4 viewMatrix = {};
static const matrix_float4x4 projectionMatrix = {};

@implementation Render{
    id <MTLDevice> _device;
    id <MTLCommandQueue> _commandQueue;
    id <MTLLibrary> _defaultLibrary;
    
    // render stage
    id <MTLRenderPipelineState> _pipelineState;
    id <MTLBuffer> _vertexBuffer;
    id <MTLBuffer> _vertexColorBuffer;
    Uniforms uniforms;
}

- (void)configure:(CustomView *)view{
    if ([view isKindOfClass:[CustomView class]]) {
        // find a usable Device
        _device = view.device;
        
        // setup view with drawable formats
        view.depthPixelFormat   = MTLPixelFormatInvalid;
        view.stencilPixelFormat = MTLPixelFormatInvalid;
        view.sampleCount        = 1;
    }
  
    // create a new command queue
    _commandQueue = [_device newCommandQueue];
    
    _defaultLibrary = [_device newDefaultLibrary];
    if(!_defaultLibrary) {
        NSLog(@">> ERROR: Couldnt create a default shader library");
        // assert here becuase if the shader libary isn't loading, nothing good will happen
        assert(0);
    }
    
    if (![self preparePipelineState:view])
    {
        NSLog(@">> ERROR: Couldnt create a valid pipeline state");
        
        // cannot render anything without a valid compiled pipeline state object.
        assert(0);
    }
    
    // set the vertex shader and buffers defined in the shader source, in this case we have 2 inputs. A position buffer and a color buffer
    // Allocate a buffer to store vertex position data (we'll quad buffer this one)
//    _vertexBuffer = [_device newBufferWithLength:sizeof(KVertexDataBuffer)  options:0];
    _vertexBuffer = [_device newBufferWithBytes:KVertexDataBuffer length:sizeof(KVertexDataBuffer) options:0];
    _vertexBuffer.label = @"Vertices";
    
    
}

- (BOOL)preparePipelineState:(CustomView*)view
{
    // load the vertex program into the library
    id <MTLFunction> vertexProgram = [_defaultLibrary newFunctionWithName:@"passThroughVertex"];
    
    // load the fragment program into the library
    id <MTLFunction> fragmentProgram = [_defaultLibrary newFunctionWithName:@"passThroughFragment"];
    
    //  create a reusable pipeline state
    MTLRenderPipelineDescriptor *pipelineStateDescriptor = [MTLRenderPipelineDescriptor new];
    pipelineStateDescriptor.label = @"MyPipeline";
    pipelineStateDescriptor.colorAttachments[0].pixelFormat = MTLPixelFormatBGRA8Unorm;
    pipelineStateDescriptor.sampleCount      = view.sampleCount;
    pipelineStateDescriptor.vertexFunction   = vertexProgram;
    pipelineStateDescriptor.fragmentFunction = fragmentProgram;
    
    NSError *error = nil;
    _pipelineState = [_device newRenderPipelineStateWithDescriptor:pipelineStateDescriptor
                                                             error:&error];
    if(!_pipelineState) {
        NSLog(@">> ERROR: Failed Aquiring pipeline state: %@", error);
        return NO;
    }
    
    return YES;
}

- (void)renderTriangle:(id<MTLRenderCommandEncoder>)renderEncoder{
    [renderEncoder pushDebugGroup:@"renderEncoder"];
    [renderEncoder setVertexBuffer:_vertexBuffer offset:0 atIndex:0];
//    [renderEncoder setVertexBuffer:_vertexColorBuffer offset:0 atIndex:1];
    [renderEncoder setRenderPipelineState:_pipelineState];
    [renderEncoder drawPrimitives:MTLPrimitiveTypeTriangle vertexStart:0 vertexCount:3];
    [renderEncoder popDebugGroup];
}

- (void)updataVertexBuffer{
//    simd::float4 *vData = (simd::float4 *)((uintptr_t)[_vertexBuffer contents]);
//    memcpy(vData, _vertexBuffer, sizeof(_vertexBuffer));
    
    
}
- (void)render:(CustomView *)view{
    
//    [self updataVertexBuffer];
    id <MTLCommandBuffer>commandBuffer = [_commandQueue commandBuffer];
    MTLRenderPassDescriptor *renderPassDescriptor = view.renderPassDescriptor;
    if (renderPassDescriptor) {
        id <MTLRenderCommandEncoder> renderEncoder = [commandBuffer renderCommandEncoderWithDescriptor:renderPassDescriptor];
        [self renderTriangle:renderEncoder];
        [renderEncoder endEncoding];
        [commandBuffer presentDrawable:[view currentDrawable]];
    }
    [commandBuffer commit];
}

- (void)reshape:(CustomView *)view{
    
}

- (void)update:(ViewController *)viewController{
    
}

- (void)viewController:(ViewController *)viewController willPause:(BOOL)pause{
    
}



@end
