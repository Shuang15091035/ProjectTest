//
//  ArchPlane.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "HomeArchPlane.h"

@interface HomeArchPlane(){
    NSMutableArray<HomeRoomPlane *> *mRoomPlanes;
    NSInteger lastSelectedIndex;
    bool isOnLine;
}

@end

@implementation HomeArchPlane

@synthesize roomPlanes = mRoomPlanes;

- (instancetype)init{
    self = [super init];
    if (self) {
        mRoomPlanes = [NSMutableArray array];
    }
    return self;
}

/**
 * @description 射线法判断点是否在多边形内部
 *  {object} p 待判断的点，格式：{ x: X坐标, y: Y坐标 }
 *  {Array} poly 多边形顶点，数组成员的格式同 p
 * @return {String} 点 p 和多边形 poly 的几何关系
 */
- (NSInteger)roomIndexOfRoomPlaneInsideRayCastingPoint:(CGPoint)RayCastPoint{
    CGFloat px = RayCastPoint.x;
    CGFloat py = RayCastPoint.y;
    NSInteger selectedRoomIndex = -1;
    NSMutableArray *intersectionArch = [NSMutableArray array];
    for (int n = 0; n < mRoomPlanes.count; n++ ) {
        HomeRoomPlane *roomPlane = mRoomPlanes[n];
        bool flag = NO;
        NSArray *roomPoints = roomPlane.roomPoints;
        for (int i = 0, l = (int)roomPoints.count, j = l - 1; i < l;j = i, i++) {
            HomeWallPoint *point1 = roomPoints[i];
            CGFloat sx = point1.wallPoint.x,sy = point1.wallPoint.y;
            HomeWallPoint *point2 = roomPoints[j];
            CGFloat tx = point2.wallPoint.x,ty = point2.wallPoint.y;
            //点与多边形顶点重合
            if((sx == px && sy == py) || (tx == px && ty == py)) {
                flag = true;
            }
            //判断线段两端点是否在射线两端
            if((sy < py && ty >= py) || (sy >= py && ty < py)) {
                // 线段上与射线 Y 坐标相同的点的 X 坐标
                CGFloat x = sx + (py - sy) * (tx - sx) / (ty - sy);
                
                // 点在多边形的边上
                if(x == px) {
                    flag = true;
                }
                // 射线穿过多边形的边界
                if(x > px) {
                    flag = !flag;
                }
            }
        }
        if (flag) {
            [intersectionArch addObject:roomPlane];
        }
    }
    if (intersectionArch.count == 1) {
        NSInteger  result =  [mRoomPlanes indexOfObject:[intersectionArch lastObject]];
        selectedRoomIndex = result; //待考虑户型重叠的情况
        lastSelectedIndex = result;
    }else if (intersectionArch.count > 2){
      selectedRoomIndex =  lastSelectedIndex;
    }
    return selectedRoomIndex;
}
- (instancetype)createRoomWithGround:(float)ground wallHeight:(float)wallHeight roomPoints:(NSMutableArray<HomeWallPoint *>*)roomPoints archItem:(NSMutableArray<HomeArchItem *> *)archItems{
    HomeRoomPlane *roomPlane = [[HomeRoomPlane alloc]initWithGround:ground wallHeight:wallHeight oomPoints:roomPoints archItem:archItems];
    [mRoomPlanes addObject:roomPlane];
    return self;
}
@end
