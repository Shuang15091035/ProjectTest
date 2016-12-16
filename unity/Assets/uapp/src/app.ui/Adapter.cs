/**
 * @file Adapter.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class Adapter<T> : DataSet, IAdapter<T> {

		public virtual int GetCount() {
			return 0;
		}

		public virtual T GetItemAt(int position) {
			return default(T);
		}

		public virtual int GetViewTypeAt(int position) {
			return 0;
		}

		public virtual int GetViewTypeCount() {
			return 1;
		}

		public virtual GameObject GetViewAt(int position, GameObject parent) {
			return null;
		}
	}
}