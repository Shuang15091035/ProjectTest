//
//  ArchComponent.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/11/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "HomeArchItem.h"

@interface HomeArchItem(){
    HomeComponentType mComponentType;
    CGPoint mComponentPosition;
    CGFloat mComponentWidth;
    CGFloat mComponentHeight;
    UIImageView *mComponentView;
    CGPoint mComponentStartP;
    CGPoint mComponentEndP;
    NSInteger mWallIndex;
    CGFloat mPercent;
}
@end

@implementation HomeArchItem

@synthesize componentType = mComponentType;
@synthesize componentWidth = mComponentWidth;
@synthesize componentHeight = mComponentHeight;
@synthesize componentView = mComponentView;
@synthesize componentStartP = mComponentStartP;
@synthesize componentEndP = mComponentEndP;
@synthesize wallIndex = mWallIndex;
@synthesize percent = mPercent;

- (CGPoint)componentPosition{
    CGPoint centerPoint = mComponentView.center;
    return centerPoint;
}

- (instancetype)initWithDoorType:(HomeComponentType)componentType doorWidth:(CGFloat)componentWidth componentHeight:(CGFloat)componentHeight {
    
    self = [super init];
    if (self) {
        mComponentType = componentType;
        mComponentWidth = componentWidth;
        mComponentHeight = componentHeight;
        mComponentView = [self getComponentWidth:componentWidth];
    }
    return self;
}

- (UIImageView *)getComponentWidth:(float)compWidth{
    UIImageView *imageView = [[UIImageView alloc]init];
    imageView.userInteractionEnabled = YES;
    imageView.tag = 300;//默认给定标示
    [imageView setBackgroundColor:[UIColor grayColor]];
    imageView.frame = CGRectMake(0, 0, compWidth, compWidth);
    return imageView;
}

@end
