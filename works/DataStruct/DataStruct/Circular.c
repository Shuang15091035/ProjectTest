//
//  Circular.c
//  DataStruct
//
//  Created by mac zdszkj on 16/8/15.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#include "Circular.h"





//List Creatlist(int n)
//{
//    List head,p;
//    int i;
//    head=(Node*)malloc(sizeof(Node));
//    if(!head)
//    {
//        printf("memory allocation error!\n");
//        exit(1);
//    }
//    head->data=1; head->next=head;
//    for(i=n;i>1;--i)
//    {
//        p=(Node*)malloc(sizeof(Node));
//        if(!p)
//        {
//            printf("memory allocation error!\n");
//            exit(1);
//        }
//        p->data=i; p->next=head->next; head->next=p;
//    }
//    return head;
//}
//
//void Output(List head)
//{
//    List p=head;
//    do
//    {
//        printf("%d",p->data);
//        p=p->next;
//    }while(p!=head);
//    printf("\n");
//}
//
//void Play(List head,int n,int m) //第一种方法
//{
//    List p,q;
//    int c,k;
//    p=head; c=1; k=n;
//    while(k>1)
//    {
//        if(c==m-1)
//        {
//            q=p->next; p->next=q->next;
//            printf("%d",q->data);
//            c=0; --k;
//        }
//        else {c++; p=p->next;}
//    }
//    printf("The winner is %d\n",p->data);
//}
//
//void Josephus(List h,int n,int m)//第二种方法
//{
//    Node* p=h,*pre=NULL;
//    int i,j;
//    for(i=0;i<n-1;++i)
//    {
//        for(j=1;j<m;++j)
//        {
//            pre=p;
//            p=p->next;
//        }
//        printf("The out number is %d\n",p->data);
//        pre->next=p->next;
//        p=pre->next;
//    }
//    printf("The winner is %d\n",p->data);
//}

//int main()
//{
//    List head;
//    int n,m;
//    printf("Input the n and m :\n");
//    scanf("%d%d",&n,&m);
//    head=Creatlist(n);
//    Output(head);
//    Josephus(head,n,m);
//    
//    return 0;
//}

Status CreateList(List *head, int n){
    
    List point = 0X00;
    *head = (List)malloc(sizeof(Node));
    if((*head) == 0X00) return FALSe;
    (*head)->next = *head;
    (*head)->data = 1;
    for (int i = 2; i < n; i++) {
        point = (List)malloc(sizeof(Node));
        if(point == 0X00) return FALSe;
        point->data = i;
        point->next = (*head)->next;
        (*head)->next = point;
    }
    return OK;
}

Status StartPlay(List head, int n, int m){
    
    
    return OK;
}

