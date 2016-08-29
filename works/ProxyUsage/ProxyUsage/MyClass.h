//
//  MyClass.h
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/29.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MyClass : NSObject<NSCoding,NSCopying>

@property (nonatomic, strong) NSArray* array;
@property (nonatomic, copy) NSString* string;

- (void)method1;
- (void)method2;
+ (void)classMethod1;

@end
