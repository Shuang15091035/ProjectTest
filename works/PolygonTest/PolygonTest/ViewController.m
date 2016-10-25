//
//  ViewController.m
//  PolygonTest
//
//  Created by mac zdszkj on 2016/10/24.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ViewController.h"


using
@interface ViewController ()

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    [self createContext];
    
}

- (void)createContext{
    self.context = [[EAGLContext alloc]initWithAPI:kEAGLRenderingAPIOpenGLES2];
    GLKView *glView = (GLKView *)self.view;
    glView.context = self.context;
    glView.drawableColorFormat = GLKViewDrawableColorFormatRGBA8888;
    glView.drawableDepthFormat = GLKViewDrawableDepthFormat24;
    //    激活当前创建的context，启用深度测试
    [EAGLContext setCurrentContext:self.context];
    glEnable(GL_DEPTH_TEST);
    
    //    创建shader
    [self createShader];
    
    glGenBuffers(1, &buffer);
    
    //       生成的数量  返回id
    glBindBuffer(GL_ARRAY_BUFFER, buffer);// 设置当前操作的buffer
    glBufferData(GL_ARRAY_BUFFER, sizeof(squareVertexData), squareVertexData, GL_STATIC_DRAW);
    //glBufferData(buffer用途, 数据大小, 数据, 缓冲区用途)
}

- (void)createShader{
    GLuint vid = glCreateShader(GL_VERTEX_SHADER);
    NSString *vPath = [[NSBundle mainBundle] pathForResource:@"Shader" ofType:@"vert"];
    NSString *vString = [NSString stringWithContentsOfFile:vPath encoding:NSUTF8StringEncoding error:nil];
    const GLchar * const vChar = [vString cStringUsingEncoding:NSUTF8StringEncoding];
    //GLint l = (GLint)vString.length;
    glShaderSource(vid, 1, &vChar, NULL);
    glCompileShader(vid);
    int err = glGetError();
    if (err != GL_NO_ERROR) {
        GLchar infolog[256];
        glGetShaderInfoLog(vid, 256, NULL, infolog);
        return;
    }
    
    GLuint fid = glCreateShader(GL_FRAGMENT_SHADER);
    NSString *fPath = [[NSBundle mainBundle] pathForResource:@"Shader" ofType:@"frag"];
    NSString *fString = [NSString stringWithContentsOfFile:fPath encoding:NSUTF8StringEncoding error:nil];
    const GLchar *fChar = [fString cStringUsingEncoding:NSUTF8StringEncoding];
    glShaderSource(fid, 1, &fChar, NULL);
    glCompileShader(fid);
    err = glGetError();
    if (err != GL_NO_ERROR) {
        GLchar infolog[256];
        glGetShaderInfoLog(vid, 256, NULL, infolog);
        return;
    }
    
    
    pid = glCreateProgram();
    glAttachShader(pid, vid);
    glAttachShader(pid, fid);
    glLinkProgram(pid);
    //    glDeleteShader(vid);
    //    glDeleteProgram(<#GLuint program#>)
    
    GLint p = GL_FALSE;
    glGetProgramiv(pid, GL_LINK_STATUS, &p);
    
    if (p == GL_FALSE) {
        GLchar infolog[256];
        glGetProgramInfoLog(pid, 256, NULL, infolog);
        return;
    }
    
    positionLocation = glGetAttribLocation(pid, "position");
    //    modelviewLocation = glGetUniformLocation(pid, "u_modelview");
    colorLocation = glGetUniformLocation(pid, "color");
    
    //
    //    GLuint texture = 0;
    //    glGenTextures(1, &texture);
    //    glBindTexture(GL_TEXTURE_2D, texture);
    
    //UIImage* image = [UIImage imageNamed:@"btn_add_n"];
    //glTexImage2D(GL_TEXTURE_2D, 0, GL_RGBA, <#GLsizei width#>, <#GLsizei height#>, <#GLint border#>, <#GLenum format#>, <#GLenum type#>, <#const GLvoid *pixels#>)
}
const GLfloat Pi = 3.1415926536f;

//- (void)update{
//    
//}
-(void)glkView:(GLKView *)view drawInRect:(CGRect)rect{
    glClearColor(0.3f, 0.6f, 1.0f, 1.0f);//设置清屏的颜色
    glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);
    //[self.effect prepareToDraw];
    
    glUseProgram(pid); // 使用自定义的shader
    glBindBuffer(GL_ARRAY_BUFFER, buffer); // 每帧滴啊用
    glEnableVertexAttribArray(positionLocation);//shader的位置
    glVertexAttribPointer(positionLocation, 3, GL_FLOAT, GL_FALSE, 4*8, (char*)NULL + 0);//传入顶点
    //glVertexAttribPointer(shader的attribute, 数据个数, 类型, 是否规划处理, 两个数据的间隔(字节), 指针类型的偏移量 从第N个开始读取)
    
    
    //    glEnableVertexAttribArray(GLKVertexAttribNormal); // 法线的设置
    //    glVertexAttribPointer(GLKVertexAttribNormal, 3, GL_FLOAT, GL_FALSE, 4*8, (char*)NULL + 12 );
    //
    //
    //    glEnableVertexAttribArray(uvLocation); // 纹理的设置
    //    glVertexAttribPointer(uvLocation, 2, GL_FLOAT, GL_FALSE, 4*8, (char*)NULL+24);
    
    //    const GLfloat mv[] = {
    //        1,0,0,0,
    //        0,1,0,0,
    //        0,0,1,0,
    //        0,0,0,1,
    //    };
    //    glUniformMatrix4fv(modelviewLocation, 1, GL_FALSE, mv); // 传入矩阵
    glUniform4f(colorLocation, 1.0, 0.5, 0.0, 1);// 传入颜色
    
    // glActiveTexture(GL_TEXTURE0);
    //    glBindTexture(GL_TEXTURE_2D, texture);
    // glUniform1i(textureLocation, GL_TEXTURE0);
    
    glDrawArrays(GL_TRIANGLES, 0, 6);
    //glDrawArrays(画的图形, 偏移量 从0开始, 顶点数量) 指定完开始画
}

@end
