//
//  WallLine.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/28.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Home/HomeWallPoint.h>
#import <Home/HomeArchItem.h>

@protocol HIWallLine <NSObject>

@property (nonatomic, readwrite) HomeWallPoint *wPoint1;
@property (nonatomic, readwrite) HomeWallPoint *wPoint2;

@property (nonatomic, readwrite) float wallHeight;
@property (nonatomic, readwrite) NSMutableArray<HomeArchItem*> *wallComponentArr;

@end

@interface HomeWallLine : NSObject<HIWallLine>

@property (nonatomic, readonly) float lineA;
@property (nonatomic, readonly) float lineB;
@property (nonatomic, readonly) float lineC;
@property (nonatomic, readonly) float minX;
@property (nonatomic, readonly) float maxX;
@property (nonatomic, readonly) float minY;
@property (nonatomic, readonly) float maxY;


- initWithWallPoint1:(HomeWallPoint *)wallPoint1 wallPoint2:(HomeWallPoint *)wallPoint2;

/**
 点到直线的距离
 */
- (float) distanceOfLineFromPoint:(CGPoint)outPoint;

/**
 判断点是否在直线上
 */
- (bool) isOnLineOfPoint:(CGPoint) outPoint;

/**
 获取点到线的垂足
 */
- (CGPoint) pedalOfLineAndVerticalAccordingToLineOutPoint:(CGPoint)outPoint;

- (CGFloat) CurrentLineAngle;

- (CGFloat) getYValueArccordingToTouchPoint:(CGPoint)TouchPoint;

- (CGFloat)getWallLineWidth;

- (CGFloat)distantof:(CGPoint)point1 point2:(CGPoint)point2;

- (void)updateComponentPercentOfWallLine;

@end
