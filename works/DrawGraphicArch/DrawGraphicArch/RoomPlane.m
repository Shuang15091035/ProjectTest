//
//  RoomPlan.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "RoomPlane.h"
#define EPS 0.00001

@interface RoomPlane(){
    NSMutableArray<WallLine *>*mWallLines;
    NSMutableArray<WallLine *>*mOutWallLines;
    NSMutableArray<WallPoint *>*mRoomPoints;
    NSMutableArray<WallPoint *>* mOutRoomPoints;
    NSMutableArray<ArchWallComponent *>*mCurrentRoomComponents;
}

@end

@implementation RoomPlane

@synthesize roomPoints = mRoomPoints;
@synthesize outRoomPoints = mOutRoomPoints;

- (instancetype)init{
    self = [super init];
    if (self) {
        mWallLines = [NSMutableArray array];
        mOutWallLines = [NSMutableArray array];
        mRoomPoints = [NSMutableArray array];
        mOutRoomPoints = [NSMutableArray array];
        mCurrentRoomComponents = [NSMutableArray array];
    }
    return self;
}

- (NSMutableArray<WallLine *> *)wallLines{

    if (mWallLines.count == 0) {
        NSUInteger pointCount = mRoomPoints.count;
        for (NSUInteger i = 0,j = pointCount - 1; i < pointCount; i++) {
            if (i == j) {
                WallLine *wl = [[WallLine alloc]initWithWallPoint1:mRoomPoints[i] wallPoint2:mRoomPoints[0]];
                [mWallLines addObject:wl];
            }else{
                WallLine *wl = [[WallLine alloc]initWithWallPoint1:mRoomPoints[i] wallPoint2:mRoomPoints[i+1]];
                [mWallLines addObject:wl];
            }
        }

    }
    return mWallLines;
}

-(NSMutableArray<WallLine *> *)outWallLines{
    
    if (mOutWallLines.count == 0) {
        NSUInteger pointCount = mOutRoomPoints.count;
        for (NSUInteger i = 0,j = pointCount - 1; i < pointCount; i++) {
            if (i == j) {
                WallLine *wl = [[WallLine alloc]initWithWallPoint1:mOutRoomPoints[i] wallPoint2:mOutRoomPoints[0]];
                [mOutWallLines addObject:wl];
            }else{
                WallLine *wl = [[WallLine alloc]initWithWallPoint1:mOutRoomPoints[i] wallPoint2:mOutRoomPoints[i+1]];
                [mOutWallLines addObject:wl];
            }
        }
    }
    return mOutWallLines;
}

- (NSMutableArray<ArchWallComponent *> *)currentRoomComponents{
    
    [mCurrentRoomComponents removeAllObjects];
    for (ArchWallComponent *wallCom in self.wallLines) {
        [mCurrentRoomComponents addObject:wallCom];
    }
    return mCurrentRoomComponents;
}

- (CGPoint)intersetctionOfWallLine1:(WallLine *)wallLine wallLine2:(WallLine *)wallLine2{
    
    CGPoint crossP;
    crossP.y = (wallLine2.lineA * wallLine.lineC - wallLine.lineA * wallLine2.lineC)/(wallLine.lineA * wallLine2.lineB - wallLine2.lineA * wallLine.lineB);
    crossP.x = -wallLine.lineC - wallLine.lineB * crossP.y;
    
    return crossP;
}
/**
 
 @param ptStart  线段起点
 @param ptEnd    线段终点
 @param ptCenter 圆心坐标
 @param Radius   半径
 * @note 与圆可能存在两个交点，如果存在两个交点在ptInter1和ptInter2都为有效值，如果有一个交点，则ptInter2的值为
 *       无效值，此处为65536.0
 */
- (bool) lineInterCircle:(WallLine *)wallLine centerPoint:(const CGPoint)ptCenter radius:(const float)radius wallComponent:(ArchWallComponent*)wallComp{
    CGPoint interPoint1 = wallComp.componentStartP;
    CGPoint interPoint2 = wallComp.componentEndP;
    interPoint1.x = interPoint1.y = 65536.0f;
    interPoint2.x = interPoint2.y = 65536.0f;
    CGPoint ptStart = wallLine.wPoint1.wallPoint;
    CGPoint ptEnd = wallLine.wPoint2.wallPoint;
    
    float deltaX = ptEnd.x - ptStart.x;
    float deltaY = ptEnd.y - ptStart.y;
    float fDis = sqrt(powf(deltaX, 2) + powf(deltaY, 2));
    
    CGPoint d;
    d.x = deltaX / fDis;
    d.y = deltaY / fDis;
    
    CGPoint E;
    E.x = ptCenter.x - ptStart.x;
    E.y = ptCenter.y - ptStart.y;
    
    float a = E.x * d.x + E.y * d.y;
    float a2 = a * a;
    
    float e2 = E.x * E.x + E.y * E.y;
    
    float r2 = radius * radius;
    
    if ((r2 - e2 + a2) < 0)
    {
        return false;
    }
    else
    {
        float f = sqrt(r2 - e2 + a2);
        
        float t = a - f;
        
        if( ((t - 0.0) > - EPS) && (t - fDis) < EPS)
        {
            interPoint1.x = ptStart.x + t * d.x;
            interPoint1.y = ptStart.y + t * d.y;
        }
        
        t = a + f;
        
        if( ((t - 0.0) > - EPS) && (t - fDis) < EPS)
        {
            interPoint2.x = ptStart.x + t * d.x;
            interPoint2.y = ptStart.y + t * d.y;
        }
        //做一个墙体组件两个交点方向的判断
        if (interPoint1.x < 65534 && interPoint2.x < 65534) {
            float interSectionX = interPoint2.x - interPoint1.x;
            float interSectionY = interPoint2.y - interPoint1.y;
        
            CGPoint normalizerWallLine = CGPointMake(d.x, d.y);
            CGPoint normalizerCompLine = CGPointMake(interSectionX, interSectionY);
            CGFloat dotProduct = normalizerWallLine.x * normalizerCompLine.x + normalizerCompLine.y * normalizerWallLine.y;
            if (dotProduct > 0) {
                wallComp.componentStartP = interPoint1;
                wallComp.componentEndP = interPoint2;
            }else{
                wallComp.componentStartP = interPoint2;
                wallComp.componentEndP = interPoint1;
            }
            wallComp.componentView.center = wallComp.componentEndP;
        }else{
            CGPoint result = CGPointZero;
            result.x = interPoint1.x < 60000 ? interPoint1.x : interPoint2.x;
            result.y = interPoint1.y < 60000 ? interPoint1.y : interPoint2.y;
            wallComp.componentEndP = result;
            wallComp.componentView.center = wallComp.componentEndP;
        }
        return true;
    }
}

- (bool) addAvailableComponentView:(ArchWallComponent *)wallComponent{
    
     //wallComponent.componentWidth
    for (WallLine *singleWall in self.wallLines) {
        if (singleWall.wallComponentArr.count == 0) {
            [self lineInterCircle:singleWall centerPoint:singleWall.wPoint1.wallPoint radius:wallComponent.componentWidth/2 wallComponent: wallComponent];
            [singleWall.wallComponentArr addObject: wallComponent];
            wallComponent.wallIndex = [self.wallLines indexOfObject:singleWall];
            return true;
        }else{
            NSInteger sWCount = singleWall.wallComponentArr.count;
            for (int i = 0; i < sWCount + 1; i++) {
                CGPoint component1Point = CGPointZero;
                CGPoint component2Point = CGPointZero;
                if (i == 0) {
                    component1Point = singleWall.wPoint1.wallPoint;
                    component2Point = singleWall.wallComponentArr[i].componentPosition;
                    CGFloat deltaX = component2Point.x - component1Point.x;
                    CGFloat deltaY = component2Point.y - component1Point.y;
                    CGFloat pointDis = sqrtf(deltaY * deltaY + deltaX * deltaX);
                    if (pointDis - singleWall.wallComponentArr[i].componentWidth/2 > wallComponent.componentWidth ) {
                        [self lineInterCircle:singleWall centerPoint:component1Point radius:wallComponent.componentWidth/2 wallComponent: wallComponent];
                    }
                }else if(i == sWCount + 1){
                    component1Point = singleWall.wallComponentArr[i].componentPosition;
                    component2Point = singleWall.wPoint2.wallPoint;
                    CGFloat deltaX = component2Point.x - component1Point.x;
                    CGFloat deltaY = component2Point.y - component1Point.y;
                    CGFloat pointDis = sqrtf(deltaY * deltaY + deltaX * deltaX);
                    if (pointDis - singleWall.wallComponentArr[i].componentWidth/2 > wallComponent.componentWidth ) {
                        [self lineInterCircle:singleWall centerPoint:component1Point radius:wallComponent.componentWidth/2 wallComponent: wallComponent];
                    }
                }else{
                    component1Point = singleWall.wallComponentArr[i].componentPosition;
                    component2Point = singleWall.wallComponentArr[i+1].componentPosition;
                    CGFloat deltaX = component2Point.x - component1Point.x;
                    CGFloat deltaY = component2Point.y - component1Point.y;
                    CGFloat pointDis = sqrtf(deltaY * deltaY + deltaX * deltaX);
                    if (pointDis - singleWall.wallComponentArr[i].componentWidth/2 > wallComponent.componentWidth ) {
                        [self lineInterCircle:singleWall centerPoint:component1Point radius:wallComponent.componentWidth/2 wallComponent: wallComponent];
                    }
                }
            }
        }
    }
    return YES;
}

@end
