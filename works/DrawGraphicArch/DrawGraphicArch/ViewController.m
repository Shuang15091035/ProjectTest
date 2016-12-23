//
//  ViewController.m
//  DrawGraphicArch
//
//  Created by mac zdszkj on 2016/11/4.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ViewController.h"
#import "DrawPlane.h"

@interface ViewController (){
    UIButton *mBtn;
}

@end

@implementation ViewController

- (void)viewDidLoad {
    [super viewDidLoad];
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
    
    DrawPlane *planDrawGraphic = [[DrawPlane alloc]initWithBtn:addBtn finishBtn:finishBtn deleteBtn:deleteBtn componentBtn:componentBtn];
    CGSize screenSize = [[UIScreen mainScreen]bounds].size;
    CGFloat screenWidth = screenSize.width;
    CGFloat screenHeight = screenSize.height;
    planDrawGraphic.frame = CGRectMake(-screenWidth, -screenHeight, screenWidth * 3, screenHeight * 3);
    [self.view addSubview:planDrawGraphic];

    [self.view addSubview:addBtn];
    [self.view addSubview:finishBtn];
    [self.view addSubview:deleteBtn];
    [self.view addSubview:componentBtn];
    
}
@end
