/**
 * @file SceneLoader.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-9-15
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public abstract class SceneLoader : ISceneLoader {

		public virtual string Pattern { 
			get {
				return null;
			}
		}

		protected abstract AsyncTask<GameObject> getLoadTask(IFile file, GameObject parent, OnSceneLoadListener listener, ISceneLoader sceneLoader);
		protected abstract AsyncTask<bool> getSaveTask(IFile file, GameObject[] objects, OnSceneSaveListener listener, ISceneLoader sceneLoader);

		public virtual GameObject Load(IFile file, GameObject parent) {
			return Load(file, parent, false, null).syncResult;
		}

		public virtual AsyncResult<GameObject> Load(IFile file, GameObject parent, bool async, OnSceneLoadListener listener) {
			AsyncResult<GameObject> result = new AsyncResult<GameObject>();
			AsyncTask<GameObject> task = getLoadTask(file, parent, listener, this);
			if (task == null) {
				result.syncResult = null;
				return result;
			}
			task.execute(async);
			result.syncResult = task.Result;
			return result;
		}

		public virtual bool Save(IFile file, GameObject[] objects) {
			return Save(file, objects, false, null).syncResult;
		}

		public virtual AsyncResult<bool> Save(IFile file, GameObject[] objects, bool async, OnSceneSaveListener listener) {
			AsyncResult<bool> result = new AsyncResult<bool>();
			AsyncTask<bool> task = getSaveTask(file, objects, listener, this);
			if (task == null) {
				result.syncResult = false;
				return result;
			}
			task.execute(async);
			result.syncResult = task.Result;
			return result;
		}
	}
}