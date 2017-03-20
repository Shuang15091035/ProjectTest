//
//  Twll.c
//  DataStruct
//
//  Created by mac zdszkj on 2017/3/6.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#include "Twll.h"
#include "stdlib.h"

void InitDLinkList(DLinkList *linkList){
    (*linkList) = (DLinkList)malloc(sizeof(DNode));
    (*linkList)->data = 0;
    (*linkList)->prior = (*linkList);
    (*linkList)->next = (*linkList);
}

void AddElem(DLinkList linkList, int data){
    DLinkList node = (DLinkList)malloc(sizeof(DNode));
    node->data = data;
    DLinkList p1 = linkList;
    while (p1->next != linkList) {
        p1 = p1->next;
    }
    node->next = p1->next;
    p1->next = node;
    node->prior = p1;
}

void PrintDlinkList(DLinkList LinkList){
    DLinkList p1 = LinkList;
    while (p1->next != LinkList) {
        p1 = p1->next;
        printf("LinkList->data:%d",p1->data);
    }
    
}
