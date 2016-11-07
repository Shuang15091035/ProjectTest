//
//  RoomPlan.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "WallLine.h"
#import "WallPoint.h"
#import "ComponentDoor.h"
#import "ComponentWindow.h"

@interface RoomPlane : NSObject

@property (nonatomic, readwrite) NSMutableArray<WallLine *>* wallLines;
@property (nonatomic, readwrite) NSMutableArray<WallLine *>* outWallLines;
@property (nonatomic, readwrite) NSMutableArray<WallPoint *>* roomPoints;
@property (nonatomic, readwrite) NSMutableArray<WallPoint *>* outRoomPoints;
@property (nonatomic, readonly) NSMutableArray<ArchWallComponent *>*currentRoomComponents;
/**
 不适用于重合的两条线
 */
- (CGPoint) intersetctionOfWallLine1:(WallLine *)wallLine wallLine2:(WallLine *)wallLine2;

/**
 
 @param ptStart  线段起点
 @param ptEnd    线段终点
 @param ptCenter 圆心坐标
 @param Radius   半径
 @param ptInter1 交点坐标
 @param ptInter2 交点坐标
 @return 是否有交点
 */
//- (bool) LineInterCircle:(WallLine *)wallLine centerPoint:(const CGPoint)ptCenter radius:(const float)radius interPoint1:(CGPoint&)interPoint1 interPoint2:(CGPoint&)interPoint2;

- (bool) addAvailableComponentView:(ArchWallComponent *)wallComponent;

@end
