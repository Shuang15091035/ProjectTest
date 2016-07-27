//
//  ViewController.m
//  Tool
//
//  Created by mac zdszkj on 16/3/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ToolViewController.h"
#import "MyView.h"

@interface ToolViewController() <AVCaptureVideoDataOutputSampleBufferDelegate>{
    AVCaptureSession* mSession;
    UIImage* im;
}

@property (nonatomic, readonly) AVCaptureDevice* backCamera;
@property (nonatomic, readonly) AVCaptureDevice* frontCamera;
@end

@implementation ToolViewController

- (void)viewDidLoad{
    [super viewDidLoad];
    
//    MyView *myView = [MyView new];
//    myView.layer.cornerRadius = 50.0f;
//    myView.layer.masksToBounds = YES;
//    myView.frame = CGRectMake(50, 50, 100, 100);
//    
//    [self.view addSubview:myView];
    
//    unsigned int outCount = 0;
//   Ivar *ivars = class_copyIvarList([self class], &outCount);
//    NSString *key = nil;
//    for (int i = 0; i < outCount; i++) {
//        Ivar thisIvar = ivars[i];
//        key = [NSString stringWithUTF8String:ivar_getName(thisIvar)];  //获取成员变量的名字
//        [NSString stringWithUTF8String:ivar_getTypeEncoding(thisIvar)]; //获取成员变量的数据类型
//        NSLog(@"variable type :%@", key);
//    }
//    free(ivars);
    
//    UIImage* mars = [UIImage imageNamed:@"animation1@2x.png"];
//    CGSize sz = [mars size];
//    UIGraphicsBeginImageContextWithOptions(CGSizeMake(sz.width * 2, sz.height * 2), NO, 0);
//        UIBezierPath* p = [UIBezierPath bezierPathWithOvalInRect:CGRectMake(0,0,100,100)];
//        [[UIColor blueColor] setFill];
//        [p fill];
    
//    [mars drawInRect:CGRectMake(0,0,sz.width*2,sz.height*2)];
//    
//    [mars drawInRect:CGRectMake(sz.width/2.0, sz.height/2.0, sz.width, sz.height) blendMode:kCGBlendModeMultiply alpha:1.0];
    
//    [mars drawAtPoint:CGPointMake(-sz.width/2.0, 0)];
    
    
    
//    UIImage *image = UIGraphicsGetImageFromCurrentImageContext();
//    UIGraphicsEndImageContext();
//    
//    UIImageView *imageVI = [[UIImageView alloc]initWithImage:image];
//    imageVI.center = self.view.center;
//    [self.view addSubview:imageVI];
    
    
//    UIImage* mars = [UIImage imageNamed:@"animation1@2x.png"];
//    CGSize sz = [mars size];
//    
//    CGImageRef marsLeft = CGImageCreateWithImageInRect([mars CGImage],CGRectMake(0,0,sz.width/2.0,sz.height));
//    
//    CGImageRef marsRight = CGImageCreateWithImageInRect([mars CGImage],CGRectMake(sz.width/2.0,0,sz.width/2.0,sz.height));
//    
//    // 将每一个CGImage绘制到图形上下文中
//    
//    UIGraphicsBeginImageContextWithOptions(CGSizeMake(sz.width*1.5, sz.height), NO, 0);
//    
//    CGContextRef con = UIGraphicsGetCurrentContext();
//    
//    CGContextDrawImage(con, CGRectMake(0,0,sz.width/2.0,sz.height), marsLeft);
//    
//    CGContextDrawImage(con, CGRectMake(sz.width,0,sz.width/2.0,sz.height), marsRight);
//    
//  UIImage *image = UIGraphicsGetImageFromCurrentImageContext();
//    
//    UIGraphicsEndImageContext();
//    
//    // 记得释放内存，ARC在这里无效
//    
//    CGImageRelease(marsLeft);
//    
//    CGImageRelease(marsRight);
//    
//    
//    UIGraphicsEndImageContext();
//    
//    UIImageView *imageVI = [[UIImageView alloc]initWithImage:image];
//    imageVI.center = self.view.center;
//    [self.view addSubview:imageVI];
}
@end
