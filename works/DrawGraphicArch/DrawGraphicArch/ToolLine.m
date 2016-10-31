//
//  DALine.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/9/20.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ToolLine.h"
#include <math.h>
#define EPS 0.00001

@interface ToolLine(){
    CGPoint mPoint1;
    CGPoint mPoint2;
    CGFloat mLineK;
}
@end

@implementation ToolLine

@synthesize point1 = mPoint1;
@synthesize point2 = mPoint2;
@synthesize likeK = mLineK;

- (id)initWithPoint1:(CGPoint)point1 point2:(CGPoint)point2{
    self = [super init];
    if (self != nil) {
        mPoint1 = point1;
        mPoint2 = point2;
        mLineK = (mPoint2.y - mPoint1.y) / (mPoint2.x - mPoint1.x);
    }
    return self;
}

/****点到直线的距离***
 * 过点（x1,y1）和点（x2,y2）的直线方程为：KX -Y + (x2y1 - x1y2)/(x2-x1) = 0
 * 设直线斜率为K = (y2-y1)/(x2-x1),C=(x2y1 - x1y2)/(x2-x1)
 * 点P(x0,y0)到直线AX + BY +C =0DE 距离为：d=|Ax0 + By0 + C|/sqrt(A*A + B*B)
 * 点（x3,y3）到经过点（x1,y1）和点（x2,y2）的直线的最短距离为：
 * distance = |K*x3 - y3 + C|/sqrt(K*K + 1)
 */

-(NSInteger) GetMinDistance:(CGPoint) pt3{
    
    double dis = 0;
    if (mPoint1.x == mPoint2.x)
    {
        dis = fabs(pt3.x - mPoint1.x);
        return dis;
    }
    double lineK = (mPoint2.y - mPoint1.y) / (mPoint2.x - mPoint1.x);
    double lineC = (mPoint2.x * mPoint1.y - mPoint1.x * mPoint2.y) / (mPoint2.x - mPoint1.x);
    dis = fabs(lineK * pt3.x - pt3.y + lineC) / (sqrt(lineK * lineK + 1));
    return dis;
}

- (BOOL) isOnLineOfPoint:(CGPoint) touchPoint{
    
    double lineK = (mPoint2.y - mPoint1.y) / (mPoint2.x - mPoint1.x);
    double lineC = (mPoint2.x * mPoint1.y - mPoint1.x * mPoint2.y) / (mPoint2.x - mPoint1.x);
    double result = lineK * touchPoint.x - touchPoint.y + lineC;
    if (result == 0) {
        return YES;
    }
    return NO;
}

/** @ brief 根据两点求出垂线过第三点的直线的交点
 @ param pt1 直线上的第一个点
 @ param pt2 直线上的第二个点
 @ param pt3 垂线上的点
 @ return 返回点到直线的垂直交点坐标
 */
//- (CGPoint) getPointOnVerticalLine:(CGPoint) pt3
//{
//    double A = (mPoint1.y - mPoint2.y)/(mPoint1.x - mPoint2.x);
//    double B = (mPoint1.y-A * mPoint1.y);
//    /// > 0 = ax +b -y;  对应垂线方程为 -x -ay + m = 0;(mm为系数)
//    /// > A = a; B = b;
//    double m = pt3.x + A * pt3.y;
//    
//    /// 求两直线交点坐标
//    CGPoint ptCross;
//    ptCross.x = (m - A * B)/(A * A + 1);
//    ptCross.y = A * ptCross.x + B;
//    return ptCross;
//}

/**第一种： 设直线方程为ax+by+c=0,点坐标为(m,n)
 则垂足为((b*b*m-a*b*n-a*c)/(a*a+b*b),(a*a*n-a*b*m-b*c)/(a*a+b*b))
 
 第二种：计算点到线段的最近点
 
 如果该线段平行于X轴（Y轴），则过点point作该线段所在直线的垂线，垂足很容
 易求得，然后计算出垂足，如果垂足在线段上则返回垂足，否则返回离垂足近的端
 点；
 
 如果该线段不平行于X轴也不平行于Y轴，则斜率存在且不为0。设线段的两端点为
 pt1和pt2，斜率为：
 k = ( pt2.y - pt1. y ) / (pt2.x - pt1.x );
 该直线方程为：
 y = k* ( x - pt1.x) + pt1.y
 其垂线的斜率为 - 1 / k，
 垂线方程为：
 y = (-1/k) * (x - point.x) + point.y
 联立两直线方程解得：
 x  =  ( k^2 * pt1.x + k * (point.y - pt1.y ) + point.x ) / ( k^2 + 1)
 y  =  k * ( x - pt1.x) + pt1.y;
 
 然后再判断垂足是否在线段上，如果在线段上则返回垂足；如果不在则计算两端点
 到垂足的距离，选择距离垂足较近的端点返回。
 求点到线段的垂足
 */
- (CGPoint) getPointOnVerticalLine:(CGPoint)touchPoint{

    if (mPoint1.x - mPoint2.x == 0) {//平行于Y轴

        CGFloat maxY = mPoint1.y > mPoint2.y ? mPoint1.y : mPoint2.y;
        CGFloat minY = mPoint1.y < mPoint2.y ? mPoint1.y : mPoint2.y;
        if (touchPoint.y <= maxY && touchPoint.y >= minY) {
            return CGPointMake(mPoint1.x,touchPoint.y);
        }else if(touchPoint.y > maxY){
            return CGPointMake(mPoint1.x,maxY);
        }else{
            return CGPointMake(mPoint1.x,minY);
        }
    }
    if(mPoint1.y == mPoint2.y){//平行于X轴
        CGFloat maxX = mPoint1.x > mPoint2.x ? mPoint1.x : mPoint2.x;
        CGFloat minX = mPoint1.x < mPoint2.x ? mPoint1.x : mPoint2.x;
        if (touchPoint.x <= maxX && touchPoint.x >= minX) {
            return CGPointMake(touchPoint.x,mPoint1.y);
        }else if(touchPoint.x > maxX){
            return CGPointMake(maxX,mPoint1.y);
        }else{
            return CGPointMake(minX,mPoint1.y);
        }
    }
    float lineK = (mPoint2.y - mPoint1.y) / (mPoint2.x - mPoint1.x);
//    float line = lineK*(x - mPoint1.x) + mPoint1.y;
//    float verticalLine = (-1/lineK) * (x - touchPoint.x) + touchPoint.y;
    float xValue = ( powf(lineK,2) * mPoint1.x + lineK * (touchPoint.y - mPoint1.y ) + touchPoint.x ) / ( powf(lineK,2) + 1);
    float yValue = lineK * ( xValue - mPoint1.x) + mPoint1.y;
    
    return CGPointMake(xValue, yValue);
}

- (CGFloat) getYValueAccordingToXValue:(CGPoint)touchPoint{
    CGFloat yValue = 0.0f;
    if (mPoint1.x == mPoint2.x) {
        yValue = touchPoint.y;
    }
    yValue = mLineK * (touchPoint.x - mPoint1.x) + mPoint1.y;
    return yValue;
}

/**
 * @brief 求线段与圆的交点
 * @return 如果有交点返回true,否则返回false
 * @note 与圆可能存在两个交点，如果存在两个交点在ptInter1和ptInter2都为有效值，如果有一个交点，则ptInter2的值为
 *       无效值，此处为65536.0
 */ 
bool LineInterCircle(
                     const CGPoint ptStart, // 线段起点
                     const CGPoint ptEnd, // 线段终点
                     const CGPoint ptCenter, // 圆心坐标
                     const float Radius,
                     CGPoint& ptInter1,
                     CGPoint& ptInter2)
{
    ptInter1.x = ptInter1.y = 65536.0f;
    ptInter2.x = ptInter2.y = 65536.0f;
    
    float fDis = sqrt((ptEnd.x - ptStart.x) * (ptEnd.x - ptStart.x) + (ptEnd.y - ptStart.y) * (ptEnd.y - ptStart.y));
    
    CGPoint d;
    d.x = (ptEnd.x - ptStart.x) / fDis;
    d.y = (ptEnd.y - ptStart.y) / fDis;
    
    CGPoint E;
    E.x = ptCenter.x - ptStart.x;
    E.y = ptCenter.y - ptStart.y;
    
    float a = E.x * d.x + E.y * d.y;
    float a2 = a * a;
    
    float e2 = E.x * E.x + E.y * E.y;
    
    float r2 = Radius * Radius;
    
    if ((r2 - e2 + a2) < 0)
    {
        return false;
    }
    else
    {
        float f = sqrt(r2 - e2 + a2);
        
        float t = a - f;
        
        if( ((t - 0.0) > - EPS) && (t - fDis) < EPS)
        {
            ptInter1.x = ptStart.x + t * d.x;
            ptInter1.y = ptStart.y + t * d.y;
        }
        
        t = a + f;
        
        if( ((t - 0.0) > - EPS) && (t - fDis) < EPS)
        {
            ptInter2.x = ptStart.x + t * d.x;
            ptInter2.y = ptStart.y + t * d.y;
        }
        return true;  
    }  
}
@end
