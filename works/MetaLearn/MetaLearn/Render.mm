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
#import "AAPLType.h"

using namespace simd;
using namespace AAPL;

static const float kWidth  = 2.0f;
static const float kHeight = 2.0f;
static const float kDepth  = 2.0f;

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

static const float KTriangleVertexData[] = {
    
    kWidth, -kHeight, kDepth,   1.0,   0.0,   0.0,   1.0, 0.0, 0.0,
    -kWidth, -kHeight, kDepth,   0.0,   1.0,   0.0,   1.0, 1.0, 0.0,
    -kWidth, -kHeight, -kDepth,   0.0,   0.0,   1.0,   1.0, 1.0, 1.0,
    kWidth, -kHeight, -kDepth,  1.0,   0.0,   0.0,   1.0, 0.0, 1.0,
    kWidth, -kHeight, kDepth,   0.0,   1.0,   0.0,   1.0, 0.0, 0.0,
    -kWidth, -kHeight, -kDepth,   0.0,   0.0,   1.0,   1.0, 1.0, 1.0,
    
//    kWidth, kHeight, kDepth,    1.0,   0.0,   0.0,   1.0,
//    kWidth, -kHeight, kDepth,   0.0,   1.0,   0.0,   1.0,
//    kWidth, -kHeight, -kDepth,  0.0,   0.0,   1.0,   1.0,
//    kWidth, kHeight, -kDepth,   1.0,   0.0,   0.0,   1.0,
//    kWidth, kHeight, kDepth,    0.0,   1.0,   0.0,   1.0,
//    kWidth, -kHeight, -kDepth,  0.0,   0.0,   1.0,   1.0,
//    
//    -kWidth, kHeight, kDepth,    1.0,   0.0,   0.0,   1.0,
//    kWidth, kHeight, kDepth,    0.0,   1.0,   0.0,   1.0,
//    kWidth, kHeight, -kDepth,   0.0,   0.0,   1.0,   1.0,
//    -kWidth, kHeight, -kDepth,   1.0,   0.0,   0.0,   1.0,
//    -kWidth, kHeight, kDepth,    0.0,   1.0,   0.0,   1.0,
//    kWidth, kHeight, -kDepth,   0.0,   0.0,   1.0,   1.0,
//    
//    -kWidth, -kHeight, kDepth,  1.0,   0.0,   0.0,   1.0,
//    -kWidth, kHeight, kDepth,   0.0,   1.0,   0.0,   1.0,
//    -kWidth, kHeight, -kDepth,  0.0,   0.0,   1.0,   1.0,
//    -kWidth, -kHeight, -kDepth,  1.0,   0.0,   0.0,   1.0,
//    -kWidth, -kHeight, kDepth,  0.0,   1.0,   0.0,   1.0,
//    -kWidth, kHeight, -kDepth,  0.0,   0.0,   1.0,   1.0,
//    
//    kWidth, kHeight,  kDepth,  1.0,   0.0,   0.0,   1.0,
//    -kWidth, kHeight,  kDepth,  0.0,   1.0,   0.0,   1.0,
//    -kWidth, -kHeight, kDepth,   0.0,   0.0,   1.0,   1.0,
//    -kWidth, -kHeight, kDepth,   1.0,   0.0,   0.0,   1.0,
//    kWidth, -kHeight, kDepth,   0.0,   1.0,   0.0,   1.0,
//    kWidth, kHeight,  kDepth,  0.0,   0.0,   1.0,   1.0,
//    
//    kWidth, -kHeight, -kDepth,  1.0,   0.0,   0.0,   1.0,
//    -kWidth, -kHeight, -kDepth,   0.0,   1.0,   0.0,   1.0,
//    -kWidth, kHeight, -kDepth,  0.0,   0.0,   1.0,   1.0,
//    kWidth, kHeight, -kDepth,  1.0,   0.0,   0.0,   1.0,
//    kWidth, -kHeight, -kDepth,  0.0,   1.0,   0.0,   1.0,
//    -kWidth, kHeight, -kDepth,  0.0,   0.0,   1.0,   1.0
//    
};

static const float kFOVY    = 65.0f;
static const float3 kEye    = {0.0f, 0.0f, -6.0f};
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
    id<MTLBuffer> vertexColor;
    id <MTLDepthStencilState> _depthState;

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
    _vertexBuffer = [_device newBufferWithBytes:KTriangleVertexData length:sizeof(KTriangleVertexData) options:0];
    _vertexBuffer.label = @"Vertices";
    
    vertexColor.label = @"vertexColor";
    
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
    
    MTLDepthStencilDescriptor *depthStateDesc = [[MTLDepthStencilDescriptor alloc] init];
    depthStateDesc.depthCompareFunction = MTLCompareFunctionLess;
    depthStateDesc.depthWriteEnabled = YES;
    _depthState = [_device newDepthStencilStateWithDescriptor:depthStateDesc];
    return YES;
}

- (void)updataVertexBuffer{
    Uniforms *unifrom =  (Uniforms *)[_dynamicConstantBuffer contents];
    float4x4 baseModelMatrix = AAPL::Math::rotate(_rotation, 1.0f, 0.0f, 0.0f);
    float4x4 baseModelViewMatrix = _viewMatrix * baseModelMatrix;
    unifrom->modelview_projection_matrix = _projectionMatrix * baseModelViewMatrix;
}

- (void)render:(CustomView *)view{

    [self updataVertexBuffer];
    id<MTLCommandBuffer> commandBuffer = [_commandQueue commandBuffer];
    MTLRenderPassDescriptor *renderPassDescriptor = view.renderPassDescriptor;
    if (renderPassDescriptor) {
        id<MTLRenderCommandEncoder> renderEncoder = [commandBuffer renderCommandEncoderWithDescriptor:view.renderPassDescriptor];
        [renderEncoder setRenderPipelineState:_pipelineState];
//        [renderEncoder setDepthStencilState:_depthState];
//        [renderEncoder setCullMode:MTLCullModeBack];
        [renderEncoder setVertexBuffer:_vertexBuffer offset:0 atIndex:0];
        [renderEncoder setVertexBuffer:_dynamicConstantBuffer offset:0 atIndex:1];
        [renderEncoder setFragmentTexture:_textureBuffer atIndex:0];
        NSUInteger triangelC = sizeof(KTriangleVertexData)/ (sizeof(KTriangleVertexData[0]) * 9);
        [renderEncoder drawPrimitives:MTLPrimitiveTypeTriangle vertexStart:0 vertexCount:triangelC];
        [renderEncoder endEncoding];
        [commandBuffer presentDrawable:[view currentDrawable]];
    }
    [commandBuffer commit];
    
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
