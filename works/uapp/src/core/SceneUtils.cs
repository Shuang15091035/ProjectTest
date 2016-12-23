using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace uapp {
	
	public class SceneUtils {

		public static Bounds GetTransformBounds(Scene scene) {
			Bounds bounds = new Bounds();
			bool boundsIsNull = true;

			var gameObjects = scene.GetRootGameObjects();
			foreach (var gameObject in gameObjects) {
				var b = uapp.ObjectUtils.GetTransformBounds(gameObject);
				if (boundsIsNull) {
					bounds = b;
					boundsIsNull = false;
				} else {
					bounds.Encapsulate(b);
				}
			}

			return bounds;
		}

		public static Bounds GetCurrentTransformBounds() {
			return GetTransformBounds(SceneManager.GetActiveScene());
		}

		public static void SetAllLightsOnOff(Scene scene, bool onOff) {
			var gameObjects = scene.GetRootGameObjects();
			foreach (var gameObject in gameObjects) {
				var lights = gameObject.GetComponentsInChildren<Light>();
				foreach (var light in lights) {
					light.enabled = onOff;
				}
			}
		}

		public static void SetCurrentAllLightsOnOff(bool onOff) {
			SetAllLightsOnOff(SceneManager.GetActiveScene(), onOff);
		}

	}

}
