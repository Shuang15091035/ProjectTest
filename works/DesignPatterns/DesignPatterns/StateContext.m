//
//  Context.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "StateContext.h"
#import "StatePattern.h"

@interface StateContext(){
    NSMutableArray<StatePattern *> *mStates;
}

@end

@implementation StateContext

- (NSMutableArray<StatePattern *> *)state{
    if (mStates == nil) {
        mStates = [NSMutableArray array];
    }
    return mStates;
}

- (void)setState:(NSMutableArray<StatePattern *> *)state{
    mStates = state;
}

- (void)addStateName:(StatePattern *)name{
    [mStates addObject:name];
}

- (void)sendMessage{
    for (id<IStateInit> state in mStates) {
        if ([state.value isEqualToString: @"state1"]) {
            [state onStateIn];
        }else if ([state.value isEqualToString:@"state2"]){
            [state onStateIn];
        }

    }
}

@end
