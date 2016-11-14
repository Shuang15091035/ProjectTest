//
//  ArchPlane.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <Home/HomeRoomPlane.h>

@protocol HIArchPlane <NSObject>

@property(nonatomic, readwrite) NSMutableArray<HomeRoomPlane *>*roomPlanes;

@end

@interface HomeArchPlane : NSObject <HIArchPlane>

/**
 射线法判断点击点在哪一个多边形内部
 
 @param RayCastPoint 点击的点
 
 @return room索引
 */
- (NSInteger)roomIndexOfRoomPlaneInsideRayCastingPoint:(CGPoint)RayCastPoint;

- (HomeRoomPlane *)createRoomWithGround:(float)ground wallHeight:(float)wallHeight roomPoints:(NSMutableArray<HomeWallPoint *>*)roomPoints archItem:(NSMutableArray<HomeArchItem *> *)archItems;

@end
