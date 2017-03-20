//
//  Storage.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
@class Memento;

@interface Storage : NSObject

@property (nonatomic, readwrite) Memento *memento;

- (instancetype)initWithStroage:(Memento *)memento;

@end
