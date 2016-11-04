//
//  ArchComponent.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/11/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ArchWallComponent.h"

@interface ArchWallComponent(){
    ComponentType mComponentType;
    CGPoint mComponentPosition;
    CGFloat mComponentWidth;
    CGFloat mComponentHeight;
    UIImageView *mComponentView;
    CGPoint mComponentStartP;
    CGPoint mComponentEndP;
    NSInteger mWallIndex;
}
@end

@implementation ArchWallComponent

@synthesize componentType = mComponentType;
@synthesize componentWidth = mComponentWidth;
@synthesize componentHeight = mComponentHeight;
@synthesize componentView = mComponentView;
@synthesize componentStartP = mComponentStartP;
@synthesize componentEndP = mComponentEndP;
@synthesize wallIndex = mWallIndex;

- (CGPoint)componentPosition{
    return mComponentView.center;
}

- (instancetype)initWithDoorType:(ComponentType)componentType doorWidth:(CGFloat)componentWidth componentHeight:(CGFloat)componentHeight {
    
    self = [super init];
    if (self) {
        mComponentType = componentType;
        mComponentWidth = componentWidth;
        mComponentHeight = componentHeight;
        mComponentView = [self getComponent];
    }
    return self;
}

- (UIImageView *)getComponent{
    UIImageView *imageView = [[UIImageView alloc]init];
    imageView.userInteractionEnabled = YES;
    imageView.tag = 300;//默认给定标示
    [imageView setBackgroundColor:[UIColor grayColor]];
    imageView.frame = CGRectMake(0, 0, 40, 40);
    return imageView;
}

@end
