//
//  Original.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "Original.h"
#import "Memento.h"

@implementation Original

- (Memento *)createMemento{
    return [[Memento alloc]init];
}

- (void)restoreMemento:(Memento *)memento{
    self.value = memento.value;
}

@end
