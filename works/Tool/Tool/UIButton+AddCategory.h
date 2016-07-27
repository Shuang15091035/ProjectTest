//
//  UIButton+AddCategory.h
//  Tool
//
//  Created by mac zdszkj on 16/3/17.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface UIButton (AddCategory)
+ (UIButton *)createButtonWithTitle:(NSString *)title btnFrame:(CGRect)frame btnColor:(UIColor *)btnColor btnBackgroundColor:(UIColor *)bgColor btnFont:(CGFloat)fontSize ;
@end
