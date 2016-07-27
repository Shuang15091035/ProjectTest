//
//  DynamicLinkList.h
//  DataStruct
//
//  Created by mac zdszkj on 16/7/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//


#ifndef DynamicLinkList_h
#define DynamicLinkList_h

#import "Common.h"

typedef struct LinkListNode{
    ElemTypeI data;
    struct LinkListNode *next;
} LinkListNode, *LinkListNodeRef;

Status initLinkList(LinkListNode **node);
LinkListNodeRef createLinkList(void);
LinkListNode *creatList(LinkListNode *pHead);
Status printLinkList(LinkListNodeRef pHead);
ElemTypeI getLengthLinkList(LinkListNodeRef PHead);
Status getElement(LinkListNodeRef pHead, ElemTypeI i, ElemTypeIRef e);Status isExistElem(LinkListNodeRef pHead, ElemTypeI e);
Status insertElem(LinkListNodeRef pHead, ElemTypeI e);
Status isEmptyLinkList(LinkListNodeRef pHead);
Status clearLinkList(LinkListNodeRef pHead);
LinkListNode *creatList(LinkListNode *pHead);
Status insertElemByPosition(LinkListNodeRef *pHead, ElemTypeI position,ElemTypeI elem);

#endif /* DynamicLinkList_h */
