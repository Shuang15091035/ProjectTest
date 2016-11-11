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
@synthesize wPoint1 = mWallPoint1;
@synthesize wPoint2 = mWallPoint2;
@synthesize wallComponentArr = mWallComponentArr;

- (float)lineA{
    mLineA = mWallPoint2.wallPoint.y - mWallPoint1.wallPoint.y;
    return mLineA;
}
- (float)lineB{
    mLineB = mWallPoint1.wallPoint.x - mWallPoint2.wallPoint.x;
    return mLineB;
}
-(float)lineC{
    mLineC = mWallPoint2.wallPoint.x*mWallPoint1.wallPoint.y-mWallPoint1.wallPoint.x*mWallPoint2.wallPoint.y;
    return mLineC;
}

- initWithWallPoint1:(WallPoint *)wallPoint1 wallPoint2:(WallPoint *)wallPoint2{
    self = [super init];
    if (self) {
        mWallPoint1 = wallPoint1;
        mWallPoint2 = wallPoint2;
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
    
    footPoint.x=(self.lineB * self.lineB * outPoint.x - self.lineA * self.lineB * outPoint.y - self.lineA * self.lineC)/(self.lineA * self.lineA + self.lineB * self.lineB);
    footPoint.y=(-self.lineA * self.lineB * outPoint.x + self.lineA * self.lineA * outPoint.y - self.lineB * self.lineC)/(self.lineA * self.lineA + self.lineB * self.lineB);
    
    return footPoint;
}

- (float) distanceOfLineFromPoint:(CGPoint)outPoint{
    
    float distance=fabs((self.lineA * outPoint.x + self.lineB * outPoint.y + self.lineC))/sqrt(self.lineA * self.lineA + self.lineB * self.lineB);
    
    return distance;
}

- (bool) isOnLineOfPoint:(CGPoint) outPoint{
    
    bool isOnLine = false;
    if (self.lineA * outPoint.x + self.lineB * outPoint.y + self.lineC == 0) {
        isOnLine = true;
    }
    return isOnLine;
}

- (CGFloat) CurrentLineAngle{
    if (mWallPoint1.wallPoint.x == mWallPoint2.wallPoint.x) {
        return M_PI_2;
    }
    CGFloat LineK = - self.lineA / self.lineB;
    return atan(LineK);
}
- (CGFloat) getYValueArccordingToTouchPoint:(CGPoint)TouchPoint{
    if (self.lineB == 0) {
        return TouchPoint.y;
    }
    return (-self.lineA * TouchPoint.x - self.lineC)/self.lineB;
}

- (void)updateComponentPercentOfWallLine{
    CGFloat wallWidth = [self getWallLineWidth];
    for (ArchWallComponent *wallComponent in mWallComponentArr) {
     wallComponent.percent = [self distantof:wallComponent.componentPosition point2:mWallPoint2.wallPoint]/wallWidth;
    }
}
- (CGFloat)getWallLineWidth{
    return sqrtf(powf((mWallPoint2.wallPoint.x - mWallPoint1.wallPoint.x), 2) + powf((mWallPoint2.wallPoint.y - mWallPoint1.wallPoint.y), 2));
}
- (CGFloat)distantof:(CGPoint)point1 point2:(CGPoint)point2{
    return sqrtf(powf((point2.x - point1.x), 2) + powf((point2.y - point1.y), 2));
}

@end
