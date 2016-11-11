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
@property (nonatomic, readonly) NSMutableArray<WallLine *>* outWallLines;
@property (nonatomic, readwrite) NSMutableArray<WallPoint *>* roomPoints;
@property (nonatomic, readonly) NSMutableArray<WallPoint *>* outRoomPoints;
@property (nonatomic, readonly)  NSMutableArray<ArchWallComponent *>*currentRoomComponents;
/**
 不适用于重合的两条线
 */
- (CGPoint) intersetctionOfWallLine1:(WallLine *)wallLine wallLine2:(WallLine *)wallLine2;
bool getComCenterlineInterCircle(WallLine * wallLine, const CGPoint ptCenter, const float radius,  CGPoint *interPoint1, CGPoint *interPoint2);

- (bool) addAvailableComponentView:(ArchWallComponent *)wallComponent;

@end
