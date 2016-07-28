//
//  StaticLinkList.c
//  DataStruct
//
//  Created by mac zdszkj on 16/7/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#include "StaticLinkList.h"
#define AVAILABLE -1

Status CreateStaticList(StaticListRef *staticList, ElemTypeI number){
    
    *staticList = (StaticListRef)malloc(sizeof(StaticList) + sizeof(ComponetNode) * (number + 1));
    if (*staticList == 0x00) return FALSe;
    (*staticList)->capacity = number;
    (*staticList)->header.data = 0;
    (*staticList)->header.next = 0;
    for (int i = 0; i < number; i++) {
        (*staticList)->node[i].next = AVAILABLE;
    }
    return OK;
}

Status getElementByPosition(StaticListRef staticList,ElemTypeI position){
    ElemTypeI result = staticList->node[position].data;
    
    return OK;
}

int StaticList_Insert(StaticList* list, StaticListNode* node, int pos)  // O(n)
{
    /*强制类型转换*/
    StaticList* sList = (StaticList*)list;
    /*合法性判断*/
    int ret = (sList != NULL);
    int current = 0;
    //定义一个空闲位置的下标index
    int index = 0;
    int i = 0;
    
    /*判断单链表要还有位置*/
    ret = ret && (sList->header.data + 1 <= sList->capacity);
    ret = ret && (pos >=0) && (node != NULL);
    
    if( ret )
    {
        /*寻找空闲位置*/
        for(i=1; i<=sList->capacity; i++)
        {
            /*只要数组里面的next元素为-1的时候就证明这个位置是空的*/
            if( sList->node[i].next == AVAILABLE )
            {
                index = i;
                break;
            }
        }
        
        sList->node[index].data = (unsigned int)node;
        
        
        /*把头结点放在这个位置就是为了后续的遍历*/
        sList->node[0] = sList->header;
        
        //sList->node[current].next保护机制，为了满足新元素的插入。
        for(i=0; (i<pos) && (sList->node[current].next != 0); i++)
        {
            current = sList->node[current].next;
        }
        
        
        sList->node[index].next = sList->node[current].next;
        sList->node[current].next = index;
        
        
        /*链表长度+1（复用的方法）*/
        sList->node[0].data++;
        
        /*更新一下表头信息*/
        sList->header = sList->node[0];
    }
    
    return ret;
}