//
//  ViewController.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/7.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "ViewController.h"
#import "FactoryPattern.h"
#import "Singleton.h"
#import "ClassAdapter.h"
#import "CommandMachine.h"
#import "Student.h"
#import "ConcreteAggregate.h"
#import "Iterator.h"
#import "Original.h"
#import "Memento.h"
#import "Storage.h"

#import "StateContext.h"
#import "StatePattern.h"

#define myReadBufferSize 1 << 5

@interface ViewController (){
    FactoryPatternManager *factorManager;
    Student *student;
}
@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    
    StateContext *context = [StateContext new];
    id<IStateInit> pattern = [StatePattern new];
    [pattern setValue:@"state1"];
    [context addStateName:pattern];
    [context sendMessage];
    
    //create Stream operate from file
//    NSString *fileStr = [[NSBundle mainBundle]pathForResource:@"fileStream" ofType:nil];
//    CFURLRef fileURL = CFURLCreateWithFileSystemPath(kCFAllocatorDefault, (CFStringRef)fileStr, kCFURLPOSIXPathStyle, NO);
//    CFReadStreamRef myReadStream = CFReadStreamCreateWithFile(kCFAllocatorDefault, fileURL);
//    if (!CFReadStreamOpen(myReadStream)) {
//        CFStreamError myErr = CFReadStreamGetError(myReadStream);
//        if (myErr.domain == kCFStreamErrorDomainPOSIX ) {
//            NSLog(@"%d",myErr.error);
//        }else if(myErr.domain == kCFStreamErrorDomainMacOSStatus){
//            NSLog(@"%d",(OSStatus)myErr.error);
//        }
//    }
//    CFIndex numBytesRead;
//    do {
//        NSLog(@"2 << 1:%d",7 << 1);
//        uint8_t buf[myReadBufferSize];// 根据需要//定义myReadBufferSize
//        numBytesRead = CFReadStreamRead(myReadStream, buf, sizeof(buf));//buf 每次读取数据的存放位置，当数据大于buf，while继续执行，
//        if(numBytesRead> 0){
//            //读取到相应的数据
//        }else if(numBytesRead <0){
//            CFStreamError error= CFReadStreamGetError(myReadStream);
//            NSLog(@"errordomain:%ld, error:%d",error.domain,error.error);
//        }
//    } while (numBytesRead > 0);
//    
//    CFReadStreamClose(myReadStream);
//    CFRelease(myReadStream);
//    myReadStream = nil;
    
    //创建一个HTTP请求
//    CFStringRef bodyString = CFSTR(""); //通常用于POST数据
//    CFDataRef data = CFStringCreateExternalRepresentation(kCFAllocatorDefault,
//    (CFStringRef)@"一个绳子",kCFStringEncodingUTF8,0);
//    
//    CFStringRef headerFieldName = CFSTR("X-我喜爱场");
//    CFStringRef headerFieldValue = CFSTR("梦");
//
//    CFStringRef URL = CFSTR("http://www.apple.com");
//    CFURLRef myURL = CFURLCreateWithString(kCFAllocatorDefault,URL,NULL);
//    
//    CFStringRef requestMethod = CFSTR("GET");
//    CFHTTPMessageRef myRequest =
//    CFHTTPMessageCreateRequest(kCFAllocatorDefault,requestMethod, myURL,
//    kCFHTTPVersion1_1);
//    
//    CFDataRef bodyDataExt = CFStringCreateExternalRepresentation(kCFAllocatorDefault,(CFStringRef)@"机身数据",kCFStringEncodingUTF8, 0);
//    CFHTTPMessageSetBody(myRequest,bodyDataExt);
//    CFHTTPMessageSetHeaderFieldValue(myRequest,headerFieldName,headerFieldValue);
//    CFDataRef mySerializedRequest = CFHTTPMessageCopySerializedMessage(myRequest);
//
//    NSString *str = [[NSString alloc]initWithData:(__bridge NSData*)mySerializedRequest encoding:NSUTF8StringEncoding];
//    NSLog(@"%@",str);
    
    
//    CFStringRef URL = CFSTR("http://www.apple.com");
//    CFURLRef myURL = CFURLCreateWithString(kCFAllocatorDefault,URL,NULL);
//    CFStringRef requestMethod = CFSTR("GET");
//    
//    CFHTTPMessageRef myRequest = CFHTTPMessageCreateRequest(kCFAllocatorDefault,
//    requestMethod,myURL,kCFHTTPVersion1_1);
//    
//    CFStringRef headerFieldName = CFSTR("X-我喜爱场");
//    CFStringRef headerFieldValue = CFSTR("梦");
//    CFDataRef bodyDataExt = CFStringCreateExternalRepresentation(kCFAllocatorDefault,(CFStringRef)@"机身数据",kCFStringEncodingUTF8, 0);
//    CFHTTPMessageSetBody(myRequest,bodyDataExt);
//    CFHTTPMessageSetHeaderFieldValue(myRequest,headerFieldName,headerFieldValue);
//    
//    CFReadStreamRef myReadStream = CFReadStreamCreateForHTTPRequest(kCFAllocatorDefault,myRequest);
//    
//    CFReadStreamOpen(myReadStream);
//    
//    CFHTTPMessageRef myResponse =(CFHTTPMessageRef)CFReadStreamCopyProperty(myReadStream,kCFStreamPropertyHTTPResponseHeader);
//    CFStringRef myStatusLine = CFHTTPMessageCopyResponseStatusLine(myResponse);
//    CFIndex myErrCode = CFHTTPMessageGetResponseStatusCode(myResponse);
   
    
    //释放一个HTTP请求
//    CFRelease(myRequest);
//    CFRelease(myURL);
//    CFRelease(URL);
//    CFRelease(mySerializedRequest);
//    myRequest = NULL;
//    mySerializedRequest = NULL;

    
//    student = [[Student alloc]init];
//    [student setValue:@"hua" forKey:@"name"];
//    [student setValue:@(10) forKey:@"height"];
//    
//
//    [student addObserver:self forKeyPath:@"name" options:NSKeyValueObservingOptionNew | NSKeyValueObservingOptionOld context:nil];
//    
////    student.name = @"liu";
//   
//
//    UIButton *btn = [UIButton buttonWithType:UIButtonTypeCustom];
//    btn.frame = CGRectMake(10, 10, 100, 100);
//    btn.backgroundColor = [UIColor colorWithRed:0.65f green:0.65f blue:0.65f alpha:1.0f];
//    [btn addTarget:self action:@selector(btnClick) forControlEvents:UIControlEventTouchUpInside];
//    [self.view addSubview:btn];
// 
//   
//    
//    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(messageSend:) name:@"你好" object:nil];
//    
//     [[NSNotificationCenter defaultCenter] postNotificationName:@"你好" object:self userInfo:@{@"wo":@"good"}];
//    memento(备忘录) sava object state
    
    [self operation];
}

- (void) operation{
    NSArray *objArray = @[@"One",@"Two",@"Three",@"Four",@"Five",@"Six"];
    //创建聚合对象
    id<IConcreteAggregate> agg = [[ConcreteAggregate alloc]initWith:objArray];
    //循环输出聚合对象中的值
    id<IIterator> it = [agg createIterator];
    while(![it isDone]){
       NSLog(@"%@",[it currentItem]);;
//        printf(@"%s",[[it currentItem] UTF8String]);
        [it next];
    }
}

- (void)messageSend:(NSNotification*)post{
    NSLog(@"post:%@",post.userInfo);
}

- (void)btnClick{
     [student setValue:@"liu" forKey:@"name"];
}
- (void)observeValueForKeyPath:(NSString *)keyPath ofObject:(id)object change:(NSDictionary<NSKeyValueChangeKey,id> *)change context:(void *)context{
    NSLog(@"oldname: %@",[change objectForKey:@"old"]);
    NSLog(@"new: %@",[change objectForKey:@"new"]);
    NSLog(@"new: %@",object);
    printf("%s",[[change objectForKey:@"new"]UTF8String]);
    printf("%s",[[change objectForKey:@"old"]UTF8String]);
}

- (void)socket{
/**
 socket usage 
 CFStream的API结构
core Foundation (CFStream, CFSocket),CFNetwork(CFFTP,CFSocketStream,CFHTTP,CFHTTPAuthentication)
 */
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


@end
