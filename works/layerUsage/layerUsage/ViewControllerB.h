//
//  ViewControllerB.h
//  Bluetooth
//
//  Created by mac zdszkj on 16/3/15.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>
typedef void(^myBlocok) (NSString *name);

@interface ViewControllerB : UIViewController
@property (nonatomic, copy) myBlocok block;
@end
