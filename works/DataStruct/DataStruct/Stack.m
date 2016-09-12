//
//  Stack.m
//  DataStruct
//
//  Created by mac zdszkj on 16/9/12.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "Stack.h"

Status InitStack(SqStack **s){
    
    (*s)->base = (int*)malloc(sizeof(int)*stack_init_size);
    if(!((*s)->base)) exit(0);
    (*s)->top = (*s)->base;
    (*s)->stackSize = stack_init_size;
    printf("initStack");
    return OK;
}

Status GetTop(SqStack *s, int *e){
    
    if(s->top == s->base) return -1;
    *e = *(s->top-1);
    printf("GetTop");
    return OK;
}
Status Push(SqStack **s, int e){
    
    if((*s)->top - (*s)->base >= (*s)->stackSize){
        (*s)->base = (int *)realloc((*s)->base,((*s)->stackSize + stackIncrement)*sizeof(int));
        if(!(*s)->base) return -1;
        (*s)->top = (*s)->base + (*s)->stackSize;
        (*s)->stackSize += stackIncrement;
    }
    *(*s)->top++ = e; //将*(*s)->top = e; (*s)->top++;
    printf("pushStack");
    return OK;
}

Status Pop(SqStack **s, int *e){
    
    if((*s)->base == (*s)->top) return -1;
    *e = *--(*s)->top;
    printf("PopStack");
    return OK;
}

#pragma mark - demo

//#pragma mark - Stack
////顺序栈的使用
////作者：nuaazdh
////时间：2011年12月5日
//#include <stdio.h>
//#include <stdlib.h>
//
//#define OK      1
//#define ERROR   0
//#define TRUE    1
//#define FALSE   0
//#define STACK_INIT_SIZE 100
//#define STACKINCREMENT 10
//
//typedef int Status; //函数返回状态
//typedef char SElemType;  //栈元素类型
//typedef struct{//栈结构定义
//    SElemType *base;
//    SElemType *top;
//    int stacksize;
//}SqStack;
//
//Status InitStack(SqStack *S);
////构造一个空栈S
//Status DestroyStack(SqStack *S);
////销毁栈S，S不再存在
//Status ClearStack(SqStack *S);
////把栈S置为空栈
//Status StackEmpty(SqStack S);
////若栈S为空栈，则返回TRUE，否则返回FALSE
//int StackLength(SqStack S);
////返回S元素的个数，即栈的长度
//Status GetTop(SqStack S,SElemType *e);
////若栈不为空，则用e返回S的栈顶元素，并返回OK；否则返回FALSE
//Status Push(SqStack *S,SElemType e);
////插入元素e为新的栈顶元素
//Status Pop(SqStack *S,SElemType *e);
////若栈S不为空，则删除S的栈顶元素，用e返回其值，并返回OK,否则返回ERROR
//Status StackTraverse(const SqStack *S);
////从栈底到栈顶依次对每个元素进行访问
//
////===========交互处理=============//
//
//void PrintMsg(){//输出操作提示信息
//    printf("--Welcome--\n");
//    printf("请输入您要进行的操作：\n");
//    printf("I：元素入栈\n");
//    printf("O：元素出栈\n");
//    printf("A：查看栈中所有元素\n");
//    printf("C：清空栈\n");
//    printf("T：查看栈顶元素\n");
//    printf("L：查看栈空间\n");
//    printf("Q：退出\n");
//    printf("others：do nothing.\n");
//    printf(">>");
//}
//
//char getOption(){//获取用户输入操作
//    char input;
//    scanf("%c",&input);
//    flushall();
//    input=toupper(input);
//    return input;
//}
//
//SElemType getElemInput(){//获得入栈元素
//    SElemType e;
//    printf("请输入一个字符：");
//    scanf("%c",&e);
//    flushall();
//    return e;
//}
//
//int main()
//{
//    char op;
//    SqStack stack;//栈
//    SElemType e;//元素
//    PrintMsg();
//    InitStack(&stack);
//    while(op=getOption()){
//        if(op=='Q')//退出
//            break;
//        switch(op){
//            case 'I'://入栈
//                e=getElemInput();
//                Push(&stack,e);
//                break;
//            case 'O'://出栈
//                Pop(&stack,&e);
//                break;
//            case 'A'://遍历栈中元素
//                StackTraverse(&stack);
//                break;
//            case 'C'://清空栈
//                ClearStack(&stack);
//                break;
//            case 'T'://获取栈顶
//                GetTop(stack,&e);
//                break;
//            case 'L'://获取栈空间
//                printf("栈空间：%d\n",StackLength(stack));
//                break;
//            default:
//                break;
//        }//switch
//        printf(">>");
//    }//while
//    printf("--Bye bye--\n");
//    return 0;
//}
//
//Status InitStack(SqStack *S){
//    //构造一个空栈S
//    S->base=(SElemType *)malloc(STACK_INIT_SIZE*sizeof(SElemType));
//    if(!S->base)//分配失败
//    {
//        printf("分配内存失败.\n");
//        exit(0);
//    }
//    S->top=S->base;
//    S->stacksize=STACK_INIT_SIZE;
//    return OK;
//}
//
//Status DestroyStack(SqStack *S){
//    //销毁栈S，S不再存在
//    if(!S)//S为空
//    {
//        printf("指针为空，释放失败.\n");
//        exit(0);
//    }
//    free(S);
//    return OK;
//}
//
//Status ClearStack(SqStack *S){
//    //把栈S置为空栈
//    if(!S)//S不存在
//        return FALSE;
//    S->top=S->base;//直接将栈顶指针指向栈底
//    return OK;
//}
//
//Status StackEmpty(SqStack S){
//    //若栈S为空栈，则返回TRUE，否则返回FALSE
//    if(S.top==S.base)
//        return TRUE;
//    else
//        return FALSE;
//}
//
//int StackLength(SqStack S){
//    //返回S元素的个数，即栈的长度
//    return S.stacksize;
//}
//
//Status GetTop(SqStack S,SElemType *e){
//    //若栈不为空，则用e返回S的栈顶元素，并返回OK；否则返回FALSE
//    if(S.top==S.base){
//        printf("栈为空.\n");
//        return FALSE;
//    }else{
//        e=S.top-1;
//        printf("栈顶元素：%c\n",*e);
//        return OK;
//    }
//}
//
//Status Push(SqStack *S,SElemType e){
//    //插入元素e为新的栈顶元素
//    if(S->top-S->base>=S->stacksize){//栈已满，追加存储空间
//        S->base=(SElemType *)realloc(S->base,(S->stacksize+STACKINCREMENT)*sizeof(SElemType));
//        if(!S->base)
//        {
//            printf("重新申请空间失败.\n");
//            exit(0);
//        }
//        S->top=S->base+S->stacksize;//更改栈顶指针
//        S->stacksize+=STACKINCREMENT;
//    }
//    *S->top++=e;
//    return OK;
//}
//
//Status Pop(SqStack *S,SElemType *e){
//    //若栈S不为空，则删除S的栈顶元素，用e返回其值，并返回OK,否则返回ERROR
//    if(S->top==S->base){//栈为空
//        printf("栈为空.\n");
//        return ERROR;
//    }
//    e=--S->top;
//    return OK;
//}
//
//Status StackTraverse(const SqStack *S){
//    //从栈底到栈顶依次对每个元素进行访问
//    SElemType *p=S->base;
//    if(S->base==S->top)
//    {
//        printf("栈为空.\n");
//        return FALSE;
//    }
//    printf("栈中元素：");
//    while(p!=S->top)
//    {
//        printf("%c ",*p++);
//    }
//    printf("\n");
//    return OK;  
//}

