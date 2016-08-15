//
//  StaticLinkList.h
//  DataStruct
//
//  Created by mac zdszkj on 16/7/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#ifndef StaticLinkList_h
#define StaticLinkList_h

#include <stdio.h>
#include "Common.h"

#define MAXSIZE 100

typedef struct {
    ElemTypeI data;
    ElemTypeI next;
}StaticListNode[MAXSIZE];

Status CreateStaticList(StaticListNode staticList);
Status StaticList_Insert(StaticListNode sList, ElemTypeI element, int pos);
ElemTypeI static_listLength(StaticListNode list);
Status staticList_delete(StaticListNode slist, ElemTypeI element, int pos);
Status staticList_traverse(StaticListNode slist);

#endif /* StaticLinkList_h */
