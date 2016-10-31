//
//  DALine.h
//  ProxyUsage
//
//  Created by mac zdszkj on 16/9/20.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "PublicData.h"

@interface ToolLine : NSObject

@property (nonatomic, readwrite) CGPoint point1;
@property (nonatomic, readwrite) CGPoint point2;
@property (nonatomic, readwrite) CGFloat likeK;

- initWithPoint1:(CGPoint)point1 point2:(CGPoint)point2;

/*
 求取点是否在直线上
 */
BOOL isOnLineOfPoint(CGPoint touchPoint);

/**
 点到直线的距离
 */
-(NSInteger) GetMinDistance:(CGPoint) pt3;

/**
 判断点是否在直线上
 */
- (BOOL) isOnLineOfPoint:(CGPoint) touchPoint;

/**
 获取点到线的垂足
 */
- (CGPoint) getPointOnVerticalLine:(CGPoint)touchPoint;

/**
 点在线上移动
 */
- (CGFloat) getYValueAccordingToXValue:(CGPoint)touchPoint;

bool LineInterCircle(
                     const CGPoint ptStart, // 线段起点
                     const CGPoint ptEnd, // 线段终点
                     const CGPoint ptCenter, // 圆心坐标
                     const float Radius,
                     CGPoint& ptInter1,
                     CGPoint& ptInter2);
@end
