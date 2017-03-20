//
//  ConcreteAggregate.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "ConcreteAggregate.h"
#import "Iterator.h"

@interface ConcreteAggregate() {
    NSMutableArray *MbjArray;
}
@end

@implementation ConcreteAggregate

/**
 * 构造方法，传入聚合对象的具体内容
 */
- (instancetype)initWith:(id)objArray
{
    self = [super init];
    if (self) {
        MbjArray = objArray;
    }
    return self;
}

- (Iterator*) createIterator{
    
    return [[Iterator alloc]initWith:self];
}
/**
 * 取值方法：向外界提供聚集元素
 */
- (id) getElement:(int) index{
    
    if(index < MbjArray.count){
        return MbjArray[index];
    }else{
        return nil;
    }
}
/**
 * 取值方法：向外界提供聚集的大小
 */
- (NSUInteger) size{
    return MbjArray.count;
}

@end
