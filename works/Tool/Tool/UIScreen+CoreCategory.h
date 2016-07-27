//
//  UIScreen+CoreCategory.h
//  Camera
//
//  Created by mac zdszkj on 16/3/16.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface UIScreen (CoreCategory)
@property (nonatomic, readonly) CGRect boundsByOrientation;
@property (nonatomic, readonly) CGRect boundsInPixels;
CGFloat screenGetHeight();
CGFloat screenGetWidth();
@end
