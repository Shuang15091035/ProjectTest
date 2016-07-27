//
//  UIScreen+CoreCategory.m
//  Camera
//
//  Created by mac zdszkj on 16/3/16.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "UIScreen+CoreCategory.h"

@implementation UIScreen (CoreCategory)
- (CGRect)boundsFixBeforeIOS8 {
    CGRect bounds = self.bounds;
    if ([UIDevice currentDevice].systemVersion.floatValue >= 8.0) {
        return bounds;
    }
    
    UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
    if (orientation == UIInterfaceOrientationLandscapeLeft || orientation == UIInterfaceOrientationLandscapeRight) {
        CGSize size = bounds.size;
        size = CGSizeMake(size.height, size.width);
        bounds = CGRectMake(bounds.origin.x, bounds.origin.y, size.width, size.height);
    }
    return bounds;
}
- (CGRect)boundsByOrientation
{
    return [self boundsFixBeforeIOS8];
}
- (CGRect)boundsInPixels {
    CGRect bounds = [self boundsByOrientation];
    //    CGRect bounds = self.bounds;
    GLfloat scale = [UIScreen mainScreen].scale;
    bounds.origin.x *= scale;
    bounds.origin.y *= scale;
    bounds.size.width *= scale;
    bounds.size.height *= scale;
    return bounds;
}
CGFloat screenGetHeight(){
    return [[UIScreen mainScreen] bounds].size.height;
}
CGFloat screenGetWidth(){
    return [[UIScreen mainScreen] bounds].size.width;
}
@end
