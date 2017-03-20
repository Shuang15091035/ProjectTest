//
//  ConcreteAggregate.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
@class Iterator;

@protocol IConcreteAggregate <NSObject>

- (Iterator *) createIterator;

@end

@interface ConcreteAggregate : NSObject <IConcreteAggregate>

- (id)initWith:(NSArray *)objArray;
- (id) getElement:(int) index;
- (NSUInteger) size;

@end
