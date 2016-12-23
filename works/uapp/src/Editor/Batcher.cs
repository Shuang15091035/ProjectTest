using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using UnityEditor;

namespace uapp {
	
	public class Batcher : Editor {

		[MenuItem("Batcher/Enable All Conlliders")]
		static void EnableAllConlliders() {
			var scene = SceneManager.GetActiveScene();
			var gameObjects = scene.GetRootGameObjects();
			foreach (var gameObject in gameObjects) {
				ObjectUtils.EnumComponent<Collider>(gameObject, (GameObject go, Collider collider) => {
					collider.enabled = true;
				}, true);
			}
		}

		[MenuItem("Batcher/Disable All Conlliders")]
		static void DisableAllConlliders() {
			var scene = SceneManager.GetActiveScene();
			var gameObjects = scene.GetRootGameObjects();
			foreach (var gameObject in gameObjects) {
				ObjectUtils.EnumComponent<Collider>(gameObject, (GameObject go, Collider collider) => {
					collider.enabled = false;
				}, true);
			}
		}
	}
}
