//
//  PublicData.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "PublicData.h"
#import "ArchWallComponent.h"
#import "ComponentWindow.h"
#import "ComponentDoor.h"



@interface PublicData(){
    
}

@end

@implementation PublicData

- (ArchWallComponent*) parseArchComponentType:(ComponentType)componentType {
    ArchWallComponent *archComponent = nil;
    switch (componentType) {
        case ComponentTypeSingleDoorInside:{
            
            break;
        }
        case ComponentTypeSingleDoorOutside:{
            
            break;
        }
        case ComponentTypeDoubleDoorInside:{
            
            break;
        }
        case ComponentTypeDoubleDoorOutside:{
            
            break;
        }
        case ComponentTypeSingleWindowInside:{
            
            break;
        }
        case ComponentTypeSingleWindowOutside:{
            
            break;
        }
        case ComponentTypeDoubleWindowInside:{
            
            break;
        }
        case ComponentTypeDoubleWindowOutside:{
            
            break;
        }
            
        default:
            break;
    }
    return archComponent;
}

@end
