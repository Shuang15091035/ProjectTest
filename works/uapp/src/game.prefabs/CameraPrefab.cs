using UnityEngine;
using System.Collections;

namespace uapp {

	public delegate void OnCameraPrefabMoveVertical(float dx, float dy);
	public delegate void OnCameraPrefabMoveHorizontal(float dx, float dz);

	public interface ICameraPrefab {

		Camera Camera { get; }
		GameObject Root { get; }

		/**
		 *  屏幕平面移动
		 */
		void moveVertical(float dx, float dy);

		/**
		 *  屏幕深度移动
		 */
		void moveHorizontal(float dx, float dz);

		bool LeftMouseToMoveVertical { get; set; }
		bool RightMouseToMoveVertical { get; set; }
		bool ScrollMouseToMoveHorizontal { get; set; }
		bool KeyWASDToMoveHorizontal { get; set; }
		Vector2 VerticalMoveSpeed { get; set; }
		Vector2 HorizontalMoveSpeed { get; set; }
		Vector2 HorizontalMoveSpeedScale { get; set; }
		bool LockCursorOnMoveVertical { get; set; }

		OnCameraPrefabMoveVertical OnMoveVertical { get; set; }
		OnCameraPrefabMoveHorizontal OnMoveHorizontal { get; set; }
	}
	
	public abstract class CameraPrefab : ICameraPrefab {

		public static float ZNearDefault = 0.01f;
		public static float ZFarDefault = 100.0f;

		protected Camera mCamera;
		protected GameObject mRoot;
		private bool mLeftMouseToMoveVertical = true;
		private bool mRightMouseToMoveVertical = true;
		private bool mScrollMouseToMoveHorizontal = false;
		private bool mKeyWASDToMoveHorizontal = true;
		private Vector2 mVerticalMoveSpeed = new Vector2(10.0f, 10.0f); 
		private Vector2 mHorizontalMoveSpeed = new Vector2(10.0f, 10.0f);
		private Vector2 mHorizontalMoveSpeedScale = Vector2.one;
		private bool mLockCursorOnMoveVertical = false;

		OnCameraPrefabMoveVertical mOnMoveVertical;
		OnCameraPrefabMoveHorizontal mOnMoveHorizontal;

		protected CameraPrefab(string name, GameObject cameraObject = null) {
			if (cameraObject == null) {
				cameraObject = new GameObject();
				mCamera = cameraObject.AddComponent<Camera>();
			} else {
				mCamera = cameraObject.GetComponent<Camera>();
				if (mCamera == null) {
					throw new UnityException("[CameraPrefab] Cannot find a camera in cameraObject.");
				}
			}
			mCamera.gameObject.name = name;
			mCamera.nearClipPlane = ZNearDefault;
			mCamera.farClipPlane = ZFarDefault;
			mCamera.tag = "MainCamera";
			var cameraPrefabBehaviour = cameraObject.AddComponent<CameraPrefabBehaviour>();
			cameraPrefabBehaviour.CameraPrefab = this;
		}

		public Camera Camera {
			get {
				return mCamera;
			} set {
				mCamera = value;
			}
		}

		public GameObject Root {
			get {
				return mRoot;
			} set {
				mRoot = value;
			}
		}

		public abstract void moveVertical(float dx, float dy);
		public abstract void moveHorizontal(float dx, float dz);

		public bool LeftMouseToMoveVertical {
			get {
				return mLeftMouseToMoveVertical;
			} set {
				mLeftMouseToMoveVertical = value;
			}
		}

		public bool RightMouseToMoveVertical {
			get {
				return mRightMouseToMoveVertical;
			} set {
				mRightMouseToMoveVertical = value;
			}
		}

		public bool ScrollMouseToMoveHorizontal {
			get {
				return mScrollMouseToMoveHorizontal;
			} set {
				mScrollMouseToMoveHorizontal = value;
			}
		}

		public bool KeyWASDToMoveHorizontal {
			get {
				return mKeyWASDToMoveHorizontal;
			} set {
				mKeyWASDToMoveHorizontal = value;
			}
		}

		public Vector2 VerticalMoveSpeed {
			get {
				return mVerticalMoveSpeed;
			} set {
				mVerticalMoveSpeed = value;
			}
		}

		public Vector2 HorizontalMoveSpeed {
			get {
				return mHorizontalMoveSpeed;
			} set {
				mHorizontalMoveSpeed = value;
			}
		}

		public Vector2 HorizontalMoveSpeedScale {
			get {
				return mHorizontalMoveSpeedScale;
			} set {
				mHorizontalMoveSpeedScale = value;
			}
		}

		public bool LockCursorOnMoveVertical {
			get {
				return mLockCursorOnMoveVertical;
			} set {
				mLockCursorOnMoveVertical = value;
			}
		}

		public OnCameraPrefabMoveVertical OnMoveVertical { 
			get {
				return mOnMoveVertical;
			} set {
				mOnMoveVertical = value;
			}
		}

		public OnCameraPrefabMoveHorizontal OnMoveHorizontal { 
			get {
				return mOnMoveHorizontal;
			}
			set {
				mOnMoveHorizontal = value;
			}
		}

	}

}
