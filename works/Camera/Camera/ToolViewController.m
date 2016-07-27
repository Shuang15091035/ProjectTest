//
//  ViewController.m
//  Camera
//
//  Created by mac zdszkj on 16/3/16.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ToolViewController.h"
#import "MyView.h"
#import "NewFile.h"
#import <CoreMotion/CoreMotion.h>
#import "Horizontal.h"
#import "ClassMeta.h"
#import <objc/runtime.h>
#import <Photos/Photos.h>

//CGFloat hue = ( arc4random() % 256 / 256.0 );\  /*  0.0 to 1.0 */\
//\CGFloat saturation = ( arc4random() % 128 / 256.0 ) + 0.5; \ /* 0.5 to 1.0,\ away from white */ \
//\CGFloat brightness = ( arc4random() % 128 / 256.0 ) + 0.5; \ /*  0.5 to 1.0, away from black */ \

#define randomColor() \
 [UIColor colorWithHue:( arc4random() % 256 / 256.0 ) saturation:( arc4random() % 128 / 256.0 ) + 0.5 brightness:( arc4random() % 128 / 256.0 ) + 0.5 alpha:1]; \

typedef struct {
    char *name;
    float hight;
}stu, *stuP;

static char *kDTActionHandlerTapGestureKey;
static char *kDTActionHandlerTapBlockKey;
@interface ToolViewController (){
    CMMotionManager *motionManager;
//    NSTimer *timer;
    NSConditionLock *theLock;
}
@property (nonatomic,) MyView *AView ;

@end

@implementation ToolViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    
//    _AView = [[MyView alloc]init];
//    [self.view addSubview:_AView];
//    ClassMeta *meta = [[ClassMeta alloc]init];
//    [meta ex_registerClassPair];
//    NSLog(@"%@",meta);
    
//    1.成员变量操作函数，主要包含以下函数： objc_class
//    
//    const char *name = class_getName([self class]);
//    NSLog(@"%s",name);
//    BOOL isMetaClass = class_isMetaClass([NSObject class]);
//    if (isMetaClass) {
//        NSLog(@"isMetaClass");
//    }
//    size_t size = class_getInstanceSize([self class]);
//    NSLog(@"%zu",size);
//    
////    objc_class中，所有的成员变量、属性的信息是放在链表ivars中的
//    // 获取类中指定名称实例成员变量的信息
//    Ivar instanceVar = class_getInstanceVariable([self class], "motionManager");
////    NSLog(@"%@",instanceVar->);
//    // 获取类成员变量的信息
//   Ivar classVar = class_getClassVariable([self class], "AView");
//    // 添加成员变量
//    BOOL addOK = class_addIvar([self class], "height", sizeof(NSInteger), NSTextAlignmentCenter, "NSInteger");
//    if (addOK) {
//        NSLog(@"addOK");
//    }
//    unsigned int outCount;
//    // 获取整个成员变量列表
//    Ivar *var = class_copyIvarList([self class], &outCount);
////    返回一个指向成员变量的数组，数组中的每一个元素都是指向该成员变量信息的objc_ivar结构体的指针
//    
//    free(var);
////    2.属性操作函数，主要包含以下函数
//    // 获取指定的属性
//    objc_property_t property = class_getProperty([self class], "AView");
//    // 获取属性列表
//    unsigned int outC;
//    objc_property_t *proper = class_copyPropertyList([self class], &outC);
//    NSLog(@"%d",outC);
////    为类添加属性
////    BOOL addProperty = class_addProperty([self class], <#const char *name#>, <#const objc_property_attribute_t *attributes#>, <#unsigned int attributeCount#>)
//    // 替换类的属性
//    
//    void class_replaceProperty ( Class cls, const char *name, const objc_property_attribute_t *attributes, unsigned int attributeCount );
//    
////    方法操作的函数
////    class_setVersion([NSObject class], 10086);
//   int version = class_getVersion([NSObject class]);
//    NSLog(@"%d",version);

//    kDTActionHandlerTapGestureKey = "name";
//   
//    
//    NSLog(@"%@",class_getProperty([self class], kDTActionHandlerTapGestureKey)) ;
//    unsigned int outCount;
////    获取属性
//    objc_property_t *properties = class_copyPropertyList([self class], &outCount);
//    for (int i = 0; i < outCount; i++) {
//        objc_property_t property = properties[i];
//        NSLog(@"property's name: %s", property_getName(property));
//    }
////    获取成员变量
//    Ivar *ivars = class_copyIvarList([self class], &outCount);
//    for (int i = 0; i < outCount; i++) {
//        Ivar ivar = ivars[i];
//         NSLog(@"instance variable's name: %s at index: %d", ivar_getName(ivar), i);
//    }
//    
//    Method *methods = class_copyMethodList([self class], &outCount);
//    for (int i = 0; i < outCount; i++) {
//        Method method = methods[i];
//        NSLog(@"method's signature: %s", method_getName(method));
//    }
//
//    free(methods);
//    [self setTapActionWithBlock:^(NSString *name){
//        NSLog(@"goood%@",name);
//    }];
//    unsigned int outCount;
//    Ivar *ivarS = class_copyIvarList([self class], &outCount);
//    for (int i = 0; i < outCount; i++) {
//        Ivar instance = ivarS[i];
//        NSLog(@"Name:%s,TypeEncoding:%s,Offset:%td,%s",ivar_getName(instance),ivar_getTypeEncoding(instance),ivar_getOffset(instance),@encode(typeof(instance)));
//    }
    

//    Class cls = objc_allocateClassPair([UIView class], "MyView", 0);
//    class_addMethod(cls, @selector(subMethod1), (IMP)imp_subMethod1, "v@:");
//    class_addIvar(cls, "_ivar1", sizeof(NSString *), log(sizeof(NSString *)), "i");
//    objc_property_attribute_t type = {"T","@\"NSString\""};
//    objc_property_attribute_t ownerShip = {"C",""};
//    objc_property_attribute_t backingIvar = {"V","_ivar1"};
//    objc_property_attribute_t attrs[] = {type,ownerShip,backingIvar};
//    class_addProperty(cls, "property2", attrs, 3);
//    objc_registerClassPair(cls);
//    
//    id instance = [[cls alloc]init];
//    [instance performSelector:@selector(subMethod1)];
//    
//    NSString *theObject = class_createInstance(NSString.class, sizeof(unsigned));//OBJC_ARC_UNAVAILABLE
//    object_getClassName(<#id obj#>);
//    object_getClass(<#id obj#>)
    
//    int numClasses;
//    
//    Class * classes = NULL;
//
//    numClasses = objc_getClassList(NULL, 0);
//    if (numClasses > 0) {
//        classes = (Class *)malloc(sizeof(Class) * numClasses);
//        numClasses = objc_getClassList(classes, numClasses);
//        
//        NSLog(@"number of classes: %d", numClasses);
//        
//        for (int i = 0; i < numClasses; i++) {
//            
//            Class cls = classes[i];
//            NSLog(@"class name: %s", class_getName(cls));
//        }
//        
//        free(classes);
//    }
    
//    RunLoop 是EVENT LOOP 让线程能随时处理事件，而不退出的一种机制。
//    do {
//        var message = get_next_message();
//        process_message = (message);
//    } while (message != quit);//有消息时处理，没有消息时休眠。避免资源被浪费
//    RUNLOOP类似一个对象，管理其需要处理的事件和消息。
 
//    CFRunLoopGetMain();
//    CFRunLoopGetCurrent();
    
//    RUNLOOP的切换需要先退出LOOP，在重新指定一个新的Mode。
//    一个 RunLoop 包含若干个 Mode，每个 Mode 又包含若干个 Source/Timer/Observer。每次调用 RunLoop 的主函数时，只能指定其中一个 Mode，这个Mode被称作 CurrentMode。如果需要切换 Mode，只能退出 Loop，再重新指定一个 Mode 进入。这样做主要是为了分隔开不同组的 Source/Timer/Observer，让其互不影响
    
//    Source包含Source0,Source1;Source0只包含一个回调。
//    CFRunLoopTimerRef timer = [CFRunLoopTimerCreate(<#CFAllocatorRef allocator#>, <#CFAbsoluteTime fireDate#>, <#CFTimeInterval interval#>, <#CFOptionFlags flags#>, <#CFIndex order#>, <#CFRunLoopTimerCallBack callout#>, <#CFRunLoopTimerContext *context#>)];
//    CFRunLoopRef currentRunLoop = CFRunLoopGetCurrent();
//   kCFRunLoopCommonModes
//    NSLog(@"%ld",index);
    
//    Mach 是一个微内核，提供了诸如处理器调度，IPC（进程通信）
//    BSD是围绕Machine层的一个外层，提供诸如IPC，文件系统，网络，
//    IOKIT层为设备驱动提供了一个面向对象的一个框架。
//    Mach 本身提供的 API 非常有限，而且苹果也不鼓励使用 Mach 的 API，但是这些API非常基础，如果没有这些API的话，其他任何工作都无法实施。在 Mach 中，所有的东西都是通过自己的对象实现的，进程、线程和虚拟内存都被称为"对象"。和其他架构不同， Mach 的对象间不能直接调用，只能通过消息传递的方式实现对象间的通信。"消息"是 Mach 中最基础的概念，消息在两个端口 (port) 之间传递，这就是 Mach 的 IPC (进程间通信) 的核心。
    
//   NSTimer *timer = [NSTimer scheduledTimerWithTimeInterval:0.5 target:self selector:@selector(print) userInfo:nil repeats:YES];//将timer加入DefaultMode的Mode中
////    NSTimer *timer = [NSTimer timerWithTimeInterval:0.5 target:self selector:@selector(print) userInfo:nil repeats:YES];
//    [[NSRunLoop currentRunLoop]addTimer:timer forMode:UITrackingRunLoopMode];//Timer添加到TrackingModel中
//    
////    NSDefaultRunLoopMode//是RunLoop平时所处的状态，当滚动时，会切换到TrackingRunLoop状态。
//    UIScrollView *scrollView = [[UIScrollView alloc]initWithFrame:CGRectMake(10, 10, 100, 100)];
//    scrollView.contentSize = CGSizeMake(200, 200);
//    scrollView.backgroundColor = [UIColor orangeColor];
//    [self.view addSubview:scrollView];
////    
////    CFRunLoopAddCommonMode(<#CFRunLoopRef rl#>, <#CFStringRef mode#>)
////    UITrackingRunLoopMode
//    UIView *view = [[UIView alloc]initWithFrame:[[UIScreen mainScreen]bounds]];
//    view.backgroundColor = [UIColor clearColor];
//    view.userInteractionEnabled = YES;
//    [self.view addSubview:view];
    NSOperationQueue *operationQueue = [NSOperationQueue mainQueue];
    NSOperationQueue *operationQueue1 = [NSOperationQueue currentQueue];
    NSLog(@"%@,%@",operationQueue,operationQueue1);
    NSLock *lock = [[NSLock alloc]init];
  

}


- (void)print{
    NSLog(@"good");
}
void imp_subMethod1(id self, SEL _cmd){
    
}
- (NSString *)propertyForKey:(NSString *)originalKey
{
    NSDictionary *dict = [NSDictionary dictionary];
    return [dict objectForKey:originalKey];
}
#pragma mark - associatedObject
- (void)setTapActionWithBlock:(void(^)(NSString *))block{
    UITapGestureRecognizer *tapGesture ;//= objc_getAssociatedObject(self, &kDTActionHandlerTapGestureKey);
    if (!tapGesture) {
        tapGesture = [[UITapGestureRecognizer alloc]initWithTarget:self action:@selector(__handleActionForTapGesture:)];
        [self.view addGestureRecognizer:tapGesture];
        objc_setAssociatedObject(self, &kDTActionHandlerTapGestureKey, tapGesture, OBJC_ASSOCIATION_RETAIN);
    }
    objc_setAssociatedObject(self, &kDTActionHandlerTapBlockKey, block, OBJC_ASSOCIATION_COPY);
}
- (void)__handleActionForTapGesture:(UITapGestureRecognizer *) gesture{
    if (gesture.state == UIGestureRecognizerStateRecognized)
        
    {
        
        void(^action)(NSString *) = objc_getAssociatedObject(self, &kDTActionHandlerTapBlockKey);
        if (action)
        {
           action(@"good");
        }
    }
}

#pragma mark - 界面布局方式
- (void)autoLayoutUsage{
    
    self.AView = [[MyView alloc]init];
    [self.view addSubview:self.AView];
#if 0
#pragma mark -
#pragma mark - 使用MAsonry
    
    UIView *superView = self.view;
    UIEdgeInsets padding = UIEdgeInsetsMake(80, 30, 80, 0);
    [self.AView mas_makeConstraints:^(MASConstraintMaker *make) {
        make.left.and.right.equalTo(superView).insets(padding);
        make.top.and.bottom.equalTo(superView).insets(padding);
    }];

    NSString *bundleId = [[NSBundle mainBundle]bundleIdentifier];
    NSLog(@"%@",bundleId);
    
#elif 0
    self.AView.frame = CGRectMake(30, 80, 500, 200);
#elif 0
#pragma mark -
#pragma mark - 使用VFL实现自动布局
    /*
     关闭autoresize布局方式
     */
    [self.AView setTranslatesAutoresizingMaskIntoConstraints:NO];
    
    NSMutableArray *tempConstraints = [NSMutableArray array];
    /*
     创建水平方向的约束:在水平方向,leftButton距离父视图左侧的距离为80，leftButton宽度为60,rightButton和leftButton之间的距离为30，rightButton宽60
     */
    [tempConstraints addObjectsFromArray:[NSLayoutConstraint constraintsWithVisualFormat:@"H:|-[_AView]-|" options:0 metrics:nil views:NSDictionaryOfVariableBindings(_AView)]];
    /*
     创建竖直方向的约束:在竖直方向上,leftButton距离父视图顶部30，leftButton高度30
     */
    [tempConstraints addObjectsFromArray:[NSLayoutConstraint                                           constraintsWithVisualFormat:@"V:|-80-[_AView]-80-|" options:0 metrics:@{@"height":@30} views:NSDictionaryOfVariableBindings(_AView)]];
    //给视图添加约束
    [self.view addConstraints:tempConstraints];
#endif
}
@end


