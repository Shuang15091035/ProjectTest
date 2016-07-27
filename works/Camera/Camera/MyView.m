//
//  MyView.m
//  Camera
//
//  Created by mac zdszkj on 16/3/16.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MyView.h"


@interface MyView()
@property (nonatomic,readwrite) UIButton *doneBtn;
@property (nonatomic,readwrite) UIButton *snapBtn;
@property (nonatomic,readwrite) UIButton *delayBtn;
@property (nonatomic,readwrite) UIButton *startBtn;
@end

@implementation MyView

- (instancetype)init{
    self = [super init];
    if (!self) return nil;
//    self.backgroundColor = [UIColor whiteColor];
    
    UIButton *doneBtn = [UIButton new];
    doneBtn.backgroundColor = [UIColor whiteColor];
    doneBtn.titleLabel.text = @"Done";
    doneBtn.titleLabel.textColor = [UIColor redColor];
    doneBtn.layer.borderWidth = 2;
    doneBtn.layer.borderColor = [self randomColor].CGColor;
    self.doneBtn = doneBtn;
    [self addSubview:self.doneBtn];
    
    UIButton *snapBtn = [UIButton new];
    snapBtn.backgroundColor = [UIColor whiteColor];
    snapBtn.titleLabel.text = @"Snap";
    snapBtn.titleLabel.textColor = [UIColor redColor];
    snapBtn.layer.borderColor = [self randomColor].CGColor;
    snapBtn.layer.borderWidth = 2;
    self.snapBtn = snapBtn;
    [self addSubview:self.snapBtn];
    
    UIButton *delayBtn = [UIButton new];
    delayBtn.backgroundColor = [UIColor whiteColor];
    delayBtn.titleLabel.text = @"Delay";
    delayBtn.titleLabel.textColor = [UIColor redColor];
    delayBtn.layer.borderWidth = 2;
    delayBtn.layer.borderColor = [self randomColor].CGColor;
    self.delayBtn = delayBtn;
    [self addSubview:self.delayBtn];
    
    UIButton *startBtn = [UIButton new];
    startBtn.backgroundColor = [UIColor whiteColor];
    startBtn.titleLabel.text = @"Star";
    startBtn.titleLabel.font = [UIFont systemFontOfSize:18];
    startBtn.titleLabel.textColor = [UIColor redColor];
    startBtn.layer.borderColor = [self randomColor].CGColor;
    startBtn.layer.borderWidth = 2;
    self.startBtn = startBtn;
    [self addSubview:self.startBtn];
    
    UIView *superView = self;
    UIEdgeInsets padding = UIEdgeInsetsMake(15, 10, 15, 10);
    
    [snapBtn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.and.bottom.equalTo(superView).insets(padding);
        make.width.equalTo(@50);
        make.height.equalTo(@30);
    }];
    
    [delayBtn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.bottom.equalTo(superView).insets(padding);
        make.right.equalTo(startBtn.mas_left).insets(padding);
        make.width.equalTo(@50);
        make.height.equalTo(@30);
    }];
    
    [startBtn mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.equalTo(delayBtn.mas_right).insets(padding);
        make.right.equalTo(superView).insets(padding);
        make.bottom.equalTo(superView).insets(padding);
        make.width.equalTo(@50);
        make.height.equalTo(@30);
    }];
    return self;
}



- (UIColor *)randomColor {
    CGFloat hue = ( arc4random() % 256 / 256.0 );  //  0.0 to 1.0
    CGFloat saturation = ( arc4random() % 128 / 256.0 ) + 0.5;  //  0.5 to 1.0, away from white
    CGFloat brightness = ( arc4random() % 128 / 256.0 ) + 0.5;  //  0.5 to 1.0, away from black
    return [UIColor colorWithHue:hue saturation:saturation brightness:brightness alpha:1];
}
@end
