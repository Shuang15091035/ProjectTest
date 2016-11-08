//
//  WallLine.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/28.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "WallLine.h"

@interface WallLine(){
    float mWallHeight;
    WallPoint *mWallPoint1;
    WallPoint *mWallPoint2;
    float mLineA;
    float mLineB;
    float mLineC;
    NSMutableArray<ArchWallComponent*> *mWallComponentArr;
}

@end

@implementation WallLine

@synthesize wallHeight = mWallHeight;
@synthesize lineA = mLineA;
@synthesize lineB = mLineB;
@synthesize lineC = mLineC;
@synthesize wPoint1 = mWallPoint1;
@synthesize wPoint2 = mWallPoint2;
@synthesize wallComponentArr = mWallComponentArr;


- initWithWallPoint1:(WallPoint *)wallPoint1 wallPoint2:(WallPoint *)wallPoint2{
    self = [super init];
    if (self) {
        mWallPoint1 = wallPoint1;
        mWallPoint2 = wallPoint2;
        mLineA = mWallPoint2.wallPoint.y - mWallPoint1.wallPoint.y;
        mLineB = mWallPoint1.wallPoint.x - mWallPoint2.wallPoint.x;
        mLineC = mWallPoint2.wallPoint.x*mWallPoint1.wallPoint.y-mWallPoint1.wallPoint.x*mWallPoint2.wallPoint.y;
        mWallComponentArr = [NSMutableArray array];
        mWallHeight = 10.0f;
    }
    return self;
}
- (float)minX{
    return mWallPoint1.wallPoint.x < mWallPoint2.wallPoint.x ? mWallPoint1.wallPoint.x : mWallPoint2.wallPoint.x;
}
- (float)maxX{
    return mWallPoint1.wallPoint.x > mWallPoint2.wallPoint.x ? mWallPoint1.wallPoint.x : mWallPoint2.wallPoint.x;
}
-(float)minY{
    return mWallPoint1.wallPoint.y < mWallPoint2.wallPoint.y ? mWallPoint1.wallPoint.y : mWallPoint2.wallPoint.y;
}
- (float)maxY{
    return mWallPoint1.wallPoint.y > mWallPoint2.wallPoint.y ? mWallPoint1.wallPoint.y : mWallPoint2.wallPoint.y;
}

- (CGPoint) pedalOfLineAndVerticalAccordingToLineOutPoint:(CGPoint)outPoint{
    CGPoint footPoint = CGPointZero;
    
    footPoint.x=(mLineB * mLineB * outPoint.x - mLineA * mLineB * outPoint.y - mLineA * mLineC)/(mLineA * mLineA + mLineB * mLineB);
    footPoint.y=(-mLineA * mLineB * outPoint.x + mLineA * mLineA * outPoint.y - mLineB * mLineC)/(mLineA * mLineA + mLineB * mLineB);
    
    return footPoint;
}

- (float) distanceOfLineFromPoint:(CGPoint)outPoint{
    
    float distance=fabs((mLineA * outPoint.x + mLineB * outPoint.y + mLineC))/sqrt(mLineA * mLineA + mLineB * mLineB);
    
    return distance;
}

- (bool) isOnLineOfPoint:(CGPoint) outPoint{
    
    bool isOnLine = false;
    if (mLineA * outPoint.x + mLineB * outPoint.y + mLineC == 0) {
        isOnLine = true;
    }
    return isOnLine;
}

- (CGFloat) CurrentLineAngle{
    if (mWallPoint1.wallPoint.x == mWallPoint2.wallPoint.x) {
        return M_PI_2;
    }
    CGFloat LineK = - mLineA / mLineB;
    return atan(LineK);
}
- (CGFloat) getYValueArccordingToTouchPoint:(CGPoint)TouchPoint{
    if (mLineB == 0) {
        return TouchPoint.y;
    }
    return (-mLineA * TouchPoint.x - mLineC)/mLineB;
}
@end
