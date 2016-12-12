//
//  main.cpp
//  VirtualFunction
//
//  Created by mac zdszkj on 2016/12/5.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#include <iostream>
using namespace std;

class Animation {
    
    
public:
    
};

class CObj
{
public:
    CObj() : mX(0), mY(0) {}
    friend class CFriend;
private:
    void PrintData() const
    {
        cout << "mX = " << mX << endl
        << "mY = " << mY << endl;
    }
    int mX;
    int mY;
};

class CFriend
{
public:
    CFriend(int x, int y)
    {
        mObj.mX = x;    //直接调用类CObj的私有数据成员
        mObj.mY = y;
    }
    void ShowData() const
    {
        mObj.PrintData();   //直接调用类CObj的私有成员函数
    }
private:
    CObj mObj;
};


int main(int argc, const char * argv[]) {
    
    
    std::cout << "Hello, World!\n";
    return 0;
}
