//
//  CustomView.h
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/20.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <Metal/Metal.h>

@protocol CustomViewDelegate;

@interface CustomView : UIView

//@property (nonatomic, weak) id <CustomViewDelegate> deleg;

@property (nonatomic, weak) id <CustomViewDelegate> delegate;

@property (nonatomic, readonly) id<MTLDevice> device;

@property (nonatomic, readonly) id<CAMetalDrawable> currentDrawable;

@property (nonatomic, readonly) MTLRenderPassDescriptor* renderPassDescriptor;

@property (nonatomic) MTLPixelFormat depthPixelFormat;
@property (nonatomic) MTLPixelFormat stencilPixelFormat;
@property (nonatomic) NSUInteger sampleCount;

- (void)display;

- (void)releaseTextures;
@end

@protocol CustomViewDelegate <NSObject>
@required
- (void)reshape:(CustomView *)view;

- (void)render:(CustomView *)view;
@end
