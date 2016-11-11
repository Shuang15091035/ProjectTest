//
//  JCPolygon.h
//  jc_core
//
//  Created by ddeyes on 16/6/23.
//  Copyright © 2016年 luojunwen123@gmail.com. All rights reserved.
//

#ifndef JCPolygon_h
#define JCPolygon_h

#include <jw/JCBase.h>
#include <jw/JCVector3.h>
#include <jw/JCList.h>
#include <jw/JCTriangle.h>

JC_BEGIN

typedef struct {
    
    JCListOf(JCVector3) points;
    JCListOf(JCPolygonRefC) holes;
    
// private
    JCListOf(JCULong) _tempIndices; // for Triangulation
    JCListOf(JCTriangleIndices) _resultIndices; // for Triangulation
    
}JCPolygon;

typedef JCPolygon* JCPolygonRef;
typedef const JCPolygon* JCPolygonRefC;

JCPolygon JCPolygonMake();
JCULong JCPolygonGetNumPoints(JCPolygonRefC polygon);
JCVector3 JCPolygonGetPoint(JCPolygonRefC polygon, JCULong index);
JCBool JCPolygonSetPoint(JCPolygonRef polygon, JCULong index, JCVector3 point);
void JCPolygonInsertPoint(JCPolygonRef polygon, JCULong index, JCVector3 point);
void JCPolygonAddPoint(JCPolygonRef polygon, JCVector3 point);
JCULong JCPolygonGetNumHoles(JCPolygonRefC polygon);
void JCPolygonAddHole(JCPolygonRef polygon, JCPolygonRefC hole);
void JCPolygonRemoveHole(JCPolygonRef polygon, JCPolygonRefC hole);
void JCPolygonClearHoles(JCPolygonRef polygon);
JCPolygonRefC JCPolygonGetHole(JCPolygonRefC polygon, JCULong index);

/**
 * XZ平面的投影面积
 */
JCFloat JCPolygonAreaProjectXZ(JCPolygonRefC polygon);

/**
 * 获取中心（忽略凹多边形）
 */
JCVector3 JCPolygonGetCenter(JCPolygonRefC polygon);

/**
 * 多边形中是否有边彼此相交
 */
JCBool JCPolygonIsEdgeIntersect(JCPolygonRefC polygon);

/**
 * 简单的多边形三角形化（忽略y轴，俯视角度看，不支持挖洞）
 */
typedef enum {
    
    JCPolygonSimpleTriangulationModeCounterClockwise = 0, // 逆时针
    JCPolygonSimpleTriangulationModeClockwise, // 顺时针
    
}JCPolygonSimpleTriangulationMode;

typedef void (*JCPolygonTriangulationAddIndexFunc)(JCObjectRef target, JCTriangleIndices indices);
//JCBool JCPolygonSimpleTriangulateXZ(JCPolygonRef polygon, JCPolygonSimpleTriangulationMode mode, JCObjectRef resultTarget, JCPolygonTriangulationAddIndexFunc resultAddIndex);
JCListRefC JCPolygonSimpleTriangulateXZ(JCPolygonRef polygon, JCPolygonSimpleTriangulationMode mode);

typedef void (*JCPolygonDebugOutputFunc)(JCPolygonRefC polygon);
void JCPolygonSetDebugOutput(JCPolygonDebugOutputFunc func);

JC_END

#endif /* JCPolygon_h */
