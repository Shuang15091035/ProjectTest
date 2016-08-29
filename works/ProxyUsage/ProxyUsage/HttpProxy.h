//
//  HttpProxy.h
//  ProxyUsage
//
//  Created by mac zdszkj on 16/8/24.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol UserHttpProtocol <NSObject>

- (NSInteger)queryUserID:(NSNumber *)userId;

@end

@protocol CommentHttpProtocol <NSObject>

- (void)getCommentsWithDate:(NSDate *)date;

@end

@interface HttpProxy : NSProxy<UserHttpProtocol,CommentHttpProtocol>

+ (instancetype)shareInstance;

- (void)registerHttpProtocol:(Protocol*)httpProtocol handler:(id)handle;

@end
