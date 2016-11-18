using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	// NOTE Unity使用系统时钟，如果时钟被修改，Unity也无法正常运行，所以在运行过程中无需检查时钟是否被修改
	public class FixKeyLicense : MonoBehaviour {

		public delegate void OnExpireDelegate();

		public string Name;

		private IDictionary<string, object> mKeyMap;
		private IFile mKeyFile;
		private IFile mTimeFile;
		private string mDateFormat;
		private ITimer mTimer = new Timer();
		private OnExpireDelegate mOnExpire;

//		void Start() {
//			if (Name == null) {
//				Debug.LogError("[FixKeyLicense] Please enter a name.");
//				return;
//			}
//			mDateFormat = "yyyy-MM-dd HH:mm:ss";
////			DateTime date;
////			if (Date == null) {
////				date = DateTime.Now + new TimeSpan(0, 0, 5);
////			} else {
////				if (!DateTime.TryParseExact(Date, mDateFormat, null, System.Globalization.DateTimeStyles.None, out date)) {
////					Debug.LogError("[FixKeyLicense] " + Date + " cannot be parse to DateTime.");
////					return;
////				}
////			}
//			mKeyFile = File.DataPath("jwlk_" + Name);
//			mTimeFile = File.DataPath("jwlt_" + Name);
////			StartOnDate(date);
//		}

		void FixedUpdate() {
			mTimer.FixedUpdate();
		}

		public void StartOnDate(DateTime date) {
			
			if (StringUtils.IsNullOrBlank(Name)) {
				Debug.LogError("[FixKeyLicense] Please enter a name.");
				return;
			}
			mDateFormat = "yyyy-MM-dd HH:mm:ss";
			mKeyFile = File.DataPath("jwlk_" + Name);
			mTimeFile = File.DataPath("jwlt_" + Name);

			// 检查系统时钟是否在启动前被恶意修改
			DateTime now = DateTime.Now;
			string timeText = mTimeFile.Text;
			if (!StringUtils.IsNullOrBlank(timeText)) {
				DateTime time;
				if (DateTime.TryParseExact(timeText, mDateFormat, null, System.Globalization.DateTimeStyles.None, out time)) {
					if (now < time) { // 时间被恶意修改为历史时间
						if (mOnExpire != null) {
							mOnExpire();
						}
					}
				}
			}
			mTimeFile.WriteText(String.Format("{0:" + mDateFormat + "}", now));

			// 验证已经存在的key
			if (!CheckExpire(CurrentKey)) {
				if (date == Constants.DateTimeNull) {
					date = DateTime.Now + new TimeSpan(0, 0, 5);
				}
				expireOnDate(date);
			}
		}

		public OnExpireDelegate OnExpire {
			get {
				return mOnExpire;
			} set {
				mOnExpire = value;
			}
		}

		public void AddKey(string key, DateTime expireDate) {
			if (mKeyMap == null) {
				mKeyMap = new Dictionary<string, object>();
			}
			mKeyMap[key] = expireDate;
		}

		public void AddKey(string key, TimeSpan expireTime) {
			if (mKeyMap == null) {
				mKeyMap = new Dictionary<string, object>();
			}
			mKeyMap[key] = expireTime;
		}

		public bool CheckExpire(string key) {
			DateTime expireDate = getExpireDateByKey(key);
			if (expireDate == Constants.DateTimeNull) {
				return false;
			}
			DateTime now = DateTime.Now;
			if (expireDate < now) {
				return false;
			}
			expireOnDate(expireDate);
			CurrentKey = key;
			return true;
		}

		private DateTime getExpireDateByKey(string key) {
			if (StringUtils.IsNullOrBlank(key) || mKeyMap == null || !mKeyMap.ContainsKey(key)) {
				return Constants.DateTimeNull;
			}
			var expireObject = mKeyMap[key];
			DateTime expireDate = Constants.DateTimeNull;
			if (expireObject is DateTime) {
				expireDate = (DateTime)expireObject;
			} else if (expireObject is TimeSpan) {
				var expireTime = (TimeSpan)expireObject;
				expireDate = DateTime.Now + expireTime;
			}
			return expireDate;
		}

		private void expireOnDate(DateTime date) {
			mTimer.Date = date;
			mTimer.OnDate = (ITimer timer) => {
				if (mOnExpire != null) {
					mOnExpire();
				}
			};
			mTimer.Start();
		}

		public string CurrentKey {
			get {
				var key = mKeyFile.Text;
				if (key == null) {
					mKeyFile.WriteText("");
					key = "";
				}
				return key;
			} set {
				mKeyFile.WriteText(value);
			}
		}

	}

}
