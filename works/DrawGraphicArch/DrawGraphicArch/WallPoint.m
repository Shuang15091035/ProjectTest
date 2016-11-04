//
//  WallPoint.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 16/10/10.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "WallPoint.h"

@interface WallPoint(){
    CGPoint mWallPoint;
}
@property (nonatomic, readwrite) UIImageView *mImageView;

@end

@implementation WallPoint

@synthesize wallPoint = mWallPoint;

- (id)initWithPoint:(CGPoint)touchP currentPointView:(UIView *)currentView{
    self = [super init];
    if (self) {

        mWallPoint = touchP;
        self.mImageView.center = mWallPoint;
        [currentView addSubview:self.mImageView];
    }
    return self;
}

- (float) distanceOfAnotherPoint:(CGPoint)anPoint{
    return sqrtf(powf((anPoint.y - mWallPoint.y), 2) + powf((anPoint.x - mWallPoint.x), 2));
}

- (UIImageView *)mImageView{
    if (_mImageView == nil) {
        _mImageView = [[UIImageView alloc]init];
        _mImageView.userInteractionEnabled = YES;
        _mImageView.backgroundColor = [self randomColor];
        _mImageView.frame = CGRectMake(0, 0, 40, 40);
    }
    return _mImageView;
}

- (UIColor *)randomColor{
    
    CGFloat red =  (CGFloat)random()/(CGFloat)RAND_MAX;
    CGFloat blue = (CGFloat)random()/(CGFloat)RAND_MAX;
    CGFloat green = (CGFloat)random()/(CGFloat)RAND_MAX;
    return [UIColor colorWithRed:red green:green blue:blue alpha:1.0f];
}
@end
