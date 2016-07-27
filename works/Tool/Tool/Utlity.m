//
//  Utlity.m
//  Tool
//
//  Created by mac zdszkj on 16/3/24.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "Utlity.h"
#import <AVFoundation/AVFoundation.h>
#import <UIKit/UIKit.h>
#import <CoreMotion/CoreMotion.h>

@interface Utlity(){
    CMMotionManager *motionManager ;
}

@property(nonatomic)UIView *AView;

@end

@implementation Utlity
- (void)AVfoundationOpenCamera{
    
    // 在iOS7 时，只有部分地区要求授权才能打开相机
    if (floor(NSFoundationVersionNumber) <= NSFoundationVersionNumber_iOS_7_1) {
        // Pre iOS 8 -- No camera auth required.
        
    }else {
        // iOS 8 后，全部都要授权
        
        // Thanks: http://stackoverflow.com/a/24684021/2611971
        
        AVAuthorizationStatus status = [AVCaptureDevice authorizationStatusForMediaType:AVMediaTypeVideo];
        switch (status) {
            case AVAuthorizationStatusNotDetermined:{
                // 许可对话没有出现，发起授权许可
                
                [AVCaptureDevice requestAccessForMediaType:AVMediaTypeVideo completionHandler:^(BOOL granted) {
                    
                    if (granted) {
                        //第一次用户接受
                        
                    }else{
                        //用户拒绝
                    }
                }];
                break;
            }
            case AVAuthorizationStatusAuthorized:{
                // 已经开启授权，可继续
                
                break;
            }
            case AVAuthorizationStatusDenied:
            case AVAuthorizationStatusRestricted:
                // 用户明确地拒绝授权，或者相机设备无法访问
                
                break;
            default:
                break;
        }
        
    }
}
#pragma mark - 屏幕旋转的方法 此种方法使用通知的方式实现可以适用于屏幕不锁定状态下屏幕的自动旋转  target(6.0-9.3)测试没有问题
- (void)screenRoation{
#if 1
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(screenRotaionNotificated) name:UIDeviceOrientationDidChangeNotification object:nil];
#elif 0
    UIDevice *device = [UIDevice currentDevice];
    [device beginGeneratingDeviceOrientationNotifications]; //调用设备的加速度信息获取
    NSNotificationCenter *nc = [NSNotificationCenter defaultCenter];
    [nc addObserver:self selector:@selector(screenRotaionNotificated)name:UIDeviceOrientationDidChangeNotification  object:device];
#endif
}
- (void)screenRotaion{
    UIDeviceOrientation orientation = [UIDevice currentDevice].orientation;
    switch (orientation) {
        case UIDeviceOrientationPortrait:{
            NSLog(@"UIDeviceOrientationPortrait");
        
            break;
        }
        case UIDeviceOrientationLandscapeLeft:{
            NSLog(@"UIDeviceOrientationLandscapeLeft");
            
            break;
        }
        case UIDeviceOrientationLandscapeRight:{
            NSLog(@"UIDeviceOrientationLandscapeRight");
          
            break;
        }
        case UIDeviceOrientationPortraitUpsideDown:{
            NSLog(@"UIDeviceOrientationPortraitUpsideDown");
         
            break;
        }
        default:
            break;
    }
}
#pragma mark - 此种方法可以适用于屏幕不锁定状态下屏幕的自动旋转  target(6.0-9.3)测试没有问题
- (void)willAnimateRotationToInterfaceOrientation:(UIInterfaceOrientation)interfaceOrientation duration:(NSTimeInterval)duration {
    
    switch (interfaceOrientation) {
        case UIInterfaceOrientationPortrait:{
            //home健在下
            CGRect bounds = [[UIScreen mainScreen]bounds];
            //(CGRect) bounds = (origin = (x = 0, y = 0), size = (width = 414, height = 736))
            
            NSLog(@"%@",@"caseUIInterfaceOrientationPortrait");
        }
            break;
        case UIInterfaceOrientationPortraitUpsideDown:{
            //home健在上
            CGRect bounds = [[UIScreen mainScreen]bounds];
            //(CGRect) bounds = (origin = (x = 0, y = 0), size = (width = 414, height = 736))
            
            NSLog(@"%@",@"caseUIInterfaceOrientationPortraitUpsideDown");
        }
            break;
        case UIInterfaceOrientationLandscapeLeft:{
            //home健在左
            
            CGRect bounds = [[UIScreen mainScreen]bounds];
            //(CGRect) bounds = (origin = (x = 0, y = 0), size = (width = 414, height = 736))
            
            NSLog(@"%@",@"caseUIInterfaceOrientationLandscapeLeft");
        }
            break;
        case UIInterfaceOrientationLandscapeRight:{
            //home健在右
            CGRect bounds = [[UIScreen mainScreen]bounds];
            //(CGRect) bounds = (origin = (x = 0, y = 0), size = (width = 414, height = 736))
            
            NSLog(@"%@",@"caseUIInterfaceOrientationLandscapeRight");
        }
            break;
        default:
            break;
    }
}
#pragma mark - 不使用代理实现屏幕的旋转，此种方法很耗性能，设备一直在检测屏幕的deviceMotion的值
- (void)deviceMotionRoation{
    motionManager  = [[CMMotionManager alloc]init];
    NSOperationQueue *queue = [[NSOperationQueue alloc]init];
    if (motionManager.isDeviceMotionAvailable) {
        [motionManager startDeviceMotionUpdatesToQueue:queue withHandler:^(CMDeviceMotion * _Nullable motion, NSError * _Nullable error) {
            [self performSelectorOnMainThread:@selector(handleDeviceMotion:) withObject:motion waitUntilDone:YES];
        }];
    }
}
- (void)handleDeviceMotion:(CMDeviceMotion *)deviceMotion{
    double x = deviceMotion.gravity.x;
    double y = deviceMotion.gravity.y;
    if (fabs(y) >= fabs(x))
    {
        if (y >= 0){
            // UIDeviceOrientationPortraitUpsideDown;
        }
        else{
            // UIDeviceOrientationPortrait;
            NSLog(@"UIDeviceOrientationPortrait");
        }
    }
    else
    {
        if (x >= 0){
            // UIDeviceOrientationLandscapeRight;
            NSLog(@"右旋转");
            
        }
        else{
            // UIDeviceOrientationLandscapeLeft;
            NSLog(@"左旋转");
        }
    }
    
}
@end
