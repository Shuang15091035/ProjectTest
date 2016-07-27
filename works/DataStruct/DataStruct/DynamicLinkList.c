//
//  DynamicLinkList.c
//  DataStruct
//
//  Created by mac zdszkj on 16/7/27.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#include "DynamicLinkList.h"

Status initLinkList(LinkListNode **node){
    *node = 0;
    return OK;
}

LinkListNode *creatList(LinkListNode *pHead){
    LinkListNode *p1;
    LinkListNode *p2;
    
    p1=p2=(LinkListNode *)malloc(sizeof(LinkListNode)); //申请新节点
    if(p1 == 0 || p2 ==0)
    {
        const char *errorInfo = 0X00;
        printf(errorInfo,"内存分配失败\n");
        exit(0);
    }
    memset(p1,0,sizeof(LinkListNode));
    
    scanf("%d",&p1->data);    //输入新节点
    p1->next = 0;         //新节点的指针置为空
    while(p1->data > 0)        //输入的值大于0则继续，直到输入的值为负
    {
        if(pHead == 0)       //空表，接入表头
        {
            pHead = p1;
        }
        else
        {
            p2->next = p1;       //非空表，接入表尾
        }
        p2 = p1;
        p1=(LinkListNode *)malloc(sizeof(LinkListNode));    //再重申请一个节点
        if(p1 == 0 || p2 ==0)
        {
            printf("内存分配失败\n");
            exit(0);
        }
        memset(p1,0,sizeof(LinkListNode));
        scanf("%d",&p1->data);
        p1->next = 0;
    }
    printf("creatList函数执行，链表创建成功\n");
    return pHead;           //返回链表的头指针
}
LinkListNodeRef createLinkList(void){
    LinkListNode *p1,*p2,*pHead = 0;
    p1 = p2 = (LinkListNodeRef)malloc(sizeof(LinkListNode));
    if (p1 == 0 && p2 == 0) return FALSe;
    memset(p1, 10, sizeof(LinkListNode));
    p1->next = 0;
    scanf("%d",&p1->data);    //输入新节点
    while (p1->data != 0) {
        if (pHead == 0) {
            pHead = p1;
        }else{
            p2->next = p1;
        }
        p2 = p1;
        p1 = (LinkListNodeRef)malloc(sizeof(LinkListNode));
        memset(p1, 0, sizeof(LinkListNode));
        scanf("%d",&p1->data);
        p1->next= 0;
    }
    printf("LinkList create finish\n");
    return pHead;
}

Status printLinkList(LinkListNodeRef pHead){
    if (pHead == 0) {
        return FALSe;
    }else{
        while ( 0 != pHead ) {
            printf("(%d)\n",pHead->data);
            pHead = pHead->next;
        }
    }
    printf("LinkList print finish\n");
    return OK;
}

Status clearLinkList(LinkListNodeRef pHead){
    LinkListNodeRef pNext;
    if (0 == pHead) {
        return FALSe;
    }
    while (pHead->next != 0) {
        pNext = pHead->next;
        free(pHead);
        pHead = pNext;//表头下移
    }
    printf("LinkList clear finish\n");
    return OK;
}

ElemTypeI getLengthLinkList(LinkListNodeRef PHead){
    ElemTypeI lenth = 0;
    if (PHead == 0) {
        lenth = 0;
    }else{
        while (PHead) {
            lenth++;
            PHead = PHead->next;
        }
    }
    printf("LinkList Lenth:%d\n",lenth);
    return lenth;
}

Status getElement(LinkListNodeRef pHead, ElemTypeI i, ElemTypeIRef e){
    if (pHead == 0) {
        return FALSe;
    }
    ElemTypeI lenth = getLengthLinkList(pHead);
    if (i >lenth || i < 0) {
        return FALSe;
    }
    ElemTypeI counter = 0;
    while (pHead) {
        counter++;
        if (i == counter) {
            *e = pHead->data;
            break;
        }
        pHead = pHead->next;
    }
    return OK;
}


Status isEmptyLinkList(LinkListNodeRef pHead){
    if (pHead == 0) {
        return FALSe;
    }
    return OK;
}

Status isExistElem(LinkListNodeRef pHead, ElemTypeI e){
    if (pHead == 0) {
        return FALSe;
    }
    while (pHead) {
        if (pHead->data == e) {
            return OK;
        }
        pHead = pHead->next;
    }
    return FALSe;
}
//添加到表尾部
Status insertElem(LinkListNodeRef pHead, ElemTypeI e){
    if (pHead == 0) {
        return FALSe;
    }
    while (pHead) {
        pHead = pHead->next;
    }
    LinkListNodeRef p1 = (LinkListNodeRef)malloc(sizeof(LinkListNode));
    p1->data = e;
    p1->next = 0;
    pHead = p1;
    return OK;
}

/* 12.向单链表中第pos个结点位置插入元素为x的结点，若插入成功返回１，否则返回０ */


/* 13.向有序单链表中插入元素x结点，使得插入后仍然有序 */
/* 14.从单链表中删除表头结点，并把该结点的值返回，若删除失败则停止程序运行 */
/* 15.从单链表中删除表尾结点并返回它的值，若删除失败则停止程序运行 */
/* 16.从单链表中删除第pos个结点并返回它的值，若删除失败则停止程序运行 */
/* 17.从单链表中删除值为x的第一个结点，若删除成功则返回1,否则返回0 */
/* 18.交换2个元素的位置 */
/* 19.将线性表进行快速排序 */
