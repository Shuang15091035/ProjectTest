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
#include "StaticLinkList.h"
#include "MergeLinkList.h"
#include "Stack.h"
#include "Twll.h"

int main(int argc, const char * argv[]) {
    //构造循环队列可以使用取余运算符%实现
    
//    dynamicLinkListUsage();
//    staticLinkList();
//    CreateLinList(&head);
//    SeqlistStackUsage();
    
    return 0;
}

Status TwoWayLinkList(){
    DLinkList LinkList;
    InitDLinkList(&LinkList);
    AddElem(LinkList, 10);
    AddElem(LinkList, 20);
    AddElem(LinkList, 30);
    AddElem(LinkList, 40);
    PrintDlinkList(LinkList);
    return OK;
}

Status SeqlistStackUsage(){

    int a = 10;
    SqStack *stack;
    InitStack(&stack);
    Push(&stack,a);
    int b = 20;
    Push(&stack,b);
    int c = 0;
    GetTop(stack,&c);
    int d = 0;
    Pop(&stack,&d);
    printf("a:%d,b:%d,c:%d,d:%d\n",a,b,c,d);
    
    return OK;
}

Status staticLinkList(){
    StaticListNode SSL;
    CreateStaticList(SSL);
    for (int i = 0; i < 5; i++)
        StaticList_Insert(SSL, i+1, i+1);
    staticList_traverse(SSL);
    staticList_delete(SSL, 3, 3);
    staticList_traverse(SSL);
    StaticList_Insert(SSL, 10, 3);
    staticList_traverse(SSL);
    return OK;
}

Status dynamicLinkListUsage(){
    LinkListNode *pList=0x00;
    int length = 0;
    
    initLinkList(&pList);
    
//    createList(pList);
    pList = createLinkList();
    printLinkList(pList);
    
    length = getLengthLinkList(pList);
    
    Status isEmpty = isEmptyLinkList(pList);
    printf("%d",isEmpty);
    
    ElemTypeI e = 0;
    getElement(pList, 3, &e);
    printLinkList(pList);
    
    insertElem(pList, 12);
    printLinkList(pList);
    
    ElemTypeI inserPosition = 2;
    if (inserPosition > 0 || inserPosition <= getLengthLinkList(pList)) {
        insertElemByPosition(&pList, inserPosition, 56);
        printLinkList(pList);
    }
    
    ListInsert_L(&pList, 1, 99);
    printLinkList(pList);

    ElemTypeI value = 0;
    deleteElemByPosition(&pList, 1, &value);
    printLinkList(pList);
    
    clearLinkList(pList);      //清空链表
    //    system("pause");
    return OK;
    
}
