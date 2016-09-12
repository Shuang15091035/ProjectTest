//
//  Stack.h
//  DataStruct
//
//  Created by mac zdszkj on 16/9/12.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//
#include <stdio.h>
#include <stdlib.h>
#include "Common.h"
#define stack_init_size 100
#define stackIncrement 10

typedef struct{
    
    int *base;
    int *top;
    int stackSize;
    
}SqStack;

Status InitStack(SqStack **s);
Status GetTop(SqStack *s, int *e);
Status Push(SqStack **s, int e);
Status Pop(SqStack **s, int *e);

