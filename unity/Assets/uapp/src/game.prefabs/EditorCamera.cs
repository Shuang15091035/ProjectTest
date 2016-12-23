using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class EditorCamera : CameraPrefab {

		protected GameObject mCameraRotateXObject;
		protected bool mPitchConstraintEnabled;
		protected float mMinPitch;
		protected float mMaxPitch;

		public EditorCamera(string name, GameObject parent, float initPitch, float initYaw, float initZoom, GameObject cameraObject = null) : base(name, cameraObject) {
			mRoot = new GameObject(name + "@root");
			ObjectUtils.SetParent(mRoot, parent);
			mCameraRotateXObject = new GameObject(name + "@rotateX");
			ObjectUtils.SetParent(mCameraRotateXObject, mRoot);
			ObjectUtils.SetParent(mCamera.gameObject, mCameraRotateXObject);
			mRoot.transform.Rotate(0.0f, initYaw, 0.0f, Space.Self);
			mCameraRotateXObject.transform.Rotate(initPitch, 0.0f, 0.0f, Space.Self);
			mCamera.gameObject.transform.Translate (0.0f, 0.0f, initZoom);
			LockCursorOnMoveVertical = true;
		}

		public bool PitchConstraintEnabled {
			get {
				return mPitchConstraintEnabled;
			}
			set {
				mPitchConstraintEnabled = value;
			}
		}

		public float MinPitch {
			get {
				return mMinPitch;
			}
			set {
				mMinPitch = value;
			}
		}

		public float MaxPitch {
			get {
				return mMaxPitch;
			}
			set {
				mMaxPitch = value;
			}
		}

		public float Pitch {
			get {
				return mCameraRotateXObject.transform.localEulerAngles.x;
			}
			set {
				mCameraRotateXObject.transform.localEulerAngles = new Vector3(value, 0.0f, 0.0f);
			}
		}

		public float Yaw {
			get {
				return mRoot.transform.localEulerAngles.y;
			}
			set {
				mRoot.transform.localEulerAngles = new Vector3(0.0f, value, 0.0f);
			}
		}

		public float Zoom {
			get {
				return mCamera.transform.localPosition.z;
			}
			set {
				mCamera.transform.localPosition = new Vector3(0.0f, 0.0f, value);
			}
		}
			
		public override void moveVertical (float dx, float dy) {
			mRoot.transform.Rotate (0.0f, dx, 0.0f);

			if (!mPitchConstraintEnabled) {
				mCameraRotateXObject.transform.Rotate(-dy, 0.0f, 0.0f);
			} else {
				var pitch = mCameraRotateXObject.transform.localEulerAngles.x - dy;
				if (mMinPitch < pitch && pitch < mMaxPitch) {
					mCameraRotateXObject.transform.Rotate(-dy, 0.0f, 0.0f);
				}
			}
		}

		public override void moveHorizontal (float dx, float dz) {
			mCamera.gameObject.transform.Translate (0.0f, 0.0f, dz);
		}
	}
}
