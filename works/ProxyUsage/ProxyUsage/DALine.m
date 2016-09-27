//
//  DALine.m
//  ProxyUsage
//
//  Created by mac zdszkj on 16/9/20.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "DALine.h"
#include <math.h>
@interface DALine(){
    CGPoint mPoint1;
    CGPoint mPoint2;
}
@end

@implementation DALine

@synthesize point1 = mPoint1;
@synthesize point2 = mPoint2;

/****点到直线的距离***
 * 过点（x1,y1）和点（x2,y2）的直线方程为：KX -Y + (x2y1 - x1y2)/(x2-x1) = 0
 * 设直线斜率为K = (y2-y1)/(x2-x1),C=(x2y1 - x1y2)/(x2-x1)
 * 点P(x0,y0)到直线AX + BY +C =0DE 距离为：d=|Ax0 + By0 + C|/sqrt(A*A + B*B)
 * 点（x3,y3）到经过点（x1,y1）和点（x2,y2）的直线的最短距离为：
 * distance = |K*x3 - y3 + C|/sqrt(K*K + 1)
 */

- (CGFloat)getdistancePointAndPoint{
    return sqrtf(powf(mPoint2.y - mPoint1.y, 2) + powf(mPoint2.x - mPoint1.x, 2));
}

-(CGFloat) GetMinDistance:(CGPoint) pt3{
    
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
    
    CGFloat maxY = mPoint1.y > mPoint2.y ? mPoint1.y : mPoint2.y;
    CGFloat minY = mPoint1.y < mPoint2.y ? mPoint1.y : mPoint2.y;
    CGFloat maxX = mPoint1.x > mPoint2.x ? mPoint1.x : mPoint2.x;
    CGFloat minX = mPoint1.x < mPoint2.x ? mPoint1.x : mPoint2.x;
    if (mPoint1.x - mPoint2.x == 0) {//平行于Y轴
        if (touchPoint.y <= maxY && touchPoint.y >= minY) {
            return CGPointMake(mPoint1.x,touchPoint.y);
        }else if(touchPoint.y > maxY){
            return CGPointMake(mPoint1.x,maxY);
        }else{
            return CGPointMake(mPoint1.x,minY);
        }
    }
    if(mPoint1.y - mPoint2.y == 0){//平行于X轴
        if (touchPoint.x <= maxX && touchPoint.x >= minX) {
            return CGPointMake(touchPoint.x,mPoint1.y);
        }else if(touchPoint.x > maxX){
            return CGPointMake(maxX,mPoint1.y);
        }else{
            return CGPointMake(minX,mPoint1.y);
        }
    }
    //以下仅仅对直线而言的点
    float lineK = (mPoint2.y - mPoint1.y) / (mPoint2.x - mPoint1.x);
    //    float line = lineK*(x - mPoint1.x) + mPoint1.y;
    //    float verticalLine = (-1/lineK) * (x - touchPoint.x) + touchPoint.y;
    float xValue = ( powf(lineK,2) * mPoint1.x + lineK * (touchPoint.y - mPoint1.y ) + touchPoint.x ) / ( powf(lineK,2) + 1);
    float yValue = lineK * ( xValue - mPoint1.x) + mPoint1.y;
    
    return CGPointMake(xValue, yValue);
    
}


/**
 1.几何方法解决
 过圆心的直线和圆的两个交点坐标(此方法只获取所需的一个)
 x = x0  +/- (x1 - x0)*r/sqrt((x1 - x0)^2 + (y1 - y0)^2)
 y = y0 +/- (y1 - y0)*r/ sqrt((x1 - x0)^2 + (y1 - y0)^2)
 其中，(x0, y0)是圆心坐标，(x1, y1)是已知点坐标，r是半径
 2.使用向量解决
 圆心为P点，A点是给定的坐标点。R是圆心半径，Pi圆周率。
 B点和C点就是要求的2个坐标点
 计算向量PA = 向量A-向量P。
 把PA换算成极坐标表示法。
 向量PB的角度=PA，长度=圆半径R
 向量PC的角度=PA+Pi，长度=圆半径R（方向差了180°）
 把向量PB、PC从极坐标转换为直角坐标表示法
 2种坐标系互相转换有很简单的计算公式的，无非就是平方开方和三角函数什么的
 最后得到的向量PB和向量PC最后分别和向量P相加，得到最后结果向量B和向量C
 */
- (CGPoint) getIntersectionOfLinerAndCircularRadiu:(CGFloat)radiu{
    CGFloat positionX = 0.0f;
    CGFloat positionY = 0.0f;
    float lineK = (mPoint2.y - mPoint1.y) / (mPoint2.x - mPoint1.x);
    if (mPoint1.x != mPoint2.x) {
        CGFloat positionPointX1 = mPoint1.x + (mPoint2.x - mPoint1.x) * radiu /sqrtf(powf((mPoint2.x - mPoint1.x),2) + powf((mPoint2.y - mPoint1.y),2));
        CGFloat positionPointX2 = mPoint1.x - (mPoint2.x - mPoint1.x)/sqrt(powf((mPoint2.x - mPoint1.x),2) + powf((mPoint2.y - mPoint1.y),2));
        CGFloat maxX = mPoint1.x > mPoint2.x ? mPoint1.x : mPoint2.x;
        CGFloat minX = mPoint1.x < mPoint2.x ? mPoint1.x : mPoint2.x;
        if (positionPointX1 >= minX && positionPointX1 <= maxX ) {
            positionX = positionPointX1;
        }else if(positionPointX2 >= minX && positionPointX2 <= maxX){
            positionX = positionPointX2;
        }
        positionY = lineK*(positionX - mPoint2.x) + mPoint2.y;
        return CGPointMake(positionX, positionY);
    }
    return CGPointMake(mPoint1.x, mPoint1.y + radiu);
}



@end
