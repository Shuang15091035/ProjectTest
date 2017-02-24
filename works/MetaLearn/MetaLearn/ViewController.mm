//
//  ViewController.m
//  MetaLearn
//
//  Created by mac zdszkj on 2017/2/20.
//  Copyright © 2017年 mac zdszkj. All rights reserved.
//

#import "ViewController.h"
#import "CustomView.h"
#import "Render.h"

@interface ViewController ()

@end

@implementation ViewController{
    CADisplayLink *_timer;
    
    BOOL _firstDrawOccurred;
    
    CFTimeInterval _timeSinceLastDrawPreviousTime;
    
    BOOL _gameLoopPaused;
    
    Render *_renderer;
    
}

- (void)dealloc{
    [[NSNotificationCenter defaultCenter] removeObserver: self
                                                    name: UIApplicationDidEnterBackgroundNotification
                                                  object: nil];
    
    [[NSNotificationCenter defaultCenter] removeObserver: self
                                                    name: UIApplicationWillEnterForegroundNotification
                                                  object: nil];
    
    if(_timer)
    {
        [self stopGameLoop];
    }
}

- (void)initCommon
{
    _renderer = [[Render alloc]init];
    
    // While this is standard across all the metal samples, this renderer doesn't use the view controller delegate.
//    self.delegate = _renderer;
    
    //  Register notifications to start/stop drawing as this app moves into the background
    [[NSNotificationCenter defaultCenter] addObserver: self
                                             selector: @selector(didEnterBackground:)
                                                 name: UIApplicationDidEnterBackgroundNotification
                                               object: nil];
    
    [[NSNotificationCenter defaultCenter] addObserver: self
                                             selector: @selector(willEnterForeground:)
                                                 name: UIApplicationWillEnterForegroundNotification
                                               object: nil];
    
    _interval = 1;
}

- (id)init
{
    self = [super init];
    
    if(self)
    {
        [self initCommon];
    }
    return self;
}

// called when loaded from nib
- (id)initWithNibName:(NSString *)nibNameOrNil
               bundle:(NSBundle *)nibBundleOrNil
{
    self = [super initWithNibName:nibNameOrNil
                           bundle:nibBundleOrNil];
    
    if(self)
    {
        [self initCommon];
    }
    
    return self;
}

// called when loaded from storyboard
- (id)initWithCoder:(NSCoder *)coder
{
    self = [super initWithCoder:coder];
    
    if(self)
    {
        [self initCommon];
    }
    
    return self;
}

- (void)viewDidLoad {
    [super viewDidLoad];
    // Do any additional setup after loading the view, typically from a nib.
    CustomView *customView = (CustomView *)self.view;
    customView.delegate = _renderer;
    [_renderer configure:customView];
}


- (void)dispatchGameLoop{
    // create a game loop timer using a display link
    _timer = [[UIScreen mainScreen] displayLinkWithTarget:self
                                                 selector:@selector(gameloop)];
    _timer.frameInterval = _interval;
    [_timer addToRunLoop:[NSRunLoop mainRunLoop]
                 forMode:NSDefaultRunLoopMode];
}
- (void)gameloop{
     [_delegate update:self];
    
    assert([self.view isKindOfClass:[CustomView class]]);
    
    // call the display method directly on the render view (setNeedsDisplay: has been disabled in the renderview by default)
    [(CustomView *)self.view display];
}
- (void)stopGameLoop
{
    if(_timer)
    [_timer invalidate];
}
- (void)setPaused:(BOOL)pause
{
    if(_gameLoopPaused == pause)
    {
        return;
    }
    
    if(_timer)
    {
        // inform the delegate we are about to pause
        [_delegate viewController:self
                        willPause:pause];
        
        if(pause == YES)
        {
            _gameLoopPaused = pause;
            _timer.paused   = YES;
            
            // ask the view to release textures until its resumed
            [(CustomView *)self.view releaseTextures];
        }
        else
        {
            _gameLoopPaused = pause;
            _timer.paused   = NO;
        }
    }
}

- (BOOL)isPaused
{
    return _gameLoopPaused;
}
- (void)didEnterBackground:(NSNotification*)notification
{
    [self setPaused:YES];
}

- (void)willEnterForeground:(NSNotification*)notification
{
    [self setPaused:NO];
}

- (void)viewWillAppear:(BOOL)animated
{
    [super viewWillAppear:animated];
    
    // run the game loop
    [self dispatchGameLoop];
}

- (void)viewWillDisappear:(BOOL)animated
{
    [super viewWillDisappear:animated];
    
    // end the gameloop
    [self stopGameLoop];
}


@end
