//
//  JCQuadrangle.h
//  jc_core
//
//  Created by ddeyes on 16/5/23.
//  Copyright © 2016年 luojunwen123@gmail.com. All rights reserved.
//

#ifndef JCQuadrangle_h
#define JCQuadrangle_h

#include <jw/JCBase.h>
#include <jw/JCVector3.h>

JC_BEGIN

typedef enum {
    
    JCQuadrangleNumPoints = 4,
    
}JCQuadrangleConstants;

typedef enum {
    
    JCQuadrangleConerTopLeft = 0,
    JCQuadrangleConerBottomLeft,
    JCQuadrangleConerBottomRight,
    JCQuadrangleConerTopRight,
    
}JCQuadrangleConer;

typedef struct {
    
    JCVector3 point[JCQuadrangleNumPoints];
    
}JCQuadrangle;

typedef JCQuadrangle* JCQuadrangleRef;
typedef const JCQuadrangle* JCQuadrangleRefC;

JCQuadrangle JCQuadrangleZero();
JCQuadrangle JCQuadrangleMake(JCVector3 topLeft, JCVector3 bottomLeft, JCVector3 bottomRight, JCVector3 topRight, JCBool sort);

/**
 * 获取中心（忽略凹多边形）
 */
JCVector3 JCQuadrangleGetCenter(JCQuadrangleRefC quad);

typedef struct {
    
    JCUShort index[JCQuadrangleNumPoints];
    
}JCQuadrangleIndices;

JCQuadrangleIndices JCQuadrangleIndicesMake(JCUShort topLeft, JCUShort bottomLeft, JCUShort bottomRight, JCUShort topRight);

JC_END

#endif /* JCQuadrangle_h */
