//
//  FactorPattern.m
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/7.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "FactoryPattern.h"

@implementation FactoryPattern

- (void)createInstance{
    
}
@end

@implementation FactoryPattern2

- (void)createInstance{
    
}


@end

@implementation FactoryPatternManager

+ (id<ISample>) creator:(int) which{
    if (which==1)
        return [[FactoryPattern alloc]init];
    else if (which==2)
        return [[FactoryPattern2 alloc]init];
    return nil;
}

@end
