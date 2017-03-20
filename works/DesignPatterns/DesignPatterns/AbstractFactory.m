//
//  AbstractFactory.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/7.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "AbstractFactory.h"

@implementation Woman

- (void)action{
    NSLog(@"women");
}

@end

@implementation Man

- (void)action{
    NSLog(@"man");
}

@end
