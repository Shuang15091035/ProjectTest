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
    printf("LinkList create finish %p\n",pHead);
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
        while (PHead != 0) {
            lenth++;
            PHead = PHead->next;
        }
    }
    printf("LinkList Lenth:%d\n",lenth);
    return lenth;
}

Status getElement(LinkListNodeRef pHead, ElemTypeI i, ElemTypeIRef e){
    if (pHead == 0) {
        printf("LinkList is inexistence");
        return FALSe;
    }
    ElemTypeI length = getLengthLinkList(pHead);
    if (i >length || i < 0) {
        printf("LinkList Length < 0 || length > length");
        return FALSe;
    }
    ElemTypeI counter = 0;
    while (pHead == 0) {
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
    while (pHead != 0) {
        if (pHead->data == e) {
            return OK;
        }
        pHead = pHead->next;
    }
    return FALSe;
}

Status insertElem(LinkListNodeRef pHead, ElemTypeI e){
    if (pHead == 0) {
        return FALSe;
    }
    while (pHead->next != 0) {
        pHead = pHead->next;
    }
    LinkListNodeRef p1 = (LinkListNodeRef)malloc(sizeof(LinkListNode));
    p1->data = e;
    p1->next = 0;
    pHead->next = p1;
    printf("insert Element finish (%p)\n",pHead);
    return OK;
}

Status insertElemByPosition(LinkListNodeRef *pHead, ElemTypeI position,ElemTypeI elem){
    //NOTE:You need to consider two cases
    if (pHead == 0) {
        return FALSe;
    }
    ElemTypeI linkListLength = getLengthLinkList(*pHead);
    if (position < 0 || position > linkListLength) {
        return FALSe;
    }
    if (position == 1) {//Insert point to nodes need to change the head to head
        LinkListNodeRef node = (LinkListNodeRef)malloc(sizeof(LinkListNode));
        memset(node, 0, sizeof(LinkListNode));
        node->data = elem;
        node->next = *pHead;
        *pHead = node;
        return OK;
    }
    ElemTypeI counter = 1;
    while (*pHead != 0) {//
        counter++;
        if (counter == position) {
            LinkListNodeRef node = (LinkListNodeRef)malloc(sizeof(LinkListNode));
            node->data = elem;
            node->next = (*pHead)->next;
            (*pHead)->next = node;
            break;
        }
        *pHead = (*pHead)->next;
    }
    return OK;
}

Status deleteLinkListElemByPositin(LinkListNodeRef *pHead,ElemTypeI position){
    //NOTE:You need to consider two cases
    if (pHead == 0) {
        return FALSe;
    }
    if (position < 0 || position > getLengthLinkList(*pHead)) {
        return FALSe;
    }
    if (position == 1) {
        LinkListNodeRef tempNode = 0x00;
        tempNode = *pHead;
        (*pHead) = (*pHead)->next;
        free(tempNode);
        return OK;
    }
    return OK;
}


/* 16.从单链表中删除第pos个结点并返回它的值，若删除失败则停止程序运行 */
Status deleteElemByPosition(LinkListNodeRef pHead,ElemTypeI position,ElemTypeI value){
    if (pHead == 0) {
        return FALSe;
    }
    if (position < 0 || position > getLengthLinkList(pHead)) {
        return FALSe;
    }
    LinkListNodeRef *changeNode = 0X00;
    ElemTypeI counter = 0;
    while (pHead != 0) {
        if (counter == position) {
            pHead->next;
        }
        counter++;
        pHead = pHead->next;
    }
    
    return OK;
}

//sysytem implement insertElement
Status ListInsert_L(LinkListNodeRef *pHead, int i, ElemTypeI e){

	p = *pHead;j = 0;
	while(p && j < i-1){p = p->next; ++j;}
	if(!p || j > i)return FALSe;
	s = (LinkListNodeReef)malloc(sizeof(LinkListNOde));
	s->data = e;s->next = p->next;
	p->next = s;
	return OK;
}


/* 17.从单链表中删除值为x的第一个结点，若删除成功则返回1,否则返回0 */
/* 18.交换2个元素的位置 */
/* 19.将线性表进行快速排序 */
