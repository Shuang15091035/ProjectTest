//
//  ViewController.h
//  DrawGraphicArch
//
//  Created by mac zdszkj on 2016/11/4.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

typedef void (^ViewControlBlock)(UIColor *color);

@interface ViewController : UIViewController

@property (nonatomic, readwrite) ViewControlBlock block;

@end
