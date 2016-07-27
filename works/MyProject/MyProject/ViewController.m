//
//  ViewController.m
//  MyProject
//
//  Created by mac zdszkj on 16/5/23.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ViewController.h"

NSString * const AFNetworkingReachabilityDidChangeNotification = @"com.alamofire.networking.reachability.change";
NSString * const AFNetworkingReachabilityNotificationStatusItem = @"AFNetworkingReachabilityNotificationStatusItem";

@interface ViewController ()

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    //左右轨的图片
//    UIImage *stetchLeftTrack = [UIImage imageNamed:@"m-bg"];
//    UIImage *stetchRightTrack = [UIImage imageNamed:@"m-fg"];
//    //滑块图片
//    UIImage *thumbImage = [UIImage imageNamed:@"m-k"];
//    
//    UISlider *slider = [[UISlider alloc] init];
//    slider.backgroundColor = [UIColor clearColor];
//    slider.value =  1.0;
//    slider.center = self.view.center;
//    slider.minimumValue =  0.0;
//    slider.maximumValue =  100.0;
//    
//    [slider setMinimumTrackImage:stetchLeftTrack forState:UIControlStateNormal];
//    [slider setMaximumTrackImage:stetchRightTrack forState:UIControlStateNormal];
//    //注意这里务必加上UIControlStateHightlighted的状态，否则当拖动滑块时滑块将变成原生的控件
//    [slider setThumbImage:thumbImage forState:UIControlStateHighlighted];
//    [slider setThumbImage:thumbImage forState:UIControlStateNormal];
//        
//    [self.view addSubview:slider];
   
#pragma mark - 寄宿图
//    CALayer *layer = [[CALayer alloc]init];
//    layer.contentsScale = 1.0f;//支持高分辨率的，设置为一表示屏幕中每个点按一个像素绘制，设置为二表示每个点以两个像素绘制。
//    layer.contentsGravity = kCAGravityResizeAspectFill;//对应图层的contentMode，这个属性是寄宿图和图层之间的关系，
//    layer.contentsRect = CGRectMake(0, 0, 0.5, 0.5);//contentsRect属性允许我们在图层边框里显示寄宿图的一个子域。这涉及到图片是如何显示和拉伸的,是一个单位坐标（0，1）,是一个相对值,默认的contentsRect是{0, 0, 1, 1}，这意味着整个寄宿图默认都是可见的，如果我们指定一个小一点的矩形，图片就会被裁剪.
//    layer.contentsCenter = CGRectMake(0.25, 0.25, 0.5, 0.5);//contentsCenter其实是一个CGRect，它定义了一个固定的边框和一个在图层上可拉伸的区域。默认情况下，contentsCenter是{0, 0, 1, 1}，这意味着如果大小（由conttensGravity决定）改变了,那么寄宿图将会均匀地拉伸开。
    
//     UIImage *image = [UIImage imageNamed:@"mode"];
//    UIView *view = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 300, 300)];
//    view.center = self.view.center;
//     view.layer.contents = (__bridge id _Nullable)(image.CGImage);
////    view.backgroundColor = [UIColor orangeColor];
//    view.layer.contentsRect = CGRectMake(0.25, 0.25, 0.5, 0.5);
////    view.layer.contentsCenter = CGRectMake(0.25, 0.25, 0.5, 0.5);
//    view.layer.contentsScale = image.scale;
//    [self.view addSubview:view];
#pragma mark - 图层几何学
//    UIView有三个比较重要的布局属性：frame，bounds和center，CALayer对应地叫做frame，bounds和position.frame并不是一个非常清晰的属性，它其实是一个虚拟属性，是根据bounds，position和transform计算而来，
    UIImage *image = [UIImage imageNamed:@"mode"];
    UIView *view = [[UIView alloc]initWithFrame:CGRectMake(0, 0, 300, 300)];
    view.center = self.view.center;
    view.backgroundColor = [UIColor orangeColor];
    view.layer.contents = (__bridge id _Nullable)(image.CGImage);
    view.layer.anchorPoint = CGPointMake(0.5, 0.9);
    view.layer.zPosition = 0.1f;//视图在Z轴方向被抬高了0.1个像素。
    
//    视图中其实是三维坐标中的一个二维平面，Zposition代表的就是在Z轴上的偏移量
    [self.view addSubview:view];
#pragma mark - 仿射变换
    CALayer *layer = [[CALayer alloc]init];
    
}

@end
