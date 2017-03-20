//
//  CommandManger.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/8.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "CommandMachine.h"

@interface CommandMachine(){
    
}
@property (nonatomic, readwrite) NSMutableArray* mCommandStack;

@end

@implementation CommandMachine{
    NSInteger mCurrentCommand;
    
}

+ (NSUInteger)MaxCommandsDefault {
    return 100000;
}
- (void)redo{
    
}

- (void)undo{
    
}

- (BOOL)canRedo{
    
    return YES;
}

@end
