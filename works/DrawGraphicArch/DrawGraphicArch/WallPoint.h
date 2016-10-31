//
//  WallPoint.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 16/10/10.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "Common.h"

@interface WallPoint : NSObject

@property (nonatomic, readwrite) CGPoint wallPoint;
@property (nonatomic, readwrite) UIImageView *pointImageView;

- initWithPoint:(CGPoint)touchP currentPointView:(UIView *)currentView;
@end
