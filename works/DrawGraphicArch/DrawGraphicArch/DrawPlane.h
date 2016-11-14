//
//  DrawPlane.h
//  DrawGraphicArch
//
//  Created by mac zdszkj on 2016/11/14.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import <Home/Home.h>

@interface DrawPlane : UIView
- (instancetype)initWithBtn:(UIButton *)btn finishBtn:(UIButton *)finishBtn deleteBtn:(UIButton *)deleteBtn componentBtn:(UIButton *)componentBtn;
@property (nonatomic, readwrite) HomeArchPlane *homeArchPlane;
@property (nonatomic, readwrite) bool isGenerateHome;
@end
