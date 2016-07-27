//
//  HomeViewController.m
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MPHomeViewController.h"
#import "HomeSubViewController.h"
#define TableViewH height(self.view.frame)-49-50-64

@interface MPHomeViewController ()<UIScrollViewDelegate>

@property (nonatomic) NSMutableArray *subControllers;
@property (nonatomic) NSArray *subVCName;
@property (nonatomic) UIScrollView *maxScrollView;
@property (nonatomic) UIView *titleView;

@property (nonatomic, assign) CGFloat lastPosition;
@property (nonatomic, assign) NSUInteger maxIndex;

@end

@implementation MPHomeViewController

- (void)viewDidLoad {
    [super viewDidLoad];
    self.view.backgroundColor = [UIColor whiteColor];

    self.subControllers = [NSMutableArray array];
    self.subVCName = @[@"推荐",@"热点",@"军事",@"历史",@"环球"];
//    创建视图子控制器的底部滚动视图
    [self bottomScrollView];
    
}
- (void)bottomScrollView{
   self.maxScrollView = [[UIScrollView alloc]initWithFrame:CGRectMake(0, maxY(self.titleView), CGRectGetWidth(self.view.frame), TableViewH)];
    self.maxScrollView.contentSize = CGSizeMake(width(self.view.frame)*5, TableViewH);
    self.maxScrollView.delegate = self;
    self.maxScrollView.bounces  = NO;
    self.maxScrollView.tag = 1000;
    self.maxScrollView.pagingEnabled = YES;
    self.maxScrollView.showsHorizontalScrollIndicator = NO;
    self.maxScrollView.contentOffset = CGPointZero;
    [self.view addSubview:self.maxScrollView];
    
//    for (int index = 0; index < self.subVCName.count; index++) {
//        HomeSubViewController *subViewController = [[HomeSubViewController alloc]init];
//        subViewController.view.frame = CGRectMake(index*screentWidth(), 0, screentWidth(), TableViewH);//打破了懒加载的模式（此种方法不好）
//        subViewController.superViewController = self;
//        subViewController.midField = [NSString stringWithFormat:@"%d",index+1];
//        [self.subControllers addObject:subViewController];
//        [_maxScrollView addSubview:subViewController.view];
//    }
    [self addSubViewControllerViewToScrollView:0];
    self.maxIndex = 0; //当前scrollView上添加的子视图
    HomeSubViewController *subViewController = self.subControllers[0];
    [subViewController fetchDataPerSubViewController];
}
// 优化，实现懒加载对应的视图控制器
- (void)scrollViewDidScroll:(UIScrollView *)scrollView{
    ScrollViewDirection direction = [self scrollViewScrollDirection:scrollView];
    switch (direction) {
        case ScrollViewDirectionLeft:{
            
            break;
        }
        case ScrollViewDirectionRight:{
            if (self.maxIndex < self.subVCName.count) {
                self.maxIndex++;
                [self addSubViewControllerViewToScrollView:self.maxIndex -1];
            }else{
                self.maxIndex = self.subVCName.count;
            }
            break;
        }
        default:
            break;
    }
}
- (ScrollViewDirection)scrollViewScrollDirection:(UIScrollView *)scrollView{
    int currentPostion = scrollView.contentOffset.y;
    if (currentPostion - self.lastPosition > 5) {
       self.lastPosition = currentPostion;
        return ScrollViewDirectionRight;
    }
    self.lastPosition = currentPostion;
    return ScrollViewDirectionLeft;
}
- (void)addSubViewControllerViewToScrollView:(NSUInteger)index{
    HomeSubViewController *subViewController = [[HomeSubViewController alloc]init];
    subViewController.view.frame = CGRectMake(index*screentWidth(), 0, screentWidth(), TableViewH);//打破了懒加载的模式（此种方法不好）
    subViewController.superViewController = self;
    subViewController.midField = [NSString stringWithFormat:@"%ld",index+1];
    [self.subControllers addObject:subViewController];
    [_maxScrollView addSubview:subViewController.view];
}
@end
