//
//  ArchPlane.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "RoomPlane.h"

@interface ArchPlane : NSObject

@property(nonatomic, readwrite) NSMutableArray<RoomPlane *>*roomPlanes;

@end
