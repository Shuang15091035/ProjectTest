//
//  RoomPlan.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Home/HomeWallLine.h>
#import <Home/HomeWallPoint.h>
#import <Home/HomeArchDoor.h>
#import <Home/HomeArchWindow.h>

@interface HomeRoomPlane : NSObject

@property (nonatomic, readwrite) NSMutableArray<HomeWallLine *>* wallLines;
@property (nonatomic, readonly) NSMutableArray<HomeWallLine *>* outWallLines;
@property (nonatomic, readwrite) NSMutableArray<HomeWallPoint *>* roomPoints;
@property (nonatomic, readonly) NSMutableArray<HomeWallPoint *>* outRoomPoints;
@property (nonatomic, readonly)  NSMutableArray<HomeArchItem *>*currentRoomComponents;
/**
 不适用于重合的两条线
 */
- (CGPoint) intersetctionOfWallLine1:(HomeWallLine *)wallLine wallLine2:(HomeWallLine *)wallLine2;
 bool getComCenterlineInterCircle(HomeWallLine * wallLine, const CGPoint ptCenter, const float radius,  CGPoint *interPoint1, CGPoint *interPoint2);

- (bool) addAvailableComponentView:(HomeArchItem *)wallComponent;

- (instancetype) initWithGround:(float)ground wallHeight:(float)wallHeight oomPoints:(NSMutableArray<HomeWallPoint *>*)roomPoints archItem:(NSMutableArray<HomeArchItem *> *)archItems;

@end
