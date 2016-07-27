//
//  ViewControllerB.m
//  Bluetooth
//
//  Created by mac zdszkj on 16/3/15.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "ViewControllerB.h"

@interface ViewControllerB (){
//    UIImagePickerController *pickerController
    
}
@property (nonatomic)UIImagePickerController *pickerController;

@end

@implementation ViewControllerB

- (void)viewDidLoad {
    [super viewDidLoad];
    
    // Do any additional setup after loading the view.
//    __weak typeof(self)weakSelf = self;
//    __weak ViewControllerB *b = self;
    _pickerController = [[UIImagePickerController alloc]init];
    self.block = ^(NSString *name){
//        UILabel *label = [UILabel new];
//        label.text = name;
//        label.frame = CGRectMake(100, 100, 100, 100);
//        [self.view addSubview:label];
        [self.pickerController takePicture];
    };
    
    UIButton *label = [UIButton new];
    label.backgroundColor = [UIColor redColor];
    label.frame = CGRectMake(200, 200, 100, 100);
    [label addTarget:self action:@selector(btn) forControlEvents:UIControlEventTouchUpInside];
    

    [self.view addSubview:label];
    NSLog(@"%ld",[self retainCount]);
   }

- (void)btn{
    [self.navigationController popToRootViewControllerAnimated:YES];
}
//- (void)viewWillDisappear:(BOOL)animated{
//    self.block(@"destory");
//}
- (void)dealloc{
    [super dealloc];
    NSLog(@"%s",__func__);
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
