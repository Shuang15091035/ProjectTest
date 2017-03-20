//
//  Singleton.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/7.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "Singleton.h"


@interface Singleton(){
    
}

@end

@implementation Singleton

+ (Singleton *)shareInstance{
    
    static Singleton *sharedAccountManagerInstance = nil;
    static dispatch_once_t predicate;
    dispatch_once(&predicate, ^{
        sharedAccountManagerInstance = [[self alloc] init];
    });
    return sharedAccountManagerInstance;
}

@end
