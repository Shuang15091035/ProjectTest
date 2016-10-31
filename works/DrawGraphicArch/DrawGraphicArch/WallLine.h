//
//  WallLine.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/28.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "WallPoint.h"

@interface WallLine : NSObject

@property (nonatomic, readwrite) WallPoint *wPoint1;
@property (nonatomic, readwrite) WallPoint *wPoint2;

- initWithWallPoint1:(WallPoint *)wallPoint1 wallPoint2:(WallPoint *)wallPoint2;

@end
