//
//  CommandManger.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/8.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "Command.h"

@protocol ICommandMachine <NSObject>

@property (nonatomic, readwrite) NSUInteger maxCommands;

- (BOOL)todoCommand:(id<ICommand>)command canUndo:(BOOL)canundo;
- (BOOL)todoCommand:(id<ICommand>)command;

- (BOOL) doneCommand:(id<ICommand>)command;

- (void)undo;

- (void)redo;

@property (nonatomic, readonly)BOOL canUndo;

@property (nonatomic, readonly)BOOL canRedo;

- (void)clear;

@end


/**
 命令行模式：四部分组成（Receiver, Invoke, ComandInterface, ConcreteCommand）
 invoke:管理遵守CommandInterface的ConcreteCommand
 Receiver:ConcreteCommand中的执行者；
 模式作用：解决命令的发送者和命令执行者之间的解耦，实现对命令的管理，需要支持撤销重做功能。
 */
@interface CommandMachine : NSObject<ICommandMachine>

@end
