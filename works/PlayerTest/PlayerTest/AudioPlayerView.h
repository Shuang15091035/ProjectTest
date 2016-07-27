//
//  AudioPlayerView.h
//  PlayerTest
//
//  Created by mac zdszkj on 16/6/21.
//  Copyright © 2016年 mac zdszkj. All rights reserved.
//

#import <UIKit/UIKit.h>
#import <AVFoundation/AVFoundation.h>
#import <MediaPlayer/MediaPlayer.h>
#import <CoreMedia/CoreMedia.h>

@interface AudioPlayerView : UIView <AVAudioPlayerDelegate>
{
    __weak IBOutlet UILabel *timeLabel;
    __weak IBOutlet UISlider *processSlider;
    __weak IBOutlet UISlider *volumeSlider;
    __weak IBOutlet UIButton *startOrStopButton;
    
    float totalDuration;
    float currentDuration;
    
    BOOL isPlay; //是否正在播放
    id mTimeObserver;
}

@property (nonatomic , strong) AVPlayer *mediaPlayer;

- (void)setAudioName:(NSString *)name;
- (void)dismissFromSuperview;
- (IBAction)startOrStopButtonAction:(id)sender;

@end
