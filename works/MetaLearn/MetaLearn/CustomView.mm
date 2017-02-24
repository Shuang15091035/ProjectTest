//
//  CustomView.m
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/20.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "CustomView.h"

@implementation CustomView{
    @private
    __weak CAMetalLayer *_metalLayer;
    id<MTLTexture> _depthTex;
    id<MTLTexture> _stencilTex;
    id<MTLTexture> _msaaTex;
    
     BOOL _layerSizeDidUpdate;
    
}

@synthesize currentDrawable = _currentDrawable;
@synthesize renderPassDescriptor = _renderPassDescriptor;

+ (Class)layerClass{
    return [CAMetalLayer class];
}

- (void)initCommon{
    self.opaque          = YES;
    self.backgroundColor = nil;
    
    _metalLayer = (CAMetalLayer *)self.layer;
    
    _device = MTLCreateSystemDefaultDevice();
    
    _metalLayer.device          = _device;
    _metalLayer.pixelFormat     = MTLPixelFormatBGRA8Unorm;
    
    // this is the default but if we wanted to perform compute on the final rendering layer we could set this to no
    _metalLayer.framebufferOnly = YES;
    
}

- (instancetype)init{
    self = [super init];
    if (self) {
        [self initCommon];
    }
    return self;
}

- (instancetype)initWithCoder:(NSCoder *)aDecoder{
    self = [super initWithCoder:aDecoder];
    if (self) {
        [self initCommon];
    }
    return self;
}

- (instancetype)initWithFrame:(CGRect)frame{
    self = [super initWithFrame:frame];
    if (self) {
        [self initCommon];
    }
    return self;
}

- (void)display{
    
    @autoreleasepool
    {
        // handle display changes here
        if(_layerSizeDidUpdate)
        {
            // set the metal layer to the drawable size in case orientation or size changes
            CGSize drawableSize = self.bounds.size;
            drawableSize.width  *= self.contentScaleFactor;
            drawableSize.height *= self.contentScaleFactor;
            
            _metalLayer.drawableSize = drawableSize;
            
            // renderer delegate method so renderer can resize anything if needed
            [_delegate reshape:self];
            
            _layerSizeDidUpdate = NO;
        }
        
        // rendering delegate method to ask renderer to draw this frame's content
        [_delegate render:self];
        
        // do not retain current drawable beyond the frame.
        // There should be no strong references to this object outside of this view class
        _currentDrawable    = nil;
    }
}

- (void)setContentScaleFactor:(CGFloat)contentScaleFactor
{
    [super setContentScaleFactor:contentScaleFactor];
    
    _layerSizeDidUpdate = YES;
}

- (void)setupRenderPassDescriptorForTexture:(id <MTLTexture>) texture
{
    // create lazily
    if (_renderPassDescriptor == nil)
    _renderPassDescriptor = [MTLRenderPassDescriptor renderPassDescriptor];
    
    // create a color attachment every frame since we have to recreate the texture every frame
    MTLRenderPassColorAttachmentDescriptor *colorAttachment = _renderPassDescriptor.colorAttachments[0];
    colorAttachment.texture = texture;
    
    // make sure to clear every frame for best performance
    colorAttachment.loadAction = MTLLoadActionClear;
    colorAttachment.clearColor = MTLClearColorMake(0.65f, 0.65f, 0.65f, 1.0f);
    
     colorAttachment.storeAction = MTLStoreActionStore;
}
- (MTLRenderPassDescriptor *)renderPassDescriptor
{
    id <CAMetalDrawable> drawable = self.currentDrawable;
    if(!drawable)
    {
        NSLog(@">> ERROR: Failed to get a drawable!");
        _renderPassDescriptor = nil;
    }
    else
    {
        [self setupRenderPassDescriptorForTexture: drawable.texture];
    }
    
    return _renderPassDescriptor;
}

- (id <CAMetalDrawable>)currentDrawable
{
    if (_currentDrawable == nil)
    _currentDrawable = [_metalLayer nextDrawable];
    
    return _currentDrawable;
}


- (void)layoutSubviews
{
    [super layoutSubviews];
    
    _layerSizeDidUpdate = YES;
}

- (void)setDeleg:(id<CustomViewDelegate>)deleg{
    
    _delegate = deleg;
}

- (id<CustomViewDelegate>)delegate{
    return _delegate;
}
@end
