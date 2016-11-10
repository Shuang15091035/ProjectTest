//
//  ArchComponent.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/11/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "PublicData.h"


@interface ArchWallComponent : NSObject

@property (nonatomic, readwrite) ComponentType componentType;
@property (nonatomic, readonly) CGPoint componentPosition;
@property (nonatomic, readwrite) CGFloat componentWidth;
@property (nonatomic, readwrite) CGFloat componentHeight;
@property (nonatomic, readonly) UIImageView *componentView;
@property (nonatomic, readwrite) CGPoint componentStartP;
@property (nonatomic, readwrite) CGPoint componentEndP;
@property (nonatomic, readwrite) NSInteger wallIndex;
@property (nonatomic, readwrite) CGFloat percent;

- (instancetype)initWithDoorType:(ComponentType)componentType doorWidth:(CGFloat)componentWidth componentHeight:(CGFloat)componentHeight;

@end
