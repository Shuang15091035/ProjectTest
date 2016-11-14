//
//  GenerateHomeVC.m
//  DrawGraphicArch
//
//  Created by mac zdszkj on 2016/11/14.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "GenerateHomeVC.h"
#import <Home/Home.h>
#import "DrawPlane.h"

@interface GenerateHomeVC (){
    NSMutableArray *_reusableImageAry;
    NSMutableArray *_tempTestAry;
    DrawPlane *plane;
}

@end

@implementation GenerateHomeVC

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    
    NSMutableArray *wallPoints = [NSMutableArray array];
    _reusableImageAry = [NSMutableArray array];
    
    UIButton *addBtn = [[UIButton alloc]initWithFrame:CGRectMake(10, 10, 60, 60)];
    [addBtn setTitle:@" + " forState:UIControlStateNormal];
    addBtn.titleLabel.font = [UIFont systemFontOfSize:15];
    addBtn.backgroundColor = [UIColor redColor];
    
    UIButton *finishBtn = [[UIButton alloc]initWithFrame:CGRectMake(10, 120, 60, 60)];
    finishBtn.hidden = YES;
    [finishBtn setTitle:@" OK " forState:UIControlStateNormal];
    finishBtn.titleLabel.font = [UIFont systemFontOfSize:15];
    finishBtn.backgroundColor = [UIColor redColor];
    
    UIButton *deleteBtn = [[UIButton alloc]initWithFrame:CGRectMake(120, 10, 60, 60)];
    deleteBtn.hidden = YES;
    [deleteBtn setTitle:@" delete " forState:UIControlStateNormal];
    deleteBtn.titleLabel.font = [UIFont systemFontOfSize:15];
    deleteBtn.backgroundColor = [UIColor redColor];
    
    UIButton *componentBtn = [[UIButton alloc]initWithFrame:CGRectMake(200, 10, 60, 60)];
    componentBtn.hidden = YES;
    [componentBtn setTitle:@" 🚪 " forState:UIControlStateNormal];
    componentBtn.titleLabel.font = [UIFont systemFontOfSize:15];
    componentBtn.backgroundColor = [UIColor redColor];
    
    CGPoint points[] = {{1358,982},{1243,1265},{1710,1257}};
    for (int i = 0; i < 3; i++) {
        HomeWallPoint *wallPoint = [[HomeWallPoint alloc]init];
        wallPoint.wallPoint  = CGPointMake(points[i].x, points[i].y);
        wallPoint.pointImageView = [self getAvailableImageViewOfGraphicPoint:wallPoint];
        [wallPoints addObject:wallPoint];
    }
    
    HomeArchPlane *archPlane = [[HomeArchPlane alloc]init];
    [archPlane createRoomWithGround:2.5f wallHeight:3.0f roomPoints:wallPoints archItem:nil];
    plane = [[DrawPlane alloc]initWithBtn:addBtn finishBtn:finishBtn deleteBtn:deleteBtn componentBtn:componentBtn];
    plane.isGenerateHome = YES;
    plane.homeArchPlane = archPlane;
    CGSize screenSize = [[UIScreen mainScreen]bounds].size;
    CGFloat screenWidth = screenSize.width;
    CGFloat screenHeight = screenSize.height;
    plane.frame = CGRectMake(-screenWidth, -screenHeight, screenWidth * 3, screenHeight * 3);
    [self.view addSubview:plane];
    [self.view addSubview:addBtn];
    [self.view addSubview:finishBtn];
    [self.view addSubview:deleteBtn];
    [self.view addSubview:componentBtn];
}
- (void)viewDidAppear:(BOOL)animated{
    [super viewDidAppear:animated];
    [plane setNeedsDisplay];
     plane.isGenerateHome = NO;
}
- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}


- (UIImageView *)getAvailableImageViewOfGraphicPoint:(HomeWallPoint *)point{
    UIImageView *imageV;
    if(!point.pointImageView) {
        if (_reusableImageAry.count != 0) {
            UIImageView *imag = [_reusableImageAry lastObject];
            [_reusableImageAry removeObject:imag];
            imag.hidden = NO;
            imag.center = point.wallPoint;
            imageV = imag;
        }else{
            UIImageView *imageView = [[UIImageView alloc]init];
            imageView.tag = 200;//默认给定标示
            imageView.userInteractionEnabled = YES;
            [imageView setBackgroundColor:[self randomColor]];
            imageView.frame = CGRectMake(0, 0, 40, 40);
            imageView.center = point.wallPoint;
            imageV = imageView;
            [_tempTestAry addObject:imageView];
        }
    }else{
        point.pointImageView.center = point.wallPoint;
        imageV = point.pointImageView;
    }
    return imageV;
}
- (UIColor *)randomColor
{
    CGFloat red =  (CGFloat)random()/(CGFloat)RAND_MAX;
    CGFloat blue = (CGFloat)random()/(CGFloat)RAND_MAX;
    CGFloat green = (CGFloat)random()/(CGFloat)RAND_MAX;
    return [UIColor colorWithRed:red green:green blue:blue alpha:1.0f];
}
@end
