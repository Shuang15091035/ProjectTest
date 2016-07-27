//
//  HomeVimeModel.m
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "HomeVimeModel.h"
#import "MPCacheManager.h"
#import "HomeModel.h"

@interface HomeVimeModel()

@property (nonatomic) NSString *cachePath;

@end

@implementation HomeVimeModel

-(void)fetchHomeDataWithUrl:(NSString *)url {
    //判断是否有缓存
    
    BOOL cache = [MPCacheManager isCacheDataInvalid:self.cachePath];
    if (!cache) {
        
        id respondData = [MPCacheManager readDataAtUrl:self.cachePath];
        [self fetchValueSuccessWithDic:respondData];
        
    }else{
        [MPNetworkTool getUrl:url parameter:nil success:^(id responseObject) {\
            
            [self fetchValueSuccessWithDic:responseObject];
            
        } failure:^(NSError *error) {
            
            [self netFailure];
            
        }];
    }
}
#pragma 获取到正确的数据，对正确的数据进行处理
-(void)fetchValueSuccessWithDic: (id) returnValue{

    //对从后台获取的数据进行处理，然后传给ViewController层进行显示
    
//    NSArray *statuses = returnValue[STATUSES];
   
    HomeModel *homeModel = [[HomeModel alloc]initWithData:returnValue error:nil];
    
    self.returnBlock(homeModel.newsList);
}

#pragma 对网路异常进行处理
-(void) netFailure{
    self.failureBlock();
}

@end
