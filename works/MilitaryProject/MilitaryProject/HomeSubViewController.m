//
//  HomeSubControllerS.m
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "HomeSubViewController.h"
#import "HomeVimeModel.h"
#import "AutoScrollView.h"
#import "UIImageView+WebCache.h"

@interface HomeSubViewController ()

@property (nonatomic) NSMutableArray *photoArr;
@property (nonatomic) HomeVimeModel *homeViewModel;
@property (nonatomic, assign) NSInteger page;

@end

@implementation HomeSubViewController
- (instancetype)init{
    self = [super init];
    if (self) {
        self.page = 0;
        self.dataSource = [NSMutableArray array];
        self.photoArr = [NSMutableArray array];
    }
    return self;
}

- (void)viewDidLoad {
    [super viewDidLoad];
    //初始化控制的属性
     self.view.backgroundColor = [UIColor whiteColor];
    self.homeViewModel = [[HomeVimeModel alloc]init];
    [self.homeViewModel setBlockWithReturnBlock:^(id returnValue) {
        _dataSource = returnValue;
        
    } WithFailureBlock:^{
        
    }];
   
}

- (void)fetchDataPerSubViewController{
    
    NSString *networkPath = [NSString stringWithFormat:COMMENT_DESIGNFUN_URL,self.midField,self.page];
    [self.homeViewModel fetchHomeDataWithUrl:networkPath];
    
    if (self.dataSource.count != 0) {
        
    }
}
@end
