using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class FPSCameraInfo : MonoBehaviour {

		private FPSCamera mCamera;
		public Vector3 Position;
		public float Height;
		public Vector2 Angles;
	
		void Start() {
			var cpb = gameObject.GetComponent<CameraPrefabBehaviour>();
			if (cpb == null) {
				throw new UnityException("FPSCamera not found.");
			}
			var cp = cpb.CameraPrefab;
			if (cp == null || !(cp is FPSCamera)) {
				throw new UnityException("FPSCamera not found.");
			}
			mCamera = cp as FPSCamera;
		}

		void Update() {
			Position = mCamera.Position;
			Height = mCamera.Height;
			Angles = mCamera.Angles;
		}
	}
}
