//
//  TimerUsage.m
//  ModuleArrangement
//
//  Created by mac zdszkj on 2017/3/30.
//  Copyright © 2017年 PersonLiu. All rights reserved.
//

#import "TimerUsage.h"
#import "NSObject+performSelector.h"

@interface TimerUsage(){
    NSTimer *mCurrentTimer;
}

@end

@implementation TimerUsage

- (void)viewDidLoad{
    [super viewDidLoad];
    [self setupDefault];
    //timer 必须加入到runLoop中才会被执行，timer作为一个事件，在多线程文档中，事件的执行必须加入到runloop中
    //1.0
//    [self timerInitImplement];
    //2.0
//    [self timerBlockImplement];
    //3.0
    [self timerInvokeImplement];
}

- (void)setupDefault{
    // 添加检测app进入后台的观察者
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(applicationEnterBackground) name: UIApplicationDidEnterBackgroundNotification object:nil];
}

- (void)applicationEnterBackground{
    NSLog(@"enterBackgroundNotification");
    
    if([mCurrentTimer isValid]){
        [mCurrentTimer invalidate];//运行循环中移除
        mCurrentTimer = nil;
    }
    
}

- (void)timerInitImplement{
    //fireData:设置为未启动，interval:设置为执行时间，userInfo:可从定时器中获取到，不会立即执行，等到执行时间才会执行
    mCurrentTimer = [[NSTimer alloc]initWithFireDate:[NSDate distantFuture] interval:2.0 target:self selector:@selector(timerInvoke) userInfo:@{@"userInfo":@"userInfo"} repeats:YES];
    //加入到runLoop中
    [[NSRunLoop mainRunLoop]addTimer:mCurrentTimer forMode:NSDefaultRunLoopMode];
    //启动定时器[NSDate distantPast]
    [mCurrentTimer setFireDate:[NSDate distantPast]];
    //此方法会让定时器立即执行，但是不影响，interval
    [mCurrentTimer fire];
}

- (void)timerBlockImplement{
    //此方法将timer 加入到默认的RunLoop中,可避免循环引用问题
    mCurrentTimer = [NSTimer scheduledTimerWithTimeInterval:2.0 repeats:YES block:^(NSTimer * _Nonnull timer) {
        [self timerInvoke];
    }];

}

- (void)timerInvokeImplement{
    SEL selector = @selector(timerInvoke);
    NSMethodSignature *signature = [TimerUsage instanceMethodSignatureForSelector:selector];
    NSInvocation *invocation = [NSInvocation invocationWithMethodSignature:signature];
    invocation.target = self;
    [invocation setSelector:selector];
    //index 设置必须从第二位开始设置，应为，0:self,1:_cmd
//    [invocation setArgument:@"value" atIndex:2];
    mCurrentTimer = [NSTimer timerWithTimeInterval:2.0 invocation:invocation repeats:YES];
    [[NSRunLoop mainRunLoop]addTimer:mCurrentTimer forMode:NSDefaultRunLoopMode];
    //invoke使用参考 NSObject+performSelector，不适用对象执行实例方法，两种方式，perform和invoke
}

- (void)timerInvoke{
    NSLog(@"%s",__func__);
}

- (void)dealloc{
    NSLog(@"%s",__func__);
}
@end




