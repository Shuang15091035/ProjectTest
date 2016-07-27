//
//  AudioPlayerView.m
//  PlayerTest
//
//  Created by mac zdszkj on 16/6/21.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

/*
 1、添加播放器()
 AudioPlayerView *audioView = [[[NSBundle mainBundle] loadNibNamed:@"AudioPlayerView" owner:self options:nil] objectAtIndex:0];
 [self.view addSubview:audioView];
 [audioView setAudioName:@"demo.mp3"];
 2、移除播放器
 [audioView dismissFromSuperview];
 audioView = nil;
 */

#import "AudioPlayerView.h"

#define animationDuration  0.3

@implementation AudioPlayerView
@synthesize mediaPlayer = _mediaPlayer;

- (id)initWithFrame:(CGRect)frame
{
    self = [super initWithFrame:frame];
    if (self) {
        // Initialization code
    }
    return self;
}

- (id)initWithCoder:(NSCoder *)aDecoder
{
    self = [super initWithCoder:aDecoder];
    if(self){
        //your corder
        currentDuration = 0;
        totalDuration = 0;
    }
    return self;
}

- (void)willMoveToSuperview:(UIView *)newSuperview
{
    if(newSuperview){    //视图将要出现时的动画
        CGRect audioFrame = self.frame;
        audioFrame.origin.y = newSuperview.bounds.size.height;
        self.frame = audioFrame;
        audioFrame.origin.y = newSuperview.bounds.size.height-self.bounds.size.height;
        [UIView animateWithDuration:animationDuration animations:^{
            self.frame = audioFrame;
        }];
    }
}

- (void)dismissFromSuperview
{
    [self stopAVPlayer];
    CGRect audioFrame = self.frame;
    audioFrame.origin.y = self.frame.size.height+self.frame.origin.y;
    [UIView animateWithDuration:animationDuration animations:^{
        self.frame = audioFrame;
    } completion:^(BOOL finished){
        [self removeFromSuperview];
    }];
}

- (void)setAudioName:(NSString *)name
{
    NSFileManager *fileManage = [NSFileManager defaultManager];
    NSString *path = [[NSBundle mainBundle] pathForResource:name ofType:nil];
    if(![fileManage fileExistsAtPath:path]){
        [self performSelector:@selector(showAlertViewWithMessage:) withObject:@"语音文件不存在" afterDelay:.5];
        return ;
    }
    
    NSURL *audioURL = [NSURL fileURLWithPath:path];
    
    AVURLAsset *urlAsset = [AVURLAsset URLAssetWithURL:audioURL options:nil];
    AVPlayerItem *playerItem = [AVPlayerItem playerItemWithAsset:urlAsset];;
    self.mediaPlayer = [AVPlayer playerWithPlayerItem:playerItem];
    [self initProcessSliderTimer];
    
    NSLog(@"totalDuration %f",totalDuration);
    CMTime totalTime = playerItem.duration;  //获取音频的总时间
    totalDuration = (CGFloat)totalTime.value/totalTime.timescale;
    [self initTimelabel];
    [self performSelector:@selector(startOrStopButtonAction:) withObject:startOrStopButton afterDelay:animationDuration];
    
    //播放视频时使用
    //    AVPlayerLayer * playerLayer = [AVPlayerLayer playerLayerWithPlayer:self.mediaPlayer];
    //    [playerLayer setFrame:CGRectMake(0, 0, 320, 280)];
    //    [self.superview.layer addSublayer:playerLayer];
}

- (void)showAlertViewWithMessage:(NSString *)message
{
    [[[UIAlertView alloc] initWithTitle:nil
                                message:message
                               delegate:self
                      cancelButtonTitle:@"确定"
                      otherButtonTitles:nil, nil] show];
}

- (void)awakeFromNib
{
    self.backgroundColor = [UIColor colorWithPatternImage:[UIImage imageNamed:@"audioPlayerBg.png"]];
    [self initSlider];
}

- (CMTime)playerItemDuration
{
    AVPlayerItem *playerItem = [self.mediaPlayer currentItem];
    if (playerItem.status == AVPlayerItemStatusReadyToPlay) {
        return([playerItem duration]);
    }
    return(kCMTimeInvalid);
}
#pragma mark - 更新进度条的Timer
-(void)initProcessSliderTimer
{
    if(!mTimeObserver){
        CMTime time = CMTimeMake(1,1);
        __weak typeof(self) weakSelf = self;
        mTimeObserver = [self.mediaPlayer addPeriodicTimeObserverForInterval:time
                                                                       queue:NULL
                                                                  usingBlock:^(CMTime time){
                                                                      [weakSelf syncProcessSlider];
                                                                  }];
    }
    
    CMTime playerDuration = [self playerItemDuration];
    if (CMTIME_IS_INVALID(playerDuration)) {
        processSlider.value = 0;
    }
}

- (void)syncProcessSlider
{
    //实时刷新播放进度条
    CMTime playerDuration = [self playerItemDuration];
    if (CMTIME_IS_INVALID(playerDuration)) {
        processSlider.minimumValue = 0.0;
        return;
    }
    totalDuration = CMTimeGetSeconds(playerDuration);
    if (isfinite(totalDuration)){
        float minValue = [processSlider minimumValue];
        float maxValue = [processSlider maximumValue];
        currentDuration = CMTimeGetSeconds([self.mediaPlayer currentTime]);
        [self initTimelabel];
        [processSlider setValue:(maxValue - minValue) * currentDuration / totalDuration + minValue];
    }
    if (processSlider.value == 1.0) {
        [self.mediaPlayer seekToTime:kCMTimeZero];
        [startOrStopButton setImage:[UIImage imageNamed:@"audioPlayerStart.png"] forState:UIControlStateNormal];
    }
}
#pragma 开始暂停
- (void)startOrStopButtonAction:(id)sender
{
    if (self.mediaPlayer.rate == 1) {
        [self.mediaPlayer pause];
        [startOrStopButton setImage:[UIImage imageNamed:@"audioPlayerStart.png"] forState:UIControlStateNormal];
    } else {
        [self.mediaPlayer play];
        
        [startOrStopButton setImage:[UIImage imageNamed:@"audioPlayerPause.png"] forState:UIControlStateNormal];
        [self initProcessSliderTimer];
    }
}

#pragma mark - 初始化时间Label
- (void)initTimelabel
{
    //更新显示的时间
    NSString *currentTime = [self formatDateWithSecond:(int)currentDuration];
    NSString *totalTime = [self formatDateWithSecond:(int)totalDuration];
    NSString *timeString = [NSString stringWithFormat:@"%@ / %@",currentTime,totalTime];
    timeLabel.text = timeString;
}
#pragma mark - 时间格式化
- (NSString *)formatDateWithSecond:(int)second
{
    if(!second || second<=0){
        return @"00:00";
    }
    int hour = second/(60*60);
    int minute = (second-(60*60)*hour)/60;
    second -= (60*60)*hour+minute*60;
    
    if(hour!=0){
        return [NSString stringWithFormat:@"%02i:%02i:%02i", hour, minute, second];
    }else{
        return [NSString stringWithFormat:@"%02i:%02i", minute, second];
    }
    return nil;
}
#pragma mark - 控制声音Slider
- (void)initSlider
{
    if([[[UIDevice currentDevice] systemVersion] floatValue]>=7.0){
        CGRect proceeFrame = processSlider.frame;
        proceeFrame.origin.y = 49;
        processSlider.frame = proceeFrame;
        CGRect volumeFrame = volumeSlider.frame;
        volumeFrame.origin.y = 49;
        volumeSlider.frame = volumeFrame;
    }
    // 自定义 UISlider 的图片
    UIImage *minImage = [UIImage imageNamed:@"audioPlayerSliderMinTrack.png"];   //进度条
    UIImage *maxImage = [UIImage imageNamed:@"audioPlayerSliderMaxTrack.png"];   //滑块的底图
    UIImage *tumbImage= [UIImage imageNamed:@"audioPlayerSliderThumb.png"];      //中间的圆形控制按钮
    
    minImage=[minImage resizableImageWithCapInsets:UIEdgeInsetsMake(2, 2, 2, 2)];
    maxImage=[maxImage resizableImageWithCapInsets:UIEdgeInsetsMake(2, 2, 2, 2)];
    
    [processSlider setMinimumTrackImage:minImage forState:UIControlStateNormal];
    [processSlider setMaximumTrackImage:maxImage forState:UIControlStateNormal];
    [processSlider setThumbImage:tumbImage forState:UIControlStateNormal];
    processSlider.minimumValue = 0.0;
    processSlider.maximumValue = 1.0;
    processSlider.continuous = YES;
    
    // Attach an action to slider
    [processSlider addTarget:self action:@selector(processSliderStartDragAction:)
            forControlEvents:UIControlEventTouchDown];
    [processSlider addTarget:self action:@selector(sliderValueChangedAction:)
            forControlEvents:UIControlEventValueChanged];
    [processSlider addTarget:self action:@selector(processSliderEndDragAction:)
            forControlEvents:UIControlEventTouchUpInside | UIControlEventTouchUpOutside];
    
    [volumeSlider setMinimumTrackImage:minImage forState:UIControlStateNormal];
    [volumeSlider setMaximumTrackImage:maxImage forState:UIControlStateNormal];
    [volumeSlider setThumbImage:tumbImage forState:UIControlStateNormal];
    volumeSlider.minimumValue = 0.0;
    volumeSlider.maximumValue = 1.0;
    volumeSlider.continuous = YES;
    volumeSlider.value = [[MPMusicPlayerController applicationMusicPlayer] volume]; //获取系统声音的值
    [volumeSlider addTarget:self action:@selector(sliderValueChangedAction:)
           forControlEvents:UIControlEventValueChanged];
}
#pragma mark - 视频进度条控制
- (void)processSliderStartDragAction:(id)sender
{
    if (self.mediaPlayer.rate == 1) {
        isPlay = true;
        [self.mediaPlayer pause];
    } else{
        isPlay = false;
    }
}
- (void)processSliderEndDragAction:(id)sender
{
    //调节播放进度
    CMTime dragedCMTime = CMTimeMake(totalDuration*processSlider.value, 1);
    [self.mediaPlayer seekToTime:dragedCMTime completionHandler:^(BOOL finish){
        if(isPlay){
            [self.mediaPlayer play];
        }
    }];
}
#pragma mark - 音频进度条控制
- (void)sliderValueChangedAction:(id)sender
{
    UISlider *targetSlider = (UISlider *)sender;
    if (targetSlider == processSlider) {
        currentDuration = totalDuration*targetSlider.value;
        [self initTimelabel];
    }else if (targetSlider == volumeSlider){
        float volume=[[NSString stringWithFormat:@"%.1f",volumeSlider.value] floatValue];
        [[MPMusicPlayerController applicationMusicPlayer]setVolume:volume];   //设置声音
    }
}


- (void)dealloc
{
    NSLog(@"AudioPlayerView  dealloc!");
    [self stopAVPlayer];
}
#pragma mark - 暂停播放
- (void)stopAVPlayer
{
    if (self.mediaPlayer.rate == 1) {
        [self.mediaPlayer pause];
    }
    self.mediaPlayer = nil;
    mTimeObserver = nil;
}

@end
