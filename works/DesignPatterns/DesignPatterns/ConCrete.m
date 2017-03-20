//
//  ConCrete.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/9.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "ConCrete.h"

@implementation ConCrete{
    id mReceiver;
}

@synthesize receiver = mReceiver;

- (void)concreteCommandA:(id)receive{
    mReceiver = receive;
}

- (void)commandUndo{
    
}

- (void)commandExcute{
    
}

@end
