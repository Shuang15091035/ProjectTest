//
//  main.c
//  DataStruct
//
//  Created by mac zdszkj on 16/7/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

//
//  main.c
//  LinkList
//
//  Created by mac zdszkj on 16/7/26.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#include <stdio.h>
#include "Common.h"
#include "DynamicLinkList.h"
//#include "StaticLinkList.h"
#include "MergeLinkList.h"

//Status dynamicLinkListUsage();
//Status staticLinkList();
//
//int main(int argc, const char * argv[]) {
//    
////    dynamicLinkListUsage();
//    
//    return 0;
//}
//Status staticLinkList(){
//    
//    return OK;
//}
//
//Status dynamicLinkListUsage(){
//    LinkListNode *pList=0x00;
//    int length = 0;
//    
//    initLinkList(&pList);
//    
////    createList(pList);
//    pList = createLinkList();
//    printLinkList(pList);
//    
//    length = getLengthLinkList(pList);
//    
//    Status isEmpty = isEmptyLinkList(pList);
//    printf("%d",isEmpty);
//    
//    ElemTypeI e = 0;
//    getElement(pList, 3, &e);
//    printLinkList(pList);
//    
//    insertElem(pList, 12);
//    printLinkList(pList);
//    
//    ElemTypeI inserPosition = 2;
//    if (inserPosition > 0 || inserPosition <= getLengthLinkList(pList)) {
//        insertElemByPosition(&pList, inserPosition, 56);
//        printLinkList(pList);
//    }
//    
//    ListInsert_L(&pList, 1, 99);
//    printLinkList(pList);
//
//    ElemTypeI value = 0;
//    deleteElemByPosition(&pList, 1, &value);
//    printLinkList(pList);
//    
//    clearLinkList(pList);      //清空链表
//    //    system("pause");
//    return OK;
//    
//}

//#define MAXSIZE 100
//
//typedef int ElemType;
///* 线性表的静态链表存储结构 */
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
//    array[MAXSIZE - 2].cur = 0;  /* 最后一个元素也是不可用的，倒数第二个元素的cur为0 ，表示不可用*/
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
//    
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