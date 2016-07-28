//
//  StaticLinkList.h
//  DataStruct
//
//  Created by mac zdszkj on 16/7/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#ifndef StaticLinkList_h
#define StaticLinkList_h

typedef void StaticListV;
typedef void StaticListNode;
#include <stdio.h>
#include "Common.h"

#define MAXSIZE 100

typedef struct {
    ElemTypeI data;
    ElemTypeI next;
}ComponetNode;
typedef struct {
    ElemTypeI capacity;
    ComponetNode header;
    ComponetNode node[];
}StaticList,*StaticListRef;

#endif /* StaticLinkList_h */
