//
//  RoomPlan.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "RoomPlane.h"

@interface RoomPlane(){
    NSMutableArray<WallLine *>*mWallLines;
}

@end

@implementation RoomPlane

- (instancetype)init{
    self = [super init];
    if (self) {
        mWallLines = [NSMutableArray array];
    }
    return self;
}

@synthesize wallLines = mWallLines;

@end
