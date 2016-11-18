using UnityEngine;
using System.Collections;

namespace uapp {

	public class CameraPrefabBehaviour : BaseBehaviour {

		private ICameraPrefab mCameraPrefab;
		private CursorLockMode mCacheCursorLockState;

		public ICameraPrefab CameraPrefab {
			get {
				return mCameraPrefab;
			} set {
				mCameraPrefab = value;
			}
		}

		protected override void OnScreenMouseDown(MouseEvent e) {
			base.OnScreenMouseDown(e);
			bool needToMoveVertical = false;
			if (mCameraPrefab.LeftMouseToMoveVertical && e.button == Buttons.Left) {
				needToMoveVertical = true;
			}
			if (mCameraPrefab.RightMouseToMoveVertical && e.button == Buttons.Right) {
				needToMoveVertical = true;
			}
			if (needToMoveVertical) {
				mCacheCursorLockState = Cursor.lockState;
				if (mCameraPrefab.LockCursorOnMoveVertical) {
					Cursor.lockState = CursorLockMode.Locked;
				}
			}
		}

		protected override void OnScreenMouseMove(MouseEvent e) {
			base.OnScreenMouseMove(e);
			bool needToMoveVertical = false;
			if (mCameraPrefab.LeftMouseToMoveVertical && e.button == Buttons.Left) {
				needToMoveVertical = true;
			}
			if (mCameraPrefab.RightMouseToMoveVertical && e.button == Buttons.Right) {
				needToMoveVertical = true;
			}
			if (needToMoveVertical) {
				Vector3 deltaPosition = e.delta;
				var dx = deltaPosition.x * mCameraPrefab.VerticalMoveSpeed.x;
				var dy = deltaPosition.y * mCameraPrefab.VerticalMoveSpeed.y;
				mCameraPrefab.moveVertical(dx, dy);
				if (mCameraPrefab.OnMoveVertical != null) {
					mCameraPrefab.OnMoveVertical(dx, dy);
				}
			}
		}

		protected override void OnScreenMouseUp(MouseEvent e) {
			base.OnScreenMouseUp(e);
			Cursor.lockState = mCacheCursorLockState;
		}

		protected override void OnScreenMouseScroll (MouseEvent e) {
			base.OnScreenMouseScroll (e);

			if (mCameraPrefab.ScrollMouseToMoveHorizontal) {
				var dx = -Input.mouseScrollDelta.x * mCameraPrefab.HorizontalMoveSpeed.x;
				var dz = -Input.mouseScrollDelta.y * mCameraPrefab.HorizontalMoveSpeed.y;
				mCameraPrefab.moveHorizontal(dx, dz);
				if (mCameraPrefab.OnMoveHorizontal != null) {
					mCameraPrefab.OnMoveHorizontal(dx, dz);
				}
			}
		}

		protected override void OnKey (KeyEvent e) {
			base.OnKey (e);

			if (mCameraPrefab.KeyWASDToMoveHorizontal) {
				float dx = 0.0f;
				float dz = 0.0f;
				bool a = e.isHold(KeyCode.A) || e.isDown(KeyCode.A);
				bool d = e.isHold(KeyCode.D) || e.isDown(KeyCode.D);
				bool w = e.isHold(KeyCode.W) || e.isDown(KeyCode.W);
				bool s = e.isHold(KeyCode.S) || e.isDown(KeyCode.S);

				float speedScaleX = 1.0f;
				float speedScaleY = 1.0f;
				bool shift = e.isHold(KeyCode.LeftShift) || e.isDown(KeyCode.LeftShift);
				if (shift) {
					speedScaleX *= mCameraPrefab.HorizontalMoveSpeedScale.x;
					speedScaleY *= mCameraPrefab.HorizontalMoveSpeedScale.y;
				}
				if (a) {
					dx -= mCameraPrefab.HorizontalMoveSpeed.x * speedScaleX;
				} else if (d) {
					dx += mCameraPrefab.HorizontalMoveSpeed.x * speedScaleX;
				}
				if (w) {
					dz += mCameraPrefab.HorizontalMoveSpeed.y * speedScaleY;
				} else if (s) {
					dz -= mCameraPrefab.HorizontalMoveSpeed.y * speedScaleY;
				}
				if (!dx.Equals(0.0f) || !dz.Equals(0.0f)) {
					mCameraPrefab.moveHorizontal(dx, dz);
				}
			}
		}

	}
}
