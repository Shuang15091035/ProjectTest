//
//  ViewController.m
//  QrCode
//
//  Created by mac zdszkj on 16/7/18.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//


#import "ViewController.h"
#import <AVFoundation/AVFoundation.h>

static const CGFloat kMargin = 30;
static const CGFloat kBorderW = 100;

@interface ViewController ()<AVCaptureMetadataOutputObjectsDelegate>{
    AVCaptureSession * _session;//输入输出的中间桥梁
    
}

@end

@implementation ViewController
- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    //获取摄像设备
    AVCaptureDevice * device = [AVCaptureDevice defaultDeviceWithMediaType:AVMediaTypeVideo];
    //创建输入流
    AVCaptureDeviceInput * input = [AVCaptureDeviceInput deviceInputWithDevice:device error:nil];
    if (!input) return;

    AVCaptureMetadataOutput *metaDataOutPut = [[AVCaptureMetadataOutput alloc]init];
    [metaDataOutPut setMetadataObjectsDelegate:self queue:dispatch_get_main_queue()];
    metaDataOutPut.rectOfInterest = CGRectMake(10, 10, 10, 10);
    
    AVCaptureSession *session = [[AVCaptureSession alloc]init];
//    高质量采集率
    [session setSessionPreset:AVCaptureSessionPresetHigh];
    [session addInput:input];
    [session addOutput:metaDataOutPut];
    
    AVCaptureVideoPreviewLayer *vedioPreviewLayer = [[AVCaptureVideoPreviewLayer alloc]initWithSession:session];
    vedioPreviewLayer.videoGravity = AVLayerVideoGravityResizeAspectFill;
    vedioPreviewLayer.frame = CGRectMake(20, 20, 400, 400);
    [self.view.layer addSublayer:vedioPreviewLayer];
    
    metaDataOutPut.metadataObjectTypes=@[AVMetadataObjectTypeQRCode,AVMetadataObjectTypeEAN13Code, AVMetadataObjectTypeEAN8Code, AVMetadataObjectTypeCode128Code];
    
    [session startRunning];
    
}
- (void)captureOutput:(AVCaptureOutput *)captureOutput didOutputMetadataObjects:(NSArray *)metadataObjects fromConnection:(AVCaptureConnection *)connection{
    if (metadataObjects.count>0) {
        //[session stopRunning];
        AVMetadataMachineReadableCodeObject * metadataObject = [metadataObjects objectAtIndex : 0 ];
        //输出扫描字符串
        NSLog(@"%@",metadataObject.stringValue);
    }
}

@end
