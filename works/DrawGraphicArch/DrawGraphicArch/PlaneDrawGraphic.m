//
//  PlaneDrawGraphic.m
//  project_mesher(refactor)
//
//  Created by mac zdszkj on 16/10/10.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "PlaneDrawGraphic.h"
#import "ArchPlane.h"
#import "RoomPlane.h"
#import "WallLine.h"
#import "WallPoint.h"
#import "ComponentDoor.h"
#import "ComponentWindow.h"

#define UNSELECTROOM -1


#define SCREENSIZE [[UIScreen mainScreen]bounds].size

@interface PlaneDrawGraphic(){
    
    ArchPlane *archPlane;
    bool isNormalState;
    bool isSelectRoomState;
    bool isAddArchState;
    bool isClickBlankArea;
    
    bool isPanGesture;
    bool isPanOfFirstPoint;
   
    bool drawAllPoint;
    
    bool isSelectedRoom;
    bool isCornerImage;
    bool isOnLine;
    bool isSelectedImage;
    bool isArchComponent;
    
    UIPinchGestureRecognizer *pinchGesture;
    UIPanGestureRecognizer *panGesture;
    
    UIButton *mAddBtn;
    UIButton *mFinishBtn;
    UIButton *mDeleteBtn;
    UIButton *mCommponentBtn;
    UIImageView *selectedComphonent;
    CGPoint panPosition;
    CGContextRef currentContextRef;
    WallPoint *changedPoint;
    
    NSInteger selectedRoomIndex;
    CGFloat xAxisOffset;
    CGFloat yAxisOffset;
    CGFloat lastScale;
    WallLine *minDisLine;
    WallLine *imageViewOfOrigLine;
    ArchWallComponent *selectedArch;
    
}

@property (nonatomic, readwrite) NSMutableArray *currentViewPoint;
@property (nonatomic, readwrite) NSMutableArray *reusableImageAry;
@property (nonatomic, readwrite) NSMutableArray *allViewPointAry;
@property (nonatomic, readwrite) NSMutableArray *allViewLineAry;

@property (nonatomic, readwrite) NSMutableArray *tempTestAry;
@end

@implementation PlaneDrawGraphic

- (instancetype)initWithBtn:(UIButton *)btn finishBtn:(UIButton *)finishBtn deleteBtn:(UIButton *)deleteBtn componentBtn:(UIButton *)componentBtn{
    self = [super init];
    if (self) {
        self.backgroundColor = [UIColor whiteColor];
        selectedRoomIndex = UNSELECTROOM;
        archPlane = [[ArchPlane alloc]init];
        
        _currentViewPoint = [NSMutableArray array];
        _reusableImageAry = [NSMutableArray array];
        _allViewPointAry = [NSMutableArray array];
        _allViewLineAry = [NSMutableArray array];
        _tempTestAry = [NSMutableArray array];
        panPosition = CGPointZero;
        [self PanGestureOperate];
        [self pinchGestureOperate];
        [btn addTarget:self action:@selector(btnAddArchiAction:) forControlEvents:UIControlEventTouchUpInside];
        [finishBtn addTarget:self action:@selector(btnfinishAction:) forControlEvents:UIControlEventTouchUpInside];
        [deleteBtn addTarget:self action:@selector(btnDeleteAction:) forControlEvents:UIControlEventTouchUpInside];
        [componentBtn addTarget:self action:@selector(componentAction:) forControlEvents:UIControlEventTouchUpInside];
        mAddBtn = btn;
        mAddBtn.hidden = NO;
        mFinishBtn = finishBtn;
        mDeleteBtn = deleteBtn;
        mCommponentBtn = componentBtn;
        isNormalState = YES;
    
    }
    return self;
}
- (void)btnAddArchiAction:(UIButton *)btn{
    isAddArchState = YES;
    isNormalState = NO;
    [_currentViewPoint removeAllObjects];
    [self removeGestureRecognizer:panGesture];
    [self removeGestureRecognizer:pinchGesture];
    mAddBtn.hidden = YES;
    mFinishBtn.hidden = NO;
    selectedRoomIndex = UNSELECTROOM;
    [self setNeedsDisplay];
}

- (void)btnfinishAction:(UIButton *)finish{
    
    isNormalState = YES;
    mAddBtn.hidden = NO;
    mDeleteBtn.hidden = NO;
    mCommponentBtn.hidden = NO;
    mFinishBtn.hidden = YES;
    drawAllPoint = YES;
    isAddArchState = NO;
    
    if (_currentViewPoint.count != 0 && _currentViewPoint.count >= 3) {
        RoomPlane *roomPlane = [[RoomPlane alloc]init];
        [roomPlane.roomPoints addObjectsFromArray:_currentViewPoint];
        [archPlane.roomPlanes addObject:roomPlane];
        selectedRoomIndex = archPlane.roomPlanes.count - 1;
        [self setNeedsDisplay];
    }else{
        for (WallPoint *gp in _currentViewPoint) {
            [gp.pointImageView removeFromSuperview];
            [_reusableImageAry addObject:gp.pointImageView];
        }
    }
   
    [self addGestureRecognizer:panGesture];
    [self addGestureRecognizer:pinchGesture];
    
}

- (void) btnDeleteAction:(UIButton *)btn{
    if (isSelectedRoom) {
        NSArray *deleteArch = [_allViewPointAry objectAtIndex:selectedRoomIndex];
        for (WallPoint *point in deleteArch) {
            point.pointImageView.hidden = YES;
            [_reusableImageAry addObject:point.pointImageView];
        }
        [_allViewPointAry removeObjectAtIndex:selectedRoomIndex];
        mDeleteBtn.hidden = YES;
        isSelectedRoom = NO;
        [self setNeedsDisplay];
    }
}
- (void)componentAction:(UIButton *)btn{
    
    ComponentDoor *componentDoor = [[ComponentDoor alloc]initWithDoorType:ComponentTypeSingleDoorInside doorWidth:50 componentHeight:5];
    RoomPlane *singleRoom = [archPlane.roomPlanes objectAtIndex:selectedRoomIndex];
    [singleRoom addAvailableComponentView:componentDoor];
    [self addSubview:componentDoor.componentView];
    //TODO多组件添加的补充
    
    
    
}
- (void)pinchGestureOperate{
    pinchGesture = [[UIPinchGestureRecognizer alloc]initWithTarget:self action:@selector(pinchGesture:)];
    [self addGestureRecognizer:pinchGesture];
}

- (void)pinchGesture:(UIPinchGestureRecognizer *)gesturePinch{
    bool isMinScaleValue = NO;
    switch (gesturePinch.state) {
        case UIGestureRecognizerStateBegan:{
            lastScale = 1.0;
            break;
        }
        case UIGestureRecognizerStateChanged:{
            CGFloat scale = 1.0 - (lastScale - [gesturePinch scale]);
            CGAffineTransform newTransform = CGAffineTransformScale(self.transform, scale, scale);
            if (scale < 1.0f && self.frame.size.width > SCREENSIZE.width) {
                [self setTransform:newTransform];
                if (self.frame.size.width < SCREENSIZE.width) {
                    CGAffineTransform newTransform = CGAffineTransformScale(self.transform, 1/scale, 1/scale);
                    [self setTransform:newTransform];
                    isMinScaleValue = YES;
                }
            }
            if (scale > 1.0f && self.frame.size.width < 3 * SCREENSIZE.width) {
                [self setTransform:newTransform];
            }
            [self setNeedsDisplay];
            break;
        }
        case UIGestureRecognizerStateEnded:
        case UIGestureRecognizerStateFailed:
        case UIGestureRecognizerStateCancelled:{
            if (!isMinScaleValue) {
                lastScale = gesturePinch.scale;
            }else{
                lastScale = 1 / gesturePinch.scale;
                isMinScaleValue = NO;
            }
            
            break;
        }
        default:
            break;
    }
}

- (void)PanGestureOperate{
    panGesture = [[UIPanGestureRecognizer alloc]initWithTarget:self action:@selector(panGesture:)];
    panGesture.minimumNumberOfTouches = 1;
    [self addGestureRecognizer:panGesture];
}

- (void)panGesture:(UIPanGestureRecognizer *)gesturePan{
    
    if (selectedRoomIndex >= 0 ) {
        CGPoint moveP = [gesturePan translationInView:self];
        RoomPlane *selectedRoom = [archPlane.roomPlanes objectAtIndex:selectedRoomIndex];
        switch (gesturePan.state) {
            case UIGestureRecognizerStateBegan:{
                isPanGesture = YES;
                break;
            }
            case UIGestureRecognizerStateChanged:{
                for (WallPoint *gp in selectedRoom.roomPoints) {
                    CGPoint tempP = gp.wallPoint;
                    gp.wallPoint = CGPointMake(tempP.x += moveP.x, tempP.y += moveP.y);
                }
                for (WallLine *wl in selectedRoom.wallLines) {
                    for (ArchWallComponent *comp in wl.wallComponentArr) {
                        CGPoint comView = comp.componentView.center;
                        comp.componentView.center = CGPointMake(comView.x += moveP.x, comView.y += moveP.y);
                    }
                }
                [panGesture setTranslation:CGPointZero inView:self];
                [self setNeedsDisplay];
                break;
            }
            case UIGestureRecognizerStateEnded:
            case UIGestureRecognizerStateFailed:
            case UIGestureRecognizerStateCancelled:{
                isPanGesture = NO;
                break;
            }
            default:{
                break;
            }
        }
        
    }else{
        CGPoint moveP = [gesturePan translationInView:self];
        switch (gesturePan.state) {
            case UIGestureRecognizerStateBegan:{
                isPanGesture = YES;
                break;
            }
            case UIGestureRecognizerStateChanged:{
                for (RoomPlane *singleRoom in archPlane.roomPlanes) {
                    for (WallPoint *gp in singleRoom.roomPoints) {
                        CGPoint tempP = gp.wallPoint;
                        gp.wallPoint = CGPointMake(tempP.x += moveP.x, tempP.y += moveP.y);
                    }
                }
                [panGesture setTranslation:CGPointZero inView:self];
                [self setNeedsDisplay];
                break;
            }
            case UIGestureRecognizerStateEnded:
            case UIGestureRecognizerStateFailed:
            case UIGestureRecognizerStateCancelled:{
                isPanGesture = NO;
                break;
            }
            default:{
                break;
            }
        }
    }
    isPanOfFirstPoint = YES;
}

- (void)drawRect:(CGRect)rect {
    int counter = 0;
    for (RoomPlane *singleRoom in archPlane.roomPlanes) {
        if (counter++ == selectedRoomIndex) {
            continue;
        }else{
            [self drawArchByNsarrayOfPoints:singleRoom.roomPoints isSeledtRoom:false];
        }
    }
    if (selectedRoomIndex != UNSELECTROOM) {
        [self drawArchByNsarrayOfPoints:archPlane.roomPlanes[selectedRoomIndex].roomPoints isSeledtRoom:true]; //写在这里是为了让选中的图形显示在最上层（后绘制的在上面）
    }
    
    if (isAddArchState) {
        [self drawArchByNsarrayOfPoints:_currentViewPoint isSeledtRoom:true];
    }else if(isNormalState){
        
    }
    

}

- (void) drawArchByNsarrayOfPoints:(NSArray *)arrPoints isSeledtRoom:(bool)isSelectRoom{
    if (arrPoints.count < 1) {
        return;
    }
     currentContextRef = UIGraphicsGetCurrentContext();
    if (isSelectRoom) {
        CGContextSetRGBStrokeColor(UIGraphicsGetCurrentContext(), 0.0, 0.0, 1.0, 1.0);  // 边线颜色
        CGContextSetFillColorWithColor(UIGraphicsGetCurrentContext(), [UIColor colorWithRed:0 green:1 blue:0 alpha:1].CGColor);// 填充色
        CGContextSetLineWidth(currentContextRef, 5.0f);
    }else{
        CGContextSetRGBStrokeColor(UIGraphicsGetCurrentContext(), 1.0, 0.0, 0.0, 1.0);  // 边线颜色
        CGContextSetFillColorWithColor(UIGraphicsGetCurrentContext(), [UIColor colorWithRed:0 green:0 blue:1 alpha:1].CGColor);// 填充色
    }
    CGMutablePathRef path = CGPathCreateMutable();
    if (arrPoints.count == 1) {
        WallPoint *curPoint = [arrPoints firstObject];
        CGPathMoveToPoint(path, NULL, curPoint.wallPoint.x, curPoint.wallPoint.y);
        UIImageView *imageV = [self getAvailableImageViewOfGraphicPoint:[arrPoints lastObject]];
        curPoint.pointImageView = imageV;
        [self addSubview:imageV];
        if (isSelectRoom) {
            imageV.hidden = NO;
        }else{
            imageV.hidden = YES;
        }
        CGContextStrokePath(currentContextRef);
    } else{
        for (WallPoint *cur  in arrPoints) {
            UIImageView *imageV = [self getAvailableImageViewOfGraphicPoint:cur];
            cur.pointImageView = imageV;
            [self addSubview:imageV];
            if (isSelectRoom) {
                imageV.hidden = NO;
            }else{
                imageV.hidden = YES;
            }
        }
        NSInteger pointCount = arrPoints.count;
        CGPoint aPoint[pointCount];
        for (int i = 0; i < pointCount; i++) {
            WallPoint *p = [arrPoints objectAtIndex:i];
            aPoint[i] = p.wallPoint;
        }
        CGContextAddLines(currentContextRef, aPoint, pointCount);
        CGContextSetLineWidth(currentContextRef, 5.0f);
        CGContextClosePath(currentContextRef);
        if (arrPoints.count == 2) {
            CGContextDrawPath(currentContextRef, kCGPathStroke);
        }else{
            CGContextDrawPath(currentContextRef, kCGPathFillStroke);
        }
    }
}

- (void)touchesBegan:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
    if (isNormalState) {
        if ([[touches anyObject].view isKindOfClass:[UIImageView class]]) {
            if([touches anyObject].view.tag == 200){
                isCornerImage = YES;
                [self removeGestureRecognizer:panGesture];
                CGPoint viewCenter = [touches anyObject].view.center;
                for (RoomPlane *singleRoom in archPlane.roomPlanes) {
                    for (WallPoint *point in singleRoom.roomPoints) {
                        if (point.wallPoint.x == viewCenter.x && point.wallPoint.y == viewCenter.y) {
                            changedPoint = point;
                            selectedRoomIndex = [archPlane.roomPlanes indexOfObject:singleRoom];
                            isSelectedRoom = YES;
                        }
                    }
                }
            }else if ([touches anyObject].view.tag == 300) {
                selectedComphonent = (UIImageView *)[touches anyObject].view;
                isArchComponent = YES;
                NSArray *selectedRoomLines = archPlane.roomPlanes[selectedRoomIndex].wallLines;
                for (WallLine *wallLine in selectedRoomLines) {
                    for (ArchWallComponent *component in wallLine.wallComponentArr) {
                        if (component.componentView == selectedComphonent) {
                            imageViewOfOrigLine = wallLine;
                            selectedArch = component;
                        }
                    }
                }
                [self removeGestureRecognizer:panGesture];
            }
        }
    }
    
}

- (void)touchesMoved:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
    if (isNormalState) {
        if (isCornerImage) {
            CGPoint touchP = [[touches anyObject] locationInView:self];
            CGPoint lastP = [[touches anyObject] previousLocationInView:self];
            changedPoint.wallPoint = CGPointMake(touchP.x, touchP.y);
            [self pointOfSelectedLineVerticalAndHoriztonal:touchP];
            [self onLineMoveOfTouchPoint:touchP];
            [self updateComponenetPositionAccordingToNowPoint:touchP lastPoint:lastP];
            [self setNeedsDisplay];
        }else if (isArchComponent) {
            CGPoint touchP = [[touches anyObject] locationInView:self];
            [self touchViewOnLineMove:touchP selectedView:selectedComphonent];
        }
    }
}
- (void)touchesEnded:(NSSet<UITouch *> *)touches withEvent:(UIEvent *)event{
    
    if (isAddArchState) {
        CGPoint point = [[touches anyObject] locationInView:self];
        WallPoint *wp = [[WallPoint alloc]init];
        wp.wallPoint = point;
        [self.currentViewPoint addObject:wp];
        [self setNeedsDisplay];
    }else if(isNormalState){
        if(isCornerImage){
            isCornerImage = NO;
            [self addGestureRecognizer:panGesture];
        }else if (isArchComponent) {
            isArchComponent = NO;
            NSArray *selectedRoomLines = archPlane.roomPlanes[selectedRoomIndex].wallLines;
            WallLine *desWallLine = nil;
            ArchWallComponent *changeArch = nil;
            NSInteger lineCount = selectedRoomLines.count;
            CGFloat imaDisLine = CGFLOAT_MAX;
            for (WallLine *wallLine in selectedRoomLines) {
                imaDisLine = [wallLine distanceOfLineFromPoint: selectedComphonent.center];
                if(imaDisLine < 1.0f){
                    desWallLine = wallLine;
                }
            }
            for (int i = 0; i < lineCount; i++) {
                WallLine *wallLine = selectedRoomLines[i];
                for (ArchWallComponent *wallImageArch in wallLine.wallComponentArr) {
                    if (wallImageArch.componentView == selectedComphonent) {
                        changeArch = wallImageArch;
                    }
                }
            }
            if (desWallLine.hash != imageViewOfOrigLine.hash) {
                [desWallLine.wallComponentArr addObject:changeArch];
                [imageViewOfOrigLine.wallComponentArr removeObject:changeArch];
            }
            [self addGestureRecognizer:panGesture];
        }else{
            CGPoint touchPoint = [[touches anyObject] locationInView:self];
            selectedRoomIndex = [archPlane roomIndexOfRoomPlaneInsideRayCastingPoint:touchPoint];
            if (selectedRoomIndex != UNSELECTROOM) {
                mDeleteBtn.hidden = NO;
                mCommponentBtn.hidden = NO;
            }else{
                mDeleteBtn.hidden = YES;
                mCommponentBtn.hidden = YES;
            }
             [self setNeedsDisplay];//设置选中重绘制边线和区域颜色
        }
    }
}

- (UIImageView *)getAvailableImageViewOfGraphicPoint:(WallPoint *)point{
    UIImageView *imageV;
    if(!point.pointImageView) {
        if (_reusableImageAry.count != 0) {
            UIImageView *imag = [_reusableImageAry lastObject];
            [_reusableImageAry removeObject:imag];
            imag.hidden = NO;
            imag.center = point.wallPoint;
            imageV = imag;
        }else{
            UIImageView *imageView = [[UIImageView alloc]init];
            imageView.tag = 200;//默认给定标示
            imageView.userInteractionEnabled = YES;
            [imageView setBackgroundColor:[self randomColor]];
            imageView.frame = CGRectMake(0, 0, 40, 40);
            imageView.center = point.wallPoint;
            imageV = imageView;
            [_tempTestAry addObject:imageView];
        }
    }else{
        point.pointImageView.center = point.wallPoint;
        imageV = point.pointImageView;
    }
    return imageV;
}

- (UIColor *)randomColor
{
    CGFloat red =  (CGFloat)random()/(CGFloat)RAND_MAX;
    CGFloat blue = (CGFloat)random()/(CGFloat)RAND_MAX;
    CGFloat green = (CGFloat)random()/(CGFloat)RAND_MAX;
    return [UIColor colorWithRed:red green:green blue:blue alpha:1.0f];
}

/**
 点在线上移动
 */
- (void)onLineMoveOfTouchPoint:(CGPoint)touchP{
    if (!isOnLine) {
        CGFloat minDistant = CGFLOAT_MAX;
        CGFloat tempDis = 0.0f;
        for (int i = 0; i < archPlane.roomPlanes.count; i++) {
            if (i == selectedRoomIndex)  continue;
            RoomPlane  *roomPlane = archPlane.roomPlanes[i];
            for (WallLine *gl in roomPlane.wallLines) {
                tempDis = [gl distanceOfLineFromPoint:touchP];
                if (tempDis < minDistant) {
                    minDistant = tempDis;
                    if (minDistant < 20) {
                        CGPoint footPoint = [gl pedalOfLineAndVerticalAccordingToLineOutPoint:touchP];
                        if (footPoint.x <= gl.maxX && footPoint.x >= gl.minX && footPoint.y <= gl.maxY && footPoint.y >= gl.minY ) {
                            changedPoint.wallPoint = footPoint;
                            isOnLine = YES;
                            minDisLine = gl;
                            return;
                        }
                    }
                }
            }
        }
    }else{
        
        CGFloat tempDis = 0.0f;
        tempDis = [minDisLine distanceOfLineFromPoint:touchP];
        if (tempDis < 20) {
            CGPoint footPoint = [minDisLine pedalOfLineAndVerticalAccordingToLineOutPoint:touchP];
            if (footPoint.x <= minDisLine.maxX && footPoint.x >= minDisLine.minX && footPoint.y <= minDisLine.maxY && footPoint.y >= minDisLine.minY) {
                changedPoint.wallPoint = footPoint;
                isOnLine = YES;
            }
        }else{
            isOnLine = NO;
            changedPoint.wallPoint = touchP;
        }
    }
   
}
- (void)updateComponenetPositionAccordingToNowPoint:(CGPoint)newPoint lastPoint:(CGPoint)lastPoint{
    RoomPlane *selecedRoom = [archPlane.roomPlanes objectAtIndex:selectedRoomIndex];
    NSInteger index = [selecedRoom.roomPoints indexOfObject:changedPoint];
    WallLine *line1 = nil;
    WallLine *line2 = nil;
    if (index == 0) {
        line1 = selecedRoom.wallLines[index + 1];
        line2 = [selecedRoom.wallLines lastObject];
    } else if(index == selecedRoom.wallLines.count - 1) {
        line1 = selecedRoom.wallLines[index - 1];
        line2 = selecedRoom.wallLines[0];
    }else{
        line1 = selecedRoom.wallLines[index - 1];
        line2 = selecedRoom.wallLines[index +1];
    }
    CGFloat deltaX = newPoint.x - lastPoint.x;
    CGFloat deltaY = newPoint.y - lastPoint.y;
    for (ArchWallComponent *component in line1.wallComponentArr) {
        CGPoint tempPoint = component.componentView.center;
        tempPoint.x += deltaX;
        tempPoint.y += deltaY;
        component.componentView.center = tempPoint;
    }
    for (ArchWallComponent *component in line2.wallComponentArr) {
        CGPoint tempPoint = component.componentView.center;
        tempPoint.x += deltaX;
        tempPoint.y += deltaY;
        component.componentView.center = tempPoint;
    }
}

- (void)touchViewOnLineMove:(CGPoint)touchP selectedView:(UIImageView *)imageView{
    
    CGFloat tempDis = 0.0f;
    CGFloat minDistant = CGFLOAT_MAX;
    NSArray *selectedRoomLines = archPlane.roomPlanes[selectedRoomIndex].wallLines;
    for (WallLine *wallLine in selectedRoomLines) {
        tempDis = [wallLine distanceOfLineFromPoint:touchP];
        if (tempDis < minDistant) {
            minDistant = tempDis;
            if (minDistant < 20){
                CGPoint footPoint = [wallLine pedalOfLineAndVerticalAccordingToLineOutPoint:touchP];
                if( [self isIntersectionOfComponent:selectedRoomLines pedal:footPoint]){
                    if (footPoint.x <= wallLine.maxX && footPoint.x >= wallLine.minX && footPoint.y <= wallLine.maxY && footPoint.y >= wallLine.minY) {
                        imageView.center = footPoint;
                    }
                }
            }
        }
    }
}
-(bool)isIntersectionOfComponent:(NSArray *)roomLines pedal:(CGPoint)pedal{
    for (WallLine *wallLine in roomLines) {
        if (wallLine.wallComponentArr.count == 1) {
            return true;
        }
        for (ArchWallComponent *component in wallLine.wallComponentArr) {
            if(component.componentView.hash == selectedComphonent.hash) continue;
            CGFloat disOfPoints = sqrtf(powf((pedal.y - component.componentView.center.y), 2) + powf((pedal.x - component.componentView.center.x), 2));
            CGFloat comRadiuDis = selectedArch.componentWidth/2 +  component.componentWidth/2;
            if (disOfPoints + 0.1f <= comRadiuDis) {
                return false;
            }
        }
    }
    return true;
}
/**
 点移动自动垂直和水平
 */
- (void)pointOfSelectedLineVerticalAndHoriztonal:(CGPoint)touchP{
    RoomPlane *selecedArch = [archPlane.roomPlanes objectAtIndex:selectedRoomIndex];
    NSInteger index = [selecedArch.roomPoints indexOfObject:changedPoint];
    WallPoint *point1 = nil;
    WallPoint *point2 = nil;
    if (index == 0) {
        point1 = selecedArch.roomPoints[index + 1];
        point2 = [selecedArch.roomPoints lastObject];
    } else if(index == selecedArch.roomPoints.count - 1) {
        point1 = selecedArch.roomPoints[index - 1];
        point2 = selecedArch.roomPoints[0];
    }else{
        point1 = selecedArch.roomPoints[index - 1];
        point2 = selecedArch.roomPoints[index +1];
    }
    CGPoint tempPoint = changedPoint.wallPoint;
    if (fabs(changedPoint.wallPoint.x - point1.wallPoint.x) < 10.0f){
        tempPoint.x = point1.wallPoint.x;
    } if (fabs(changedPoint.wallPoint.y - point2.wallPoint.y) < 10.0f){
        tempPoint.y = point2.wallPoint.y;
    } if (fabs(changedPoint.wallPoint.x - point2.wallPoint.x) < 10.0f) {
        tempPoint.x = point2.wallPoint.x;
    } if (fabs(changedPoint.wallPoint.y - point1.wallPoint.y) < 10.0f) {
        tempPoint.y = point1.wallPoint.y;
    }
    changedPoint.wallPoint = tempPoint;
}



@end