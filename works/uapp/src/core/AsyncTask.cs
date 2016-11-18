/**
 * @file AsyncTask.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-21
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public abstract class AsyncTask<T> : Cancellable {

		private T mResult;

		public virtual void OnPreExecute() {

		}

		public abstract IEnumerator DoInBackground();

		public virtual void OnPostExecute(T result) {

		}

		public Coroutine DoObjective(IEnumerator objective) {
			return GlobalCaller.StartCoroutine(objective);
		}

		public static AsyncTaskCaller GlobalCaller {
			get {
				var asyncTaskCaller = Lib.GlobalObject.GetComponent<AsyncTaskCaller>();
				if (asyncTaskCaller == null) {
					asyncTaskCaller = Lib.GlobalObject.AddComponent<AsyncTaskCaller>();
				}
				return asyncTaskCaller;
			}
		}

		public void execute(bool async) {
			if (async) {
				GlobalCaller.StartCoroutine(Executable);
			} else {
				OnPreExecute();
				if (mCancelled) {
					return;
				}
				IEnumerator routine = DoInBackground();
				if (routine == null) {
					OnPostExecute(default(T));
				}
//				while(routine.MoveNext()) {
//					if (Debug.isDebugBuild) {
//						Debug.Log("yelid " + routine.Current);
//					}
//				}
				while (routine.MoveNext())
					;
				OnPostExecute(mResult);
			}
		}

		public IEnumerator Executable {
			get {
				OnPreExecute();
				if (mCancelled) {
					yield break;
				}
				//yield return DoInBackground();
				IEnumerator e = DoInBackground();
				while (e.MoveNext()) {
					yield return e.Current;
				}
				OnPostExecute(mResult);
			}
		}

		public void Return(T result) {
			mResult = result;
		}

		public T Result {
			get {
				return mResult;
			}
		}
	}
}

