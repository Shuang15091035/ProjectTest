//
//  StaticLinkList.c
//  DataStruct
//
//  Created by mac zdszkj on 16/7/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#include "StaticLinkList.h"
#define AVAILABLE -1

Status CreateStaticList(StaticListNode **staticList){

    (*staticList[MAXSIZE -1])->next = 0;
    for (ElemTypeI i = 0; i < MAXSIZE -2; i++) {
        (*staticList[i])->next = i+1;
        (*staticList[i])->next = 0;
    }
    return OK;
}

ElemTypeI static_listLength(StaticListNode list){
    ElemTypeI length = 0;
    int i = list[MAXSIZE-1].next;
    while (i)
    {
        ++length;
        i = list[i].next;
    }
    return length;
}

Status getElementByPosition(StaticListNode staticList,ElemTypeI position, ElemTypeIRef e){
    int j = 1, k = staticList[MAXSIZE -1].next;
    if(position < 1 || position > static_listLength(staticList))
    while (k && j < position) {
        ++j;
        k = staticList[k].next;
    }
    *e = staticList[k].data;
    return OK;
}


Status StaticList_Insert(StaticListNode* sList, ElemTypeI element, int pos){

    return OK;
}