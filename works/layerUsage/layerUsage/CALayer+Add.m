//
//  CALayer+Add.m
//  Bluetooth
//
//  Created by mac zdszkj on 16/3/14.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "CALayer+Add.h"

@implementation CALayer (Add)
- (UIImage *)snapshotImage {
    UIGraphicsBeginImageContextWithOptions(self.bounds.size, self.opaque, 0);
    CGContextRef context = UIGraphicsGetCurrentContext();
    [self renderInContext:context];
    UIImage *image = UIGraphicsGetImageFromCurrentImageContext();
    UIGraphicsEndImageContext();
    return image;
}
@end
