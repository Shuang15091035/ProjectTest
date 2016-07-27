//
//  MPTabBarViewController.m
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MPTabBarViewController.h"
#import "MPTabBar.h"
#import "MPNavigationViewController.h"
#import "MPHomeViewController.h"
#import "MPPhotoLibraryViewController.h"
#import "MPCommunityViewController.h"
#import "MPProfileViewController.h"

@interface MPTabBarViewController ()
/** 上一次选择的tabBarItem */
@property(nonatomic, strong) UITabBarItem *lastSelectedTabBarItem;
@property(nonatomic, strong) MPTabBar *mpTabBar;

@end

@implementation MPTabBarViewController

/** 类初始化的时候调用 */
//+ (void)initialize {
//    // 初始化导航栏样式
//    [[UITabBarItem appearance]setTitleTextAttributes:@{NSForegroundColorAttributeName:[UIColor blackColor],NSFontAttributeName:[UIFont systemFontOfSize:15]} forState:UIControlStateNormal];
//    
//}
- (void)viewDidLoad {
    [super viewDidLoad];
    // 使用自定义的TabBar
    MPTabBar *mpTabBar = [[MPTabBar alloc] init];
    // 重设tabBar，由于tabBar是只读成员，使用KVC相当于直接修改_tabBar
    [self setValue:mpTabBar forKey:@"tabBar"];
    self.mpTabBar = mpTabBar;
    
    // 添加子控制器
    [self addAllChildViewControllers];
    
}
/** 添加所有子控制器 */
- (void)addAllChildViewControllers{
    NSArray *vcTitles = @[@"军事",@"图片",@"论坛",@"我的"];
    NSArray *vcNames = @[@"MPHomeViewController",@"MPPhotoLibraryViewController",@"MPCommunityViewController",@"MPProfileViewController"];
    NSArray *normalImageNames = @[@"main_home_normal",@"main_pic_normal",@"main_forum_normal",@"main_user_normal"];
    NSArray *selectedImageNames = @[@"main_home_press",@"main_pic_press",@"main_forum_press",@"main_user_press"];
    for (int index = 0; index < vcNames.count; index++) {
        UIViewController *viewController = [[NSClassFromString(vcNames[index]) alloc] init];
        [self addChildViewController:viewController WithTitle:vcTitles[index] image:normalImageNames[index] seletectedImage:selectedImageNames[index]];
    }
}
/** 添加tab子控制器 */
- (void) addChildViewController:(UIViewController *) viewController WithTitle:(NSString *) title image:(NSString *) imageName seletectedImage:(NSString *) selectedImageName {
    viewController.title = title;
    // 设置图标
    viewController.tabBarItem.image = [UIImage imageWithNamed:imageName];
    
    // 被选中时图标
    UIImage *selectedImage = [UIImage imageWithNamed:selectedImageName];
    // 如果是iOS7，不要渲染被选中的tab图标（iOS7中会自动渲染成为蓝色）
    if (iOS7) {
        selectedImage = [selectedImage imageWithRenderingMode:UIImageRenderingModeAlwaysOriginal];
    }
    viewController.tabBarItem.selectedImage = selectedImage;
    
    MPNavigationViewController *nav = [[MPNavigationViewController alloc] initWithRootViewController:viewController];
    [self addChildViewController:nav];
}
/** 选中了某个tabBarItem */
- (void)tabBar:(UITabBar *)tabBar didSelectItem:(UITabBarItem *)item{
    UINavigationController *nav = (UINavigationController *)self.selectedViewController;
    
    if ([[nav.viewControllers firstObject] isKindOfClass:[MPHomeViewController class]]) { // 如果是“首页”item被点击
        if (self.lastSelectedTabBarItem == item) { // 重复点击
//            [self.homeVC refreshStatusFromAnother:NO];
        } else { // 跳转点击
//            [self.homeVC refreshStatusFromAnother:YES];
        }
    } else {
       
    }
}

@end
