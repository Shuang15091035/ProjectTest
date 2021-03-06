//
//  DrawPlane.m
//  DrawGraphicArch
//
//  Created by mac zdszkj on 2016/11/14.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import "DrawPlane.h"
#import <Home/Home.h>

#define UNSELECTROOM -1
#define SCREENSIZE [[UIScreen mainScreen]bounds].size

@interface DrawPlane(){
    HomeArchPlane *mArchPlane;
    bool isNormalState;
    bool isAddArchState;
    bool isSelectedRoom;
    bool isCornerImage;
    bool isOnLine;
    bool isArchComponent;
    
    UIPinchGestureRecognizer *pinchGesture;
    UIPanGestureRecognizer *panGesture;
    HomeWallLine *imageViewOfOrigLine;
    CGFloat lastScale;
    HomeWallLine *minDisLine;
    UIButton *mAddBtn;
    UIButton *mFinishBtn;
    UIButton *mDeleteBtn;
    UIButton *mCommponentBtn;
    
    UIImageView *selectedComphonent;
    HomeWallPoint *selectedPoint;
    NSInteger selectedRoomIndex;
    HomeArchItem *selectedArch;
    HomeWallLine *associatedWL1;
    HomeWallLine *associatedWL2;
    
}
@property (nonatomic, readwrite) NSMutableArray *currentViewPoint;
@property (nonatomic, readwrite) NSMutableArray *reusableImageAry;
@property (nonatomic, readwrite) NSMutableArray *tempTestAry;
@end

@implementation DrawPlane

@synthesize homeArchPlane = mArchPlane;
- (instancetype)initWithBtn:(UIButton *)btn finishBtn:(UIButton *)finishBtn deleteBtn:(UIButton *)deleteBtn componentBtn:(UIButton *)componentBtn{
    self = [super init];
    if (self) {
        self.backgroundColor = [UIColor whiteColor];
        selectedRoomIndex = UNSELECTROOM;
        mArchPlane = [[HomeArchPlane alloc]init];
        _currentViewPoint = [NSMutableArray array];
        _reusableImageAry = [NSMutableArray array];
        _tempTestAry = [NSMutableArray array];
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
    isAddArchState = NO;
    
    if (_currentViewPoint.count != 0 && _currentViewPoint.count >= 3) {
        HomeRoomPlane *roomPlane = [[HomeRoomPlane alloc]init];
        [roomPlane.roomPoints addObjectsFromArray:_currentViewPoint];
        [mArchPlane.roomPlanes addObject:roomPlane];
        selectedRoomIndex = mArchPlane.roomPlanes.count - 1;
        [self setNeedsDisplay];
    }else{
        for (HomeWallPoint *gp in _currentViewPoint) {
            [gp.pointImageView removeFromSuperview];
            [_reusableImageAry addObject:gp.pointImageView];
        }
    }
    [self addGestureRecognizer:panGesture];
    [self addGestureRecognizer:pinchGesture];
}

- (void) btnDeleteAction:(UIButton *)btn{
    if (isSelectedRoom) {
        HomeRoomPlane *deleteArch = [mArchPlane.roomPlanes objectAtIndex:selectedRoomIndex];
        for (HomeWallPoint *point in deleteArch.roomPoints) {
            point.pointImageView.hidden = YES;
            [_reusableImageAry addObject:point.pointImageView];
        }
        [mArchPlane.roomPlanes removeObjectAtIndex:selectedRoomIndex];
        mDeleteBtn.hidden = YES;
        isSelectedRoom = NO;
        [self setNeedsDisplay];
    }
}
- (void)componentAction:(UIButton *)btn{
    if (selectedRoomIndex == UNSELECTROOM) return;
    HomeArchDoor *componentDoor = [[HomeArchDoor alloc]initWithDoorType:HomeComponentTypeSingleDoorInside doorWidth:50 componentHeight:5];
    HomeRoomPlane *singleRoom = [mArchPlane.roomPlanes objectAtIndex:selectedRoomIndex];
    [singleRoom addAvailableComponentView:componentDoor];
    [self addSubview:componentDoor.componentView];
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
        HomeRoomPlane *selectedRoom = [mArchPlane.roomPlanes objectAtIndex:selectedRoomIndex];
        switch (gesturePan.state) {
            case UIGestureRecognizerStateBegan:{
                break;
            }
            case UIGestureRecognizerStateChanged:{
                for (HomeWallPoint *gp in selectedRoom.roomPoints) {
                    CGPoint tempP = gp.wallPoint;
                    gp.wallPoint = CGPointMake(tempP.x += moveP.x, tempP.y += moveP.y);
                }
                for (HomeWallLine *wl in selectedRoom.wallLines) {
                    for (HomeArchItem *comp in wl.wallComponentArr) {
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
                break;
            }
            case UIGestureRecognizerStateChanged:{
                for (HomeRoomPlane *singleRoom in mArchPlane.roomPlanes) {
                    for (HomeWallPoint *gp in singleRoom.roomPoints) {
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
                break;
            }
            default:{
                break;
            }
        }
    }
}

- (void)drawRect:(CGRect)rect {
    CGContextRef currentContextRef = UIGraphicsGetCurrentContext();
    int counter = 0;
    for (HomeRoomPlane *singleRoom in mArchPlane.roomPlanes) {
        if (counter++ == selectedRoomIndex) {
            continue;
        }else{
            [self drawArchByNsarrayOfPoints:singleRoom.roomPoints isSeledtRoom:false currentContext:currentContextRef];
        }
    }
    if (selectedRoomIndex != UNSELECTROOM) {
        [self drawArchByNsarrayOfPoints:mArchPlane.roomPlanes[selectedRoomIndex].roomPoints isSeledtRoom:true currentContext:currentContextRef]; //写在这里是为了让选中的图形显示在最上层（后绘制的在上面）
    }
    
    if (isAddArchState) {
        [self drawArchByNsarrayOfPoints:_currentViewPoint isSeledtRoom:true currentContext:currentContextRef];
    }else if(isNormalState){
        if (_isGenerateHome) {
            [self drawArchByNsarrayOfPoints:[mArchPlane.roomPlanes lastObject].roomPoints isSeledtRoom:false currentContext:currentContextRef];
        }
    }
    
    
}

- (void) drawArchByNsarrayOfPoints:(NSArray *)arrPoints isSeledtRoom:(bool)isSelectRoom currentContext:(CGContextRef)currentContextRef{
    if (arrPoints.count < 1) {
        return;
    }
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
        HomeWallPoint *curPoint = [arrPoints firstObject];
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
        for (HomeWallPoint *cur  in arrPoints) {
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
            HomeWallPoint *p = [arrPoints objectAtIndex:i];
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
                for (HomeRoomPlane *singleRoom in mArchPlane.roomPlanes) {
                    for (HomeWallPoint *point in singleRoom.roomPoints) {
                        if (point.wallPoint.x == viewCenter.x && point.wallPoint.y == viewCenter.y) {
                            selectedPoint = point;
                            selectedRoomIndex = [mArchPlane.roomPlanes indexOfObject:singleRoom];
                            isSelectedRoom = YES;
                        }
                    }
                }
                if (selectedPoint) {
                    [self getPercentOfBeforeMoving];
                }
            }else if ([touches anyObject].view.tag == 300) {
                selectedComphonent = (UIImageView *)[touches anyObject].view;
                isArchComponent = YES;
                NSArray *selectedRoomLines = mArchPlane.roomPlanes[selectedRoomIndex].wallLines;
                for (HomeWallLine *wallLine in selectedRoomLines) {
                    for (HomeArchItem *component in wallLine.wallComponentArr) {
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
            selectedPoint.wallPoint = CGPointMake(touchP.x, touchP.y);
            [self pointOfSelectedLineVerticalAndHoriztonal:touchP]; //墙体线的水平垂直
            [self onLineMoveOfTouchPoint:touchP];//墙体吸附
            [self updateComponenetPosition];//更新组件位置信息
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
        HomeWallPoint *wp = [[HomeWallPoint alloc]init];
        wp.wallPoint = point;
        [self.currentViewPoint addObject:wp];
        [self setNeedsDisplay];
    }else if(isNormalState){
        if(isCornerImage){
            isCornerImage = NO;
            [self addGestureRecognizer:panGesture];
        }else if (isArchComponent) {
            isArchComponent = NO;
            [self changeIndexOfWallComponentInSelectedRoomPlanes];
            [self addGestureRecognizer:panGesture];
        }else{
            CGPoint touchPoint = [[touches anyObject] locationInView:self];
            selectedRoomIndex = [mArchPlane roomIndexOfRoomPlaneInsideRayCastingPoint:touchPoint];
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

- (UIImageView *)getAvailableImageViewOfGraphicPoint:(HomeWallPoint *)point{
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


//多个room时，墙体点在线上移动
- (void)onLineMoveOfTouchPoint:(CGPoint)touchP{
    if (selectedRoomIndex == UNSELECTROOM)return;
    if (!isOnLine) {
        CGFloat minDistant = CGFLOAT_MAX;
        CGFloat tempDis = 0.0f;
        for (int i = 0; i < mArchPlane.roomPlanes.count; i++) {
            if (i == selectedRoomIndex)  continue;
            HomeRoomPlane  *roomPlane = mArchPlane.roomPlanes[i];
            for (HomeWallLine *gl in roomPlane.wallLines) {
                tempDis = [gl distanceOfLineFromPoint:touchP];
                if (tempDis < minDistant) {
                    minDistant = tempDis;
                    if (minDistant < 20) {
                        CGPoint footPoint = [gl pedalOfLineAndVerticalAccordingToLineOutPoint:touchP];
                        if (footPoint.x <= gl.maxX && footPoint.x >= gl.minX && footPoint.y <= gl.maxY && footPoint.y >= gl.minY ) {
                            selectedPoint.wallPoint = footPoint;
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
                selectedPoint.wallPoint = footPoint;
                isOnLine = YES;
            }
        }else{
            isOnLine = NO;
            selectedPoint.wallPoint = touchP;
        }
    }
    
}
//更改墙体点时，组件的位置变化（待完善）
- (void)updateComponenetPosition{
    if (selectedRoomIndex == UNSELECTROOM) return;
    for (HomeArchItem *archComp in associatedWL1.wallComponentArr) {
        CGFloat Length = [associatedWL1 getWallLineWidth] * archComp.percent;
        CGPoint interPoint1 = CGPointZero;
        CGPoint interPoint2 = CGPointZero;
        getComCenterlineInterCircle(associatedWL1, associatedWL1.wPoint2.wallPoint, Length, &interPoint1, &interPoint2);
        archComp.componentView.center = interPoint2;
        archComp.componentView.transform = CGAffineTransformMakeRotation([associatedWL1 CurrentLineAngle]);
    }
    for (HomeArchItem *archComp in associatedWL2.wallComponentArr) {
        CGFloat Length = [associatedWL2 getWallLineWidth] * archComp.percent;
        CGPoint interPoint1 = CGPointZero;
        CGPoint interPoint2 = CGPointZero;
        getComCenterlineInterCircle(associatedWL2, associatedWL2.wPoint2.wallPoint, Length, &interPoint1, &interPoint2);
        archComp.componentView.center = interPoint2;
        archComp.componentView.transform = CGAffineTransformMakeRotation([associatedWL2 CurrentLineAngle]);
    }
}

//墙体组件的移动
- (void)touchViewOnLineMove:(CGPoint)touchP selectedView:(UIImageView *)imageView{
    if (selectedRoomIndex == UNSELECTROOM)return;
    CGFloat tempDis = 0.0f;
    CGFloat minDistant = CGFLOAT_MAX;
    HomeWallLine *minLine = nil;
    NSArray *selectedRoomLines = mArchPlane.roomPlanes[selectedRoomIndex].wallLines;
    for (HomeWallLine *wallLine in selectedRoomLines) {
        tempDis = [wallLine distanceOfLineFromPoint:touchP];
        if (tempDis < minDistant) {
            minDistant = tempDis;
            minLine = wallLine;
        }
    }
    if (minDistant) {
        CGPoint footPoint = [minLine pedalOfLineAndVerticalAccordingToLineOutPoint:touchP];
        if( ![self isIntersectionOfComponentsCurrentLine:minLine pedal:footPoint]){
            if (footPoint.x <= minLine.maxX && footPoint.x >= minLine.minX && footPoint.y <= minLine.maxY && footPoint.y >= minLine.minY) {
                imageView.center = footPoint;
                imageView.transform = CGAffineTransformMakeRotation([minLine CurrentLineAngle]);
            }
        }
    }
}

//判断组件是否相交
-(bool)isIntersectionOfComponentsCurrentLine:(HomeWallLine *)currentLine pedal:(CGPoint)pedal {
    if (currentLine.wallComponentArr.count == 1) {
        return false;
    }
    for (HomeArchItem *component in currentLine.wallComponentArr) {
        if(component.componentView.hash == selectedComphonent.hash) continue;
        CGFloat disOfPoints = sqrtf(powf((pedal.y - component.componentView.center.y), 2) + powf((pedal.x - component.componentView.center.x), 2));
        CGFloat comRadiuDis = selectedArch.componentWidth/2 +  component.componentWidth/2;
        if (disOfPoints + 0.1f <= comRadiuDis) {
            return true;
        }
    }
    return false;
}

// 墙体点移动自动垂直和水平
- (void)pointOfSelectedLineVerticalAndHoriztonal:(CGPoint)touchP{
    if (selectedRoomIndex == UNSELECTROOM) return;
    HomeRoomPlane *selecedArch = [mArchPlane.roomPlanes objectAtIndex:selectedRoomIndex];
    NSInteger index = [selecedArch.roomPoints indexOfObject:selectedPoint];
    HomeWallPoint *point1 = nil;
    HomeWallPoint *point2 = nil;
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
    CGPoint tempPoint = selectedPoint.wallPoint;
    if (fabs(selectedPoint.wallPoint.x - point1.wallPoint.x) < 10.0f){
        tempPoint.x = point1.wallPoint.x;
    } if (fabs(selectedPoint.wallPoint.y - point2.wallPoint.y) < 10.0f){
        tempPoint.y = point2.wallPoint.y;
    } if (fabs(selectedPoint.wallPoint.x - point2.wallPoint.x) < 10.0f) {
        tempPoint.x = point2.wallPoint.x;
    } if (fabs(selectedPoint.wallPoint.y - point1.wallPoint.y) < 10.0f) {
        tempPoint.y = point1.wallPoint.y;
    }
    selectedPoint.wallPoint = tempPoint;
}

//墙体组件移动后，更改组件在墙体的位置
- (void)changeIndexOfWallComponentInSelectedRoomPlanes{
    if (selectedRoomIndex == UNSELECTROOM) return;
    NSArray *selectedRoomLines = mArchPlane.roomPlanes[selectedRoomIndex].wallLines;
    HomeWallLine *desWallLine = nil;
    HomeArchItem *changeArch = nil;
    CGFloat imaDisLine = CGFLOAT_MAX;
    for (HomeWallLine *wallLine in selectedRoomLines) {
        imaDisLine = [wallLine distanceOfLineFromPoint: selectedComphonent.center];
        if(imaDisLine < 2.0f){
            desWallLine = wallLine;
        }
    }
    changeArch = selectedArch;
    if (desWallLine.hash != imageViewOfOrigLine.hash) {
        [desWallLine.wallComponentArr addObject:changeArch];
        changeArch.wallIndex = [selectedRoomLines indexOfObject:desWallLine];
        [imageViewOfOrigLine.wallComponentArr removeObject:changeArch];
    }
}

- (void)getPercentOfBeforeMoving{
    HomeRoomPlane *selecedRoom = [mArchPlane.roomPlanes objectAtIndex:selectedRoomIndex];
    NSInteger index = [selecedRoom.roomPoints indexOfObject:selectedPoint];
    associatedWL1 = nil;
    associatedWL2 = nil;
    if (index == 0) {
        associatedWL1 = selecedRoom.wallLines[0];
        associatedWL2= [selecedRoom.wallLines lastObject];
    }else{
        associatedWL1 = selecedRoom.wallLines[index];
        associatedWL2 = selecedRoom.wallLines[index -1];
    }
    [associatedWL1 updateComponentPercentOfWallLine];
    [associatedWL2 updateComponentPercentOfWallLine];
}

@end

