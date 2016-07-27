//
//  HomeVimeModel.h
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "ViewModelClass.h"
#import "HomeModel.h"

@interface HomeVimeModel : ViewModelClass

//获取首页数据
-(void)fetchHomeDataWithUrl:(NSString *)url;

@end
