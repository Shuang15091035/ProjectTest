//
//  MyView.m
//  Tool
//
//  Created by mac zdszkj on 16/3/18.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MyView.h"
#import <CoreGraphics/CoreGraphics.h>

@implementation MyView

- (instancetype)init{
    self = [super init];
    if (self) {
        self.backgroundColor = [UIColor redColor];
    }
    return self;
}
- (void)drawRect:(CGRect)rect {
    NSLog(@"rect:%@,x:%f,y:%f",NSStringFromCGRect(rect),self.center.x,self.center.y);
    [self drawCircleAtX:rect.size.width/2 Y:rect.size.height/2];
}

- (void)drawCircleAtX:(float)x Y:(float)y {
    
//    // 获取当前图形，视图推入堆栈的图形，相当于你所要绘制图形的图纸
//    CGContextRef ctx = UIGraphicsGetCurrentContext();
//    
//    // 创建一个新的空图形路径。
//    CGContextSetFillColorWithColor(ctx, [UIColor orangeColor].CGColor);
//    
//    /**
//     *  @brief 在当前路径添加圆弧 参数按顺序说明
//     *
//     *  @param c           当前图形
//     *  @param x           圆弧的中心点坐标x
//     *  @param y           曲线控制点的y坐标
//     *  @param radius      指定点的x坐标值
//     *  @param startAngle  弧的起点与正X轴的夹角，
//     *  @param endAngle    弧的终点与正X轴的夹角
//     *  @param clockwise   指定1创建一个顺时针的圆弧，或是指定0创建一个逆时针圆弧
//     *
//     */
//    CGContextAddArc(ctx, x, y, 50, 0, 2 * M_PI, 0);
//    //绘制当前路径区域
//    CGContextFillPath(ctx);

}
@end
