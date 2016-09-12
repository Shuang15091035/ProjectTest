//
//  CustomView.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/9/12.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "CustomView.h"

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
- (void)drawRect:(CGRect)rect {
    CGContextRef content = UIGraphicsGetCurrentContext();
    CGMutablePathRef path = CGPathCreateMutable();
    CGPathAddLineToPoint(path, <#const CGAffineTransform * _Nullable m#>, <#CGFloat x#>, <#CGFloat y#>)
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

@end
