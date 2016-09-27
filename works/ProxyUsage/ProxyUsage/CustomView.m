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
}

@end

@implementation CustomView

- (instancetype)initWithFrame:(CGRect)frame{
    self = [super initWithFrame:frame];
    if (self) {
        _pointsArr = [NSMutableArray array];
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
    CGContextSaveGState(currentContent);
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
    CGContextRestoreGState(currentContent);
    
    //画矩形
    //   CGFloat myFillColorBlue[] = {0,0,1,1}; //red;
    //    CGContextAddRect(currentContent, CGRectMake(100, 100, 200, 200));
    //    CGContextSetFillColor(currentContent, myFillColorBlue);
    //    CGContextFillPath(currentContent);
    
}

@end
