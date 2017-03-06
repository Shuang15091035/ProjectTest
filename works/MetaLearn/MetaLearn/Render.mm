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
#import "AAPLTransforms.h"
#import <MetalKit/MetalKit.h>

using namespace simd;


struct Uniforms
{
    simd::float4x4 modelview_projection_matrix;
    simd::float4x4 normal_matrix;
    simd::float4   ambient_color;
    simd::float4   diffuse_color;
    int            multiplier;
} __attribute__ ((aligned (256)));

static const float kWidth  = 0.75f;
static const float kHeight = 0.75f;
static const float kDepth  = 0.75f;

static const float kCubeVertexData[] =
{
    kWidth, -kHeight, kDepth,   0.0, -1.0,  0.0,
    -kWidth, -kHeight, kDepth,   0.0, -1.0, 0.0,
    -kWidth, -kHeight, -kDepth,   0.0, -1.0,  0.0,
    kWidth, -kHeight, -kDepth,  0.0, -1.0,  0.0,
    kWidth, -kHeight, kDepth,   0.0, -1.0,  0.0,
    -kWidth, -kHeight, -kDepth,   0.0, -1.0,  0.0,
    
    kWidth, kHeight, kDepth,    1.0, 0.0,  0.0,
    kWidth, -kHeight, kDepth,   1.0,  0.0,  0.0,
    kWidth, -kHeight, -kDepth,  1.0,  0.0,  0.0,
    kWidth, kHeight, -kDepth,   1.0, 0.0,  0.0,
    kWidth, kHeight, kDepth,    1.0, 0.0,  0.0,
    kWidth, -kHeight, -kDepth,  1.0,  0.0,  0.0,
    
    -kWidth, kHeight, kDepth,    0.0, 1.0,  0.0,
    kWidth, kHeight, kDepth,    0.0, 1.0,  0.0,
    kWidth, kHeight, -kDepth,   0.0, 1.0,  0.0,
    -kWidth, kHeight, -kDepth,   0.0, 1.0,  0.0,
    -kWidth, kHeight, kDepth,    0.0, 1.0,  0.0,
    kWidth, kHeight, -kDepth,   0.0, 1.0,  0.0,
    
    -kWidth, -kHeight, kDepth,  -1.0,  0.0, 0.0,
    -kWidth, kHeight, kDepth,   -1.0, 0.0,  0.0,
    -kWidth, kHeight, -kDepth,  -1.0, 0.0,  0.0,
    -kWidth, -kHeight, -kDepth,  -1.0,  0.0,  0.0,
    -kWidth, -kHeight, kDepth,  -1.0,  0.0, 0.0,
    -kWidth, kHeight, -kDepth,  -1.0, 0.0,  0.0,
    
    kWidth, kHeight,  kDepth,  0.0, 0.0,  1.0,
    -kWidth, kHeight,  kDepth,  0.0, 0.0,  1.0,
    -kWidth, -kHeight, kDepth,   0.0,  0.0, 1.0,
    -kWidth, -kHeight, kDepth,   0.0,  0.0, 1.0,
    kWidth, -kHeight, kDepth,   0.0,  0.0,  1.0,
    kWidth, kHeight,  kDepth,  0.0, 0.0,  1.0,
    
    kWidth, -kHeight, -kDepth,  0.0,  0.0, -1.0,
    -kWidth, -kHeight, -kDepth,   0.0,  0.0, -1.0,
    -kWidth, kHeight, -kDepth,  0.0, 0.0, -1.0,
    kWidth, kHeight, -kDepth,  0.0, 0.0, -1.0,
    kWidth, -kHeight, -kDepth,  0.0,  0.0, -1.0,
    -kWidth, kHeight, -kDepth,  0.0, 0.0, -1.0
};
static const float kFOVY    = 65.0f;
static const float3 kEye    = {0.0f, 0.0f, -5.0f};
static const float3 kCenter = {0.0f, 0.0f, 1.0f};
static const float3 kUp     = {0.0f, 1.0f, 0.0f};

@implementation Render{
    id <MTLDevice> _device;
    id <MTLCommandQueue> _commandQueue;
    id <MTLLibrary> _defaultLibrary;
    id <MTLBuffer> _dynamicConstantBuffer;
    // render stage
    id <MTLRenderPipelineState> _pipelineState;
    id <MTLComputePipelineState> _computePipelineState;
    id <MTLBuffer> _vertexBuffer;
    id <MTLTexture> _textureBuffer;
    id <MTLBuffer> _vertexColorBuffer;
    id <MTLTexture> inputImage;
    float4x4 _projectionMatrix;
    float4x4 _viewMatrix;
    float _rotation;

}

- (void)configure:(CustomView *)view{
    if ([view isKindOfClass:[CustomView class]]) {
        // find a usable Device
        _device = view.device;
        
        // setup view with drawable formats
        view.depthPixelFormat   = MTLPixelFormatDepth32Float;
        view.stencilPixelFormat = MTLPixelFormatInvalid;
        view.sampleCount        = 1;
    }
//    _constantDataBufferIndex = 0;
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
    _dynamicConstantBuffer = [_device newBufferWithLength:sizeof(Uniforms) options:0];
    [_dynamicConstantBuffer setLabel:[NSString stringWithFormat:@"constBuffer "]];
}

- (BOOL)preparePipelineState:(CustomView*)view
{
    // load the vertex program into the library
    id <MTLFunction> vertexProgram = [_defaultLibrary newFunctionWithName:@"passThroughVertex"];
    
    // load the fragment program into the library
    id <MTLFunction> fragmentProgram = [_defaultLibrary newFunctionWithName:@"passThroughFragment"];
    
    //setup the vertex buffers
    _vertexBuffer = [_device newBufferWithBytes:kCubeVertexData length:sizeof(kCubeVertexData) options:0];
    _vertexBuffer.label = @"Vertices";
    
    MTKTextureLoader *textureLoader = [[MTKTextureLoader alloc]initWithDevice:_device];
    NSData *data = [NSData dataWithContentsOfFile:[[NSBundle mainBundle]pathForResource:@"picture" ofType:@"jpg"]];
    _textureBuffer = [textureLoader newTextureWithData:data options:nil error:nil];
    
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
//    MTLComputePipelineDescriptor *computePipelineDes = [MTLComputePipelineDescriptor new];
//    computePipelineDes.label = @"computePipeline";
    
//    id <MTLFunction> func  = [_defaultLibrary newFunctionWithName:@"filter_main"];
//   _computePipelineState = [_device newComputePipelineStateWithFunction:func error:&error];
//    if (!_computePipelineState) {
//        NSLog(@">> ERROR: Failed Aquiring pipeline state: %@", error);
//        return NO;
//    }
    
    return YES;
}

- (void)updataVertexBuffer{
    float4x4 baseModelViewMatrix = AAPL::Math::rotate(_rotation, 1.0f, 1.0f, 1.0f);
    baseModelViewMatrix = _viewMatrix * baseModelViewMatrix;
    
    Uniforms *constant_buffer = (Uniforms *)[_dynamicConstantBuffer contents];
    for (int i = 0; i < 1; i++)
    {
        // calculate the Model view projection matrix of each box
        // for each box, if its odd, create a negative multiplier to offset boxes in space
        int multiplier = ((i % 2 == 0)?1:-1);
        simd::float4x4 modelViewMatrix = AAPL::Math::translate(0.0f, 0.0f, multiplier*1.5f) * AAPL::Math::rotate(_rotation, 1.0f, 1.0f, 1.0f);
        modelViewMatrix = baseModelViewMatrix * modelViewMatrix;
        
        constant_buffer[i].normal_matrix = inverse(transpose(modelViewMatrix));
        constant_buffer[i].modelview_projection_matrix = _projectionMatrix * modelViewMatrix;
        
        // change the color each frame
        // reverse direction if we've reached a boundary
        if (constant_buffer[i].ambient_color.y >= 0.8) {
            constant_buffer[i].multiplier = -1;
            constant_buffer[i].ambient_color.y = 0.79;
        } else if (constant_buffer[i].ambient_color.y <= 0.2) {
            constant_buffer[i].multiplier = 1;
            constant_buffer[i].ambient_color.y = 0.21;
        } else
            constant_buffer[i].ambient_color.y += constant_buffer[i].multiplier * 0.01*i;
    }
}

- (void)render:(CustomView *)view{
    
    [self updataVertexBuffer];
    
    id <MTLCommandBuffer>commandBuffer = [_commandQueue commandBuffer];
    MTLRenderPassDescriptor *renderPassDescriptor = view.renderPassDescriptor;
    if (renderPassDescriptor) {
        id <MTLRenderCommandEncoder> renderEncoder = [commandBuffer renderCommandEncoderWithDescriptor:renderPassDescriptor];
         [renderEncoder pushDebugGroup:@"renderEncoder"];
         [renderEncoder setRenderPipelineState:_pipelineState];
       
        [renderEncoder setVertexBuffer:_vertexBuffer offset:0 atIndex:0];
         [renderEncoder setVertexBuffer:_dynamicConstantBuffer offset:0 atIndex:1];
        [renderEncoder setFragmentTexture:_textureBuffer atIndex:0];
        [renderEncoder drawPrimitives:MTLPrimitiveTypeTriangle vertexStart:0 vertexCount:36];
        [renderEncoder endEncoding];
        [renderEncoder popDebugGroup];
        [commandBuffer presentDrawable:[view currentDrawable]];
    }
    [commandBuffer commit];
    
//    id <MTLCommandBuffer> commandbuffer2 = [_commandQueue commandBuffer];
//    id <MTLComputeCommandEncoder> computeCE = [commandbuffer2 computeCommandEncoder];
//    [computeCE setComputePipelineState:_computePipelineState];
//    [computeCE setTexture:inputImage atIndex:0];
//    MTLSize threadsPerGroup = {16,16,1};
//    MTLSize numThreadGroups = {inputImage.width/threadsPerGroup.width,
//        inputImage.height/threadsPerGroup.height, 1};
//    [computeCE dispatchThreadgroups:numThreadGroups threadsPerThreadgroup:threadsPerGroup];
//    [computeCE endEncoding];
//    [commandbuffer2 commit];
    
}

- (void)reshape:(CustomView *)view{
    float aspect = fabs(view.bounds.size.width / view.bounds.size.height);
    _projectionMatrix = AAPL::Math::perspective_fov(kFOVY, aspect, 0.1f, 100.0f);
    _viewMatrix = AAPL::Math::lookAt(kEye, kCenter, kUp);
}

- (void)update:(ViewController *)controller{
    _rotation += controller.timeSinceLastDraw * 50.0f;
}

- (void)viewController:(ViewController *)viewController willPause:(BOOL)pause{
    
}



@end
