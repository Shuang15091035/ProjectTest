//
//  CustomView.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/9/12.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "CustomView.h"
#import "DALine.h"

@interface CustomView(){
    CGPoint _touchPoint;
    NSMutableArray *_pointsArr;
    CGFloat lastScale;
}

@end

@implementation CustomView

- (instancetype)init{
    self = [super init];
    if (self) {
        [self pinchGestureOperate];
    }
    return self;
}

- (instancetype)initWithFrame:(CGRect)frame{
    self = [super initWithFrame:frame];
    if (self) {
        _pointsArr = [NSMutableArray array];
        [self pinchGestureOperate];
    }
    return self;
}
- (instancetype)initWithCoder:(NSCoder *)aDecoder{
    self = [super initWithCoder:aDecoder];
    if(self){
        _pointsArr = [NSMutableArray array];
    }
    
    return self;
}

- (void)touchesBegan:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
    CGPoint currentPoint = [[touches anyObject] locationInView:self];
    _touchPoint = currentPoint;
    NSString *point = NSStringFromCGPoint(_touchPoint);
    
    [_pointsArr addObject:point];
}
- (void)touchesMoved:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
    
}
-(void)touchesEnded:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
    
    
}

- (void)drawRect:(CGRect)rect {
    
    CGContextRef currentContent = UIGraphicsGetCurrentContext();
    
    //画三角形
//    CGContextSaveGState(currentContent);
    CGContextSetLineCap(currentContent, kCGLineCapRound);  //边缘样式
    CGContextSetAllowsAntialiasing(currentContent, YES);
    CGContextSetRGBStrokeColor(currentContent, 1.0, 0.0, 0.0, 1.0);  //颜色
    CGContextSetFillColorWithColor(currentContent, [UIColor colorWithRed:0 green:1 blue:0 alpha:1].CGColor);
    CGPoint sPoint[3];
    sPoint[0] = CGPointMake(100, 100);
    sPoint[1] = CGPointMake(200, 200);
    sPoint[2] = CGPointMake(500, 100);
    UIView *view1 = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 20, 20)];
    view1.backgroundColor = [UIColor redColor];
    view1.center = sPoint[0];
    [self addSubview:view1];
    
    UIView *view2 = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 20, 20)];
    view2.backgroundColor = [UIColor redColor];
    view2.center = sPoint[1];
    [self addSubview:view2];
    DALine *line = [DALine new];
    line.point1 = sPoint[0];
    line.point2 = sPoint[1];
    CGFloat dis = [line getdistancePointAndPoint];
    NSLog(@"dis:%f",dis);
    CGPoint point = [line getIntersectionOfLinerAndCircularRadiu:50 ];
    NSLog(@"%@",NSStringFromCGPoint(point));
    UIView *view = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 20, 20)];
    view.backgroundColor = [UIColor redColor];
    view.center = point;
    [self addSubview:view];
    CGContextAddLines(currentContent, sPoint, 3);
    CGContextClosePath(currentContent);
    CGContextDrawPath(currentContent, kCGPathFillStroke); //根据坐标绘制路径
//    CGContextRestoreGState(currentContent);
    
//    画矩形
//     CGContextSaveGState(currentContent);
       CGFloat myFillColorBlue[] = {0,0,1,1}; //red;
        CGContextAddRect(currentContent, CGRectMake(300, 300, 200, 200));
        CGContextSetFillColor(currentContent, myFillColorBlue);
        CGContextFillPath(currentContent);
    self.backgroundColor = [UIColor redColor];
//    CGContextRestoreGState(currentContent);
    
    
    CGContextSetLineCap(currentContent, kCGLineCapRound);  //边缘样式
    CGContextSetAllowsAntialiasing(currentContent, YES);
    CGContextSetRGBStrokeColor(currentContent, 1.0, 0.0, 0.0, 1.0);  //颜色
    CGContextSetFillColorWithColor(currentContent, [UIColor colorWithRed:0 green:1 blue:0 alpha:1].CGColor);
    CGPoint Point[3];
    Point[0] = CGPointMake(500, 100);
    Point[1] = CGPointMake(500, 200);
    Point[2] = CGPointMake(600, 100);
    UIView *view3 = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 20, 20)];
    view3.backgroundColor = [UIColor redColor];
    view3.center = Point[0];
    [self addSubview:view3];
    
    UIView *view4 = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 20, 20)];
    view4.backgroundColor = [UIColor redColor];
    view4.center = Point[1];
    [self addSubview:view4];
    DALine *line2 = [DALine new];
    line2.point1 = sPoint[0];
    line2.point2 = sPoint[1];
    CGFloat dis1 = [line getdistancePointAndPoint];
    NSLog(@"dis:%f",dis1);
    CGPoint point1 = [line getIntersectionOfLinerAndCircularRadiu:50 ];
    NSLog(@"%@",NSStringFromCGPoint(point1));
    UIView *view5 = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 20, 20)];
    view5.backgroundColor = [UIColor redColor];
    view5.center = point;
    [self addSubview:view5];
    CGContextAddLines(currentContent, Point, 3);
    CGContextClosePath(currentContent);
    CGContextDrawPath(currentContent, kCGPathFillStroke); //根据坐标绘制路径
    
    CGContextSetRGBFillColor(currentContent, 0.0f, 1.0f, 0.0f, 1.0f);
    CGContextSetRGBStrokeColor(currentContent, 0.0f, 0.0f, 1.0f, 1.0f);
    CGContextSetLineWidth(currentContent, 10.0f);
    UIBezierPath *path = [UIBezierPath bezierPathWithArcCenter:CGPointMake(60, 60) radius:50 startAngle:0.0f endAngle:M_PI * 3 / 2 clockwise:YES];
    CGContextAddPath(currentContent, path.CGPath);
    CGContextDrawPath(currentContent, kCGPathFillStroke);
    
    
    
}

- (void)pinchGestureOperate{
    UIPinchGestureRecognizer *pinchGesture = [[UIPinchGestureRecognizer alloc]initWithTarget:self action:@selector(pinchGesture:)];
    [self addGestureRecognizer:pinchGesture];
}

- (void)pinchGesture:(UIPinchGestureRecognizer *)gesturePinch{
    
    if([gesturePinch state] == UIGestureRecognizerStateBegan) {
        lastScale = 1.0;
    }
    if ([gesturePinch state] == UIGestureRecognizerStateChanged) {
        CGFloat scale = 1.0 - (lastScale - [gesturePinch scale]);
        CATransform3D transform3D = CATransform3DScale(self.layer.transform,scale, scale, 1);
        [self.layer setTransform:transform3D];
        [self setNeedsDisplay];
        lastScale = [gesturePinch scale];
    }
    if([gesturePinch state] == UIGestureRecognizerStateEnded) {
        self.bounds = [[UIScreen mainScreen]bounds];
    }
}

@end
