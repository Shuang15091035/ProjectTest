//
//  Storage.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "Storage.h"

@implementation Storage

- (instancetype)initWithStroage:(Memento *)memento{
    self = [super init];
    if (self) {
        _memento = memento;
    }
    return self;
}

@end
