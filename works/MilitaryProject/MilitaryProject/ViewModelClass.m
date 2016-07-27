//
//  ViewModelClass.m
//  MilitaryProject
//
//  Created by mac zdszkj on 16/4/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ViewModelClass.h"

@implementation ViewModelClass

#pragma 获取网络可到达状态
-(void) netWorkStateWithNetConnectBlock: (NetWorkBlock) netConnectBlock WithURlStr: (NSString *) strURl;
{
    BOOL netState = [MPNetworkTool netWorkReachabilityWithURLString:strURl];
    netConnectBlock(netState);
}

#pragma 接收穿过来的block
-(void) setBlockWithReturnBlock: (ReturnValueBlock) returnBlock
               WithFailureBlock: (FailureBlock) failureBlock
{
    _returnBlock = returnBlock;
    _failureBlock = failureBlock;
}

@end
