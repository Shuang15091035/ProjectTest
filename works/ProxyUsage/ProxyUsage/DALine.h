//
//  DALine.h
//  ProxyUsage
//
//  Created by mac zdszkj on 16/9/20.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@interface DALine : NSObject

@property (nonatomic, readwrite) CGPoint point1;
@property (nonatomic, readwrite) CGPoint point2;


/**
 获取两点之间的距离
 */
- (CGFloat)getdistancePointAndPoint;

/*
 点到直线的距离
 */

NSInteger GetMinDistance(CGPoint pt1, CGPoint pt2, CGPoint pt3);
/*
 求取点是否在直线上
 */
BOOL isOnLineOfPoint(CGPoint touchPoint, CGPoint pt1, CGPoint pt2);


- (CGPoint) getIntersectionOfLinerAndCircularRadiu:(CGFloat)radiu ;

@end
