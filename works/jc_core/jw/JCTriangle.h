//
//  JCTriangle.h
//  jc_core
//
//  Created by ddeyes on 16/5/23.
//  Copyright © 2016年 luojunwen123@gmail.com. All rights reserved.
//

#ifndef JCTriangle_h
#define JCTriangle_h

#include <jw/JCBase.h>
#include <jw/JCVector3.h>

JC_BEGIN

typedef enum {
    
    JCTriangleNumPoints = 3,
    
}JCTriangleConstants;

typedef enum {
    
    JCTrianglePositionStart = 0,
    JCTrianglePositionMiddle,
    JCTrianglePositionEnd,
    
}JCTrianglePosition;

typedef struct {
    
    JCVector3 point[JCTriangleNumPoints];
    
}JCTriangle;

typedef JCTriangle* JCTriangleRef;
typedef const JCTriangle* JCTriangleRefC;

JCTriangle JCTriangleMake(JCVector3 start, JCVector3 middle, JCVector3 end);

/**
 * 获取中心
 */
JCVector3 JCTriangleGetCenter(JCTriangleRefC triangle);

/**
 * 点是否在三角形内（只计算XZ平面投影）
 */
JCBool JCTrianglePointIsInsideXZ(JCTriangleRefC triangle, JCVector3 point);

typedef struct {
    
    JCUInt index[JCTriangleNumPoints];
    
}JCTriangleIndices;

JCTriangleIndices JCTriangleIndicesMake(JCUShort start, JCUShort middle, JCUShort end);

JC_END

#endif /* JCTriangle_h */
