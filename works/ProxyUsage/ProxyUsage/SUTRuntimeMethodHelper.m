//
//  SUTRuntimeMethodHelper.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/30.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "SUTRuntimeMethodHelper.h"
#import <objc/runtime.h>

@implementation SUTRuntimeMethodHelper

- (void)viewDidLoad {
    [super viewDidLoad];
    self.view.backgroundColor = [UIColor blackColor];
}
- (void)method{
    NSLog(@"MethodIMPFinish");
}

+ (void)load{//程序运行后立即执行
    
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        Class class = [self class];
        // When swizzling a class method, use the following:
        // Class class = object_getClass((id)self);
        SEL originalSelector = @selector(viewWillAppear:);
        SEL swizzledSelector = @selector(xxx_viewWillAppear:);
        
        
        Method originalMethod = class_getInstanceMethod(class, originalSelector);
        Method swizzledMethod = class_getInstanceMethod(class, swizzledSelector);
//        BOOL didAddMethod = class_addMethod(class,
//                                            originalSelector,
//                                            method_getImplementation(swizzledMethod),
//                                            method_getTypeEncoding(swizzledMethod));
//        添加失败系统已经存在了一个 originalSelector
//        if (didAddMethod) {
//            class_replaceMethod(class,
//                                swizzledSelector,
//                                method_getImplementation(originalMethod),
//                                method_getTypeEncoding(originalMethod));
//        } else {
//            method_exchangeImplementations(originalMethod, swizzledMethod);
        
//        }
    });
    
//    class_replaceMethod(class, originalSelector, method_getImplementation(swizzledMethod), method_getTypeEncoding(swizzledMethod));//改变方法的实现方式
}
- (void)viewWillAppear:(BOOL)animated{
    [super viewWillAppear:animated];
    
    NSLog(@"%s",__func__);
}
#pragma mark - Method Swizzling
- (void)xxx_viewWillAppear:(BOOL)animated {//方法
    [self xxx_viewWillAppear:animated];//方式的实现
    NSLog(@"viewWillAppear: %@", self);
}

@end
