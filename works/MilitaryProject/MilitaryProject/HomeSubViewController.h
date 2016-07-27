//
//  HomeSubControllerS.h
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface HomeSubViewController : UIViewController

@property (nonatomic, copy) NSString* midField;
@property (nonatomic, strong) UIViewController* superViewController;
@property (nonatomic, ) NSMutableArray* dataSource;

- (void)fetchDataPerSubViewController;
@end
