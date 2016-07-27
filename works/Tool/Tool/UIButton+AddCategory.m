//
//  UIButton+AddCategory.m
//  Tool
//
//  Created by mac zdszkj on 16/3/17.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "UIButton+AddCategory.h"

@implementation UIButton (AddCategory)
+ (UIButton *)createButtonWithTitle:(NSString *)title btnFrame:(CGRect)frame btnColor:(UIColor *)btnColor btnBackgroundColor:(UIColor *)bgColor btnFont:(CGFloat)fontSize {
    UIButton *btn = [[UIButton alloc]initWithFrame:frame];
    [btn setTitle:title forState:UIControlStateNormal];
    [btn setTitleColor:btnColor forState:UIControlStateNormal];
    btn.contentMode = UIViewContentModeCenter;
    btn.backgroundColor = bgColor;
    btn.titleLabel.font = [UIFont systemFontOfSize:fontSize];
    return btn;
}
@end
