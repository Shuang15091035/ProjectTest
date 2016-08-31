//
//  Runtime.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/31.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "Runtime.h"
#import "HttpProxy.h"
#import "CommentHttpHandlerIMP.h"
#import "UserHttpHandlerIMP.h"
#import "SUTRuntimeMethodHelper.h"
#import "AppDelegate.h"
#import <objc/runtime.h>
#import "Person.h"
#import "MyClass.h"
#import <objc/message.h>

@interface Runtime(){
      SUTRuntimeMethodHelper *_helper;
}

@end

@implementation Runtime

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    [self 拾遗];
    
}
#pragma mark - 拾遗(包括对类与对象、成员变量与属性、方法与消息、分类与协议的处理)

- (void)拾遗{
    //    self是类的一个隐藏参数，每个方法的实现的第一个参数即为self
    //    NSLog(@"%s",__func__);
    //    而super并不是隐藏参数，它实际上只是一个”编译器标示符”
    //    struct objc_super {
    //        id receiver;
    //        Class superClass;
    //    };
    
    //    objc_msgSendSuper();父类发送消息时调用的方法
    
    //    objc_msgSend() ENABLE_STRICT_OBJC_MSGSEND = NO可以使用如下方法 preprocessing
    //    objc_msgSend(<#id self#>, <#SEL op, ...#>)
    
    //    objc_copyImageNames(<#unsigned int *outCount#>) //获取所有的OC中的动态库和框架的名称
    // 获取指定类所在动态库
    //    const char * class_getImageName ( Class cls );
    // 获取指定库或框架中所有类的类名
    //    const char ** objc_copyClassNamesForImage ( const char *image, unsigned int *outCount );
    //    unsigned int outCount = 0;
    //    NSLog(@"获取指定类所在动态库");
    //    NSLog(@"UIView's Framework: %s", class_getImageName(NSClassFromString(@"UIView")));
    //    NSLog(@"获取指定库或框架中所有类的类名");
    //    const char ** classes = objc_copyClassNamesForImage(class_getImageName(NSClassFromString(@"UIView")), &outCount);
    //    for (int i = 0; i < outCount; i++) {
    //        NSLog(@"class name: %s", classes[i]);
    //    }
    
    
    //    imp_implementationWithBlock(<#id block#>)
    //    imp_getBlock(<#IMP anImp#>)
    //    imp_removeBlock(<#IMP anImp#>)//解除block与IMP的关联关系，释放block的拷贝
    
    
    
    int end = 0;
}


#pragma mark - protocolAndCategory(runtime 五)

- (void)protocolAndCatagory{
    //    分类可以添加方法扩充它但不能添加新的实例变量
    //    struct objc_category *Categeroy;
    //    struct objc_category{
    //        char *category_name; //分类名
    //        char *class_name;   //分类所属的类名
    //        struct objc_method_list *instance_methods;  //实例方法列表
    //        struct objc_method_list *class_methods; //类方法列表
    //        struct objc_protocol_list *protocols;   //分类所实现的协议列表
    //    }
    //    typedef struct objc_object Procotol; //对象结构体
    //    // 返回指定的协议
    //    Protocol * objc_getProtocol ( const char *name );
    //    // 获取运行时所知道的所有协议的数组
    //    Protocol ** objc_copyProtocolList ( unsigned int *outCount );
    //    // 创建新的协议实例
    //    Protocol * objc_allocateProtocol ( const char *name );
    //    // 在运行时中注册新创建的协议
    //    void objc_registerProtocol ( Protocol *proto );
    //    // 为协议添加方法
    //    void protocol_addMethodDescription ( Protocol *proto, SEL name, const char *types, BOOL isRequiredMethod, BOOL isInstanceMethod );
    //    // 添加一个已注册的协议到协议中
    //    void protocol_addProtocol ( Protocol *proto, Protocol *addition );
    //    // 为协议添加属性
    //    void protocol_addProperty ( Protocol *proto, const char *name, const objc_property_attribute_t *attributes, unsigned int attributeCount, BOOL isRequiredProperty, BOOL isInstanceProperty );
    //    // 返回协议名
    //    const char * protocol_getName ( Protocol *p );
    //    // 测试两个协议是否相等
    //    BOOL protocol_isEqual ( Protocol *proto, Protocol *other );
    //    // 获取协议中指定条件的方法的方法描述数组
    //    struct objc_method_description * protocol_copyMethodDescriptionList ( Protocol *p, BOOL isRequiredMethod, BOOL isInstanceMethod, unsigned int *outCount );
    //    // 获取协议中指定方法的方法描述
    //    struct objc_method_description protocol_getMethodDescription ( Protocol *p, SEL aSel, BOOL isRequiredMethod, BOOL isInstanceMethod );
    //    // 获取协议中的属性列表
    //    objc_property_t * protocol_copyPropertyList ( Protocol *proto, unsigned int *outCount );
    //    // 获取协议的指定属性
    //    objc_property_t protocol_getProperty ( Protocol *proto, const char *name, BOOL isRequiredProperty, BOOL isInstanceProperty );
    //    // 获取协议采用的协议
    //    Protocol ** protocol_copyProtocolList ( Protocol *proto, unsigned int *outCount );
    //    // 查看协议是否采用了另一个协议
    //    BOOL protocol_conformsToProtocol ( Protocol *proto, Protocol *other );
}


#pragma mark - MethodSwizzling(runtime 四)

- (void)MethodSwizzling{
    //    Method Swizzling是改变一个selector的实际实现的技术。通过这一技术，我们可以在运行时通过修改类的分发表中selector对应的函数，来修改方法的实现。
    //
    //    SUTRuntimeMethodHelper *helper = [[SUTRuntimeMethodHelper alloc]init];
    //    self.view.backgroundColor = [UIColor redColor];
    //    [self.navigationController pushViewController:helper animated:NO];
    
    //    +load会在类初始加载时调用，+initialize会在第一次调用类的类方法或实例方法之前被调用。
    //    Swizzling应该总是在+load中执行(在Objective-C中，运行时会自动调用每个类的两个方法)+load能保证在类的初始化过程中被加载，并保证这种改变应用级别的行为的一致性。相比之下，+initialize在其执行时不提供这种保证–事实上，如果在应用中没为给这个类发送消息，则它可能永远不会被调用。
    
    //    Swizzling应该总是在dispatch_once中执行
    //    与上面相同，因为swizzling会改变全局状态，所以我们需要在运行时采取一些预防措施。原子性就是这样一种措施，它确保代码只被执行一次，不管有多少个线程。GCD的dispatch_once可以确保这种行为，我们应该将其作为method swizzling的最佳实践。
    //    SEL 运行时的一个方法的名称,是一个C字符串，运行时被注册，由编译器生成，并且在类被加载时由运行时自动做映射操作
    //    Method 表示方法的类型 IMP函数的入口
    
    //    Swizzling通常被称作是一种黑魔法，容易产生不可预知的行为和无法预见的后果。虽然它不是最安全的，但如果遵从以下几点预防措施的话，还是比较安全的：
    //   1. 总是调用方法的原始实现(除非有更好的理由不这么做)：API提供了一个输入与输出约定，但其内部实现是一个黑盒。Swizzle一个方法而不调用原始实现可能会打破私有状态底层操作，从而影响到程序的其它部分。
    //   2. 避免冲突：给自定义的分类方法加前缀，从而使其与所依赖的代码库不会存在命名冲突。
    //   3. 明白是怎么回事：简单地拷贝粘贴swizzle代码而不理解它是如何工作的，不仅危险，而且会浪费学习Objective-C运行时的机会。阅读Objective-C Runtime Reference和查看<objc/runtime.h>头文件以了解事件是如何发生的。
    //   4. 小心操作：无论我们对Foundation, UIKit或其它内建框架执行Swizzle操作抱有多大信心，需要知道在下一版本中许多事可能会不一样。
    
    int end = 0;
    
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

#pragma mark - MethodAndSelector(消息处理机制)
- (void)MethodAndSelector{
    //    typedef struct objc_selector *SEL;//运行时方法的名称，Objective-C在编译时，会依据每一个方法的名字、参数序列，生成一个唯一的整型标识(Int类型的地址)，这个标识就是SEL//不能使用运算法重载
    //    SEL sel1 = @selector(method1);SEL实际就是根据方法名hash化得一个字符串，本质上是一个指向方法的指针(准确的说，只是一个根据方法名hash化了的KEY值，能唯一代表一个方法)它的存在只是为了加快方法的查询速度
    //    获取SEL
    //    sel_registerName(<#const char *str#>)
    //    @selector(<#selector#>)
    //    NSSelectorFromString(<#NSString * _Nonnull aSelectorName#>)
    //    NSLog(@"sel : %p", sel1);
    
    //    IMP实际上是一个函数指针，指向方法实现的首地址id (*IMP)(id, SEL, ...)第一个参数指向self的指针（如果是实例方法，则是类实例的内存地址；如果是类方法，则是指向元类的指针），第二个参数方法选择器，其他是方法的实际参数列表
    
    Class CustomClass = objc_allocateClassPair([NSObject class], "CustomClass", 0);
    SEL selector = NSSelectorFromString(@"CustomMethodIMP");
    //    SEL customSEL = @selector(customMethodIMP);
    class_addMethod([CustomClass class], selector, (IMP)customMethodIMP, "@@:qq");
    objc_registerClassPair(CustomClass);
    //    typedef struct objc_method *Method;
    //    struct objc_method{
    //        SEL method_name;
    //        char *method_types;
    //        IMP method_imp;
    //    };
    
    //    struct objc_method_description {
    //        SEL name; //方法的描述
    //        char *types; //方法的类型
    //    };
    
    //   method_invoke();
    //    Method m = class_getInstanceMethod(CustomClass, selector);
    //    IMP implement =  method_getImplementation(m);
    //    sel_isEqual(<#SEL lhs#>, <#SEL rhs#>)比较两个SEL
    //    sel_getUid(<#const char *str#>)Runtime中注册一个方法
    
    //    objc_msgSend(CustomClass, selector, arg1, arg2);
    //    selector 找寻方法的实现，找寻不到，会执行消息转发，objc_cache缓存加快消息的处理
    //    [self methodForSelector:<#(SEL)#>]
    
    //    消息转发机制基本上分为三个步骤
    //    1.动态方法解析，2.备用接受者，3.完整转发
    
    //    接收到未知消息时，首先会调用所属类的类方法
    //    + (BOOL)resolveClassMethod:(SEL)sel
    //    + (BOOL)resolveInstanceMethod:(SEL)sel
    //    class_addMethod(<#__unsafe_unretained Class cls#>, <#SEL name#>, <#IMP imp#>, <#const char *types#>)//动态的添加实现
    //    备用接收者
    /*    如果一个对象实现了这个方法，并返回一个非nil的结果，则这个对象会作为消息的新接收者，且消息会被分发到这个对象。当然这个对象不能是self自身，否则就是出现无限循环。当然，如果我们没有指定相应的对象来处理aSelector，则应该调用父类的实现来返回结果
     - (id)forwardingTargetForSelector:(SEL)aSelector*/
    //    执行 [self performSelector:@selector(method)];method 方法未找到
    //    执行 (id)forwardingTargetForSelector:(SEL)aSelector {
    //        NSLog(@"forwardingTargetForSelector");
    //        NSString *selectorString = NSStringFromSelector(aSelector);
    //        // 将消息转发给_helper来处理
    //        if ([selectorString isEqualToString:@"method2"]) {
    //            return _helper;
    //        }
    //        return [super forwardingTargetForSelector:aSelector];
    //    }但这一步无法对消息进行处理，如操作消息的参数和返回值。
    //      完整消息转发，以上还不能处理未知消息，唯一能做的就是启用完整的消息转发机制了；调用一下方法
    //    - (void)forwardInvocation:(NSInvocation *)anInvocation;
    //    运行时系统会在这一步给消息接收者最后一次机会将消息转发给其它对象，创建一个表示消息的NSInvocation对象，包括selector，target，argument；
    
    //forwardInvocation:方法的实现有两个任务：
    //    定位可以响应封装在anInvocation中的消息的对象。这个对象不需要能处理所有未知消息。
    //    使用anInvocation作为参数，将消息发送到选中的对象。anInvocation将会保留调用结果，运行时系统会提取这一结果并将其发送到消息的原始发送者。
    
    //    还有一个很重要的问题，我们必须重写以下方法
    //    - (NSMethodSignature *)methodSignatureForSelector:(SEL)aSelector;
    //    消息转发机制通过这个方法中获取的信息来创建NSInvacation对象，必须重写这个方法，为给定的selector提供一个合适的方法签名
    //    NSObject的forwardInvocation:方法实现只是简单调用了doesNotRecognizeSelector:方法，它不会转发任何消息。
    //    从某种意义上来讲，forwardInvocation:就像一个未知消息的分发中心，将这些未知的消息转发给其它对象。或者也可以像一个运输站一样将所有未知消息都发送给同一个接收对象。这取决于具体的实现。
#pragma mark - 消息转发机制
    //- (NSMethodSignature *)methodSignatureForSelector:(SEL)aSelector{
    //    NSMethodSignature *signature = [super methodSignatureForSelector:aSelector];
    //    if (!signature) {
    //        if ([SUTRuntimeMethodHelper instancesRespondToSelector:aSelector]) {
    //            signature = [SUTRuntimeMethodHelper instanceMethodSignatureForSelector:aSelector];
    //        }
    //    }
    //    return signature;
    //}
    //
    //- (void)forwardInvocation:(NSInvocation *)anInvocation{
    //    if ([SUTRuntimeMethodHelper instanceMethodSignatureForSelector:anInvocation.selector]) {
    //        [anInvocation invokeWithTarget:_helper];
    //    }
    //}
    //    消息转发和多重继承
    //    这种方式可以允许一个对象和其他对象建立关系。以及处理某些未知消息
    
    
    
    int end = 0;
    
}
- (instancetype)init {
    self = [super init];
    if (self != nil) {
        _helper = [[SUTRuntimeMethodHelper alloc] init];
    }
    return self;
}

NSString *customMethodIMP(id self, SEL _cmd, NSInteger age, NSInteger height){
    
    return [NSString stringWithFormat:@"%ld,%ld",age,height];
}


#pragma mark - IVarAndProperty（成员变量和属性runtime 二）

- (void)IVarAndProperty{
    //    类型编码 编译器将每个方法的返回值和参数类型编码为一个字符串，并将其与方法的selector关联在一起可以使用@encode编译器指令来获取它,事实上，任何可以作为sizeof()操作参数的类型都可以用于@encode()。
    //    char *s = @encode(NSValue);
    //    float a[] = {1.0, 2.0, 3.0};
    //    NSLog(@"array encoding type: %s", @encode(typeof(a)));
    
    //    typedef struct objc_ivar *IVar;
    //    struct objc_ivar {
    //        char *ivar_name;    //变量名
    //        char *ivar_type;    //变量类型
    //        char *ivar_offse;   //基地址的偏移字节
    //#ifdef __LP64
    //        int space;
    //#endif
    //    };
    
    //    typedef struct objc_property *objc_property_t;
    
    //    typedef struct {
    //        const char *name;   //特性名
    //        const char *value; //特性值
    //    }objc_property_attribute_t;
    
    //    关联对象类似于成员变量，在运行时添加的
    //    objc_getAssociatedObject(<#id object#>, <#const void *key#>)//通过给定的key连接到类的一个实例上，指定内存管理策略
    //    OBJC_ASSOCIATION_ASSIGN
    //    OBJC_ASSOCIATION_RETAIN_NONATOMIC
    //    OBJC_ASSOCIATION_COPY_NONATOMIC
    //    OBJC_ASSOCIATION_RETAIN
    //    OBJC_ASSOCIATION_COPY
    //    如果指定的策略是assign，则宿主释放时，关联对象不会被释放；而如果指定的是retain或者是copy，则宿主释放时，关联对象会被释放。我们甚至可以选择是否是自动retain/copy。当我们需要在多个线程中处理访问关联对象的多线程代码时，这就非常有用了
    //    我们将一个对象连接到其它对象
    //    static char myKey;
    //    objc_setAssociatedObject(self, &myKey, anObject, OBJC_ASSOCIATION_RETAIN);
    //如果我们使用同一个key来关联另外一个对象时，也会自动释放之前关联的对象，这种情况下，先前的关联对象会被妥善地处理掉，并且新的对象会使用它的内存。
    //    objc_removeAssociatedObjects(<#id object#>)移除关联对象
    //    objc_setAssociatedObject(<#id object#>, <#const void *key#>, <#id value#>, <#objc_AssociationPolicy policy#>)将key设置为nil也可以实现
    
    
    //    - (void)setTapActionWithBlock:(void (^)(void))block
    //    {
    //        UITapGestureRecognizer *gesture = objc_getAssociatedObject(self, &kDTActionHandlerTapGestureKey);
    //
    //        if (!gesture)
    //        {
    //            gesture = [[UITapGestureRecognizer alloc] initWithTarget:self action:@selector(__handleActionForTapGesture:)];
    //            [self addGestureRecognizer:gesture];
    //            objc_setAssociatedObject(self, &kDTActionHandlerTapGestureKey, gesture, OBJC_ASSOCIATION_RETAIN);
    //        }
    //        objc_setAssociatedObject(self, &kDTActionHandlerTapBlockKey, block, OBJC_ASSOCIATION_COPY);
    //    }
    ////    ```
    ////    这段代码检测了手势识别的关联对象。如果没有，则创建并建立关联关系。同时，将传入的块对象连接到指定的key上。注意`block`对象的关联内存管理策略。
    ////    手势识别对象需要一个`target`和`action`，所以接下来我们定义处理方法：
    ////    ```objc
    //    - (void)__handleActionForTapGesture:(UITapGestureRecognizer *)gesture
    //    {
    //        if (gesture.state == UIGestureRecognizerStateRecognized)
    //        {
    //            void(^action)(void) = objc_getAssociatedObject(self, &kDTActionHandlerTapBlockKey);
    //            if (action)
    //            {
    //                action();
    //            }
    //        }
    //    }
    //
    //    property_getName(<#objc_property_t property#>)
    //    property_getAttributes(<#objc_property_t property#>)
    //    property_copyAttributeList(<#objc_property_t property#>, <#unsigned int *outCount#>)
    //    property_copyAttributeValue(<#objc_property_t property#>, <#const char *attributeName#>)
    //    property_copyAttributeValue函数，返回的char *在使用完后需要调用free()释放。
    //    property_copyAttributeList函数，返回值在使用完后需要调用free()释放。
    //
    
    //    - (void)setDataWithDic:(NSDictionary *)dic
    //    {
    //        [dic enumerateKeysAndObjectsUsingBlock:^(NSString *key, id obj, BOOL *stop) {
    //            NSString *propertyKey = [self propertyForKey:key];
    //            if (propertyKey)
    //            {
    //                objc_property_t property = class_getProperty([self class], [propertyKey UTF8String]);
    //                // TODO: 针对特殊数据类型做处理
    //                NSString *attributeString = [NSString stringWithCString:property_getAttributes(property) encoding:NSUTF8StringEncoding];
    //                ...
    //                [self setValue:obj forKey:propertyKey];
    //            }
    //        }];
    //    }关联对象的使用不是很明白
    
    int end = 0;
    
    
}

#pragma mark - unRecognizeMessageSendToInstance
- (void)unRecognizeMessageSendToInstance{
    //    +(BOOL)resolveClassMethod:(SEL)sel{
    //        return [super resolveClassMethod:sel];
    //    }
    //
    //    +(BOOL)resolveInstanceMethod:(SEL)sel{
    //        if (sel == @selector(dynamicMethodIMP)) {
    //            class_addMethod([self class], sel, (IMP)dynamicMethodIMP/*C实现*/, "v:@");//v返回值,:
    //        }
    //        return [super resolveInstanceMethod:sel];
    //    }
    //    void dynamicMethodIMP (id self, SEL _cmd){
    //
    //    }
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
}

#pragma mark - 类与对象(runTime 一)
- (void)ClassAndInstanc{
    //    [self myClassImplement];
    
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
    //    objc_allocateClassPair(<#__unsafe_unretained Class superclass#>, <#const char *name#>, <#size_t extraBytes#>)extraBytes通常指定为0，该参数是分配给类和元类对象尾部的索引ivars的字节数。
    //    objc_disposeClassPair(<#__unsafe_unretained Class cls#>)//销毁一个类以及相关联的类，类和子类对象
    //    objc_registerClassPair(<#__unsafe_unretained Class cls#>)在应用中注册由objc_registerClassPair()创建的类
    //    class_addIvar(<#__unsafe_unretained Class cls#>, <#const char *name#>, <#size_t size#>, <#uint8_t alignment#>, <#const char *types#>)//成员变量的按字节最小对齐量是1<<alignment。这取决于ivar的类型和机器的架构。如果变量的类型是指针类型，则传递log2(sizeof(pointer_type)),type根据类型编码决定
#pragma mark - 2.3实例操作函数
    //动态创建类
    //    Class cls = objc_allocateClassPair(MyClass.class, "MySubClass", 0);
    //    class_addMethod(cls, @selector(submethod1), (IMP)imp_submethod1, "v@:");
    //    class_replaceMethod(cls, @selector(method1), (IMP)imp_submethod1, "v@:");
    //    class_addIvar(cls, "_ivar1", sizeof(NSString *), log(sizeof(NSString *)), "i");
    //    objc_property_attribute_t type = {"T", "@\"NSString\""};
    //    objc_property_attribute_t ownership = { "C", "" };
    //    objc_property_attribute_t backingivar = { "V", "_ivar1"};
    //    objc_property_attribute_t attrs[] = {type, ownership, backingivar};
    //    class_addProperty(cls, "property2", attrs, 3);
    //    objc_registerClassPair(cls);
    //    id instance = [[cls alloc] init];
    //    [instance performSelector:@selector(submethod1)];
    //    [instance performSelector:@selector(method1)];
#pragma mark - 动态创建对象（ARCDiscard）
    //    class_createInstance(Class cls, size_t extraBytes);ARC dispose 创建类实例
    //    objc_constructInstance(Class cls,  void *bytes)在指定位置创建类实例
    //    objc_destructInstance(id obj)销毁类实例
    
    //    object_copy(id obj, size_t size);
    //    object_dispos(id obj);
    //    object_setClass(<#id obj#>, <#__unsafe_unretained Class cls#>)(UNDiscard)
    
    // 修改类实例的实例变量的值
    //    Ivar object_setInstanceVariable ( id obj, const char *name, void *value );
    //    // 获取对象实例变量的值
    //    Ivar object_getInstanceVariable ( id obj, const char *name, void **outValue );
    //    // 返回指向给定对象分配的任何额外字节的指针
    //    void * object_getIndexedIvars ( id obj );
    //    // 返回对象中实例变量的值
    //    id object_getIvar ( id obj, Ivar ivar );
    //    // 设置对象中实例变量的值
    //    void object_setIvar ( id obj, Ivar ivar, id value );
#pragma mark - 2.4获取类定义
    
    
    
#pragma mark - #########################
    Person *xiaoming = [Person new];
    
    //    Ivar ivar = class_getInstanceVariable([xiaoming class], "_name");
    //    object_setIvar(xiaoming, ivar, @"string");
    //    NSString *stirng = object_getIvar(xiaoming, ivar);
    
    SEL classSEL = @selector(test1);
    
    Method test1Method = class_getInstanceMethod([xiaoming class], @selector(test1));
    Method test2Method = class_getInstanceMethod([xiaoming class], @selector(test2));
    
    SEL customSEL = @selector(custom:);
    Method customM = class_getInstanceMethod([xiaoming class], @selector(custom:));
    
    BOOL MethodSuccessed = class_addMethod([xiaoming class], customSEL, (IMP)method_getImplementation(customM), method_getTypeEncoding(customM));
    //    class_replaceMethod([xiaoming class], customM, (IMP)customM, <#const char *types#>)//type,牵扯到类型编码
    /*???:添加标记 */
    /*!!!:添加标记 */
    /*MARK:添加标记 */
    /*FIXME:添加标记 */
    /*TODO:添加标记 */
#warning 添加警告
    
#pragma mark - 得到系统中所用的类的list
    int numClass;
    Class *classes = NULL;
    numClass = objc_getClassList(NULL, 0);
    if (numClass > 0) {
        classes = (Class *)malloc(sizeof(Class) * numClass);
        numClass = objc_getClassList(classes, numClass);
        NSLog(@"numberofClasses%d",numClass);
        for (int i = 0; i < numClass; i++) {
            Class cls = classes[i];
            NSLog(@"Class name:%s",class_getName(cls));
        }
        free(classes);
    }
    
#pragma mark - 获取类定义的方法
    //    objc_lookUpClass(<#const char *name#>)类未注册返回nil
    //    objc_getClass(<#const char *name#>)调用类回调，再次确认是否注册，确认未注册返回il
    //    objc_getRequiredClass(<#const char *name#>)本方法上同，如果没有找到，kill progess
    //    objc_getMetaClass(<#const char *name#>)如果没有注册，调用回调，确认未注册返回nil
    //    ，不过，每个类定义都必须有一个有效的元类，所以函数总是会返回一个元类定义，不管是否有效。
}

//- (void)myClassImplement{
//    MyClass *myClass = [[MyClass alloc] init];
//    unsigned int outCount = 0;
//    Class cls = myClass.class;
//    // 类名
//    NSLog(@"class name: %s", class_getName(cls));
//    NSLog(@"==========================================================");
//    // 父类
//    NSLog(@"super class name: %s", class_getName(class_getSuperclass(cls)));
//    NSLog(@"==========================================================");
//    // 是否是元类
//    NSLog(@"MyClass is %@ a meta-class", (class_isMetaClass(cls) ? @"" : @"not"));
//    NSLog(@"==========================================================");
//    Class meta_class = objc_getMetaClass(class_getName(cls));
//    NSLog(@"%s's meta-class is %s", class_getName(cls), class_getName(meta_class));
//    NSLog(@"==========================================================");
//    // 变量实例大小
//    NSLog(@"instance size: %zu", class_getInstanceSize(cls));
//    NSLog(@"==========================================================");
//    // 成员变量
//    Ivar *ivars = class_copyIvarList(cls, &outCount);
//    for (int i = 0; i < outCount; i++) {
//        Ivar ivar = ivars[i];
//        NSLog(@"instance variable's name: %s at index: %d", ivar_getName(ivar), i);
//    }
//    free(ivars);
//    Ivar string = class_getInstanceVariable(cls, "_string");
//    if (string != NULL) {
//        NSLog(@"instace variable %s", ivar_getName(string));
//    }
//    NSLog(@"==========================================================");
//    // 属性操作
//    objc_property_t * properties = class_copyPropertyList(cls, &outCount);
//    for (int i = 0; i < outCount; i++) {
//        objc_property_t property = properties[i];
//        NSLog(@"property's name: %s", property_getName(property));
//    }
//    free(properties);
//    objc_property_t array = class_getProperty(cls, "array");
//    if (array != NULL) {
//        NSLog(@"property %s", property_getName(array));
//    }
//    NSLog(@"==========================================================");
//    // 方法操作
//    Method *methods = class_copyMethodList(cls, &outCount);
//    for (int i = 0; i < outCount; i++) {
//        Method method = methods[i];
//        NSLog(@"method's signature: %s", method_getName(method));
//    }
//    free(methods);
//    Method method1 = class_getInstanceMethod(cls, @selector(method1));
//    if (method1 != NULL) {
//        NSLog(@"method %s", method_getName(method1));
//    }
//    Method classMethod = class_getClassMethod(cls, @selector(classMethod1));
//    if (classMethod != NULL) {
//        NSLog(@"class method : %s", method_getName(classMethod));
//    }
//    NSLog(@"MyClass is%@ responsd to selector: method3WithArg1:arg2:", class_respondsToSelector(cls, @selector(method3WithArg1:arg2:)) ? @"" : @" not");
//    IMP imp = class_getMethodImplementation(cls, @selector(method1));
//    imp();//方法执行
//    NSLog(@"==========================================================");
//    // 协议
//    Protocol * __unsafe_unretained * protocols = class_copyProtocolList(cls, &outCount);
//    Protocol * protocol;
//    for (int i = 0; i < outCount; i++) {
//        protocol = protocols[i];
//        NSLog(@"protocol name: %s", protocol_getName(protocol));
//    }
//    NSLog(@"MyClass is%@ responsed to protocol %s", class_conformsToProtocol(cls, protocol) ? @"" : @" not", protocol_getName(protocol));
//    NSLog(@"==========================================================");
//}
//void CustomSetName(id self, SEL _cmd){
//    
//    NSLog(@"goodName");
//}


@end
