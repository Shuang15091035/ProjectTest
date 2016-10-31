//
//  WallLine.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/28.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "WallLine.h"

@interface WallLine(){
    WallPoint *mWallPoint1;
    WallPoint *mWallPoint2;
}

@end

@implementation WallLine

- initWithWallPoint1:(WallPoint *)wallPoint1 wallPoint2:(WallPoint *)wallPoint2{
    self = [super init];
    if (self) {
        mWallPoint1 = wallPoint1;
        mWallPoint2 = wallPoint2;
    }
    return self;
}

@synthesize wPoint1 = mWallPoint1;
@synthesize wPoint2 = mWallPoint2;


@end
