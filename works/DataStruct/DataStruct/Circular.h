//
//  Circular.h
//  DataStruct
//
//  Created by mac zdszkj on 16/8/15.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#ifndef Circular_h
#define Circular_h
#include "Common.h"
#include <stdio.h>

typedef struct CLLNode{
    ElemTypeI data;
    struct CLLNode *proidNode;
    struct CLLNode *nextNode;
};

#endif /* Circular_h */
