//
//  ClassMeta.m
//  Camera
//
//  Created by mac zdszkj on 16/4/15.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//


#import "ClassMeta.h"
#import <objc/runtime.h>

void TestMetaClass(id self, SEL _cmd) {
    
    
    
    NSLog(@"This objcet is %p", self);
    
    NSLog(@"Class is %@, super class is %@", [self class], [self superclass]);
    
    
    
    Class currentClass = [self class];
    
    for (int i = 0; i < 4; i++) {
        
        NSLog(@"Following the isa pointer %d times gives %p", i, currentClass);
        
        currentClass = objc_getClass((__bridge void *)currentClass);
        
    }
    
    NSLog(@"NSObject's class is %p", [NSObject class]);
    
    NSLog(@"NSObject's meta class is %p", objc_getClass((__bridge void *)[NSObject class]));
    
}

#pragma mark -

@implementation ClassMeta



- (void)ex_registerClassPair {
    Class newClass = objc_allocateClassPair([UIView class], "TestClass", 0);
    
    class_addMethod(newClass, @selector(testMetaClass), (IMP)TestMetaClass, "v@:");
    
//    class_addIvar(<#__unsafe_unretained Class cls#>, <#const char *name#>, <#size_t size#>, <#uint8_t alignment#>, <#const char *types#>)//此方法只能在 objc_allocateClassPair 和objc_registerClassPair之间调用，运行时创建类，为类添加一个成员变量。
    objc_registerClassPair(newClass);
    
    
    
//    id instance = [[newClass alloc] initWithDomain:@"some domain" code:0 userInfo:nil];
    
    id instance = [[newClass alloc]init];
    
    [instance performSelector:@selector(testMetaClass)];
   
}



@end