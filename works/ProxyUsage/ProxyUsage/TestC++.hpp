//
//  TestC++.hpp
//  ProxyUsage
//
//  Created by mac zdszkj on 2016/11/8.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#ifndef TestC___hpp
#define TestC___hpp
#include <stdio.h>

class A {
public:
    A(int x):a1(x){}
    void show();
    ~A();
private:
    int a1;
};

class B {
public:
    B(int b):b1(b){}
    void show();
    ~B();
    friend class A;
private:
    int b1;
};

#endif /* TestC___hpp */
