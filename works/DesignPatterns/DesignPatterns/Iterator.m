//
//  Iterator.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "Iterator.h"
#import "ConcreteAggregate.h"

@interface Iterator(){
    //持有被迭代的具体的聚合对象
    ConcreteAggregate *mAgg;
    //内部索引，记录当前迭代到的索引位置
    int mIndex;
    //记录当前聚集对象的大小
    NSUInteger mSize;
}

@end

@implementation Iterator

- (instancetype)initWith:(ConcreteAggregate *)age{
    self = [super init];
    if (self) {
        mAgg = age;
        mSize = [mAgg size];
        mIndex = 0;
    }
    return self;
}

/**
 * 迭代方法：返还当前元素
 */
- (NSString *) currentItem {
    return [mAgg getElement:mIndex];
}
/**
 * 迭代方法：移动到第一个元素
 */
- (void) first {
    
    mIndex = 0;
}
/**
 * 迭代方法：是否为最后一个元素
 */
- (bool) isDone {
    return (mIndex >= mSize);
}
/**
 * 迭代方法：移动到下一个元素
 */
- (void) next {
    
    if(mIndex < mSize)
    {
        mIndex ++;
    }
}

@end
