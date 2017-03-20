//
//  FactorPattern.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/7.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

// new object need to Init
@protocol ISample <NSObject>

- (void)createInstance;

@end

@interface FactoryPattern : NSObject <ISample>


@end

@interface FactoryPattern2 : NSObject <ISample>


@end

@interface FactoryPatternManager : NSObject

+ (id<ISample>) creator:(int) which;

@end
