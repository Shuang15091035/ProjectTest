//
//  ViewModelClass.h
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import "MPNetworkTool.h"

//定义返回请求数据的block类型
typedef void (^ReturnValueBlock) (id returnValue);
typedef void (^FailureBlock)();
typedef void (^NetWorkBlock)(BOOL netConnetState);

@interface ViewModelClass : NSObject

@property (strong, nonatomic) ReturnValueBlock returnBlock;
@property (strong, nonatomic) FailureBlock failureBlock;

#pragma 获取网络可到达状态
-(void) netWorkStateWithNetConnectBlock: (NetWorkBlock) netConnectBlock WithURlStr: (NSString *) strURl;


#pragma 接收传过来的block
-(void) setBlockWithReturnBlock: (ReturnValueBlock) returnBlock
               WithFailureBlock: (FailureBlock) failureBlock;

@end
