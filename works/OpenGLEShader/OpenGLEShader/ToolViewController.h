//
//  ViewController.h
//  OpenGLEShader
//
//  Created by mac zdszkj on 16/2/26.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <GLKit/GLKit.h>

@interface ToolViewController : GLKViewController

@property(strong,nonatomic)EAGLContext* context;
@property(strong,nonatomic)GLKBaseEffect* effect;

@end

