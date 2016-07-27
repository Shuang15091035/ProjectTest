//
//  ViewController.m
//  SizeClassAndAutoLayout
//
//  Created by mac zdszkj on 16/5/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "Student.h"
#import "ViewController.h"
#import "GDataXMLNode.h"
//#import <QuartzCore/QuartzCore.h>
#define DEF_SCALE 828/1080*DEF_SCREEN_WIDTH/414.0

@interface ViewController ()<NSXMLParserDelegate>{
    UIImageView *imageViewForAnimation;
}
//
//@property(nonatomic) NSMutableArray *dataSource;
//@property(nonatomic) NSString *startTag;
@property(nonatomic,assign) int index;
@property (strong, nonatomic)  UIImageView *iconView;
@property (nonatomic) NSMutableArray *imageArr;
@end

@implementation ViewController

//- (NSMutableArray *)dataSource{
//    if (!_dataSource) {
//        _dataSource = [NSMutableArray array ];
//    }
//    return _dataSource;
//}
- (void)viewDidLoad {
    [super viewDidLoad];
    NSString * str = @"str";
    
//    // Do any additional setup after loading the view, typically from a nib.
//    _imageArr = [NSMutableArray array];
//    for (int i = 0; i < 18; i++) {
//        NSString *imgName = [[NSBundle mainBundle]pathForResource:[NSString stringWithFormat:@"animation%02d@2x",i] ofType:@"png"];
//        UIImage *image = [UIImage imageWithContentsOfFile:imgName];
//        [_imageArr addObject:image];
//    }
//    dispatch_queue_t queue = dispatch_queue_create("gcdTest", DISPATCH_QUEUE_CONCURRENT);
//    dispatch_async(queue, ^{
//        NSLog(@"1");
//    });
//    dispatch_barrier_sync(queue, ^{
//        NSLog(@"2");
//    });
//    利用信号量控制线程的并发执行
//    dispatch_group_t group = dispatch_group_create();
//    dispatch_semaphore_t semaphore = dispatch_semaphore_create(10);
//    dispatch_queue_t queue = dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0);
//    for (int i = 0; i < 100; i++)
//    {
//        dispatch_semaphore_wait(semaphore, DISPATCH_TIME_FOREVER);
//        dispatch_group_async(group, queue, ^{
//            NSLog(@"%i",i);
//            sleep(2);
//            dispatch_semaphore_signal(semaphore);
//        });
//    }
//    dispatch_group_wait(group, DISPATCH_TIME_FOREVER);
//    
//    Student *stu = [[Student alloc]init];
//    Student *s = [stu copy];
//    
//     NSLog(@"%p,%p",stu,s);
//    
//    NSArray *arr = @[@"good",@"ok"];
//    NSArray *ar = [arr copy];
//     NSLog(@"%p,%p",arr,ar);
//    
//    UIImageView *myImageView = [[UIImageView alloc]initWithFrame:CGRectMake(10, 10, 100, 50)];
//    myImageView.image = [UIImage imageWithContentsOfFile:[[NSBundle mainBundle]pathForResource:[NSString stringWithFormat:@"animation00@2x"] ofType:@"png"]];
//    [self.view addSubview:myImageView];
//    
//    CAKeyframeAnimation *animation = [CAKeyframeAnimation animationWithKeyPath:@"position"];
//    CGMutablePathRef path = CGPathCreateMutable();
//    CGPathMoveToPoint(path, NULL, 300, 0);
//    CGPathAddQuadCurveToPoint(path, NULL, 300, 300, 0, 300);
//    CGPathRelease(path);
//    animation.path = path;
//    animation.fillMode = kCAFillModeForwards;
//    animation.removedOnCompletion = NO;
//    animation.duration = 5.0f;
//    
////    myImageView.animationImages = _imageArr;
////    myImageView.animationDuration = 3.0;
////    [myImageView startAnimating];
//    [myImageView.layer addAnimation:animation forKey:nil];
////    导航控制的动画通过CATransition
//    
//    CABasicAnimation *basicAnimation = [CABasicAnimation animationWithKeyPath:@"transform"];
//    basicAnimation.duration = 3.0f;
    
////    NSXMLParser *parser =[[NSXMLParser alloc]initWithData:[NSData dataWithContentsOfFile:[[NSBundle mainBundle] pathForResource:@"XML" ofType:nil]]];
////    parser.delegate = self;
////    [parser parse];
//    NSData *data = [NSData dataWithContentsOfFile:[[NSBundle mainBundle] pathForResource:@"XML" ofType:nil]];
//    GDataXMLDocument *doc = [[GDataXMLDocument alloc]initWithData:data options:0 error:nil];
//    GDataXMLElement *element = [doc rootElement];
//    NSArray *arr = [element elementsForName:@"user"];
//    for (GDataXMLElement *ele in arr) {
//        GDataXMLElement *nameElement = [[ele elementsForName:@"key"] objectAtIndex:0];
//        NSString *name = [nameElement stringValue];
//        NSLog(@"User name is:%@",name);
//        GDataXMLElement *ageElement = [[ele elementsForName:@"string"] objectAtIndex:0];
//        NSString *age = [ageElement stringValue];
//        NSLog(@"User name is:%@",age);
//    }
    
//    UIImage *image1 = [UIImage imageNamed:@"1.jpg"];
//   _iconView = [[UIImageView alloc]init];
//    _iconView.center = self.view.center;
//    [self.view addSubview:_iconView];
    
//    CATransition *transition = [CATransition animation];
//    transition.type = @"cube";
//    transition.subtype = kCATransitionFromRight;
//    transition.duration = 2.0;
//    transition.startProgress = 0.5;
//    [_iconView.layer addAnimation:transition forKey:@"transitionAnimation"];
//    
//    UIImage *image = [UIImage imageNamed:@"1.jpg"];
//    imageViewForAnimation = [[UIImageView alloc] initWithImage:image];
//    imageViewForAnimation.alpha = 1.0f;
//    CGRect imageFrame = imageViewForAnimation.frame;
    //frame.origin，动画开始的地方
//    CGPoint viewOrigin = imageViewForAnimation.frame.origin;
//    viewOrigin.y = viewOrigin.y + imageFrame.size.height / 2.0f;
//    viewOrigin.x = viewOrigin.x + imageFrame.size.width / 2.0f;
//    
//    imageViewForAnimation.frame = imageFrame;
//    imageViewForAnimation.layer.position = CGPointMake(0, 0);
//    [self.view addSubview:imageViewForAnimation];
    
//    // 淡出效果
//    CABasicAnimation *fadeOutAnimation = [CABasicAnimation animationWithKeyPath:@"opacity"];
//    [fadeOutAnimation setToValue:[NSNumber numberWithFloat:0.3]];
//    fadeOutAnimation.fillMode = kCAFillModeForwards;
//    fadeOutAnimation.removedOnCompletion = NO;
//    
//    //设置缩放
//    CABasicAnimation *resizeAnimation = [CABasicAnimation animationWithKeyPath:@"bounds.size"];
//    [resizeAnimation setToValue:[NSValue valueWithCGSize:CGSizeMake(40.0f, imageFrame.size.height * (40.0f / imageFrame.size.width))]];
//    resizeAnimation.fillMode = kCAFillModeForwards;
//    resizeAnimation.removedOnCompletion = NO;
    
//    // 设置路径运动
//    CAKeyframeAnimation *pathAnimation = [CAKeyframeAnimation animationWithKeyPath:@"position"];
//    pathAnimation.calculationMode = kCAAnimationPaced;
//    pathAnimation.fillMode = kCAFillModeForwards;
//    pathAnimation.duration = 3.0f;
//    pathAnimation.repeatCount = MAXFLOAT;
//    pathAnimation.removedOnCompletion = NO;
////    //设置动画结束点
////    CGPoint endPoint = CGPointMake(480.0f - 30.0f, 40.0f);
////    //在最后一个选项卡结束动画
////    //CGPoint endPoint = CGPointMake( 320-40.0f, 480.0f);
////    CGMutablePathRef curvedPath = CGPathCreateMutable();
////    CGPathMoveToPoint(curvedPath, NULL, viewOrigin.x, viewOrigin.y);
////    CGPathAddCurveToPoint(curvedPath, NULL, endPoint.x, viewOrigin.y, endPoint.x, viewOrigin.y, endPoint.x, endPoint.y);
////    pathAnimation.path = curvedPath;
////    CGPathRelease(curvedPath);
//    CGMutablePathRef path = CGPathCreateMutable();
//    CGPathMoveToPoint(path, NULL, 300, 0);
//    CGPathAddQuadCurveToPoint(path, NULL, 300, 300, 0, 300);
//
////    CGPathAddArcToPoint(path, NULL, 300, 0, 0, 300, 300);
//    pathAnimation.path = path;
//    CGPathRelease(path);
//    
//    CAAnimationGroup *group = [CAAnimationGroup animation];
////    group.fillMode = kCAFillModeForwards;
////    group.removedOnCompletion = NO;
//////    [group setAnimations:[NSArray arrayWithObjects:fadeOutAnimation, pathAnimation, resizeAnimation, nil]];
////    [group setAnimations:[NSArray arrayWithObjects: pathAnimation, nil]];
////    group.duration = 3.0f;
//    group.delegate = self;
////    [group setValue:imageViewForAnimation forKey:@"imageViewBeingAnimated"];
////    
//    [imageViewForAnimation.layer addAnimation:pathAnimation forKey:@"savingAnimation"];
    
//    
}
//- (void)touchesBegan:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
////    [imageViewForAnimation.layer removeAnimationForKey:@"savingAnimation"];
//    UIImage *image1 = [UIImage imageNamed:@"1.jpg"];
//    UIImageView *view = [[UIImageView alloc]initWithImage:image1];
//    view.center = self.view.center;
//    [self.view addSubview:view];
//    
//    CATransition *transition = [CATransition animation];
//    transition.type = @"cube";
//    transition.subtype = kCATransitionFromRight;
//    transition.duration = 2.0;
//    transition.startProgress = 0.5;
//    [view.layer addAnimation:transition forKey:@"transitionAnimation"];
//

//}

//- (NSInteger)getRightResultParmera:(NSInteger)parmera parmera2:(NSInteger)parmera2{
//    NSInteger result;
//    for (int i = 0; i < 10; i++) {
//        result += (parmera + parmera2);
//    }
//    return result;
//}
//
//- (void)preOnClick:(UIButton *)sender {
//        self.index--;
//      if (self.index<1) {
//             self.index=2;
//     }
//     self.iconView.image=[UIImage imageNamed: [NSString stringWithFormat:@"%d.jpg",self.index]];
//  
//     //创建核心动画
//       CATransition *ca=[CATransition animation];
//     //告诉要执行什么动画
//     //设置过度效果
//       ca.type=@"cube";
//       //设置动画的过度方向（向左）
//ca.subtype=kCATransitionFromLeft;
//       //设置动画的时间
//     ca.duration=2.0;
//     //添加动画
//      [self.iconView.layer addAnimation:ca forKey:nil];
// }
//
// //下一张
// - (void)nextOnClick:(UIButton *)sender {
//    self.index++;
//       if (self.index>1) {
//               self.index=1;
//         }
//          self.iconView.image=[UIImage imageNamed: [NSString stringWithFormat:@"%d.jpg",self.index]];
// 
//      //1.创建核心动画
//      CATransition *ca=[CATransition animation];
// 
//     //1.1告诉要执行什么动画
//    //1.2设置过度效果
//       ca.type=@"cube";
//        //1.3设置动画的过度方向（向右）
//     ca.subtype=kCATransitionFromRight;
//        //1.4设置动画的时间
//       ca.duration=2.0;
//     //1.5设置动画的起点
//       ca.startProgress=0.5;
//       //1.6设置动画的终点
//   //    ca.endProgress=0.5;
//     
//      //2.添加动画
//       [self.iconView.layer addAnimation:ca forKey:nil];
//   }
//- (void)animationDidStart:(CAAnimation *)anim{
//    
//}
//- (void)animationDidStop:(CAAnimation *)anim finished:(BOOL)flag{
//    
//}
//#pragma mark - NSXMLParserDelegate中的方法
//- (void)parserDidStartDocument:(NSXMLParser *)parser{
//    
//}
//- (void)parserDidEndDocument:(NSXMLParser *)parser{
//    
//}
//
//- (void)parser:(NSXMLParser *)parser didStartElement:(NSString *)elementName namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName attributes:(NSDictionary<NSString *,NSString *> *)attributeDict{
//    
//    NSLog(@"%s\n<%@>",__func__,elementName);
//    self.startTag = elementName;
//    if ([elementName isEqualToString:@"dic"]) {
//        Student *stu = [[Student alloc]init];
//        [self.dataSource addObject:stu];
//    }
//    
//}
//- (void)parser:(NSXMLParser *)parser didEndElement:(NSString *)elementName namespaceURI:(NSString *)namespaceURI qualifiedName:(NSString *)qName{
//    self.startTag = nil;
//    
//}
//- (void)parser:(NSXMLParser *)parser foundCharacters:(NSString *)string{
//    NSLog(@"%s--%@",__func__,string);
//    Student *stu = [self.dataSource lastObject];
//    if ([self.startTag isEqualToString:@"key"]) {
//        stu.name = string;
//    }else if ([self.startTag isEqualToString:@"string"]) {
//        stu.age = string;
//    }
//}
//
//- (void)didReceiveMemoryWarning {
//    [super didReceiveMemoryWarning];
//    // Dispose of any resources that can be recreated.
//    
//}

@end

