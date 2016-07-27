//
//  ViewController.m
//  PlayerTest
//
//  Created by mac zdszkj on 16/6/20.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MediaPlayer.h"
#import <AVFoundation/AVFoundation.h>
#import <objc/runtime.h>
#import "CustomTextField.h"
#import <MediaPlayer/MediaPlayer.h>

@interface MediaPlayer ()<UITextFieldDelegate>{
    UITextField *_textField;
    NSTimer *timer;
    CGFloat totalMovieDuration;
    UILabel *label;
    NSDateFormatter *formatter;
    UIView *containerView;
    UIView *containerView2;
}

@end

@implementation MediaPlayer

- (void)viewDidLoad{
    [super viewDidLoad];
//    UIView *subView = [[UIView alloc]init];
//    subView.backgroundColor = [UIColor orangeColor];
//    subView.frame = self.view.bounds;
//    [self.view addSubview:subView];
    containerView = [UIView new];
    containerView.frame = CGRectMake(10, 10, 120, 120);
    [self.view addSubview:containerView];
    
    containerView2 = [UIView new];
    containerView2.frame = CGRectMake(10, 10, 120, 120);
    containerView2.backgroundColor = [UIColor orangeColor];
    containerView2.center = self.view.center;
    [self.view addSubview:containerView2];
    
    UIImageView *view1 = [UIImageView new];
    view1.image = [UIImage imageNamed:@"btn_build_p"];
    view1.frame = CGRectMake(10, 10, 80, 80);
    view1.backgroundColor = [UIColor blackColor];
//    [containerView addSubview:view1];
    
    UIImageView *view2 = [UIImageView new];
    view2.image = [UIImage imageNamed:@"btn_build_p"];
    view2.frame = CGRectMake(90, 10, 80, 80);
    view2.backgroundColor = [UIColor blackColor];
//    [containerView addSubview:view2];
    
    CATransform3D perspective = CATransform3DIdentity;
    perspective.m34 = - 1.0 / 500.0;
    containerView.layer.sublayerTransform = perspective;
    //rotate layerView1 by 45 degrees along the Y axis
    CATransform3D transform1 = CATransform3DMakeRotation(M_PI_4, 0, 1, 0);
    view1.layer.transform = transform1;
    //rotate layerView2 by 45 degrees along the Y axis
    CATransform3D transform2 = CATransform3DMakeRotation(-M_PI_4, 0, 1, 0);
    view2.layer.transform = transform2;
    
//    UIButton *btn = [UIButton buttonWithType:UIButtonTypeCustom];
//        unsigned int outCount;
//    //    获取属性
//        objc_property_t *properties = class_copyPropertyList([UIButton class], &outCount);
//        for (int i = 0; i < outCount; i++) {
//            objc_property_t property = properties[i];
//            NSLog(@"property's name: %s", property_getName(property));
//        }
//    
//    btn.frame = CGRectMake(100, 100, 50, 50);
//    btn.backgroundColor = [UIColor redColor];
//    [btn setImage:[UIImage imageNamed:@"btn_add_n@2x"] forState:UIControlStateNormal];
//    btn.imageEdgeInsets = UIEdgeInsetsMake(10, 20, 30, 40);
////    [btn addTarget:self action:@selector(btnClick) forControlEvents:UIControlEventTouchUpInside];
//    [self.view addSubview:btn];

    UIImageView *image = [[UIImageView alloc]initWithFrame:CGRectMake(0, 0, 120, 120)];
    image.backgroundColor = [UIColor orangeColor];
    image.layoutMargins = UIEdgeInsetsMake(10, 10, 10, 10);
    image.image = [UIImage imageNamed:@"btn_add_n@2x"];
    [containerView2 addSubview:image];
#pragma mark -视频播放
//    NSString *filePath = [[NSBundle mainBundle] pathForResource:@"a.mp4" ofType:nil];
//    NSURL *sourceMovieURL = [NSURL fileURLWithPath:filePath];
//    
//    AVAsset *movieAsset = [AVURLAsset URLAssetWithURL:sourceMovieURL options:nil];
//    AVPlayerItem *playerItem = [AVPlayerItem playerItemWithAsset:movieAsset];
//    AVPlayer *player = [AVPlayer playerWithPlayerItem:playerItem];
//    AVPlayerLayer *playerLayer = [AVPlayerLayer playerLayerWithPlayer:player];
//    playerLayer.frame = self.view.bounds;
//    playerLayer.videoGravity = AVLayerVideoGravityResizeAspect;
//    [subView.layer addSublayer:playerLayer];
//    [player play];
    
#pragma mark - 控制声音滑条
//    UISlider *volumeSlider = [[UISlider alloc]init];
//    volumeSlider.center = self.view.center;
//    volumeSlider.minimumValue = 0.0f;
//    volumeSlider.maximumValue = 1.0f;
//    volumeSlider.value =  [[MPMusicPlayerController applicationMusicPlayer] volume];
//    [self.view addSubview:volumeSlider];
#pragma mark - test
#pragma mark - shuangTest
    
#pragma mark - masterTest
    
#pragma mark - masterBranchForward
//    UITextField *textField = [[UITextField alloc]init];
//    textField.frame = CGRectMake(10, 10, 100, 50);
//    textField.center = self.view.center;
//    textField.backgroundColor = [UIColor blackColor];
//    textField.placeholder = @"请输入你的姓名";
//    [self.view addSubview:textField];
//    
//    unsigned int count = 0;
//    // 拷贝出所有的成员变量列表
//    Ivar *ivars = class_copyIvarList([UITextField class], &count);
//    for (int i = 0; i < count; i++) {
//        // 取出成员变量
//        Ivar ivar = ivars[i];
//        // 打印成员变量名字
//        NSLog(@"%s", ivar_getName(ivar));
//    }
//    // 释放
//    free(ivars);
//    [textField setValue:[UIColor whiteColor] forKeyPath:@"_placeholderLabel.textColor"];
    
//    NSDictionary *attrs = @{NSForegroundColorAttributeName:[UIColor whiteColor],NSFontAttributeName:[UIFont boldSystemFontOfSize:20]};
//    
////    attrs[NSForegroundColorAttributeName] = [UIColor whiteColor];
////    attrs[NSFontAttributeName] = [UIFont boldSystemFontOfSize:20];
//    NSAttributedString *placeholder = [[NSAttributedString alloc] initWithString:@"请输入你的姓名" attributes:attrs];
//    textField.attributedPlaceholder = placeholder;
    
    //下面是使用CustomTextField的代码，可放在viewDidLoad等方法中
//    _textField = [[CustomTextField alloc]initWithFrame:CGRectMake(20,150,280,30)];
//    _textField.backgroundColor = [UIColor blackColor];
//    _textField.placeholder = @"请输入帐号信息";
//    _textField.delegate = self;
////    _textField.text = @"aa";
//    [self.view addSubview:_textField];
//    
//    [self.view bringSubviewToFront:_textField];
//    
//    [self.view sendSubviewToBack:_textField];
    
//    播放结束时候需要通知重新从零播放
//    [[NSNotificationCenter defaultCenter]
//     addObserver:self
//     selector:@selector(playerItemDidReachEnd:)
//     name:AVPlayerItemDidPlayToEndTimeNotification
//     object:[self.player currentItem]];
}
- (void)btnClick{
    NSURL *url = [NSURL URLWithString:@"playerTest:"];
    [[UIApplication sharedApplication] openURL:url];
}
@end
