/**
 * @file IObjectLoader.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public interface OnObjectLoadListener {
		void onObjectLoaded(GameObject obj);
	}

	public interface IObjectLoader {
		GameObject load(IFile file);
		AsyncResult<GameObject> load(IFile file, bool async, OnObjectLoadListener listener);
	}
}