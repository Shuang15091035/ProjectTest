//
//  StatePattern.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/9.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 状态是对象的行为模式：
 环境角色(context)：上下文，定义客户端所感兴趣的接口，并且保留一个具体状态类的实例.这个具体状态类的实例给出此环境对象的现有状态。
 抽象状态角色(state)：定义一个接口，用以封装环境（Context）对象的一个特定的状态所对应的行为
 具体状态角色(concreteState):实现了抽象状态角色接口的对象，实现一个状态对应的行为

*/

@protocol IStateInit <NSObject>

- (void)onStateIn;

- (void)onStateOut;

@property (nonatomic, readwrite) NSString *value;

@end


@interface StatePattern : NSObject <IStateInit>




@end
