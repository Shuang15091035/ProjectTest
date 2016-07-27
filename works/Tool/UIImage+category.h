//
//  UIImage+category.h
//  Tool
//
//  Created by mac zdszkj on 16/4/13.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface UIImage (category)
- (UIImage *)scaleToSize:(UIImage *)img size:(CGSize)size;
- (UIImage*)imageByScalingAndCroppingForSize:(CGSize)targetSize;
@end
