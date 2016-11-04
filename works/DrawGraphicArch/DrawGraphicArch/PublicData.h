//
//  PublicData.h
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <Foundation/Foundation.h>

typedef NS_ENUM(NSInteger,ComponentType) {
    ComponentTypeUnknow,
    ComponentTypeSingleDoorInside,
    ComponentTypeSingleDoorOutside,
    ComponentTypeDoubleDoorInside,
    ComponentTypeDoubleDoorOutside,
    ComponentTypeSingleWindowInside,
    ComponentTypeSingleWindowOutside,
    ComponentTypeDoubleWindowInside,
    ComponentTypeDoubleWindowOutside,
};

@class ArchWallComponent;
@class ComponentDoor;
@class ComponentWindow;

@interface PublicData : NSObject

- (ArchWallComponent*) parseArchComponentType:(ComponentType)componentType;

@end
