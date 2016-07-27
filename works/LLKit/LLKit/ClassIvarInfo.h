//
//  ClassIvarInfo.h
//  LLKit
//
//  Created by mac zdszkj on 16/2/29.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <objc/runtime.h>

@interface ClassIvarInfo : NSObject{
    NSString *_name;
    NSString *_age;
}

@property (nonatomic, readonly) NSString *firstName;

- (NSUInteger)changeDataWithFirstData:(NSUInteger)firstData secondData:(NSUInteger)secondData;
//+ (instancetype)new UNAVAILABLE_ATTRIBUTE;
@end
