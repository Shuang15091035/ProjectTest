//
//  TestC++.cpp
//  ProxyUsage
//
//  Created by mac zdszkj on 2016/11/8.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#include "TestC++.hpp"


void A::show(){
    printf("show A message%d",this->a1);
}

A::~A(){
    
}

void B::show(){
    printf("show B message");
}
B::~B(){

}
