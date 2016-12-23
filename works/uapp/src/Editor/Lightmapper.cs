using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using UnityEditor;

namespace uapp {
	
	public class Lightmapper : Editor {

		[MenuItem("Lightmapper/Save Lightmaps")]
		public static void SaveLightmaps() {
			var scene = SceneManager.GetActiveScene();
			var gameObjects = scene.GetRootGameObjects();
			foreach (var gameObject in gameObjects) {
				var renderers = gameObject.GetComponentsInChildren<Renderer>();
				foreach (var renderer in renderers) {
					if (renderer.lightmapIndex < 0) {
						continue;
					}
					var lightmapInfo = uapp.ObjectUtils.AddComponentIfNotExists<LightmapInfo>(renderer.gameObject);
					lightmapInfo.SceneName = scene.name;
					lightmapInfo.LightmapIndex = renderer.lightmapIndex;
					lightmapInfo.LightmapScaleOffset = renderer.lightmapScaleOffset;
				}
			}
		}

		[MenuItem("Lightmapper/Restore Lightmaps")]
		public static void RestoreLightmaps() {
			var scene = SceneManager.GetActiveScene();
			var gameObjects = scene.GetRootGameObjects();
			foreach (var gameObject in gameObjects) {
				var renderers = gameObject.GetComponentsInChildren<Renderer>();
				foreach (var renderer in renderers) {
					var lightmapInfo = renderer.gameObject.GetComponent<LightmapInfo>();
					if (lightmapInfo == null) {
						continue;
					}
					renderer.lightmapIndex = lightmapInfo.LightmapIndex;
					renderer.lightmapScaleOffset = lightmapInfo.LightmapScaleOffset;
				}
			}
		}

		[MenuItem("Lightmapper/Clear Lightmaps")]
		public static void ClearLightmaps() {
			var scene = SceneManager.GetActiveScene();
			var gameObjects = scene.GetRootGameObjects();
			foreach (var gameObject in gameObjects) {
				var renderers = gameObject.GetComponentsInChildren<Renderer>();
				foreach (var renderer in renderers) {
					var lightmapInfos = renderer.gameObject.GetComponents<LightmapInfo>();
					if (lightmapInfos == null) {
						continue;
					}
					foreach (var lightmapInfo in lightmapInfos) {
						GameObject.DestroyImmediate(lightmapInfo);
					}
				}
			}
		}
	}

}
