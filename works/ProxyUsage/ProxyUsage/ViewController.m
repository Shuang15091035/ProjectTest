//
//  ViewController.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/24.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ViewController.h"
#import "HttpProxy.h"
#import "CommentHttpHandlerIMP.h"
#import "UserHttpHandlerIMP.h"
#import "AppDelegate.h"
#import <objc/runtime.h>
//#import <objc/objc-runtime.h>
#import "Person.h"
#import "MyClass.h"

@interface ViewController ()

@end

@implementation ViewController

void CustomSetName(id self, SEL _cmd){
 
    NSLog(@"goodName");
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    
    [self myClassImplement];
    
//   [[HttpProxy shareInstance] registerHttpProtocol:@protocol(UserHttpProtocol) handler:[UserHttpHandlerIMP new]];

//    typedef struct objc_class{
//        Class isa;
//    } Class;
//    Class a = [Person class];//类本身也是一个对象，内部有一个Class isa，指向metaClass
////    在OC中super_class:指该类的父类，如果是NSObject或者是NSProxy，返回NULL;
////    ojbc_cache：表示用于缓存最近使用的方法,所用方法都存放在MethodLists中
////    Class 表示一个struct objc_class类型的指针
//    
////    objc_object表示一个类的实例的结构体
//    Person *person = [Person new];
//    class_createInstance([Person class],0);在alloc或者allocWithZone的同时创建一个ojbc_object的结构体对象指向其类
    
#pragma mark - 类与对象基础数据结构,class,objc_cache,MetaClass
//    Class 表示struct objc_class {
//        struct objc_class *isa;
//        struct objc_class *super_class;
//        const char *name;
//        long version;
//        long info;
//        long instance_size;
//        struct objc_ivar_list *ivars;
//        struct objc_method_list **methodLists;
//        struct objc_cache *cache;
//        struct objc_protocol_list *protocols;
//    };
//    SEL 表示一个方法的名称
//id 表示typedef struct objc_object {
//   Class isa;
//} *id/可以看到,iOS中很重要的id实际上就是objc_object的指针.而NSObject的第一个对象就是Class类型的isa。因此id可以标示所有基于NSObject的对象
//    IMP 函数指针，表示方法的地址
//    Method表示@selector和IMP的管理者 typedef struct objc_method *Method;
//    struct objc_method {
//        SEL method_name;
//        char *method_types;
//        IMP method_imp;
//    };
#pragma mark - 2.类相关操作函数
#pragma mark - 2.1.1类名
    //    class_getName(<#__unsafe_unretained Class cls#>)
#pragma mark - 2.1.2父类和元类
//    class_getSuperclass(<#__unsafe_unretained Class cls#>)
//    class_isMetaClass(<#__unsafe_unretained Class cls#>)
//    objc_getMetaClass(<#const char *name#>)
#pragma mark - 2.1.3实例变量大小
//    class_getInstanceSize(<#__unsafe_unretained Class cls#>)
#pragma mark - 2.1.4成员变量及属性
//    class_getInstanceVariable(<#__unsafe_unretained Class cls#>, <#const char *name#>)
//    class_getInstanceMethod(<#__unsafe_unretained Class cls#>, <#SEL name#>)
//    class_getClassVariable(<#__unsafe_unretained Class cls#>, <#const char *name#>)
//    class_addProperty(<#__unsafe_unretained Class cls#>, <#const char *name#>, <#const objc_property_attribute_t *attributes#>, <#unsigned int attributeCount#>)
//    class_copyIvarList(<#__unsafe_unretained Class cls#>, <#unsigned int *outCount#>)
    
//    class_getProperty(<#__unsafe_unretained Class cls#>, <#const char *name#>)
//    class_copyPropertyList(<#__unsafe_unretained Class cls#>, <#unsigned int *outCount#>)
//    class_addProperty(<#__unsafe_unretained Class cls#>, <#const char *name#>, <#const objc_property_attribute_t *attributes#>, <#unsigned int attributeCount#>)
//    class_replaceProperty(<#__unsafe_unretained Class cls#>, <#const char *name#>, <#const objc_property_attribute_t *attributes#>, <#unsigned int attributeCount#>)
    
#pragma mark - 方法
//    class_getInstanceMethod(<#__unsafe_unretained Class cls#>, <#SEL name#>)
//    class_addMethod(<#__unsafe_unretained Class cls#>, <#SEL name#>, <#IMP imp#>, <#const char *types#>)
//    class_getClassMethod(<#__unsafe_unretained Class cls#>, <#SEL name#>)
//    class_copyMethodList(<#__unsafe_unretained Class cls#>, <#unsigned int *outCount#>)
//    class_replaceMethod(<#__unsafe_unretained Class cls#>, <#SEL name#>, <#IMP imp#>, <#const char *types#>)
//    class_getMethodImplementation(<#__unsafe_unretained Class cls#>, <#SEL name#>)
//    class_getMethodImplementation_stret(<#__unsafe_unretained Class cls#>, <#SEL name#>)
//    class_respondsToSelector(<#__unsafe_unretained Class cls#>, <#SEL sel#>)
    
#pragma mark - 协议
//    class_addProtocol(<#__unsafe_unretained Class cls#>, <#Protocol *protocol#>)
//    class_copyProtocolList(<#__unsafe_unretained Class cls#>, <#unsigned int *outCount#>)
//    class_conformsToProtocol(<#__unsafe_unretained Class cls#>, <#Protocol *protocol#>)
    
    
#pragma mark - 版本
//    class_getVersion(<#__unsafe_unretained Class cls#>)
//    class_setVersion(<#__unsafe_unretained Class cls#>, <#int version#>)
    
#pragma mark - 其他
//    class_getfeatureClass(<#const char *name#>);
//    void objc_setFutureClass(<#Class cls#>, <#const char *name#>)不直接使用
    
#pragma mark - 实例
    
#pragma mark - 2.2动态创建类和对象
    
#pragma mark - 2.3实例操作函数
    
#pragma mark - 2.4获取类定义
    
    Person *xiaoming = [Person new];

    SEL classSEL = @selector(test1);
    
    Method test1Method = class_getInstanceMethod([xiaoming class], @selector(test1));
    Method test2Method = class_getInstanceMethod([xiaoming class], @selector(test2));
    
    SEL customSEL = @selector(custom:);
    Method customM = class_getInstanceMethod([xiaoming class], @selector(custom:));

    BOOL MethodSuccessed = class_addMethod([xiaoming class], customSEL, (IMP)method_getImplementation(customM), method_getTypeEncoding(customM));
//    class_replaceMethod([xiaoming class], customM, (IMP)customM, <#const char *types#>)//type,牵扯到类型编码

    
    
     int end = 0;
}

+(BOOL)resolveClassMethod:(SEL)sel{
    return [super resolveClassMethod:sel];
}

+(BOOL)resolveInstanceMethod:(SEL)sel{
    if (sel == @selector(dynamicMethodIMP)) {
        class_addMethod([self class], sel, (IMP)dynamicMethodIMP/*C实现*/, "v:@");//v返回值,:
    }
    return [super resolveInstanceMethod:sel];
}
void dynamicMethodIMP (id self, SEL _cmd){
    
}
//- (NSMethodSignature *)methodSignatureForSelector:(SEL)aSelector{
//    return [NSMethodSignature methodSignatureForSelector:aSelector];
//}
//- (void)forwardInvocation:(NSInvocation *)anInvocation{
//    SEL selector = anInvocation.selector;
//    AppDelegate *delegate = [AppDelegate new];
//    if (delegate && [delegate respondsToSelector:selector]) {
//        [anInvocation invokeWithTarget:delegate];
//    }
//}
- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

- (void)myClassImplement{
    MyClass *myClass = [[MyClass alloc] init];
    unsigned int outCount = 0;
    Class cls = myClass.class;
    // 类名
    NSLog(@"class name: %s", class_getName(cls));
    NSLog(@"==========================================================");
    // 父类
    NSLog(@"super class name: %s", class_getName(class_getSuperclass(cls)));
    NSLog(@"==========================================================");
    // 是否是元类
    NSLog(@"MyClass is %@ a meta-class", (class_isMetaClass(cls) ? @"" : @"not"));
    NSLog(@"==========================================================");
    Class meta_class = objc_getMetaClass(class_getName(cls));
    NSLog(@"%s's meta-class is %s", class_getName(cls), class_getName(meta_class));
    NSLog(@"==========================================================");
    // 变量实例大小
    NSLog(@"instance size: %zu", class_getInstanceSize(cls));
    NSLog(@"==========================================================");
    // 成员变量
    Ivar *ivars = class_copyIvarList(cls, &outCount);
    for (int i = 0; i < outCount; i++) {
        Ivar ivar = ivars[i];
        NSLog(@"instance variable's name: %s at index: %d", ivar_getName(ivar), i);
    }
    free(ivars);
    Ivar string = class_getInstanceVariable(cls, "_string");
    if (string != NULL) {
        NSLog(@"instace variable %s", ivar_getName(string));
    }
    NSLog(@"==========================================================");
    // 属性操作
    objc_property_t * properties = class_copyPropertyList(cls, &outCount);
    for (int i = 0; i < outCount; i++) {
        objc_property_t property = properties[i];
        NSLog(@"property's name: %s", property_getName(property));
    }
    free(properties);
    objc_property_t array = class_getProperty(cls, "array");
    if (array != NULL) {
        NSLog(@"property %s", property_getName(array));
    }
    NSLog(@"==========================================================");
    // 方法操作
    Method *methods = class_copyMethodList(cls, &outCount);
    for (int i = 0; i < outCount; i++) {
        Method method = methods[i];
        NSLog(@"method's signature: %s", method_getName(method));
    }
    free(methods);
    Method method1 = class_getInstanceMethod(cls, @selector(method1));
    if (method1 != NULL) {
        NSLog(@"method %s", method_getName(method1));
    }
    Method classMethod = class_getClassMethod(cls, @selector(classMethod1));
    if (classMethod != NULL) {
        NSLog(@"class method : %s", method_getName(classMethod));
    }
    NSLog(@"MyClass is%@ responsd to selector: method3WithArg1:arg2:", class_respondsToSelector(cls, @selector(method3WithArg1:arg2:)) ? @"" : @" not");
    IMP imp = class_getMethodImplementation(cls, @selector(method1));
    imp();
    NSLog(@"==========================================================");
    // 协议
    Protocol * __unsafe_unretained * protocols = class_copyProtocolList(cls, &outCount);
    Protocol * protocol;
    for (int i = 0; i < outCount; i++) {
        protocol = protocols[i];
        NSLog(@"protocol name: %s", protocol_getName(protocol));
    }
    NSLog(@"MyClass is%@ responsed to protocol %s", class_conformsToProtocol(cls, protocol) ? @"" : @" not", protocol_getName(protocol));
    NSLog(@"==========================================================");
}


@end
