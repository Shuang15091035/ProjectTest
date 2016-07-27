//
//  MPNetworkTool.h
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface MPNetworkTool : NSObject
/** 监测网络的可链接性**/
+ (BOOL) netWorkReachabilityWithURLString:(NSString *) strUrl;

/** get方法发送请求 */
+ (void)getUrl:(NSString *)url parameter:(NSDictionary *)parameter success:(void(^)(id responseObject))success failure:(void(^)(NSError *error))failure;

/** post方法发送请求 */
+ (void) post:(NSString *)url parameters:(NSDictionary *)parameters success:(void (^)(id responseObject))success failure:(void (^)(NSError * error))failure;

/** post方法发送请求（带文件数据) */
+ (void) post:(NSString *)url parameters:(NSDictionary *) parameters filesData:(NSArray *)filesData success:(void (^)(id responseObject))success failure:(void (^)(NSError *error))failure;

@end
