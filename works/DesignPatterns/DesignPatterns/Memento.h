//
//  Memento.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>



@interface Memento : NSObject

@property (nonatomic, readwrite) NSString *value;

- (instancetype)initWithValue:(NSString *)value;

@end
