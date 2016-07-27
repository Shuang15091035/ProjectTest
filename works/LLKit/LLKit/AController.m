
//
//  AController.m
//  LLKit
//
//  Created by mac zdszkj on 16/3/23.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "AController.h"
#import <AVFoundation/AVFoundation.h>

//C枚举
enum Student{
    liu,
    shuang,
    good,
}stu;

typedef NS_ENUM(NSInteger,StudentType_) {
    StudentType_Name,
    StudentType_No,
};

@interface AController (){
    UIView *containView;
}

@end

@implementation AController

- (void)viewDidLoad {
    [super viewDidLoad];
    containView = [[UIView alloc]initWithFrame:CGRectMake(20, 20, 100, 50)];
    containView.backgroundColor = [UIColor clearColor];
    [self.view addSubview:containView];
    // Do any additional setup after loading the view.
    NSURL *url = [[NSBundle mainBundle]URLForResource:@"PHIDEON" withExtension:@"mp4"];
    AVPlayer *player = [AVPlayer playerWithURL:url];
    AVPlayerLayer *playerLayer = [AVPlayerLayer playerLayerWithPlayer:player];
    playerLayer.frame = containView.bounds;
    [containView.layer addSublayer:playerLayer];
 
    
    CATransform3D transform3D = CATransform3DIdentity;
    transform3D.m34 = -1.0 / 500.0;
    transform3D = CATransform3DRotate(transform3D, M_PI_4, 1, 1, 0);
    playerLayer.transform = transform3D;
    
    playerLayer.masksToBounds = YES;
    playerLayer.cornerRadius = 20.0;
    playerLayer.borderColor = [UIColor redColor].CGColor;
    playerLayer.borderWidth = 5.0;
    [player play];
    

}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

@end
