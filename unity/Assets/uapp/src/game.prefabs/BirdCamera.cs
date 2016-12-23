using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class BirdCamera : CameraPrefab {

		protected float mMinHeight = 0.0f;
		private float mOriginOrthoSize = -1.0f;

		public BirdCamera(string name, GameObject parent, float initHeight, GameObject cameraObject = null) : base(name, cameraObject) {
			mRoot = new GameObject(name + "@root");
			ObjectUtils.SetParent(mRoot, parent);
			ObjectUtils.SetParent(mCamera.gameObject, mRoot);
			mCamera.gameObject.transform.Rotate(90.0f, 0.0f, 0.0f);
			mCamera.gameObject.transform.Translate (0.0f, 0.0f, -initHeight);
			LockCursorOnMoveVertical = true;
		}

		public Vector3 Position {
			get {
				return mRoot.transform.position;
			}
			set {
				mRoot.transform.position = value;
			}
		}

		public Vector2 PositionXZ {
			get {
				var pos = Vector2.zero;
				pos.x = mRoot.transform.position.x;
				pos.y = mRoot.transform.position.y;
				return pos;
			}
			set {
				var pos = mRoot.transform.position;
				pos.x = value.x;
				pos.z = value.y;
				mRoot.transform.position = pos;
			}
		}

		public float Height {
			get {
				return mCamera.gameObject.transform.localPosition.y;
			}
			set {
				var p = mCamera.gameObject.transform.localPosition;
				p.y = value;
				mCamera.gameObject.transform.localPosition = p;
			}
		}

		public override void moveVertical (float dx, float dy) {
			mRoot.transform.Translate(-dx, 0.0f, -dy);
		}

		public override void moveHorizontal (float dx, float dz) {
//			if (mOriginOrthoSize < 0.0f) {
//				mOriginOrthoSize = mCamera.orthographicSize;
//			}
			float t = mCamera.gameObject.transform.localPosition.y;
			float nt = t - dz;
			if (nt < -mMinHeight) {
//				if (mCamera.orthographic) {
//					mCamera.orthographicSize = mOriginOrthoSize;
//				}
//				mCamera.gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, -mMinHeight);
				return;
			}
			if (mCamera.orthographic) {
				float s = 1.0f;
				if (t != 0.0f) {
					s = nt / t;
				}
				mCamera.orthographicSize *= s;
			}
			mCamera.gameObject.transform.Translate(0.0f, 0.0f, dz);
		}

	}
}
