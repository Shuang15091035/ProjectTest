//
//  JCList.h
//  June Winter
//
//  Created by GavinLo on 16-6-23.
//  Copyright 2015 luojunwen123@gmail.com. All rights reserved.
//

#ifndef __jw__JCList__
#define __jw__JCList__

#include <jw/JCArray.h>

JC_BEGIN

typedef JCInt (*JCListComparator)(JCObjectRefC l, JCObjectRefC r);

typedef struct {
    
    JCArray array;
    JCULong count;
    
// private
    JCListComparator _comparator;
    
}JCList;

typedef JCList* JCListRef;
typedef const JCList* JCListRefC;

JCList JCListNull();
JCList JCListMake(JCULong sizeOfElement);
void JCListFree(JCListRef list);
JCULong JCListGetCount(JCListRefC list);
JCBool JCListGet(JCListRefC list, JCULong index, JCOut JCObjectRef value);
JCBool JCListSet(JCListRef list, JCULong index, JCObjectRefC value);
JCBool JCListMove(JCListRef list, JCULong from, JCULong to, JCObjectRef temp);
void JCListInsert(JCListRef list, JCULong index, JCObjectRefC value);
void JCListPush(JCListRef list, JCObjectRefC value);
void JCListSetComparator(JCListRef list, JCListComparator comparator);
JCULong JCListIndexOf(JCListRefC list, JCObjectRefC value, JCObjectRef tempValue);
JCBool JCListRemove(JCListRef list, JCULong index, JCObjectRef value);
void JCListClear(JCListRef list);
JCBool JCListIsNull(JCListRefC list);

#define JCListOf(type) JCList
#define JCListNew(type) JCListMake(sizeof(type))

JC_END

#endif /* defined(__jw__JCList__) */
