//
//  Adapter.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/7.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "ClassAdapter.h"

@implementation Source
- (void)method1{
    NSLog(@"this is original method!");
}
@end

@implementation ClassAdapter

- (void)method2{
    NSLog(@"this is the targetable method!");
}

@end
