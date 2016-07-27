//
//  Horizontal.m
//  Camera
//
//  Created by mac zdszkj on 16/4/8.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "Horizontal.h"

//static UIEdgeInsets const kPadding = {0, 0, 0, 0};

@interface Horizontal()
@end

@implementation Horizontal

- (instancetype)init
{
    self = [super init];
    if (self) {
        [self customView];
    }
    return self;
}
- (void)customView{
    UIImageView *imageView = [UIImageView new];
    imageView.backgroundColor = [UIColor redColor];
    [self addSubview:imageView];
    
//    UILabel *titleNext = [UILabel new];
//    titleNext.backgroundColor = [self randomColor];
//    titleNext.text = @"下一个";
//    titleNext.textColor = [UIColor blackColor];
//    [self addSubview:titleNext];
//    
//    UIImageView *nextView = [UIImageView new];
//    nextView.backgroundColor = [self randomColor];
//    [self addSubview:nextView];
    UIView *superView = self;
    [imageView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.equalTo(superView);
        make.top.equalTo(superView);
        make.bottom.equalTo(superView);
        make.right.equalTo(superView);
    }];
    
//    [titleNext mas_makeConstraints:^(MASConstraintMaker *make) {
//        make.top.and.right.equalTo(superview).insets(kPadding);
//        make.left.equalTo(imageView.mas_right).insets(kPadding);
//        make.width.equalTo(imageView);
//        make.height.equalTo(@200);
//    }];
    
//    [nextView mas_makeConstraints:^(MASConstraintMaker *make) {
//        make.top.equalTo(titleNext.mas_bottom).offset(10);
//        make.left.equalTo(titleNext).offset(10);
//        make.right.equalTo(self).offset(-10);
//    }];
    
}
- (UIColor *)randomColor {
    CGFloat hue = ( arc4random() % 256 / 256.0 );  //  0.0 to 1.0
    CGFloat saturation = ( arc4random() % 128 / 256.0 ) + 0.5;  //  0.5 to 1.0, away from white
    CGFloat brightness = ( arc4random() % 128 / 256.0 ) + 0.5;  //  0.5 to 1.0, away from black
    return [UIColor colorWithHue:hue saturation:saturation brightness:brightness alpha:1];
}
@end
