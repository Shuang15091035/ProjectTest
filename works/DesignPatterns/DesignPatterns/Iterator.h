//
//  Iterator.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "Template.h"

//#import "ConcreteAggregate.h" //本地使用#import 导致循环引用（circular dependency）的问题，会产生编译报错的问题，使用@class告诉编译器，本地使用到的对象只作为一个类型出现。

@class ConcreteAggregate;

@protocol IIterator <NSObject>

- (void) first;

- (void) next;

- (bool) isDone;

- (NSString *) currentItem;

@end

@interface Iterator : NSObject <IIterator>

- (instancetype)initWith:(ConcreteAggregate *)age;

@end
