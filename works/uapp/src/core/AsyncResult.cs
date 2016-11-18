/**
 * @file AsyncResult.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class AsyncResult<T> {

		private T mSyncResult;

		public T syncResult {
			get {
				return mSyncResult;
			} set {
				mSyncResult = value;
			}
		}
	}
}