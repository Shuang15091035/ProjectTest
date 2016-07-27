//
//  DiscParent.h
//  LLKit
//
//  Created by mac zdszkj on 16/7/14.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
@protocol DiscProtocol <NSObject>

@required

- (void)getDiscByDiscName:(NSString *)discName;

- (void)sellDiscByDiscName:(NSString *)discName;

@optional

@end

@interface DiscParent : NSObject<DiscProtocol>

@end

