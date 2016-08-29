//
//  HttpProxy.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/24.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "HttpProxy.h"
#import <objc/runtime.h>

@interface HttpProxy()

@property(nonatomic,strong)NSMutableDictionary *selTohandlerMap;

@end

@implementation HttpProxy

+ (instancetype)shareInstance{
#pragma mark - firstMethod
//    static HttpProxy *httpProxy = nil;
//    
//    if (httpProxy ==nil) {
//        httpProxy = [HttpProxy alloc];
//        httpProxy.selTohandlerMap = [NSMutableDictionary dictionary];
//    }
//    return httpProxy;
#pragma mark - secondMethod
    static HttpProxy *httpProxy = nil;
    
    static dispatch_once_t onceToken;
    dispatch_once(&onceToken, ^{
        httpProxy = [HttpProxy alloc];
        httpProxy.selTohandlerMap = [NSMutableDictionary new];
    });
    
    return httpProxy;

}

- (void)registerHttpProtocol:(Protocol *)httpProtocol handler:(id)handle{
    
    unsigned int numberOfMethod = 0;
    struct objc_method_description *methods = protocol_copyMethodDescriptionList(httpProtocol, YES, YES, &numberOfMethod);
    
    for (int i = 0; i < numberOfMethod; i++) {
        struct objc_method_description method = methods[i];
        [_selTohandlerMap setValue:handle forKey: NSStringFromSelector(method.name)];
    }
}
//生成方法签名
- (NSMethodSignature *)methodSignatureForSelector:(SEL)sel{
    
    NSString *methodName = NSStringFromSelector(sel);
    id handler = [_selTohandlerMap valueForKey:methodName];
    if (handler && [handler respondsToSelector:sel]) {
        return [handler methodSignatureForSelector:sel];
    }else{
        return [super methodSignatureForSelector:sel];
    }
    
}
//实现消息转发
- (void)forwardInvocation:(NSInvocation *)invocation{
    
    NSString *methodName = NSStringFromSelector(invocation.selector);
    id handler = [_selTohandlerMap valueForKey:methodName];
    if (handler && [handler respondsToSelector:invocation.selector]) {
        [invocation invokeWithTarget:handler];
    }else{
        [super forwardInvocation:invocation];
    }
}
@end
