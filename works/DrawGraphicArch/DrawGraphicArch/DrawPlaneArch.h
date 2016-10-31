//
//  DrawPlaneArch.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/28.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>
#import "ArchPlane.h"
#import "RoomPlane.h"
#import "WallLine.h"
#import "WallPoint.h"
#import "ToolLine.h"

@interface DrawPlaneArch : UIView

@property (nonatomic, readwrite) ArchPlane *archPlane;

@end
