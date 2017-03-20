//
//  Template.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/10.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol ITemplateMethod <NSObject>

- (int*)split:(NSString *)exp opt:(NSString *)opt;
- (int)calculate:(NSString *)exp opt:(NSString *)opt;

@end

@interface Template : NSObject<ITemplateMethod>

@end
