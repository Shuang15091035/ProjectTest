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


@interface ViewController (){
    
}


@end

@implementation ViewController
- (void)viewDidLoad{
    [super viewDidLoad];
    self.navigationController.navigationBarHidden = YES;
    CustomView *view = [[CustomView alloc]initWithFrame:[[UIScreen mainScreen]bounds]];
    [self.view addSubview:view];

    
    
}
- (void)getScreenPoint{

    
    
}

#pragma mark - QuartzGraphicsContext

- (void)quartzGraphicsContext{
//    一个Graphics Context表示一个绘制目标
//    UIView通过将修改Quartz的Graphics Context的CTM[原点平移到左下角，同时将y轴反转(y值乘以-1)]以使其与UIView匹配。
    self.navigationController.navigationBarHidden = YES;
    MyQuartzView *quartzView = [[MyQuartzView alloc]initWithFrame:CGRectMake(20, 20, 300, 300)];
    
    quartzView.backgroundColor = [UIColor blueColor];
    [self.view addSubview:quartzView];
    
//    创建路径
//    当在图形上下文中构建一个路径时，需要CGContextBeginPath
    
    int end = 0;
    
}


#pragma mark - QuartzSummary(一)

- (void)quartz2DSummary{
//    路径绘制，透明度，描影，绘制阴影，透明层，颜色管理，反锯齿，PDF文档生成，PDF元数据访问。
//    Quartz 2D在图像中使用了绘画者模型(painter’s model)offscreen(幕后)
//    aLayerOfPaint放置于画布canvas(page);page可以是一张纸(如果输出设备是打印机)，也可以是虚拟的纸张(如果输出设备是PDF文件)，还可以是bitmap图像。这根据实际使用的graphics context而定。
//    图形状态
//    CGContextSaveGState(<#CGContextRef  _Nullable c#>)保存图形状态
//    CGContextRestoreGState(<#CGContextRef  _Nullable c#>)还原图形状态
    
//    Quartz 2D 坐标系统（user-space coordination system）
//    Quartz通过使用当前转换矩阵(current transformation matrix， CTM)将一个独立的坐标系统(user space)映射到输出设备的坐标系统(device space)，以此来解决设备依赖问题， CTM是一种特殊类型的矩阵(affine transform, 仿射矩阵)，通过平移(translation)、旋转(rotation)、缩放(scale)操作将点从一个坐标空间映射到另外一个坐标空间。用户空间的坐标为左下角
//    在Mac OS X中，重写过isFlipped方法以返回yes的NSView类的子类
//    在iOS中，由UIView返回的绘图上下文
//    在iOS中，通过调用UIGraphicsBeginImageContextWithOptions函数返回的绘图上下文
//    下文的原点做一个平移(移到左上角)和用-1对y坐标值进行缩放(坐标系的转换)
//    内存管理：对象所有权
//    Quartz使用Core Foundation内存管理模型(引用计数)。所以，对象的创建与销毁与通常的方式是一样的。在Quartz中，需要记住如下一些规则：
//    
//    如果创建或拷贝一个对象，你将拥有它，因此你必须释放它。通常，如果使用含有”Create”或“Copy”单词的函数获取一个对象，当使用完后必须释放，否则将导致内存泄露。
//    如果使用不含有”Create”或“Copy”单词的函数获取一个对象，你将不会拥有对象的引用，不需要释放它。
//    如果你不拥有一个对象而打算保持它，则必须retain它并且在不需要时release掉。可以使用Quartz 2D的函数来指定retain和release一个对象。例如，如果创建了一个CGColorspace对象，则使用函数CGColorSpaceRetain和CGColorSpaceRelease来retain和release对象。同样，可以使用Core Foundation的CFRetain和CFRelease，但是注意不能传递NULL值给这些函数。
//    
//    
    
}

@end
