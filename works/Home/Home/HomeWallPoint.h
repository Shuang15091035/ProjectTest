//
//  WallPoint.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 16/10/10.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>

@protocol HIWallPoint <NSObject>

@property (nonatomic, readwrite) CGPoint wallPoint;
@property (nonatomic, readwrite) UIImageView *pointImageView;

@end

@interface HomeWallPoint : NSObject<HIWallPoint>

- initWithPoint:(CGPoint)touchP currentPointView:(UIView *)currentView;

- (float) distanceOfAnotherPoint:(CGPoint)anPoint;

@end
