using UnityEngine;
using System.Collections;

namespace project {
	
	public class Photographer {

		private uapp.CameraGroup mCameraGroup;
		private uapp.BirdCamera mBirdCamera;
		private uapp.EditorCamera mEditorCamera;
		private uapp.FPSCamera mFPSCamera;
		private uapp.BirdCamera mLimnCamera;
		public static float DefaultFPSCameraHeight = 1.65f;
		public static float DefaultBirdCameraHeight = 8.0f;

		private GameObject getDefaultCamera() {
			return GameObject.Instantiate(Resources.Load<GameObject>("Camera")) as GameObject;
		}

		private void highlightCamera(Camera camera) {
			var highlightRenderer = camera.gameObject.AddComponent<HighlightingSystem.HighlightingRenderer>();
			highlightRenderer.downsampleFactor = 2;
			highlightRenderer.iterations = 3;
			highlightRenderer.blurMinSpread = 0.5f;
			highlightRenderer.blurSpread = 0.5f;
			highlightRenderer.blurIntensity = 0.28f;
		}

		public uapp.BirdCamera BirdCamera {
			get {
				if (mBirdCamera == null) {
					mBirdCamera = new uapp.BirdCamera("BirdCamera", null, 0.0f);
					mBirdCamera.LeftMouseToMoveVertical = false;
					mBirdCamera.RightMouseToMoveVertical = true;
					mBirdCamera.ScrollMouseToMoveHorizontal = true;
					mBirdCamera.VerticalMoveSpeed = new Vector2(0.5f, 0.5f);
					mBirdCamera.HorizontalMoveSpeed = new Vector2(0.1f, 0.1f);
//					mBirdCamera.Camera.orthographic = true;
					highlightCamera(mBirdCamera.Camera);
				}
				return mBirdCamera;
			}
		}

		public uapp.EditorCamera EditorCamera {
			get {
				if (mEditorCamera == null) {
					mEditorCamera = new uapp.EditorCamera("EditorCamera", null, 45, 45, -10, getDefaultCamera());
					mEditorCamera.LeftMouseToMoveVertical = false;
					mEditorCamera.RightMouseToMoveVertical = true;
					mEditorCamera.ScrollMouseToMoveHorizontal = true;
					mEditorCamera.VerticalMoveSpeed = new Vector2(2.5f, 2.5f);
					mEditorCamera.HorizontalMoveSpeed = new Vector2(0.1f, 0.1f);
					mEditorCamera.PitchConstraintEnabled = true;
					mEditorCamera.MinPitch = 10.0f;
					mEditorCamera.MaxPitch = 90.0f;
					highlightCamera(mEditorCamera.Camera);
				}
				return mEditorCamera;
			}
		}

		public uapp.FPSCamera FPSCamera {
			get {
				if (mFPSCamera == null) {
					mFPSCamera = new uapp.FPSCamera("FPSCamera", null, DefaultFPSCameraHeight, getDefaultCamera());
					mFPSCamera.LeftMouseToMoveVertical = false;
					mFPSCamera.RightMouseToMoveVertical = true;
					mFPSCamera.ScrollMouseToMoveHorizontal = true;
					mFPSCamera.VerticalMoveSpeed = new Vector2(1.5f, 1.5f);
					mFPSCamera.HorizontalMoveSpeed = new Vector2(0.01f, 0.01f);
					mFPSCamera.HorizontalMoveSpeedScale = new Vector2(5.0f, 5.0f);
					highlightCamera(mFPSCamera.Camera);
				}
				return mFPSCamera;
			}
		}

		public uapp.BirdCamera LimnCamera {
			get {
				if (mLimnCamera == null) {
					mLimnCamera = new uapp.BirdCamera("LimnCamera", null, 15);
					mLimnCamera.LeftMouseToMoveVertical = false;
					mLimnCamera.RightMouseToMoveVertical = true;
					mLimnCamera.ScrollMouseToMoveHorizontal = true;
					mLimnCamera.VerticalMoveSpeed = new Vector2(0.5f, 0.5f);
					mLimnCamera.HorizontalMoveSpeed = new Vector2(0.1f, 0.1f);
					mLimnCamera.Camera.orthographic = true;
				}
				return mLimnCamera;
			}
		}

		public void ChangeCamera(uapp.ICameraPrefab camera) {
			if (mCameraGroup == null) {
				mCameraGroup = new uapp.CameraGroup();
				mCameraGroup.AddCamera(BirdCamera.Camera);
				mCameraGroup.AddCamera(EditorCamera.Camera);
				mCameraGroup.AddCamera(FPSCamera.Camera);
				mCameraGroup.AddCamera(LimnCamera.Camera);
			}
			var cam = camera == null ? null : camera.Camera;
			mCameraGroup.ChangeCamera(cam);
            if (cam != null) { // NOTE VR camera的fovy继承到其他camera去了，这个得问问unity的工程师是不是脑袋进水了
                cam.fieldOfView = 60;
            }
		}

		public uapp.ICameraPrefab CurrentCamera {
			get {
				var currentCamera = mCameraGroup.CurrentCamera;
				if (currentCamera == null) {
					return null;
				}
				var b = currentCamera.GetComponent<uapp.CameraPrefabBehaviour>();
				return b.CameraPrefab;
			}
		}

		public void SetCurrentCameraEnabled(bool enabled) {
			var currentCamera = mCameraGroup.CurrentCamera;
			if (currentCamera == null) {
				return;
			}
            //uapp.ObjectUtils.SetBehavioursEnabled(currentCamera.gameObject, enabled);
            currentCamera.gameObject.SetActive (enabled);
		}

		public int CullingMask {
			set {
				BirdCamera.Camera.cullingMask = value;
				EditorCamera.Camera.cullingMask = value;
				FPSCamera.Camera.cullingMask = value;
				LimnCamera.Camera.cullingMask = value;
			}
		}
	}
}
