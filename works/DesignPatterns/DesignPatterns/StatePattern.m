//
//  StatePattern.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/9.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "StatePattern.h"

@interface StatePattern(){
    NSString *mValue;
}

@end

@implementation StatePattern

- (void)onStateIn{
    printf("mValue:%s,%s",__func__,[mValue UTF8String]);
}

- (void)onStateOut{
    printf("mValue:%s,%s",__func__,[mValue UTF8String]);
}

@synthesize value = mValue;

@end
