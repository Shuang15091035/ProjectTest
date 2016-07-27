//
//  MPControllerTool.m
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MPControllerTool.h"
#import "MPNewFeatureViewController.h"
#import "MPTabBarViewController.h"

@interface MPControllerTool (){
    
}
@end

@implementation MPControllerTool

+ (void) chooseRootViewController{
    UIWindow *keyWindow = [[UIApplication sharedApplication]keyWindow];
    NSString *version = (__bridge NSString *)kCFBundleVersionKey;
    NSDictionary *info = [[NSBundle mainBundle]infoDictionary];
    NSString *currentVersion = [info valueForKey:version];
    
//     上次使用的版本
    NSUserDefaults *userDefault = [NSUserDefaults standardUserDefaults];
    NSString *lastVersion = [userDefault valueForKey:version];
    
    if ([lastVersion isEqualToString:currentVersion]) {
//        存储当前版本信息
        [userDefault setObject:currentVersion forKey:version];
        [userDefault synchronize];
//         开启app显示新特性
        MPNewFeatureViewController *newFeatureVC = [[MPNewFeatureViewController alloc] init];
        keyWindow.rootViewController = newFeatureVC;
    }else{
        MPTabBarViewController *tabBarController = [[MPTabBarViewController alloc]init];
        keyWindow.rootViewController = tabBarController;
    }
}
@end
