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
    
    initLinkList(&pList);       //链表初始化
    
    pList = createLinkList();
    //    pList = createLinkList(); //创建链表
    printLinkList(pList);
    
    //遍历链表，打印链表
    length = getLengthLinkList(pList);
    
    isEmptyLinkList(pList);     //判断链表是否为空链表
    ElemTypeI e;
    getElement(pList, 3, &e);
    
    //获取第三个元素，如果元素不足3个，则返回0
    printf("getElement函数执行，位置 3 中的元素为 %d\n",e);
    printLinkList(pList);
    
    
    insertElem(pList, 12);
    
    //    modifyElem(pList,4,1);  //将链表中位置4上的元素修改为1
    //    printLinkList(pList);
    //
    //    insertHeadList(&pList,5);   //表头插入元素12
    //    printLinkList(pList);
    //
    //    insertLastList(&pList,10);  //表尾插入元素10
    //   printLinkList(pList);
    
    clearLinkList(pList);      //清空链表
    //    system("pause");
    return OK;
    
}