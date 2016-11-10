//
//  ViewController.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/24.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ViewController.h"
#import <QuartzCore/QuartzCore.h>
#import "MyQuartzView.h"
#import <CoreGraphics/CoreGraphics.h>
#import "CustomView.h"
#import <GLKit/GLKit.h>
#import "Ap.h"
#import "Bp.h"
#import "TestC++.hpp"

#define LIGHT_DIRECTION 0, 1, -0.5
#define AMBIENT_LIGHT 0.5

@interface ViewController ()<CALayerDelegate,CAAnimationDelegate,CAMediaTiming>{
    UIView *mView;
    bool isAnimation;
}

@property (nonatomic, strong) UIView *containerView;
@property (nonatomic, strong) NSMutableArray< UIView*> *faces;

@property (nonatomic,strong) UIView *layerView;
@property (nonatomic,strong) CALayer *colorLayer;
@property (nonatomic,strong) UIButton *btn;

@property (nonatomic, strong) UIImageView *hourHand;
@property (nonatomic, strong) UIImageView *minuteHand;
@property (nonatomic, strong) UIImageView *secondHand;
@property (nonatomic, strong) NSTimer *timer;

@end

@implementation ViewController
- (void)btnClic{
    CGFloat red = arc4random() / (CGFloat)INT_MAX;
    CGFloat green = arc4random() / (CGFloat)INT_MAX;
    CGFloat blue = arc4random() / (CGFloat)INT_MAX;
    UIColor *color = [UIColor colorWithRed:red green:green blue:blue alpha:1.0];
    //create a basic animation
    CABasicAnimation *colorAnimation = [CABasicAnimation animation];
    colorAnimation.keyPath = @"backgroundColor";
    colorAnimation.toValue = (__bridge id)color.CGColor;
    //apply animation to layer
    [self.colorLayer addAnimation:colorAnimation forKey:nil];

    
    if (isAnimation) {
        return;
    }
    isAnimation = NO;
    CABasicAnimation *animation = [CABasicAnimation animationWithKeyPath:@"position.y"];
    //animation.fromValue = @(M_PI_2);
    //animation.toValue = @(M_PI);
    animation.duration = 2.0f;
    animation.fromValue = @(100);
    animation.toValue = @(500);
    //animation.timingFunction = [CAMediaTimingFunction functionWithName:kCAMediaTimingFunctionEaseInEaseOut];
    animation.autoreverses = YES;
    animation.delegate = self;
    animation.repeatCount = HUGE_VALF;
    animation.beginTime = CACurrentMediaTime();
    //防止动画结束后回到初始状态
    animation.removedOnCompletion = YES;
    animation.fillMode = kCAFillModeForwards;
    [animation setValue:@"animation" forKey:@"AnimationKey"];
    [mView.layer addAnimation:animation forKey:nil];
    
    CABasicAnimation *transformAnima = [CABasicAnimation animationWithKeyPath:@"transform.rotation.y"];
    transformAnima.fromValue = @(0);
    animation.duration = 2.0f;
    transformAnima.toValue = @(M_PI);
    transformAnima.autoreverses = YES;
    transformAnima.repeatCount = HUGE_VALF;
    transformAnima.timingFunction = [CAMediaTimingFunction functionWithName:kCAMediaTimingFunctionEaseInEaseOut];
    [transformAnima setValue:@"transformAnima" forKey:@"AnimationKey"];
    [mView.layer addAnimation:transformAnima forKey:nil];
    
    /*
    CAAnimationGroup *group = [[CAAnimationGroup alloc]init];
    group.duration = 2.0f;
    group.autoreverses = YES;
    group.delegate = self;
    group.repeatCount = HUGE_VALF;
    group.fillMode = kCAFillModeForwards;
    group.removedOnCompletion = YES;
    group.animations = @[animation,transformAnima];
    [group setValue:@"AnimationGroup" forKey:@"AnimationKey"];
    [mView.layer addAnimation:group forKey:nil];//图层属性动画
    */
}
//动画开始时
- (void)animationDidStart:(CAAnimation *)anim
{
    isAnimation = YES;
    if ([[anim valueForKey:@"AnimationKey"]isEqualToString:@"animation"]) {
       NSLog(@"平移动画执行了");
    }else if ([[anim valueForKey:@"AnimationKey"]isEqualToString:@"transformAnima"]){
        NSLog(@"变换动画执行了");
    }else if ([[anim valueForKey:@"AnimationKey"]isEqualToString:@"AnimationGroup"]){
         NSLog(@"动画组执行了");
    }
}

//动画结束时
- (void)animationDidStop:(CAAnimation *)anim finished:(BOOL)flag
{
    //方法中的flag参数表明了动画是自然结束还是被打断,比如调用了removeAnimationForKey:方法或removeAnimationForKey方法，flag为NO，如果是正常结束，flag为YES；
    if ([[anim valueForKey:@"AnimationKey"]isEqualToString:@"animation"]) {
        NSLog(@"平移动画结束");
    }else if ([[anim valueForKey:@"AnimationKey"]isEqualToString:@"transformAnima"]){
        NSLog(@"变换动画结束");
    }else if([[anim valueForKey:@"AnimationKey"]isEqualToString:@"AnimationGroup"]){
        NSLog(@"动画组结束");
    }
    isAnimation = NO;
}

- (void)viewDidLoad
{
    [super viewDidLoad];
    /*
    C++ test
    A obj(2);
    obj.show();
    B bObj(10);
    bObj.show();
    obj.A::show();
     */
    
    //使用CATransaction
    /*
    _containerView = [[UIView alloc]initWithFrame:CGRectMake(300, 300, 300, 300)];
    _containerView.backgroundColor = [UIColor blueColor];
    [self.view addSubview:_containerView];
    */
    UIButton *btn = [[UIButton alloc]initWithFrame:CGRectMake(10, 10, 100, 100)];
    btn.backgroundColor = [UIColor redColor];
    [btn addTarget:self action:@selector(btnClic) forControlEvents:UIControlEventTouchUpInside];
    [self.view addSubview:btn];
    
    mView = [[UIView alloc]initWithFrame:CGRectMake(100, 100, 300, 200)];
    mView.backgroundColor = [UIColor blueColor];
    
    [self.view addSubview:mView];
    
    self.colorLayer = [CALayer layer];
    self.colorLayer.frame = CGRectMake(50.0f, 50.0f, 100.0f, 100.0f);
    self.colorLayer.backgroundColor = [UIColor blueColor].CGColor;
    //add it to our view
    [mView.layer addSublayer:self.colorLayer];
    
    
     /*
    CALayer *shipLayer = [CALayer layer];
    shipLayer.frame = CGRectMake(0, 0, 128, 128);
    shipLayer.position = CGPointMake(150, 150);
    shipLayer.contents = (__bridge id)[UIImage imageNamed: @"211500.jpg"].CGImage;
    [self.containerView.layer addSublayer:shipLayer];
    //animate the ship rotation
    
    CABasicAnimation *animation = [CABasicAnimation animation];
//    animation.keyPath = @"transform.rotation";
    animation.duration = 10.0;
    animation.valueFunction = [CAValueFunction functionWithName:kCAValueFunctionScale];
    animation.toValue = @(0.8);
//    animation.toValue = @(M_PI * 2);
    [shipLayer addAnimation:animation forKey:nil];
    */
//    _hourHand = [[UIImageView alloc]initWithFrame:CGRectMake(100, 100, 10, 50)];
//    _hourHand.backgroundColor = [UIColor redColor];
//    [self.view addSubview:_hourHand];
//    _minuteHand = [[UIImageView alloc]initWithFrame:CGRectMake(100, 100, 10, 50)];
//    _minuteHand.backgroundColor = [UIColor greenColor];
//    [self.view addSubview:_minuteHand];
//    _secondHand = [[UIImageView alloc]initWithFrame:CGRectMake(100, 100, 10, 50)];
//    _secondHand.backgroundColor = [UIColor blueColor];
//    [self.view addSubview:_secondHand];
//    
//    //adjust anchor points
//    self.secondHand.layer.anchorPoint = CGPointMake(0.5f, 0.9f);
//    self.minuteHand.layer.anchorPoint = CGPointMake(0.5f, 0.9f);
//    self.hourHand.layer.anchorPoint = CGPointMake(0.5f, 0.9f);
//    //start timer
//    self.timer = [NSTimer scheduledTimerWithTimeInterval:1.0 target:self selector:@selector(tick) userInfo:nil repeats:YES];
//    //set initial hand positions
//    [self updateHandsAnimated:NO];
}

- (void)tick
{
    [self updateHandsAnimated:YES];
}

- (void)updateHandsAnimated:(BOOL)animated
{
    //convert time to hours, minutes and seconds
    NSCalendar *calendar = [[NSCalendar alloc] initWithCalendarIdentifier:NSGregorianCalendar];
    NSUInteger units = NSHourCalendarUnit | NSMinuteCalendarUnit | NSSecondCalendarUnit;
    NSDateComponents *components = [calendar components:units fromDate:[NSDate date]];
    //calculate hour hand angle
    CGFloat hourAngle = (components.hour / 12.0) * M_PI * 2.0;
    //calculate minute hand angle
    CGFloat minuteAngle = (components.minute / 60.0) * M_PI * 2.0;
    //calculate second hand angle
    CGFloat secondAngle = (components.second / 60.0) * M_PI * 2.0;
    //rotate hands
    [self setAngle:hourAngle forHand:self.hourHand animated:animated];
    [self setAngle:minuteAngle forHand:self.minuteHand animated:animated];
    [self setAngle:secondAngle forHand:self.secondHand animated:animated];
}

- (void)setAngle:(CGFloat)angle forHand:(UIView *)handView animated:(BOOL)animated
{
    //generate transform
    CATransform3D transform = CATransform3DMakeRotation(angle, 0, 0, 1);
    if (animated) {
        //create transform animation
        CABasicAnimation *animation = [CABasicAnimation animation];
        [self updateHandsAnimated:NO];
        animation.keyPath = @"transform";
        animation.toValue = [NSValue valueWithCATransform3D:transform];
        animation.duration = 0.5;
        animation.delegate = self;
        [animation setValue:handView forKey:@"handView"];
        [handView.layer addAnimation:animation forKey:nil];
    } else {
        //set transform directly
        handView.layer.transform = transform;
    }
}

//- (void)changeColor
//{
//    
//    //randomize the layer background color
//    [CATransaction begin];
//    [CATransaction setAnimationDuration:2.0f];
//    CGFloat red = arc4random() / (CGFloat)INT_MAX;
//    CGFloat green = arc4random() / (CGFloat)INT_MAX;
//    CGFloat blue = arc4random() / (CGFloat)INT_MAX;
//    _colorLayer.backgroundColor = [UIColor colorWithRed:red green:green blue:blue alpha:1.0].CGColor;
//    [CATransaction commit];
//}
////- (id<CAAction>)actionForLayer:(CALayer *)layer forKey:(NSString *)event{
////    
////}
//- (void)touchesBegan:(NSSet *)touches withEvent:(UIEvent *)event
//{
//    //get the touch point
//    CGPoint point = [[touches anyObject] locationInView:self.view];
//    //check if we've tapped the moving layer
//    if ([self.colorLayer.presentationLayer hitTest:point]) {
//        //randomize the layer background color
//        CGFloat red = arc4random() / (CGFloat)INT_MAX;
//        CGFloat green = arc4random() / (CGFloat)INT_MAX;
//        CGFloat blue = arc4random() / (CGFloat)INT_MAX;
//        self.colorLayer.backgroundColor = [UIColor colorWithRed:red green:green blue:blue alpha:1.0].CGColor;
//    } else {
//        //otherwise (slowly) move the layer to new position
//        [CATransaction begin];
//        [CATransaction setAnimationDuration:4.0];
//        self.colorLayer.position = point;
//        [CATransaction commit];
//    }
//}
//- (void)viewDidLoad{
//    [super viewDidLoad];
//    self.view.backgroundColor = [UIColor whiteColor];
//    self.navigationController.navigationBarHidden = YES;
//    
//    self.colorLayer = [CALayer layer];
//    self.colorLayer.frame = CGRectMake(0, 0, 100, 100);
//    self.colorLayer.position = CGPointMake(self.view.bounds.size.width / 2, self.view.bounds.size.height / 2);
//    self.colorLayer.backgroundColor = [UIColor redColor].CGColor;
//    [self.view.layer addSublayer:self.colorLayer];
//    
//    self.layerView = [[UIView alloc]initWithFrame:CGRectMake(300, 300, 200, 200)];
//    self.layerView.backgroundColor = [UIColor whiteColor];
//    [self.view addSubview:self.layerView];
////
//    _btn = [[UIButton alloc]initWithFrame:CGRectMake(200, 200, 50, 50)];
//    _btn.backgroundColor = [UIColor redColor];
//    [_btn addTarget:self action:@selector(changeColor) forControlEvents:UIControlEventTouchUpInside];
//    [self.view addSubview:_btn];
////
////    //create sublayer
////    self.colorLayer = [CALayer layer];
////    self.colorLayer.frame = CGRectMake(50.0f, 50.0f, 100.0f, 100.0f);
////    self.colorLayer.backgroundColor = [UIColor blueColor].CGColor;
////    //add a custom action
////    CATransition *transition = [CATransition animation];
////    transition.type = kCATransitionReveal;
////    transition.subtype = kCATransitionFromRight;
////    self.colorLayer.actions = @{@"backgroundColor": transition};
////    //add it to our view
////    [self.layerView.layer addSublayer:self.colorLayer];
//    
//    
//
//    //图层中的MVC架构
//    
//    
//    
//    //    _containerView = [[UIView alloc]initWithFrame:[[UIScreen mainScreen]bounds]];
//    //    [self.view addSubview:_containerView];
//    
//    //    _faces = [NSMutableArray array];
//    //    CustomView *custom = [[CustomView alloc]initWithFrame:[[UIScreen mainScreen]bounds]];
//    //    [self.view addSubview:custom];
//    
//    //    UIBezierPath *path = [UIBezierPath bezierPathWithArcCenter:CGPointMake(60, 60) radius:60 startAngle:0.0f endAngle:M_PI * 3 / 2 clockwise:YES];
//    //    CALayer *blueLayer = [CALayer layer];
//    //    blueLayer.frame = CGRectMake(10, 10, 100, 100);
//    //    blueLayer.position = CGPointMake(100, 100);
//    //    blueLayer.anchorPoint = CGPointMake( 0.25, 0.25);//anchorPoint 位置在图层的position的位置
//    //    blueLayer.backgroundColor = [UIColor whiteColor].CGColor;
//    //    blueLayer.shadowOpacity = 0.5f;
//    //    blueLayer.shadowPath = path.CGPath;
//    //    blueLayer.shadowOffset = CGSizeMake(10, 10);
//    //    blueLayer.shadowColor = [UIColor redColor].CGColor;
//    //    blueLayer.shadowRadius = 10.0f;
//    
//    //    CGAffineTransform transform = CGAffineTransformMake(1, 2, 3, 1, 0, 0);//直接叠加效果
//    //     CGAffineTransformIdentity;//基础矩阵 常量值
//    //    UIImage *myImage1 = [UIImage imageNamed:@"211500.jpg"];
//    //    UIView *myView1 = [[UIView alloc]initWithFrame:CGRectMake(100, 100, 800, 600)];
//    //    myView1.backgroundColor = [UIColor redColor];
//    //    myView1.layer.position = self.view.center;
//    //    myView1.layer.contents = (__bridge id)myImage1.CGImage;
//    //    myView1.layer.contentsScale = [[UIScreen mainScreen]scale];
//    //    myView1.layer.contentsRect = CGRectMake(0, 0, 1.0f, 1.0f);
//    //    myView1.layer.anchorPoint = CGPointMake(0.5f, 0.5f);
//    //    myView1.layer.contentsGravity = kCAGravityCenter;//按照宽高最小值进行填充
//    //    NSLog(@"%@",[myView1.layer hitTest:CGPointMake(150, 150)]);
//    ////    [myView1 setTransform:transform];
//    //    [self.view addSubview:myView1];
//    //    [myView1.layer addSublayer:blueLayer];
//    
//    //    CATransform3D transform = CATransform3DIdentity;
//    //    //apply perspective
//    //    transform.m34 = -1/500; //不能实现视图的缩放，可实现透视投影 m34取值范围在(-1/500,-1/1000)效果最好
//    //    //rotate by 45 degrees along the Y axis
//    //    transform = CATransform3DRotate(transform, M_PI_4, 0, 1, 0);
//    //    //apply to layer
//    //    myView1.layer.transform = transform;
//    
//    //3D绘图中的灭点，(当在透视角度绘图的时候，远离相机视角的物体将会变小变远，当远离到一个极限距离，它们可能就缩成了一个点，于是所有的物体最后都汇聚消失在同一个点。视图的锚点)
//    
//    //    给视图设置寄宿图还可以通过drawRect的方法设置，(当实现drawRect方法的时候会自动设置代理，实现代理方法) 图层本身是有一个代理的， 当需要被重绘时，CALayer会请求它的代理给他一个寄宿图来显示，
//    //    CALayer *newLayer = [CALayer layer];
//    //    newLayer.frame = CGRectMake(0, 0, 100, 100);
//    ////    newLayer.backgroundColor = [UIColor orangeColor].CGColor;
//    //    newLayer.delegate = self;
//    //    newLayer.contentsScale = [[UIScreen mainScreen]scale];
//    //    [self.view.layer addSublayer:newLayer];
//    //    [newLayer display];
//    //
//    //对两个视图做透视投影
//    //    UIView *view1 = [[UIView alloc]initWithFrame:CGRectMake(200, 200, 200, 200)];
//    //    view1.backgroundColor = [UIColor blueColor];
//    //    UIView *view2 = [[UIView alloc]initWithFrame:CGRectMake(600, 200, 200, 200)];
//    //    view2.backgroundColor = [UIColor blueColor];
//    //    [self.view addSubview:view1];
//    //    [self.view addSubview:view2];
//    //    CATransform3D perspective = CATransform3DIdentity;
//    //    perspective.m34 = - 1.0 / 500.0;
//    //    self.view.layer.sublayerTransform = perspective;
//    //    //rotate layerView1 by 45 degrees along the Y axis
//    //    CATransform3D transform1 = CATransform3DMakeRotation(M_PI_4, 0, 1, 0);
//    //    view1.layer.transform = transform1;
//    //    //rotate layerView2 by 45 degrees along the Y axis
//    //    CATransform3D transform2 = CATransform3DMakeRotation(-M_PI_4, 0, 1, 0);
//    //    view2.layer.transform = transform2;
//    //    self.view.layer.doubleSided = NO;
//    //    self.view.layer.sublayerTransform; //它影响到所有的子图层,这意味着你可以一次性对包含这些图层的容器做变换,于是所有的子图层都自动继承了这个变换方法。
//    //这是由于尽管Core Animation图层存在于3D空间之内，但它们并不都存在同一个3D空间。每个图层的3D场景其实是扁平化的，当你从正面观察一个图层，看到的实际上由子图层创建的想象出来的3D场景，但当你倾斜这个图层，你会发现实际上这个3D场景仅仅是被绘制在图层的表面。
//    //在iOS中构建固体对象。
//    
////    for (int i = 0; i < 6; i++) {
////        UIView *view = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 200, 200)];
////        view.backgroundColor = [self randomColor];
////        [self.view addSubview:view];
////        [_faces addObject:view];
////    }
//    
////    CATransform3D perspective = CATransform3DIdentity;
////    perspective.m34 = -1.0 / 500.0;
////    perspective = CATransform3DRotate(perspective, -M_PI_4, 1, 0, 0);
////    perspective = CATransform3DRotate(perspective, -M_PI_4, 0, 1, 0);
////    self.containerView.layer.sublayerTransform = perspective;
////    //add cube face 1
////    CATransform3D transform = CATransform3DMakeTranslation(0, 0, 100);
////    [self addFace:0 withTransform:transform];
////    //add cube face 2
////    transform = CATransform3DMakeTranslation(100, 0, 0);
////    transform = CATransform3DRotate(transform, M_PI_2, 0, 1, 0);
////    [self addFace:1 withTransform:transform];
////    //add cube face 3
////    transform = CATransform3DMakeTranslation(0, -100, 0);
////    transform = CATransform3DRotate(transform, M_PI_2, 1, 0, 0);
////    [self addFace:2 withTransform:transform];
////    //add cube face 4
////    transform = CATransform3DMakeTranslation(0, 100, 0);
////    transform = CATransform3DRotate(transform, -M_PI_2, 1, 0, 0);
////    [self addFace:3 withTransform:transform];
////    //add cube face 5
////    transform = CATransform3DMakeTranslation(-100, 0, 0);
////    transform = CATransform3DRotate(transform, -M_PI_2, 0, 1, 0);
////    [self addFace:4 withTransform:transform];
////    //add cube face 6
////    transform = CATransform3DMakeTranslation(0, 0, -100);
////    transform = CATransform3DRotate(transform, M_PI, 0, 1, 0);
////    [self addFace:5 withTransform:transform];
//}
//- (UIColor *) randomColor
//{
//    CGFloat hue = ( arc4random() % 256 / 256.0 );  //  0.0 to 1.0
//    CGFloat saturation = ( arc4random() % 128 / 256.0 ) + 0.5;  //  0.5 to 1.0, away from white
//    CGFloat brightness = ( arc4random() % 128 / 256.0 ) + 0.5;  //  0.5 to 1.0, away from black
//    return [UIColor colorWithHue:hue saturation:saturation brightness:brightness alpha:1];
//}
//- (void)addFace:(NSInteger)index withTransform:(CATransform3D)transform
//{
//    //get the face view and add it to the container
//    UIView *face = self.faces[index];
//    [self.containerView addSubview:face];
//    //center the face view within the container
//    CGSize containerSize = self.containerView.bounds.size;
//    face.center = CGPointMake(containerSize.width / 2.0, containerSize.height / 2.0);
//    // apply the transform
//    face.layer.transform = transform;
//    //apply lighting
////    [self applyLightingToFace:face.layer];
//}
//
//- (void)applyLightingToFace:(CALayer *)face
//{
//    //add lighting layer
//    CALayer *layer = [CALayer layer];
//    layer.frame = face.bounds;
//    [face addSublayer:layer];
//    //convert the face transform to matrix
//    //(GLKMatrix4 has the same structure as CATransform3D)
//    //译者注：GLKMatrix4和CATransform3D内存结构一致，但坐标类型有长度区别，所以理论上应该做一次float到CGFloat的转换，感谢[@zihuyishi](https://github.com/zihuyishi)同学~
//    CATransform3D transform = face.transform;
//    GLKMatrix4 matrix4 = *(GLKMatrix4 *)&transform;
//    GLKMatrix3 matrix3 = GLKMatrix4GetMatrix3(matrix4);
//    //get face normal
//    GLKVector3 normal = GLKVector3Make(0, 0, 1);
//    normal = GLKMatrix3MultiplyVector3(matrix3, normal);
//    normal = GLKVector3Normalize(normal);
//    //get dot product with light direction
//    GLKVector3 light = GLKVector3Normalize(GLKVector3Make(LIGHT_DIRECTION));
//    float dotProduct = GLKVector3DotProduct(light, normal);
//    //set lighting layer opacity
//    CGFloat shadow = 1 + dotProduct - AMBIENT_LIGHT;
//    UIColor *color = [UIColor colorWithWhite:0 alpha:shadow];
//    layer.backgroundColor = color.CGColor;
//}
//
////- (void)displayLayer:(CALayer *)layer{
//////        默认调用此方法，没有则调用drawLayer:(CALayer *)layer inContext:(CGContextRef)ctx
////}
//
//- (void)drawLayer:(CALayer *)layer inContext:(CGContextRef)ctx{
//    CGContextSetLineWidth(ctx, 1.0f);
//    CGContextSetStrokeColorWithColor(ctx, [UIColor redColor].CGColor);
//    CGContextStrokeEllipseInRect(ctx, layer.bounds);
//}
//
//- (NSInteger)tableView:(UITableView *)tableView numberOfRowsInSection:(NSInteger)section{
//    
//    return 5;
//}
//
//- (UITableViewCell *)tableView:(UITableView *)tableView cellForRowAtIndexPath:(NSIndexPath *)indexPath{
//    UITableViewCell *cell = [tableView cellForRowAtIndexPath:indexPath];
//    if (!cell) {
//        cell = [[UITableViewCell alloc]initWithStyle:UITableViewCellStyleDefault reuseIdentifier:@"cell"];
//    }
//    
//    UIView *view = [[UIView alloc]initWithFrame:CGRectMake(0, 0, cell.contentView.frame.size.width, cell.contentView.frame.size.height)];
//    view.backgroundColor = [UIColor greenColor];
//    [cell.contentView addSubview:view];
//    return cell;
//}
//
//#pragma mark - QuartzGraphicsContext
//
//- (void)quartzGraphicsContext{
//    //    一个Graphics Context表示一个绘制目标
//    //    UIView通过将修改Quartz的Graphics Context的CTM[原点平移到左下角，同时将y轴反转(y值乘以-1)]以使其与UIView匹配。
//    self.navigationController.navigationBarHidden = YES;
//    MyQuartzView *quartzView = [[MyQuartzView alloc]initWithFrame:CGRectMake(20, 20, 300, 300)];
//    
//    quartzView.backgroundColor = [UIColor blueColor];
//    [self.view addSubview:quartzView];
//    
//    //    创建路径
//    //    当在图形上下文中构建一个路径时，需要CGContextBeginPath
//    
//    int end = 0;
//    
//}
//
//
//#pragma mark - QuartzSummary(一)
//
//- (void)quartz2DSummary{
//    //    路径绘制，透明度，描影，绘制阴影，透明层，颜色管理，反锯齿，PDF文档生成，PDF元数据访问。
//    //    Quartz 2D在图像中使用了绘画者模型(painter’s model)offscreen(幕后)
//    //    aLayerOfPaint放置于画布canvas(page);page可以是一张纸(如果输出设备是打印机)，也可以是虚拟的纸张(如果输出设备是PDF文件)，还可以是bitmap图像。这根据实际使用的graphics context而定。
//    //    图形状态
//    //    CGContextSaveGState(<#CGContextRef  _Nullable c#>)保存图形状态
//    //    CGContextRestoreGState(<#CGContextRef  _Nullable c#>)还原图形状态
//    
//    //    Quartz 2D 坐标系统（user-space coordination system）
//    //    Quartz通过使用当前转换矩阵(current transformation matrix， CTM)将一个独立的坐标系统(user space)映射到输出设备的坐标系统(device space)，以此来解决设备依赖问题， CTM是一种特殊类型的矩阵(affine transform, 仿射矩阵)，通过平移(translation)、旋转(rotation)、缩放(scale)操作将点从一个坐标空间映射到另外一个坐标空间。用户空间的坐标为左下角
//    //    在Mac OS X中，重写过isFlipped方法以返回yes的NSView类的子类
//    //    在iOS中，由UIView返回的绘图上下文
//    //    在iOS中，通过调用UIGraphicsBeginImageContextWithOptions函数返回的绘图上下文
//    //    下文的原点做一个平移(移到左上角)和用-1对y坐标值进行缩放(坐标系的转换)
//    //    内存管理：对象所有权
//    //    Quartz使用Core Foundation内存管理模型(引用计数)。所以，对象的创建与销毁与通常的方式是一样的。在Quartz中，需要记住如下一些规则：
//    //
//    //    如果创建或拷贝一个对象，你将拥有它，因此你必须释放它。通常，如果使用含有”Create”或“Copy”单词的函数获取一个对象，当使用完后必须释放，否则将导致内存泄露。
//    //    如果使用不含有”Create”或“Copy”单词的函数获取一个对象，你将不会拥有对象的引用，不需要释放它。
//    //    如果你不拥有一个对象而打算保持它，则必须retain它并且在不需要时release掉。可以使用Quartz 2D的函数来指定retain和release一个对象。例如，如果创建了一个CGColorspace对象，则使用函数CGColorSpaceRetain和CGColorSpaceRelease来retain和release对象。同样，可以使用Core Foundation的CFRetain和CFRelease，但是注意不能传递NULL值给这些函数。
//    //    
//    //    
//    
//}
//
@end
