/**
 * @file ISceneLoader.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-9-15
 * @brief
 */
using UnityEngine;
using System;
using System.Collections;

namespace uapp {

	public interface OnSceneLoadListener {
		void OnResourceLoaded(UnityEngine.Object res, IFile resFile);
		void OnSceneLoadObject(GameObject obj);
		void OnSceneLoadingProgress(float progress);
		void OnSceneFinishLoaded(GameObject obj, GameObject parent, IFile file);
		void OnSceneFailToLoad(GameObject parent, IFile file, Exception error);
	}

	public interface OnSceneSaveListener {
		void OnSceneFinishSaved(GameObject[] objects, IFile file);
		void OnSceneFailToSave(GameObject[] objects, IFile file, Exception error);
	}

	public interface ISceneLoader {

		string Pattern { get; }
		GameObject Load(IFile file, GameObject parent);
		AsyncResult<GameObject> Load(IFile file, GameObject parent, bool async, OnSceneLoadListener listener);
		bool Save(IFile file, GameObject[] objects);
		AsyncResult<bool> Save(IFile file, GameObject[] objects, bool async, OnSceneSaveListener listener);
	}

	public class BaseOnSceneLoadListener : OnSceneLoadListener {
		public delegate void OnResourceLoadedDelegate(UnityEngine.Object res, IFile resFile);
		public delegate void OnSceneLoadObjectDelegate(GameObject obj);
		public delegate void OnSceneLoadingProgressDelegate(float progress);
		public delegate void OnSceneFinishLoadedDelegate(GameObject obj, GameObject parent, IFile file);
		public delegate void OnSceneFailToLoadDelegate(GameObject parent, IFile file, Exception error);

        private OnResourceLoadedDelegate mOnResource;
		private OnSceneLoadObjectDelegate mOnObject;
		private OnSceneLoadingProgressDelegate mOnProgress;
		private OnSceneFinishLoadedDelegate mOnFinish;
		private OnSceneFailToLoadDelegate mOnFailed;

        public OnResourceLoadedDelegate OnResource {
            get {
                return mOnResource;
            } set {
                mOnResource = value;
            }
        }

		public OnSceneLoadObjectDelegate OnOjbect {
			get {
				return mOnObject;
			} set {
				mOnObject = value;
			}
		}

		public OnSceneLoadingProgressDelegate OnProgress {
			get {
				return mOnProgress;
			} set {
				mOnProgress = value;
			}
		}

		public OnSceneFinishLoadedDelegate OnFinish {
			get {
				return mOnFinish;
			} set {
				mOnFinish = value;
			}
		}

		public OnSceneFailToLoadDelegate OnFailed {
			get {
				return mOnFailed;
			} set {
				mOnFailed = value;
			}
		}

		public void OnResourceLoaded(UnityEngine.Object res, IFile resFile) {
            if (mOnResource != null) {
                mOnResource(res, resFile);
            }
        }

		public void OnSceneLoadObject(GameObject obj) {
			if (mOnObject != null) {
				mOnObject(obj);
			}
		}

		public void OnSceneLoadingProgress(float progress) {
			if (mOnProgress != null) {
				mOnProgress(progress);
			}
		}

		public void OnSceneFinishLoaded(GameObject obj, GameObject parent, IFile file) {
			if (mOnFinish != null) {
				mOnFinish(obj, parent, file);
			}
		}

		public void OnSceneFailToLoad(GameObject parent, IFile file, Exception error) {
			if (mOnFailed != null) {
				mOnFailed(parent, file, error);
			}
		}
	}

	public class BaseOnSceneSaveListener : OnSceneSaveListener {
		public delegate void OnSceneFinishSavedDelegate(GameObject[] objects, IFile file);
		public delegate void OnSceneFailToSaveDelegate(GameObject[] objects, IFile file, Exception error);

		private OnSceneFinishSavedDelegate mOnFinish;
		private OnSceneFailToSaveDelegate mOnFailed;

		public OnSceneFinishSavedDelegate OnFinish {
			get {
				return mOnFinish;
			} set {
				mOnFinish = value;
			}
		}

		public OnSceneFailToSaveDelegate OnFailed {
			get {
				return mOnFailed;
			} set {
				mOnFailed = value;
			}
		}

		public void OnSceneFinishSaved(GameObject[] objects, IFile file) {
			if (mOnFinish != null) {
				mOnFinish(objects, file);
			}
		}

		public void OnSceneFailToSave(GameObject[] objects, IFile file, Exception error) {
			if (mOnFailed != null) {
				mOnFailed(objects, file, error);
			}
		}
	}
}
