//
//  Person.h
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/25.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface Person : NSObject{
    NSString *_name;
}

@property(nonatomic,assign)NSInteger age;


- (void)test1;

- (void)test2;

@end
