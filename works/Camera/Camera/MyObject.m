//
//  MyObject.m
//  Camera
//
//  Created by mac zdszkj on 16/4/19.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MyObject.h"

static NSMutableDictionary *map = nil;

@implementation MyObject
+ (void)load{
    
    map = [NSMutableDictionary dictionary];
    
    map[@"name1"] = @"name";
    
    map[@"status1"] = @"status";
    
    map[@"name2"] = @"name";
    
    map[@"status2"] = @"status";
    
}


@end
