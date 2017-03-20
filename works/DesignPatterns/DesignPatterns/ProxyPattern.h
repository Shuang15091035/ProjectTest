//
//  ProxyPattern.h
//  DesignPatterns
//
//  Created by mac zdszkj on 2017/3/9.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

/**
 代理模式：为了加强对代理类的控制，不能改变接口，而适配器模式是为了：是为了增强功能，适配器模式：改变所考虑对象的接口
 */
@protocol Image <NSObject>

- (void)display;

@end

@interface ProxyPattern : NSObject<Image>


@end
