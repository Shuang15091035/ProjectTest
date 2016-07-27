//
//  OpenGLView.h
//  OpenGLEShader
//
//  Created by mac zdszkj on 16/6/30.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <QuartzCore/QuartzCore.h>
#import <OpenGLES/ES2/gl.h>
#import <OpenGLES/ES2/glext.h>

@interface OpenGLView : UIView{
    CAEAGLLayer *_eaglLayer;
    EAGLContext *_context;
    GLuint _colorRenderBuffer;
}

@end
