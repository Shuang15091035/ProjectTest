//
//  Render.h
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/20.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "CustomView.h"
#import "ViewController.h"

@interface Render : NSObject<ViewControllerDelegate,CustomViewDelegate>

- (void)configure:(CustomView *)view;

@end
