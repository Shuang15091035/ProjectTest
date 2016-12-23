using UnityEngine;
using System;
using System.Collections;

namespace uapp {
	
	public class Timer : ITimer {

		private long mCycle;
		private DateTime mDate;
		private bool mIsPaused = true;
		private long elapsedTime = 0;
		private OnTimerUpdateDelegate mOnUpdate = null;
		private OnTimerDateDelegate mOnDate = null;

		public Timer(long cycle = 17) {
			mCycle = cycle;
		}

		public long Cycle {
			get {
				return mCycle;
			}
			set {
				mCycle = value;
			}
		}

		public DateTime Date {
			get {
				return mDate;
			}
			set {
				mDate = value;
				Debug.Log("[Timer] will expire on " + mDate + ".");
			}
		}

		public void Start() {
			mIsPaused = false;
			elapsedTime = 0;
		}

		public void Pause() {
			mIsPaused = true;
		}

		public void FixedUpdate() {
			if (mIsPaused) {
				return;
			}
			long engineDeltaTime = (long)(Time.fixedDeltaTime * TimeUtils.Seconds);
			elapsedTime += engineDeltaTime;
			if (elapsedTime < mCycle) {
				return;
			}
			elapsedTime = elapsedTime - mCycle;
			if (mOnUpdate != null) {
				mOnUpdate(this);
			}
			if (mOnDate != null) {
				if (mDate != Constants.DateTimeNull && mDate < DateTime.Now) {
					Debug.Log("[Timer] date expire on " + mDate + ".");
					mOnDate(this);
					mDate = Constants.DateTimeNull;
				}
			}
		}

		public OnTimerUpdateDelegate OnUpdate {
			get {
				return mOnUpdate;
			}
			set {
				mOnUpdate = value;
			}
		}

		public OnTimerDateDelegate OnDate {
			get {
				return mOnDate;
			}
			set {
				mOnDate = value;
			}
		}

	}
}
