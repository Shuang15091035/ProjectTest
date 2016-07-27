//
//  NewFile.m
//  Camera
//
//  Created by mac zdszkj on 16/3/17.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "NewFile.h"

@implementation NewFile
- (id) init
{
    self = [super init];
    if (nil != self) {
        _hate = @"LiLei";
    }
    return self;
}
- (void)setHate:(NSString *)aHate{
    [self willChangeValueForKey:aHate];
    _hate = aHate;
    [self didChangeValueForKey:aHate];
}
- (NSString *)hate{
    return _hate;
}
+ (BOOL)automaticallyNotifiesObserversForKey:(NSString *)key{
    if ([key isEqualToString:@"_hate"]) {
         return [super automaticallyNotifiesObserversForKey:key];
    }
    return NO;
}
@end
