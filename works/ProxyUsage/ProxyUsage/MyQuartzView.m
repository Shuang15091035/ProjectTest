//
//  MyQuartzView.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/31.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MyQuartzView.h"
#import <UIKit/UIKit.h>

@implementation MyQuartzView

- (instancetype)initWithFrame:(CGRect)frame{
   self = [super initWithFrame:frame];
    return self;
}

- (void)drawRect:(CGRect)rect {
    
    CGContextRef mycontenxt = UIGraphicsGetCurrentContext();

//    CGContextMoveToPoint(<#CGContextRef  _Nullable c#>, <#CGFloat x#>, <#CGFloat y#>)设置起始点
    
   const CGPoint points[] = {
        CGPointMake(5, 5),
        CGPointMake(10, 5),
        CGPointMake(20, 30),
    };
    
    CGContextAddLines(mycontenxt, points, sizeof(points)/sizeof(points[0]));//根据点数组画线
    CGContextSetLineWidth(mycontenxt, 2.0f);//设置线宽
    CGContextSetStrokeColorWithColor(mycontenxt, [UIColor redColor].CGColor);

//    CGContextAddLineToPoint(mycontenxt, 40, 60);
    CGContextAddArc(mycontenxt, 80, 60, 10, - M_PI_4, M_PI_4, YES);//clockwise(顺时针)//angle弧度制,从endAngleTOstarAngle//弧度
//    CGContextClearRect(mycontenxt, rect);//清除alpha通道
    CGContextStrokePath(mycontenxt);
//    以度数表示的角度，把数字乘以π/180便转换成弧度；以弧度表示的角度，乘以180/π便转换成度数.
//    CGContextAddArcToPoint(<#CGContextRef  _Nullable c#>, <#CGFloat x1#>, <#CGFloat y1#>, <#CGFloat x2#>, <#CGFloat y2#>, <#CGFloat radius#>)根据两个点创建两个正切线，//弧度
    
    //    立方体(cubic)curve(弯曲线)quad
//    CGContextAddQuadCurveToPoint(<#CGContextRef  _Nullable c#>, <#CGFloat cpx#>, <#CGFloat cpy#>, <#CGFloat x#>, <#CGFloat y#>
//    创建一个四边形
//    英  [kwɒd]四 ellipse(椭圆)[ɪ'lɪps]
//    CGContextAddEllipseInRect(<#CGContextRef  _Nullable c#>, <#CGRect rect#>) 绘制一个椭圆

//    CGContextClosePath(<#CGContextRef  _Nullable c#>) //设置路径闭合
    
    
    /*!!!专门的路径创建*/
    
//    绘制完路径后，将清空图形上下文，特别是在绘制复杂场景时，我们需要反复使用。基于此，Quartz提供了两个数据类型来创建可复用路径—CGPathRef和CGMutablePathRef。Quartz提供了一个类似于操作图形上下文的CGPath的函数集合。这些路径函数操作CGPath对象，而不是图形上下文
//    CGPathCreateMutable，取代CGContextBeginPath
//    CGPathMoveToPoint，取代CGContextMoveToPoint
//    CGPathAddLineToPoint，取代CGContexAddLineToPoint
//    CGPathAddCurveToPoint，取代CGContexAddCurveToPoint
//    CGPathAddEllipseInRect，取代CGContexAddEllipseInRect
//    CGPathAddArc，取代CGContexAddArc
//    CGPathAddRect，取代CGContexAddRect
//    CGPathCloseSubpath，取代CGContexClosePat
//    将路径保存到上下文中通过  CGContextAddPath(<#CGContextRef  _Nullable c#>, <#CGPathRef  _Nullable path#>)
    /*???绘制路径*/
//    可以绘制填充或者描边的路径，影响描边的属性
//    lineWide lineJoin lineCap,MiterLimit LineDashPattern StrokeColorSpace StrokeColor StrokePattern
    
//    dashLine(虚线样式)
//    CGContextSetBlendMode(<#CGContextRef  _Nullable c#>, <#CGBlendMode mode#>)
    //    kCGBlendModeNormal, //result = (alpha * foreground) + (1 - alpha) *background该模式使用如下公式来计算前景绘图与背景绘图如何混合
//    kCGBlendModeMultiply,//正片叠底
//    kCGBlendModeScreen,//屏幕
//    kCGBlendModeOverlay,//叠加
//    kCGBlendModeDarken,//暗化
//    kCGBlendModeLighten,//亮化
//    kCGBlendModeColorDodge,//色彩减淡
//    kCGBlendModeColorBurn,//色彩加深
//    kCGBlendModeSoftLight,//柔光
//    kCGBlendModeHardLight,//强光
//    kCGBlendModeDifference,//差值
//    kCGBlendModeExclusion,//排除
//    kCGBlendModeHue,//色相
//    kCGBlendModeSaturation,//饱和度
//    kCGBlendModeColor,//颜色
//    kCGBlendModeLuminosity,//亮度

    
#pragma mark - 颜色和颜色空间
//    colorSpace (RGB ,HSB,CMYK,BGR);
    
    
#pragma mark - 修改颜色空间
//    在绘制图像操作CTM来旋转缩放或平移page(CTM有四中操作，平移，旋转，缩放，联结);
//    CGContextDrawImage(mycontenxt, CGRectMake(100, 100, 60, 60), <#CGImageRef  _Nullable image#>)
//    CGContextScaleCTM(<#CGContextRef  _Nullable c#>, <#CGFloat sx#>, <#CGFloat sy#>)缩放CGImage
//    CGContextTranslateCTM(<#CGContextRef  _Nullable c#>, <#CGFloat tx#>, <#CGFloat ty#>)
//    CGContextRotateCTM(<#CGContextRef  _Nullable c#>, <#CGFloat angle#>)
//    CGContextConcatCTM(<#CGContextRef  _Nullable c#>, <#CGAffineTransform transform#>)//联结，通过调用CGContextConcatCTM来联接CTM和仿射矩阵。
//    仿射变换操作在矩阵上，而不是在CTM上。仿射变换函数能实现与CTM函数相同的操作–平移、旋转、缩放、联合
//    Quartz同样提供了一个仿射变换函数(CGAffineTransformInvert)来倒置矩阵。倒置操作通常用于在变换对象中提供点的倒置变换。当我们需要恢复一个被矩阵变换的值时，可以使用倒置操作。将值与倒置矩阵相乘，就可得到原先的值。我们通常不需要倒置操作，因为我们可以通过保存和恢复图形状态来倒置CTM的效果。
//    
//    CGPointApplyAffineTransform(<#CGPoint point#>, <#CGAffineTransform t#>)仅仅需要改变一个点或一个大小
//    CGSizeApplyAffineTransform(<#CGSize size#>, <#CGAffineTransform t#>)
//    CGRectApplyAffineTransform(<#CGRect rect#>, <#CGAffineTransform t#>)
//    CGAffineTransformMake(<#CGFloat a#>, <#CGFloat b#>, <#CGFloat c#>, <#CGFloat d#>, <#CGFloat tx#>, <#CGFloat ty#>)
//    a  b  0
//    c  d  0
//    tx ty 1
    
#pragma mark - stencil patterns(模板模式),color patterns(颜色模式)
//    模板模式是非着色模式、甚至可以作为图像蒙版。
//    tilling(平铺)将模式单元格绘制到页面的某个部分的过程。在用户空间定义的模式单元格在渲染到设备时可能无法精确匹配，这是用户空间单元和设备像素之间的差异造成的。
//    Quratz有三个平铺选项，在必要时调整模式
//    1.没有失真(no distortion): 以细微调整模式单元格之间的间距为代价，但通常不超过一个设备像素。
//    2.最小的失真的恒定间距：设定单元格之间的间距，以细微调整单元大小为代价，但通常不超过一个设备像素。
//    3.恒定间距：设定单元格之间间距，以调整单元格大小为代价，以求尽快的平铺
    
//    CGContextSetFillColor(CGContextRef  _Nullable c, <#const CGFloat * _Nullable components#>)
//    CGContextFillRect(<#CGContextRef  _Nullable c#>, <#CGRect rect#>)
//    CGContextSetFillPattern(<#CGContextRef  _Nullable c#>, <#CGPatternRef  _Nullable pattern#>, <#const CGFloat * _Nullable components#>)
    
//    这里有个例子说明Quartz在幕后是如何绘制一个模式的。当我们填充或描边一个模式时，Quartz会按照以下指令来绘制每一个模式单元格：
//    
//    保存图形状态
//    将当前转换矩阵应用到原始的模式单元格上
//    连接CTM与模式矩阵
//    裁剪模式单元格的边界矩形
//    调用绘制回调函数来绘制单元格
//    恢复图形状态
    
    
    
}



@end
