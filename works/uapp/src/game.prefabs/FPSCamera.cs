using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class FPSCamera : CameraPrefab {
	
		public FPSCamera(string name, GameObject parent, float initHeight, GameObject cameraObject = null) : base(name, cameraObject) {
			mRoot = new GameObject(name + "@root");
			ObjectUtils.SetParent(mRoot, parent);
			ObjectUtils.SetParent(mCamera.gameObject, mRoot);
			mCamera.gameObject.transform.Translate (0.0f, initHeight, 0.0f);
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
				pos.y = mRoot.transform.position.z;
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

		public Vector2 Angles {
			get {
				var angles = Vector2.zero;
				angles.x = mRoot.transform.localEulerAngles.y;
				angles.y = -mCamera.transform.localEulerAngles.x;
				return angles;
			}
			set {
				var angles = value;
				var a = mRoot.transform.localEulerAngles;
				a.y = angles.x;
				mRoot.transform.localEulerAngles = a;
				a = mCamera.transform.localEulerAngles;
				a.x = -angles.y;
				mCamera.transform.localEulerAngles = a;
			}
		}

		public override void moveVertical (float dx, float dy) {
			mRoot.transform.Rotate(0.0f, dx, 0.0f);
			mCamera.gameObject.transform.Rotate(-dy, 0.0f, 0.0f);
		}

		public override void moveHorizontal (float dx, float dz) {
			mRoot.transform.Translate(dx, 0.0f, dz);
		}

	}
}
