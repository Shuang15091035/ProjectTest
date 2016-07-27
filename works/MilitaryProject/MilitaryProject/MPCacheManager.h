//
//  ZolCachManager.h
//  Zol
//
//  Created by qianfeng on 15/9/28.
//  Copyright (c) 2015年 Lly. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MPCacheManager : NSObject

//保存url 对应的数据
+ (void)saveData:(id)object atUrl:(NSString*)url;

//读取url对应的数据
+ (id)readDataAtUrl:(NSString*)ulr;

//判断缓存数据是否有效
+ (BOOL)isCacheDataInvalid:(NSString*)url;

//计算缓存的大小
+ (double)cacheSize;

//清除缓存
+ (void)clearDisk;
@end
