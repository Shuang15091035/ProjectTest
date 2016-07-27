//
//  NewFile.h
//  Camera
//
//  Created by mac zdszkj on 16/3/17.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

@interface NewFile : UIView
{
    NSString *_hate;

}
- (void)setHate:(NSString *)aHate;
- (NSString *)hate;
@property (nonatomic, readwrite) NSString* name;
@property (nonatomic, readwrite) NSInteger age;

@end
