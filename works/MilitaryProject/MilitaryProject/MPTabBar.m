//
//  MPTabBar.m
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MPTabBar.h"

@implementation MPTabBar

+(void)initialize{
    [[self appearance]setBarTintColor:[UIColor colorWithRed:148/255.0 green:182/255.0 blue:222/255.0 alpha:1]];
}
- (void)layoutSubviews{
    [super layoutSubviews];
    
    // 设置文本属性
    [self initTextAttr];
    
    // 设置BarButton的位置
    [self initBarButtonPosition];
    
}
- (void)initTextAttr{
    NSMutableDictionary *attr = [NSMutableDictionary dictionary];
    attr[NSForegroundColorAttributeName] = [UIColor orangeColor];
    attr[NSFontAttributeName] = [UIFont systemFontOfSize:14];
    NSMutableDictionary *attr1 = [NSMutableDictionary dictionary];
    attr1[NSFontAttributeName] = [UIFont systemFontOfSize:12];
    for (UITabBarItem *item in self.items) {
        // 设置字体颜色
        [item setTitleTextAttributes:attr1 forState:UIControlStateNormal];
        [item setTitleTextAttributes:attr forState:UIControlStateSelected];
 
    }
}
- (void)initBarButtonPosition{
    
    for (UIView *tabBarButton in self.subviews) {
        if ([tabBarButton isKindOfClass:NSClassFromString(@"UITabBarButton")]) {
           
            CGFloat width = self.width / (self.items.count);
//            tabBarButton.backgroundColor = randomColor();
            tabBarButton.width = width;
            
        }
    }
}
@end
