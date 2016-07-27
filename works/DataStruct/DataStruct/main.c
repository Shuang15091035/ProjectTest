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
#import "Common.h"
#import "DynamicLinkList.h"
#import "StaticLinkList.h"
#import "MergeLinkList.h"

Status dynamicLinkListUsage();
Status staticLinkList();

int main(int argc, const char * argv[]) {
    
    dynamicLinkListUsage();
    
    return 0;
}
Status staticLinkList(){
    
    return OK;
}
Status dynamicLinkListUsage(){
    LinkListNode *pList=0x00;
    int length = 0;
    
    initLinkList(&pList);
    
    pList = createLinkList();
    printLinkList(pList);
    
    length = getLengthLinkList(pList);
    
    Status isEmpty = isEmptyLinkList(pList);
    printf("%d",isEmpty);
    
    ElemTypeI e = 0;
    getElement(pList, 3, &e);
    printLinkList(pList);
    
//    insertElem(pList, 12);
//    printLinkList(pList);
//    
    ElemTypeI inserPosition = 2;
    if (inserPosition > 0 || inserPosition <= getLengthLinkList(pList)) {
        insertElemByPosition(&pList, inserPosition, 56);
        printLinkList(pList);
    }
//    ElemTypeI modifyPosition = 2;
//    if (modifyPosition > 0 || modifyPosition <= getLengthLinkList(pList)) {
//        modifyElem(pList,modifyPosition,1);
//        printLinkList(pList);
//    }
    
    //
    //    insertHeadList(&pList,5);
    //    printLinkList(pList);
    //
    //    insertLastList(&pList,10);
    //   printLinkList(pList);
    
    clearLinkList(pList);      //清空链表
    //    system("pause");
    return OK;
    
}