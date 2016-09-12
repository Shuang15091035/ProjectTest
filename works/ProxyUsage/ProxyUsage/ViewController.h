//
//  ViewController.h
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/24.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

extern int a = 10;
static int b = 10;

@interface ViewController : UIViewController{

    
    
 void (^finishHandlerProgress)(void);

}
@end

