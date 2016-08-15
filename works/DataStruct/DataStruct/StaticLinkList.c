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

Status CreateStaticList(StaticListNode staticList){

    
    for (ElemTypeI i = 0; i < MAXSIZE -2; i++) {
        staticList[i].next = i+1;
        staticList[i].data = 0;
    }
    staticList[MAXSIZE - 1].next = 0;
    staticList[MAXSIZE - 2].next = 0;
    return OK;
}

static ElemTypeI Malloc_SLinkList(StaticListNode array){
    ElemTypeI k = array[0].next;
    if (k)
        array[0].next = array[k].next;//Looking for spare spare node in the linklist
    return k;
}

static Status Free_SLinkList(StaticListNode array, ElemTypeI pos){
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

Status StaticList_Insert(StaticListNode slist, ElemTypeI element, int pos){

    if (pos < 0 || pos > static_listLength(slist)+1) return FALSe;
    
    int k = MAXSIZE - 1;
    int i = Malloc_SLinkList(slist);
    if (i) {
        slist[i].data = element;
        for (int l = 1; l < pos; l++)
            k = slist[k].next;
        slist[i].next = slist[k].next;
        slist[k].next = i;
        return OK;
    }
    return FALSe;
}

Status staticList_delete(StaticListNode slist, ElemTypeI element, int pos){
    
    if (pos < 1 || pos > static_listLength(slist)) return FALSe;
    int k = MAXSIZE - 1;
        for (int l = 1; l < pos; l++)
            k = slist[k].next;
        slist[k].next = slist[pos].next;
        Free_SLinkList(slist, pos);
    
    return OK;
}

Status staticList_traverse(StaticListNode slist){
    
//    int length =  static_listLength(slist);
//    if (static_listLength(slist) < 1) return FALSe;
//    int k =slist[MAXSIZE - 1].next;
//    if (k) {
//        for (int i = 0; i < length; i++) {
//            printf("%d",slist[i].data);
//        }
//    }

//    simple implement
    printf("-------------\n");
    int k = MAXSIZE - 1;
    while (slist[k].next != 0)
    {
        k = slist[k].next;
        printf("array[%d].data = %d\n",k,slist[k].data);
    }
    
    return OK;
}


#pragma mark - internation demo

// #define MAXSIZE 100
// 
// typedef int ElemType;
// /* 线性表的静态链表存储结构 */
//typedef struct Node
//{
//    ElemType data;
//    int cur; //为0时表示无指向
//} StaticLinkList[MAXSIZE];
//
///* 将一维数组array中各分量链成一个备用链表，array[0].cur为头指针，"0"表示空指针 */
//Status InitList(StaticLinkList array)
//{
//    printf("InitList...\n");
//    for (int i = 0; i < MAXSIZE - 2; i++)
//    {
//        array[i].cur = i + 1;
//    }
//    array[MAXSIZE - 2].cur = 0;  /* 最后一个元素也是不可用的，倒数第二个元素的cur为0 */
//    array[MAXSIZE - 1].cur = 0;   /* 目前静态链表为空，最后一个元素的cur为0 */
//    
//    return OK;
//}
///* 若备用空间链表非空，则返回分配的结点下标，否则返回0 */
//int Malloc_SLL(StaticLinkList array)
//{
//    int k = array[0].cur;
//    if (k)
//        array[0].cur = array[k].cur;/* 下一个分量用来做备用 */
//    return k;
//}
///*  将下标为pos的空闲结点回收到备用链表 */
//void Free_SLL(StaticLinkList array, int pos)
//{
//    array[pos].cur = array[0].cur; /* 把第一个元素的cur值赋给要删除的分量cur */
//    array[0].cur = pos; /* 把要删除的分量下标赋值给第一个元素的cur */
//}
//
//int ListLength(StaticLinkList array)//bff5b0
//{
//    int i = array[MAXSIZE - 1].cur;
//    int j = 0;
//    while(i)
//    {
//        i = array[i].cur;
//        ++j;
//    }
//    return j;
//}
///*  在array中第pos个元素之前插入新的数据元素Elem  */
//Status ListInsert(StaticLinkList array, int pos, ElemType Elem)
//{
//    printf("Insert List from %d Item %d: \n",Elem,pos);
//    if (pos < 1 || pos > ListLength(array) + 1)
//        return FALSe;
//    
//    int k = MAXSIZE - 1;
//    int i = Malloc_SLL(array); /* 获得空闲分量的下标 */
//    
//    if (i)
//    {
//        array[i].data = Elem;
//        
//        for (int l = 1; l <= pos - 1; l++)
//            k = array[k].cur;
//        
//        array[i].cur = array[k].cur;/* 把第pos个元素之前的cur赋值给新元素的cur */
//        array[k].cur = i;/* 把新元素的下标赋值给第pos个元素之前元素的cur */
//        return OK;
//    }
//    
//    return FALSe;
//}
///*  删除在array中第pos个数据元素   */
//Status ListDelete(StaticLinkList array, int pos)
//{
//    printf("Delete List from pos: %d\n",pos);
//    if (pos < 1 || pos > ListLength(array))
//        return FALSe;
//    
//    int k = MAXSIZE - 1;
//    
//    for (int l = 1; l <= pos - 1; l++)
//        k = array[k].cur;
//    
//    int j = array[k].cur;
//    array[k].cur = array[pos].cur;
//    
//    Free_SLL(array, j);
//    return OK;
//}
//
//Status ListTraverse(StaticLinkList array)
//{
//    printf("List Traverse : ");
//    int k = MAXSIZE - 1;
//    while (array[k].cur != 0)
//    {
//        k = array[k].cur;
//        printf("array[%d].data = %d\n",k,array[k].data);
//    }
//    
//    return OK;
//}
//
//
//int main(void)
//{
//    StaticLinkList SSL;
//    InitList(SSL);
//    for (int i = 1; i < 5; i++)
//        ListInsert(SSL, i, i);
//    ListTraverse(SSL);
//    
//    ListDelete(SSL, 3);
//    ListTraverse(SSL);
//    printf("List Length : %d",ListLength(SSL));
//    return 0;
//}
