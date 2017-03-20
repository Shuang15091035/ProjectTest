//
//  Context.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
@class StatePattern;


@interface StateContext : NSObject

//@property (nonatomic, readwrite) NSMutableArray <StatePattern *>*state;

- (void)addStateName:(StatePattern *)name;

- (void)sendMessage;

@end
