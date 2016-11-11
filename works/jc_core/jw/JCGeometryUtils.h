//
//  JCGeometryUtils.h
//  June Winter
//
//  Created by GavinLo on 14/12/10.
//  Copyright 2015 luojunwen123@gmail.com. All rights reserved.
//

#ifndef __jw__JCGeometryUtils__
#define __jw__JCGeometryUtils__

#include <jw/JCBase.h>
#include <jw/JCRay3.h>
#include <jw/JCBounds3.h>
#include <jw/JCLine.h>
#include <jw/JCPlane.h>
#include <jw/JCTriangle.h>

JC_BEGIN

typedef struct {
    
    JCBool hit;
    JCFloat distance;
    
}JCRayBounds3IntersectResult;

JCRayBounds3IntersectResult JCRayBounds3IntersectResultMake(JCBool hit, JCFloat distance);

typedef struct {
    
    JCBool hit;
    JCFloat distance;
    JCVector3 point;
    
}JCRayPlaneIntersectResult;

JCRayPlaneIntersectResult JCRayPlaneIntersectResultMake(JCBool hit, JCFloat distance);
JCVector3 JCRayPlaneIntersectResultGetHitPoint(const JCRayPlaneIntersectResult* result, JCRay3RefC ray);

JCRayBounds3IntersectResult JCRayBounds3Intersect(JCRay3RefC ray, JCBounds3RefC bounds);
JCRayPlaneIntersectResult JCRayPlaneIntersect(JCRay3RefC ray, JCPlaneRefC plane);

typedef struct {
    
    JCBool hit;
    JCFloat distance;
    JCVector3 point;
    
}JCRayTriangleIntersectResult;

JCRayTriangleIntersectResult JCRayTriangleIntersectResultTrue();
JCRayTriangleIntersectResult JCRayTriangleIntersectResultFalse();
JCRayTriangleIntersectResult JCRayTriangleIntersectResultMake(JCBool hit, JCFloat distance);
JCVector3 JCRayTriangleIntersectResultGetHitPoint(const JCRayTriangleIntersectResult* result, JCRay3RefC ray);

/**
 * 射线三角形求交
 * @param ray 射线
 * @param triangle 三角形
 * @param cullBack 是否排除背面求交
 */
JCRayTriangleIntersectResult JCRayTriangleIntersect(JCRay3RefC ray, JCTriangleRefC triangle, JCBool cullBack);

typedef struct {
    
    JCBool hit;
    JCFloat t;
    JCVector3 point;
    
}JCLineIntersectResult;

JCLineIntersectResult JCLineIntersectResultTrue();
JCLineIntersectResult JCLineIntersectResultFalse();
JCLineIntersectResult JCLineIntersectResultMake(JCBool hit, JCVector3 start, JCFloat t);
JCVector3 JCLineIntersectResultGetHitPoint(const JCLineIntersectResult* result, JCLineRefC line);

JCLineIntersectResult JCLineIntersect(JCLineRefC line1, JCLineRefC line2);

JC_END

#endif /* defined(__jw__JCGeometryUtils__) */
