//
//  Memento.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "Memento.h"

@implementation Memento

- (instancetype)initWithValue:(NSString *)value{
    self = [super init];
    if (self) {
        _value = value;
    }
    return self;
}

@end
