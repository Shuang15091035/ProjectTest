//
//  Student.m
//  SizeClassAndAutoLayout
//
//  Created by mac zdszkj on 16/5/4.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "Student.h"

@implementation Student

- (id)copyWithZone:(NSZone *)zone{
    Student * copy = [[[self class] allocWithZone:zone] init];
    
    // 拷贝名字给副本对象
    copy.name = self.name;
    copy.age = self.age;
    return copy;
}
@end
