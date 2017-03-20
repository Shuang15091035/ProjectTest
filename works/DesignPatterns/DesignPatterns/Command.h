//
//  Command.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/8.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol ICommand <NSObject>

- (void)commandExcute;
- (void)commandUndo;

@end

@interface Command : NSObject<ICommand>

@end
