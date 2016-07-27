//
//  OpenGLView.m
//  OpenGLEShader
//
//  Created by mac zdszkj on 16/6/30.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "OpenGLView.h"

@implementation OpenGLView

//设置layer类为CAEAGLLayer（iOS中存在多种用途的layer，CAShapeLayer，CATextLayer）
+ (Class)layerClass{
    return [CAEAGLLayer class];
}

- (void)setupLayer{
    _eaglLayer = (CAEAGLLayer *)self.layer;
    _eaglLayer.opaque = YES;//缺省为透明，性能负荷大，特别是OpenGL的层，tableview cell 也一样
}

- (void)setupContext{
    EAGLRenderingAPI api = kEAGLRenderingAPIOpenGLES2;//指定API的版本
    _context = [[EAGLContext alloc]initWithAPI:api];
    if (!_context) {
        exit(1);
    }
    if (![EAGLContext setCurrentContext:_context]) {
        exit(1);
    }
}

- (void)setupRenderBuffer{//渲染缓冲区,（存放渲染过的图像）
    glGenRenderbuffers(1, &_colorRenderBuffer);//存放显示的颜色，创建一个OpenGL的RenderBuffer对象，将_colorRenderBuffer赋值过去
    glBindRenderbuffer(GL_RENDERBUFFER, _colorRenderBuffer);//指定以后的GL_RENDERBUFFER为_colorRenderBuffer
    [_context renderbufferStorage:GL_RENDERBUFFER fromDrawable:_eaglLayer];
    //为GL_RENDERBUFFER分配控件
}

- (void)setupFrameBuffer{
    GLuint frameBuffer;
    glGenFramebuffers(1, &frameBuffer);//Frame buffer也是OpenGL的对象，它包含了前面提到的render buffer，以及其它后面会讲到的诸如：depth buffer、stencil buffer 和 accumulation buffer。
    glBindFramebuffer(GL_FRAMEBUFFER, frameBuffer);
    glFramebufferRenderbuffer(GL_FRAMEBUFFER, GL_COLOR_ATTACHMENT0, GL_RENDERBUFFER, _colorRenderBuffer);//buffer render依附在frame buffer的GL_COLOR_ATTACHMENT0位置上。
}

-(void)render{
    glClearColor(0, 104.0/255.0, 55.0/255.0, 1.0);//设置一个RGB颜色和透明度，接下来会用这个颜色涂满全屏
    glClear(GL_COLOR_BUFFER_BIT);//进行这个“填色”的动作,GL_COLOR_BUFFER_BIT来声明要清理哪一个缓冲区。
    [_context presentRenderbuffer:GL_RENDERBUFFER];//把缓冲区（render buffer和color buffer）的颜色呈现到UIView上
}

- (id)initWithFrame:(CGRect)frame{
    self = [super initWithFrame:frame];
    if (self != nil) {
        [self setupLayer];
        [self setupContext];
        [self setupRenderBuffer];
        [self setupFrameBuffer];
        [self render];
    }
    return self;
}

- (GLuint) compileShader:(NSString *)shaderName withType:(GLenum)shaderType{
    //1.
    NSString *shaderPath = [[NSBundle mainBundle]pathForResource:shaderName ofType:@"glsl"];
    NSError *error;
    NSString *shaderString = [NSString stringWithContentsOfFile:shaderPath encoding:NSUTF8StringEncoding error:&error];
    if (!shaderString) {
        NSLog(@"Error loading shader: %@",error.localizedDescription);
        exit(1);
    }
    //2.
    GLuint shaderHandle = glCreateShader(shaderType);
    //3.
    const char * sharderStringUTF8 = [shaderString UTF8String];
    int shaderStringLength = (int)[shaderString length];
    glShaderSource(shaderHandle, 1, &sharderStringUTF8, &shaderStringLength);
    //4.
    glCompileShader(shaderHandle);
    //5.
    GLint compileSuccess;
    glGetShaderiv(shaderHandle, GL_COMPILE_STATUS, &compileSuccess);
    if (compileSuccess == GL_FALSE) {
        GLchar message[256];
        glGetShaderInfoLog(shaderHandle, sizeof(message), 0, &message[0]);
        NSString *messageString = [NSString stringWithUTF8String:message];
    }
    return shaderHandle;
}
@end
