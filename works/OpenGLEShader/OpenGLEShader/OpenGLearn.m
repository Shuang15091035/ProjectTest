//
//  OpenGLearn.m
//  OpenGLEShader
//
//  Created by mac zdszkj on 16/7/1.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "OpenGLearn.h"
#import "OpenGLView.h"

@interface OpenGLearn (){
    OpenGLView *_glView;
}

@end

@implementation OpenGLearn

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view.
    _glView = [[OpenGLView alloc]initWithFrame:[[UIScreen mainScreen]bounds]];
    [self.view addSubview:_glView];
}

- (void)didReceiveMemoryWarning {
    [super didReceiveMemoryWarning];
    // Dispose of any resources that can be recreated.
}

/*
#pragma mark - Navigation

// In a storyboard-based application, you will often want to do a little preparation before navigation
- (void)prepareForSegue:(UIStoryboardSegue *)segue sender:(id)sender {
    // Get the new view controller using [segue destinationViewController].
    // Pass the selected object to the new view controller.
}
*/

@end
