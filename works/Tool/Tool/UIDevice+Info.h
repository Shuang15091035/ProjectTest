//
//  UIDevice+Info.h
//  Tool
//
//  Created by mac zdszkj on 16/3/2.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface UIDevice (Info)
//获取设备的类型 example:iPad Air 2 (WiFi)）
- (NSString *)platformString;

@property (nonatomic, strong) NSString* name;
@end
