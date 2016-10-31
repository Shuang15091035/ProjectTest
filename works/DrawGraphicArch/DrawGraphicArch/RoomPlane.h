//
//  RoomPlan.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "WallLine.h"

@interface RoomPlane : NSObject

@property (nonatomic, readwrite) NSMutableArray<WallLine *>* wallLines;

@end
