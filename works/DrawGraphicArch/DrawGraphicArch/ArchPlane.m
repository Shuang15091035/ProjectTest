//
//  ArchPlane.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ArchPlane.h"

@interface ArchPlane(){
    NSMutableArray<RoomPlane *> *mRoomPlanes;
}

@end

@implementation ArchPlane

- (instancetype)init{
    self = [super init];
    if (self) {
        mRoomPlanes = [NSMutableArray array];
    }
    return self;
}

@synthesize roomPlanes = mRoomPlanes;

@end
