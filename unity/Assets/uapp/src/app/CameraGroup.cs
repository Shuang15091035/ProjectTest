using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public class CameraGroup {

		public delegate void OnCameraChangedDelegate(Camera camera);

		private IList<Camera> mCameras;
		private Camera mCurrentCamera;
		private OnCameraChangedDelegate mOnCameraChangedDelegate;

		public CameraGroup() {
			
		}

		public void AddCamera(Camera camera) {
			if (mCameras == null) {
				mCameras = new List<Camera>();
			}
			mCameras.Add(camera);
		}

		public void RemoveCamera(Camera camera) {
			if (mCameras == null) {
				return;
			}
			mCameras.Remove(camera);
		}

		public void ChangeCamera(Camera camera) {
			if (mCameras == null) {
				return;
			}
			foreach (var cam in mCameras) {
				cam.gameObject.SetActive(cam == camera);
			}
			mCurrentCamera = camera;
			if (mOnCameraChangedDelegate != null) {
				mOnCameraChangedDelegate(camera);
			}
		}

		public Camera CurrentCamera {
			get {
				return mCurrentCamera;
			}
		}

		public OnCameraChangedDelegate OnCameraChanged {
			get {
				return mOnCameraChangedDelegate;
			} set {
				mOnCameraChangedDelegate = value;
			}
		}
	}
}
