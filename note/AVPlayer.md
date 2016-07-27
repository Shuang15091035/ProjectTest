###视频播放工具 
	**MPMoviePlayerViewController和AVPlayer
	MPMoviePlayerController足够强大，几乎不用写几行代码就能完成一个播放器，但是正是由于它的高度封装使得要自定义这个播放器变得很复杂，甚至是不可能完成。例如有些时候需要自定义播放器的样式，那么如果要使用MPMoviePlayerController就不合适了，如果要对视频有自由的控制则可以使用AVPlayer。AVPlayer存在于AVFoundation中，它更加接近于底层，所以灵活性也更强：
###音频播放
	**AVAudioPlayer
	1.音频文件，2，初始化音频播放对象，3设置播放对象，4，预播放，5播放