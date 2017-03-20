//
//  AbstractFactory.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/7.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol Abstracted <NSObject>

- (void)realCreate;

@end

@interface Woman : NSObject

@end

@interface Man : NSObject

@end
