//
//  HomeViewController.h
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

typedef NS_ENUM(NSInteger, ScrollViewDirection) {
    ScrollViewDirectionLeft,
    ScrollViewDirectionRight,
};
@interface MPHomeViewController : UIViewController

@property (nonatomic)ScrollViewDirection direction;

@end
