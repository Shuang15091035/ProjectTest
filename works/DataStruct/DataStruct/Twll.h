//
//  Twll.h
//  DataStruct
//
//  Created by mac zdszkj on 2017/3/6.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#ifndef Twll_h
#define Twll_h

#include <stdio.h>

typedef struct Node {
    int data;
    struct Node *prior;
    struct Node *next;
}DNode, *DLinkList;

void InitDLinkList(DLinkList *linkList);
void AddElem(DLinkList linkList, int data);
void PrintDlinkList(DLinkList LinkList);

#endif /* Twll_h */
