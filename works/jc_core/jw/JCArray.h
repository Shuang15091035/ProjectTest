//
//  JCArray.h
//  June Winter
//
//  Created by GavinLo on 14-10-17.
//  Copyright 2015 luojunwen123@gmail.com. All rights reserved.
//

#ifndef __jw__JCArray__
#define __jw__JCArray__

#include <jw/JCBuffer.h>

JC_BEGIN

typedef struct {
    
    JCULong sizeOfElement;
    JCBuffer buffer;
    
}JCArray;

typedef JCArray* JCArrayRef;
typedef const JCArray* JCArrayRefC;

JCArray JCArrayMake(JCULong sizeOfElement, JCULong numElements);
void JCArrayFree(JCArrayRef array);
JCULong JCArrayGetNumElements(JCArrayRefC array);
JCBool JCArrayGet(JCArrayRefC array, JCULong index, JCOut JCObjectRef value);
JCBool JCArraySet(JCArrayRef array, JCULong index, JCObjectRefC value);
JCBool JCArrayAdd(JCArrayRef array, JCObjectRefC value);
JCBool JCArrayMove(JCArrayRef array, JCULong fromIndex, JCULong toIndex);
void JCArrayClear(JCArrayRef array);

/**
 * 数据复制
 * 注：如果目标数组与源数组元素大小不一致，则返回false
 */
JCBool JCArrayCopy(JCArrayRefC src, JCArrayRef dst);

JC_END

#endif /* defined(__jw__JCArray__) */
