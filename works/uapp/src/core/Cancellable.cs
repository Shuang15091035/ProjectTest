/**
 * @file Cancellable.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-21
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class Cancellable {

		protected bool mCancelled;
		
		public bool isCancelled {
			get {
				return mCancelled;
			}
		}

		public void cancel() {
			mCancelled = true;
		}
	}
}

