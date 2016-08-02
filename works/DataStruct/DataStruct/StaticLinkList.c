//
//  StaticLinkList.c
//  DataStruct
//
//  Created by mac zdszkj on 16/7/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//


//整个空间分为两部分，array[0].next存储的备用空间,array[MAXSIZE-2].next存储的为存放的数据的空间

#include "StaticLinkList.h"
#define AVAILABLE -1

Status CreateStaticList(StaticListNode *staticList){

    staticList[MAXSIZE - 1]->next = 0;
    staticList[MAXSIZE - 2]->next = 0;
    for (ElemTypeI i = 0; i < MAXSIZE -2; i++) {
        staticList[i]->next = i+1;
    }
    return OK;
}

ElemTypeI Malloc_SLinkList(StaticListNode array){
    ElemTypeI k = array[0].next;
    if (k)
        array[0].next = array[k].next;//Looking for spare spare node in the linklist
    return k;
}

Status Free_SLinkList(StaticListNode array, ElemTypeI pos){
    array[pos].next = array[0].next;
    array[0].next = pos; //Recycling will be idle nodes to the standby list
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

Status StaticList_Insert(StaticListNode sList, ElemTypeI element, int pos){

    if (pos < 1 || pos > static_listLength(sList)) return FALSe;
    
    int k = sList[MAXSIZE - 1].next;
    int i = Malloc_SLinkList(sList);
    if (i) {
        for (int l = 0; l < pos - 1; l++)
            k = sList[k].next;
        sList[i].next = sList[k].next;
        sList[k].next = i;
        return OK;
    }
    return FALSe;
}

Status staticList_delete(StaticListNode slist, ElemTypeI element, int pos){
    
    return OK;
}

