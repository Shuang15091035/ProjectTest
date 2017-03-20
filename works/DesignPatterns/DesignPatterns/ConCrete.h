//
//  ConCrete.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/9.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "Command.h"

@interface ConCrete : Command

@property (nonatomic, readwrite) id receiver;

@end
