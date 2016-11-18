/**
 * @file ObjectLoader.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class ObjectLoader : IObjectLoader {
		private static IObjectLoader globalLoader;

		public static IObjectLoader global {
			get {
				if (globalLoader == null) {
					globalLoader = new ObjectLoader();
				}
				return globalLoader;
			}
		}

		public ObjectLoader() {
		}

		public GameObject load(IFile file) {
			GameObject obj = null;
			switch (file.Type) {
				case FileType.Resources: {
					GameObject origin = Resources.Load(file.Path, typeof(GameObject)) as GameObject;
					if (origin != null) {
						obj = GameObject.Instantiate(origin) as GameObject;
					}
					break;
				}
				default:
					break;
			}
			return obj;
		}

		public AsyncResult<GameObject> load(IFile file, bool async, OnObjectLoadListener listener) {
			// TODO
			AsyncResult<GameObject> result = null;
			return result;
		}
	}
}

