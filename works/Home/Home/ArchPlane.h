//
//  ArchPlane.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "RoomPlane.h"

@protocol HIArchPlane <NSObject>

@property(nonatomic, readwrite) NSMutableArray<RoomPlane *>*roomPlanes;

@end

@interface ArchPlane : NSObject <HIArchPlane>

/**
 射线法判断点击点在哪一个多边形内部
 
 @param RayCastPoint 点击的点
 
 @return room索引
 */
- (NSInteger)roomIndexOfRoomPlaneInsideRayCastingPoint:(CGPoint)RayCastPoint;

@end
