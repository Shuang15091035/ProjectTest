//
//  Original.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
@class Memento;

@interface Original : NSObject

@property (nonatomic, readwrite) NSString *value;

- (Memento *)createMemento;

- (void)restoreMemento:(Memento *)memento;

@end
