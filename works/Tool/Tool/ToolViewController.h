//
//  ViewController.h
//  Tool
//
//  Created by mac zdszkj on 16/3/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <AVFoundation/AVFoundation.h>
#import <objc/runtime.h>

typedef struct objc_ivar{
    char *ivar_name;
    char *ivar_type;
    int ivar_offset;
#ifdef __LP64
    int space;
#endif
} *ivar;

@interface ToolViewController : UIViewController

@end

