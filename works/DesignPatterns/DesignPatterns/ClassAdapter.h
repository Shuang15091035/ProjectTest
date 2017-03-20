//
//  Adapter.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/7.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

//source : method1()
//AdapterTest:

@protocol Targetable <NSObject>

- (void)method1;
- (void)method2;

@end

@interface Source : NSObject

- (void)method1;

@end

@interface ClassAdapter : Source <Targetable>

@end
