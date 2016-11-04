//
//  GLViewController.h
//  OpenGLEShader
//
//  Created by mac zdszkj on 16/7/18.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <GLKit/GLKit.h>

@interface GLViewController : GLKViewController

@property (nonatomic, strong) EAGLContext* context;
@property (nonatomic, strong) GLKBaseEffect* effect;

@end
