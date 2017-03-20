//
//  Student.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/9.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

@protocol IPerson <NSObject>

@property (nonatomic, readwrite) NSString *name;
@property (nonatomic, readwrite) NSUInteger height;

@end

@interface Student : NSObject

@property (nonatomic, readwrite) NSString *name;
@property (nonatomic, readwrite) NSUInteger height;

@end
