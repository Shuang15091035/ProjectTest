using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class BirdCameraInfo : MonoBehaviour {
	
		private BirdCamera mCamera;
		public Vector3 Position;
		public float Height;

		void Start() {
			var cpb = gameObject.GetComponent<CameraPrefabBehaviour>();
			if (cpb == null) {
				throw new UnityException("FPSCamera not found.");
			}
			var cp = cpb.CameraPrefab;
			if (cp == null || !(cp is BirdCamera)) {
				throw new UnityException("FPSCamera not found.");
			}
			mCamera = cp as BirdCamera;
		}

		void Update() {
			Position = mCamera.Position;
			Height = mCamera.Height;
		}
	}
}
