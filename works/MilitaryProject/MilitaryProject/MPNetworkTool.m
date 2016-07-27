//
//  MPNetworkTool.m
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "MPNetworkTool.h"
#import "AFNetworking.h"

@implementation MPNetworkTool
/** 监测网络的可链接性**/
+ (BOOL) netWorkReachabilityWithURLString:(NSString *) strUrl
{
    __block BOOL netState = NO;
    
    NSURL *baseURL = [NSURL URLWithString:strUrl];
    
    AFHTTPRequestOperationManager *manager = [[AFHTTPRequestOperationManager alloc] initWithBaseURL:baseURL];
    
    NSOperationQueue *operationQueue = manager.operationQueue;
    
    [manager.reachabilityManager setReachabilityStatusChangeBlock:^(AFNetworkReachabilityStatus status) {
        switch (status) {
            case AFNetworkReachabilityStatusReachableViaWWAN:
            case AFNetworkReachabilityStatusReachableViaWiFi:
                [operationQueue setSuspended:NO];
                netState = YES;
                break;
            case AFNetworkReachabilityStatusNotReachable:
                netState = NO;
            default:
                [operationQueue setSuspended:YES];
                break;
        }
    }];
    
    [manager.reachabilityManager startMonitoring];
    
    return netState;
}
/** get方法发送请求 */
+ (void)getUrl:(NSString *)url parameter:(NSDictionary *)parameter success:(void(^)(id responseObject))success failure:(void(^)(NSError *error))failure{
    AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
    manager.responseSerializer = [AFHTTPResponseSerializer serializer];
    [manager GET:url parameters:parameter success:^(AFHTTPRequestOperation *operation, id responseObject) {
        
        success(responseObject);
        
    } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
        
        NSLog(@"%@",[error description]);
        failure(error);
        
    }];
}

/** post方法发送请求 */
+ (void) post:(NSString *)url parameters:(NSDictionary *)parameters success:(void (^)(id responseObject))success failure:(void (^)(NSError * error))failure {
    // 创建http操作管理者
    AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
    
    // 发送请求
    [manager POST:url parameters:parameters success:^(AFHTTPRequestOperation *operation, id responseObject) {
        if (success) {
            success(responseObject);
        }
    } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
        if (failure) {
            failure(error);
        }
    }];
}

/** post方法发送请求（带文件数据) */
+ (void) post:(NSString *)url parameters:(NSDictionary *) parameters filesData:(NSArray *)filesData success:(void (^)(id responseObject))success failure:(void (^)(NSError *error))failure {
    // 创建http操作管理者
    AFHTTPRequestOperationManager *manager = [AFHTTPRequestOperationManager manager];
    
    // 发送请求
    [manager POST:url parameters:parameters constructingBodyWithBlock:^(id<AFMultipartFormData> formData) {
        
        // 读取文件参数
//        for (HVWFileDataParam *fileDataParam in filesData) {
//            [formData appendPartWithFileData:fileDataParam.fileData name:fileDataParam.name fileName:fileDataParam.fileName mimeType:fileDataParam.mimeType];
//        }
    } success:^(AFHTTPRequestOperation *operation, id responseObject) {
        if (success) {
            success(responseObject);
        }
    } failure:^(AFHTTPRequestOperation *operation, NSError *error) {
        if (failure) {
            failure(error);
        }
    }];
}

@end
