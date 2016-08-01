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
    int i = list[MAXSIZE-2].next;
    while (i)
    {
        ++length;
        i = list[i].next;
    }
    return length;
}

Status insertElementByPosition(StaticListNode staticList, ElemTypeI position, ElemTypeI elem){
    
    ElemTypeI k = staticList[MAXSIZE - 2].next;
    ElemTypeI i = Malloc_SLL(staticList);
    if (i) {
        for (int l = 0; l < position-1; l++)
            k = staticList[k].next;
        staticList[i].next = staticList[k].next;
        staticList[k].next = i;
    }
    return OK;
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