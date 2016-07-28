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
#include "StaticLinkList.h"
#include "MergeLinkList.h"

Status dynamicLinkListUsage();
Status staticLinkList();

int main(int argc, const char * argv[]) {
    
//    dynamicLinkListUsage();
    
    return 0;
}
Status staticLinkList(){
    
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