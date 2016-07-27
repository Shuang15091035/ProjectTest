//
//  ViewController.m
//  Bluetooth
//
//  Created by mac zdszkj on 16/3/4.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ToolViewController.h"
#import "ViewControllerB.h"
#import <CoreText/CoreText.h>

@interface ToolViewController (){
    NSString *_text;
    NSTimer *mTimer;
    CGFloat X ;
    CGFloat Y ;
}

@property (nonatomic, readwrite) UIView *LabelView;

@end

@implementation ToolViewController

- (void)viewDidLoad {
    [super viewDidLoad];

//    NSArray *array = [[NSArray alloc] initWithObjects:@"a", @"b", @"c", nil];
//    
//    for (NSArray *one in array) {
//        int val = [one count];
//        NSLog(@"%d",val);
//    }
//   NSLog(@"%@",array);
//    
//    NSLog(@"%@",array);
//    NSLog(@"%@",array);

//    [super viewDidLoad];
//    array= [[NSMutableArray alloc]initWithCapacity:5];
    
//    UIButton *label = [UIButton new];
//    label.backgroundColor = [UIColor redColor];
//    label.frame = CGRectMake(100, 100, 100, 100);
//    [label addTarget:self action:@selector(btn) forControlEvents:UIControlEventTouchUpInside];
//    [self.view addSubview:label];
    
//    UIImageView *imageView = [[UIImageView alloc]initWithFrame:CGRectMake(100, 100, 100, 100)];
//    imageView.image = [UIImage imageNamed:@"flighterModel"];
//    UIBezierPath *maskPath = [UIBezierPath bezierPathWithRoundedRect:imageView.bounds byRoundingCorners:UIRectCornerAllCorners cornerRadii:imageView.bounds.size];
//    
//    CAShapeLayer *maskLayer = [[CAShapeLayer alloc]init];
//    //设置大小
//    maskLayer.frame = imageView.bounds;
//    //设置图形样子
//    maskLayer.path = maskPath.CGPath;
//    imageView.layer.mask = maskLayer;
//    [self.view addSubview:imageView];

//    CATextLayer也要比UILabel渲染得快得多,iOS 6及之前的版本,UILabel其实是通过WebKit来实现绘制的，这样就造成了当有很多文字的时候就会有极大的性能压力
    mTimer = [NSTimer scheduledTimerWithTimeInterval:1.0f target:self selector:@selector(timerStart) userInfo:nil repeats:YES];
    
    _LabelView = [UIView new];
//    _LabelView.frame = CGRectMake(60, 60, 0, 0);
    _LabelView.center = self.view.center;
    _LabelView.backgroundColor = [UIColor whiteColor];
    [self.view addSubview:_LabelView];
//
//    CATextLayer *textLayer = [CATextLayer layer];
//    textLayer.frame = _LabelView.bounds;
//    [_LabelView.layer addSublayer:textLayer];
//    textLayer.foregroundColor = [UIColor orangeColor].CGColor;
//    textLayer.alignmentMode = kCAAlignmentCenter;
//    textLayer.wrapped = YES;
//    textLayer.contentsScale = [UIScreen mainScreen].scale;
//    
//    //set layer font
//    UIFont *font = [UIFont systemFontOfSize:15];
//    CFStringRef fontName = (__bridge CFStringRef) font.fontName;
//    CGFontRef fontRef = CGFontCreateWithFontName(fontName);
//    textLayer.font = fontRef;
//    textLayer.fontSize = font.pointSize;
//    CGFontRelease(fontRef);
//    
//    NSString *text = @"Lorem ipsum dolor sit amet, consectetur adipiscing \n elit. Quisque massa arcu, eleifend vel varius in, facilisis pulvinar \n leo. Nunc quis nunc at mauris pharetra condimentum ut ac neque. Nunc elementum, libero ut porttitor dictum, diam odio congue lacus, vel \n fringilla sapien diam at purus. Etiam suscipit pretium nunc sit amet \n lobortis";
//    textLayer.string = text;
    
     X = 100;
     Y = 100;
//    富文本
    
    CATextLayer *textLayer = [CATextLayer layer];
    textLayer.frame = _LabelView.bounds;
    textLayer.contentsScale = [UIScreen mainScreen].scale;
    [_LabelView.layer addSublayer:textLayer];
    
    //text attributes
    textLayer.alignmentMode = kCAAlignmentJustified;
    textLayer.wrapped = YES;
    
    //choose a font
    UIFont *font = [UIFont systemFontOfSize:15];
    
    //choose some text
    _text = @"Lorem ipsum dolor sit amet, consectetur adipiscing elit. Quisque massa arcu, eleifend vel varius in, facilisis pulvinar  leo. Nunc quis nunc at mauris pharetra condimentum ut ac neque. Nunc  elementum, libero ut porttitor dictum, diam odio congue lacus, vel  fringilla sapien diam at purus. Etiam suscipit pretium nunc sit amet  lobortis";
   
    //create attribute string
    NSMutableAttributedString *string = nil;
    string = [[NSMutableAttributedString alloc]initWithString:_text];
    
    //conver UIFont to a CTFont
    CFStringRef fontName = (__bridge CFStringRef)font.fontName;
    CGFloat fontSize = font.pointSize;
    CTFontRef fontRef = CTFontCreateWithName(fontName, fontSize, NULL);
    
    //set text attributes
    NSDictionary *attribs = @{
                              (__bridge id)kCTForegroundColorAttributeName:(__bridge id)[UIColor blackColor].CGColor,
                              (__bridge id)kCTFontAttributeName: (__bridge id)fontRef
                              };
    
    [string setAttributes:attribs range:NSMakeRange(0, [_text length])];
    attribs = @{
                (__bridge id)kCTForegroundColorAttributeName: (__bridge id)[UIColor redColor].CGColor,
                (__bridge id)kCTUnderlineStyleAttributeName: @(kCTUnderlineStyleSingle),
                (__bridge id)kCTFontAttributeName: (__bridge id)fontRef
                };
    [string setAttributes:attribs range:NSMakeRange(6, 5)];
    CFRelease(fontRef);
    
    //set layer text
    textLayer.string = string;
}
- (void)btn{
    ViewControllerB *viewB = [[ViewControllerB alloc]init];
    [self.navigationController pushViewController:viewB animated:YES];
}
- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

//- (void)timerStart{
//    CGRect textRect = [_text boundingRectWithSize:CGSizeMake(500, MAXFLOAT) options:NSStringDrawingUsesFontLeading attributes:@{NSForegroundColorAttributeName:[UIColor orangeColor],NSFontAttributeName:[UIFont systemFontOfSize:15],NSUnderlineStyleAttributeName:@(NSUnderlineStyleDouble),} context:NULL];
//    
//    CGSize textSize = textRect.size ;
//    textSize = CGSizeMake(textRect.size.width, textRect.size.height);
//    
//    _LabelView.frame = CGRectMake(60, 60, X++, Y++);
////    [_LabelView setNeedsLayout];
//    
//}
@end
