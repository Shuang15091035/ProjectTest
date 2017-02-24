//
//  ViewController.h
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/20.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>

@protocol ViewControllerDelegate;

@interface ViewController : UIViewController

@property (nonatomic, weak) id<ViewControllerDelegate> delegate;

@property (nonatomic, readonly) NSTimeInterval timeSinceLastDraw;

@property (nonatomic) NSUInteger interval;

@property (nonatomic, getter=isPause) BOOL pause;

- (void)dispatchGameLoop;

- (void)stopGameLoop;

@end

@protocol ViewControllerDelegate <NSObject>

//Note this method is called from the thread the main game loop is run
- (void)update:(ViewController *)viewController;

//call when the main game loop is paused, such a when the app is backgrounded
- (void)viewController:(ViewController *)viewController willPause:(BOOL)pause;

@end
