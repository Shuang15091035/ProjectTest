//
//  DrawPlaneArch.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 2016/10/28.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "DrawPlaneArch.h"

@interface DrawPlaneArch(){
    ArchPlane *mArchPlane;
    bool isAddRoom;
    bool isImageView;
}
@property (nonatomic, readwrite) UIButton *transfromBtn;
@property (nonatomic, readwrite) UIButton *addRoom;
@property (nonatomic, readwrite) NSMutableArray *addRoomPoints;

@end

@implementation DrawPlaneArch

@synthesize archPlane = mArchPlane;

- (instancetype)init{
    self = [super init];
    if (self) {
        mArchPlane = [[ArchPlane alloc]init];
        self.backgroundColor = [UIColor whiteColor];
        [self addSubview:self.transfromBtn];
        [self addSubview:self.addRoom];
    }
    return self;
}
- (NSMutableArray *)addRoomPoints{
    if (_addRoomPoints == nil) {
        _addRoomPoints = [NSMutableArray array];
    }
    return _addRoomPoints;
}

- (UIButton *)transfromBtn{
    if (_transfromBtn == nil) {
        _transfromBtn = [[UIButton alloc]initWithFrame:CGRectMake(20, 20, 100, 60)];
        _transfromBtn.backgroundColor = [UIColor redColor];
        [_transfromBtn setTitle:@"Transform" forState:UIControlStateNormal];
        [_transfromBtn addTarget:self action:@selector(transformBtnClick:) forControlEvents:UIControlEventTouchUpInside];
    }
    return _transfromBtn;
}

- (UIButton *)addRoom{
    if (_addRoom == nil) {
        _addRoom = [[UIButton alloc]initWithFrame:CGRectMake(20, 100, 100, 60)];
        _addRoom.backgroundColor = [UIColor blueColor];
        [_addRoom setTitle:@"AddRoom" forState:UIControlStateNormal];
        [_addRoom addTarget:self action:@selector(addRoomBtnClick:) forControlEvents:UIControlEventTouchUpInside];
    }
    return _addRoom;
}

- (void)transformBtnClick:(UIButton *)transformBtn{
    
}

- (void)addRoomBtnClick:(UIButton *)addRoomBtn{
    isAddRoom = YES;
}

- (void)drawRect:(CGRect)rect {
    
    
}
- (void)touchesBegan:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
    CGPoint touchPoint = [[touches anyObject]locationInView:self];
    if (isAddRoom) {
        WallPoint *wp = [[WallPoint alloc]initWithPoint:touchPoint currentPointView:self];
        [self.addRoomPoints addObject:wp];
    }
}

- (void)touchesMoved:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
   
}
- (void)touchesEnded:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
    
}

@end
