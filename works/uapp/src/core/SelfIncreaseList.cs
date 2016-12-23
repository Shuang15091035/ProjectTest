using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public class SelfIncreaseList<T> : UObject {

		private List<T> mList;

		public T this[int index] {
			get {
				if (mList == null) {
					return default(T);
				}
				if (index < 0 || index >= mList.Count) {
					return default(T);
				}
				return mList[index];
			}
			set {
				if (index < 0) {
					return;
				}
				if (mList == null) {
					mList = new List<T>();
				}
				var count = mList.Count;
				if (index >= count) {
					for (var i = count; i <= index; i++) {
						mList.Add(default(T));
					}
				}
				mList[index] = value;
			}
		}

		public void Clear() {
			if (mList == null) {
				return;
			}
			mList.Clear();
		}
	}
}
