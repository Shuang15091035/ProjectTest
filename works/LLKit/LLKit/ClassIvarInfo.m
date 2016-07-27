//
//  ClassIvarInfo.m
//  LLKit
//
//  Created by mac zdszkj on 16/2/29.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ClassIvarInfo.h"


@implementation ClassIvarInfo

#pragma mark - 实现以下效果
//c（9，5） ＝ 9*8*7*6*5/1*2*3*4*5
//c(7,6) = 7*6*5*4*3*2/1*2*3*4*5*6
- (NSUInteger)changeDataWithFirstData:(NSUInteger)firstData secondData:(NSUInteger)secondData{
    NSUInteger firstResult = 1;
    NSUInteger secondResult = 1;
    for (int i = 0; i < secondData; i++) {
        firstResult *= (firstData-i);
        secondResult *= (i+1);
    }
    if(secondResult != 0){
        return firstResult/secondResult;
    }
    return 0;
}
@end
