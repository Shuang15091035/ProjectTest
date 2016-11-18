/**
 * @file IDataSet.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class DataSet : IDataSet {

		protected IDataSet mDataSet;
		protected ArrayList mObservers;

		public DataSet():this(null) {

		}

		public DataSet(IDataSet dataSet) {
			if (dataSet == null) {
				mDataSet = this;
			} else {
				mDataSet = dataSet;
			}
		}

		public void RegisterDataSetObserver(IDataSetObserver observer) {
			if (observer == null) {
				return;
			}
			observers.Add(observer);
		}

		public void UnregisterDataSetObserver(IDataSetObserver observer) {
			if (observer == null) {
				return;
			}
			observers.Remove(observer);
		}

		public virtual void NotifyDataSetChanged() {
			foreach(IDataSetObserver observer in observers) {
				observer.OnDataSetChanged(mDataSet);
			}
		}

		protected ArrayList observers {
			get {
				if (mObservers == null) {
					mObservers = new ArrayList();
				}
				return mObservers;
			}
		}
	}
}