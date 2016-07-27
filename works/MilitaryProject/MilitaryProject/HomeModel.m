//
//  militaryModel.m
//  DesignFun
//
//  Created by qianfeng on 15/9/30.
//  Copyright (c) 2015年 qianfeng. All rights reserved.
//

#import "HomeModel.h"

@implementation PicsList
+(JSONKeyMapper *)keyMapper{
    
    return [[JSONKeyMapper alloc]initWithDictionary:@{@"id":@"picsId"}];
}
@end

@implementation NewsList
+(JSONKeyMapper *)keyMapper{
    
    return [[JSONKeyMapper alloc]initWithDictionary:@{@"id":@"newsId"}];
}
@end

@implementation HomeModel

@end
