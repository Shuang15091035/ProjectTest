//
//  RoomPlan.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "HomeRoomPlane.h"
#define EPS 0.00001

@interface HomeRoomPlane(){
    NSMutableArray<HomeWallLine *>*mWallLines;
    NSMutableArray<HomeWallLine *>*mOutWallLines;
    NSMutableArray<HomeWallPoint *>*mRoomPoints;
    NSMutableArray<HomeWallPoint *>* mOutRoomPoints;
    NSMutableArray<HomeArchItem *>*mCurrentRoomComponents;
}

@end

@implementation HomeRoomPlane

@synthesize roomPoints = mRoomPoints;
@synthesize outRoomPoints = mOutRoomPoints;

bool getComCenterlineInterCircle(HomeWallLine * wallLine, const CGPoint ptCenter, const float radius,  CGPoint *interPoint1, CGPoint *interPoint2);
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
- (instancetype)initWithGround:(float)ground wallHeight:(float)wallHeight oomPoints:(NSMutableArray<HomeWallPoint *> *)roomPoints archItem:(NSMutableArray<HomeArchItem *> *)archItems{
    mRoomPoints = roomPoints;
    mCurrentRoomComponents = archItems;
    return self;
}
- (NSMutableArray<HomeWallLine *> *)wallLines{

    if (mWallLines.count == 0) {
        NSUInteger pointCount = mRoomPoints.count;
        for (NSUInteger i = 0,j = pointCount - 1; i < pointCount; i++) {
            if (i == j) {
                HomeWallLine *wl = [[HomeWallLine alloc]initWithWallPoint1:mRoomPoints[i] wallPoint2:mRoomPoints[0]];
                [mWallLines addObject:wl];
            }else{
                HomeWallLine *wl = [[HomeWallLine alloc]initWithWallPoint1:mRoomPoints[i] wallPoint2:mRoomPoints[i+1]];
                [mWallLines addObject:wl];
            }
        }
    }
    return mWallLines;
}

-(NSMutableArray<HomeWallLine *> *)outWallLines{
    
    if (mOutWallLines.count == 0) {
        NSUInteger pointCount = mOutRoomPoints.count;
        for (NSUInteger i = 0,j = pointCount - 1; i < pointCount; i++) {
            if (i == j) {
                HomeWallLine *wl = [[HomeWallLine alloc]initWithWallPoint1:mOutRoomPoints[i] wallPoint2:mOutRoomPoints[0]];
                [mOutWallLines addObject:wl];
            }else{
                HomeWallLine *wl = [[HomeWallLine alloc]initWithWallPoint1:mOutRoomPoints[i] wallPoint2:mOutRoomPoints[i+1]];
                [mOutWallLines addObject:wl];
            }
        }
    }
    return mOutWallLines;
}

- (NSMutableArray<HomeArchItem *> *)currentRoomComponents{
    
    [mCurrentRoomComponents removeAllObjects];
    for (HomeArchItem *wallCom in self.wallLines) {
        [mCurrentRoomComponents addObject:wallCom];
    }
    return mCurrentRoomComponents;
}

- (CGPoint)intersetctionOfWallLine1:(HomeWallLine *)wallLine wallLine2:(HomeWallLine *)wallLine2{
    
    CGPoint crossP;
    crossP.y = (wallLine2.lineA * wallLine.lineC - wallLine.lineA * wallLine2.lineC)/(wallLine.lineA * wallLine2.lineB - wallLine2.lineA * wallLine.lineB);
    crossP.x = -wallLine.lineC - wallLine.lineB * crossP.y;
    
    return crossP;
}
/**
 线段起点,线段终点,圆心坐标,半径
 * @note 与圆可能存在两个交点，如果存在两个交点在ptInter1和ptInter2都为有效值，如果有一个交点，则ptInter2的值为
 *       无效值，此处为65536.0
 */
- (bool) lineInterCircle:(HomeWallLine *)wallLine centerPoint:(const CGPoint)ptCenter radius:(const float)radius wallComponent:(HomeArchItem*)wallComp{
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
            wallComp.componentView.center = ptCenter;
        }
        return true;
    }
}

- (bool) addAvailableComponentView:(HomeArchItem *)wallComponent{
    
     //wallComponent.componentWidth
    for (HomeWallLine *singleWall in self.wallLines) {
        bool isAvaliblePosition = false;
        if (singleWall.wallComponentArr.count == 0) {
            CGPoint interPoint1 = CGPointZero;
            CGPoint interPoint2 = CGPointZero;
            getComCenterlineInterCircle(singleWall, singleWall.wPoint1.wallPoint, wallComponent.componentWidth/2, &interPoint1, &interPoint2);
            [self lineInterCircle:singleWall centerPoint:interPoint2 radius:wallComponent.componentWidth/2 wallComponent: wallComponent];
            [singleWall.wallComponentArr addObject: wallComponent];
            wallComponent.wallIndex = [self.wallLines indexOfObject:singleWall];
            wallComponent.componentView.transform = CGAffineTransformMakeRotation(singleWall.CurrentLineAngle);
            break;
        }else{
            NSInteger sWCount = singleWall.wallComponentArr.count;
            for (int i = 0; i < sWCount ; i++) { //墙上的组件由近到远重新排列
                for (int j = 0; j < sWCount - 1 - i; j++) {
                    CGFloat dis1 = [singleWall.wPoint1 distanceOfAnotherPoint: singleWall.wallComponentArr[i].componentPosition];
                    CGFloat dis2 = [singleWall.wPoint1 distanceOfAnotherPoint: singleWall.wallComponentArr[i+1].componentPosition];
                    if (dis1 > dis2) {
                        [singleWall.wallComponentArr exchangeObjectAtIndex:i withObjectAtIndex:i+1];
                    }
                }
            }
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
                        CGPoint interPoint1 = CGPointZero;
                        CGPoint interPoint2 = CGPointZero;
                        getComCenterlineInterCircle(singleWall, component1Point, wallComponent.componentWidth/2, &interPoint1, &interPoint2);
                        [self lineInterCircle:singleWall centerPoint:interPoint2 radius:wallComponent.componentWidth/2 wallComponent: wallComponent];
                        [singleWall.wallComponentArr addObject: wallComponent];
                        wallComponent.wallIndex = [self.wallLines indexOfObject:singleWall];
                        wallComponent.componentView.transform = CGAffineTransformMakeRotation(singleWall.CurrentLineAngle);
                        isAvaliblePosition = true;
                        break;
                    }
                }else if(i == sWCount){
                    component1Point = singleWall.wallComponentArr[i-1].componentPosition;
                    component2Point = singleWall.wPoint2.wallPoint;
                    CGFloat deltaX = component2Point.x - component1Point.x;
                    CGFloat deltaY = component2Point.y - component1Point.y;
                    CGFloat pointDis = sqrtf(deltaY * deltaY + deltaX * deltaX);
                    if (pointDis - singleWall.wallComponentArr[i-1].componentWidth/2 - wallComponent.componentWidth/2 > wallComponent.componentWidth ) {
                        CGPoint interPoint1 = CGPointZero;
                        CGPoint interPoint2 = CGPointZero;
                        getComCenterlineInterCircle(singleWall, singleWall.wallComponentArr[i-1].componentPosition, wallComponent.componentWidth, &interPoint1, &interPoint2);
                        [self lineInterCircle:singleWall centerPoint:interPoint2 radius:wallComponent.componentWidth/2 wallComponent: wallComponent];
                        [singleWall.wallComponentArr addObject: wallComponent];
                        wallComponent.wallIndex = [self.wallLines indexOfObject:singleWall];
                        wallComponent.componentView.transform = CGAffineTransformMakeRotation(singleWall.CurrentLineAngle);
                        isAvaliblePosition = true;
                        break;
                    }
                }else{
                    component1Point = singleWall.wallComponentArr[i-1].componentPosition;
                    component2Point = singleWall.wallComponentArr[i].componentPosition;
                    CGFloat deltaX = component2Point.x - component1Point.x;
                    CGFloat deltaY = component2Point.y - component1Point.y;
                    CGFloat pointDis = sqrtf(deltaY * deltaY + deltaX * deltaX);
                    if (pointDis - singleWall.wallComponentArr[i-1].componentWidth/2 - singleWall.wallComponentArr[i].componentWidth/2 > wallComponent.componentWidth ) {
                        CGPoint interPoint1 = CGPointZero;
                        CGPoint interPoint2 = CGPointZero;
                        getComCenterlineInterCircle(singleWall, singleWall.wallComponentArr[i-1].componentPosition, wallComponent.componentWidth, &interPoint1, &interPoint2);
                        [self lineInterCircle:singleWall centerPoint:interPoint2 radius:wallComponent.componentWidth/2 wallComponent: wallComponent];
                        [singleWall.wallComponentArr addObject: wallComponent];
                        wallComponent.wallIndex = [self.wallLines indexOfObject:singleWall];
                        wallComponent.componentView.transform = CGAffineTransformMakeRotation(singleWall.CurrentLineAngle);
                        isAvaliblePosition = true;
                        break;
                    }
                }
            }
            
        }
        if (isAvaliblePosition) {
            break;
        }
       
    }
   
    return YES;
}

//判断线段上一点为圆心画圆和直线相交的距离
bool getComCenterlineInterCircle(HomeWallLine * wallLine, const CGPoint ptCenter, const float radius,  CGPoint *interPoint1, CGPoint *interPoint2){
    interPoint1->x = interPoint1->y = 65536.0f;
    interPoint2->x = interPoint2->y = 65536.0f;
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
            interPoint1->x = ptStart.x + t * d.x;
            interPoint1->y = ptStart.y + t * d.y;
        }
        t = a + f;
        if( ((t - 0.0) > - EPS) && (t - fDis) < EPS)
        {
            interPoint2->x = ptStart.x + t * d.x;
            interPoint2->y = ptStart.y + t * d.y;
        }
        //做一个墙体组件两个交点方向的判断
        if (interPoint1->x < 65533 && interPoint2->x < 65533) {
            float interSectionX = interPoint2->x - interPoint1->x;
            float interSectionY = interPoint2->y - interPoint1->y;
            
            CGPoint normalizerWallLine = CGPointMake(d.x, d.y);
            CGPoint normalizerCompLine = CGPointMake(interSectionX, interSectionY);
            CGFloat dotProduct = normalizerWallLine.x * normalizerCompLine.x + normalizerCompLine.y * normalizerWallLine.y;
            if (dotProduct < 0) {
                CGPoint tempP = CGPointZero;
                tempP.x = interPoint2->x;
                tempP.y = interPoint2->y;
                interPoint2->x = interPoint1->x;
                interPoint2->y = interPoint1->y;
                interPoint1->x = tempP.x;
                interPoint1->y = tempP.y;
            }
        }else{
            CGPoint result = CGPointZero;
            result.x = interPoint1->x < 60000 ? interPoint1->x : interPoint2->x;
            result.y = interPoint1->y < 60000 ? interPoint1->y : interPoint2->y;
            interPoint1->x = interPoint1->y = 65536.0f;
            interPoint2->x = result.x;
            interPoint2->y = result.y;
        }
        return true;
    }
}

@end
