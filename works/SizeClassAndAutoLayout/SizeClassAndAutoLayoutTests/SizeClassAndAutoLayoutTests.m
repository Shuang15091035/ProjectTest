//
//  SizeClassAndAutoLayoutTests.m
//  SizeClassAndAutoLayoutTests
//
//  Created by mac zdszkj on 16/5/3.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <XCTest/XCTest.h>
#import "ViewController.h"

@interface SizeClassAndAutoLayoutTests : XCTestCase

@end

@implementation SizeClassAndAutoLayoutTests

- (void)setUp {
    [super setUp];
    // Put setup code here. This method is called before the invocation of each test method in the class.
}

- (void)tearDown {
    // Put teardown code here. This method is called after the invocation of each test method in the class.
    [super tearDown];
}

- (void)testExample {
    // This is an example of a functional test case.
    // Use XCTAssert and related functions to verify your tests produce the correct results.
}

- (void)testAddResult{
    ViewController *v = [[ViewController   alloc]init];
    NSInteger re = [v getRightResultParmera:10 parmera2:10];
    XCTAssertEqual(re, 200);
}
- (void)testPerformanceExample {
    // This is an example of a performance test case.
    [self measureBlock:^{
        // Put the code you want to measure the time of here.
    }];
}

@end
