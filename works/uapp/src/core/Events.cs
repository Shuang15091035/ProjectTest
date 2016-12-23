/**
 * @file Events.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class Event {
		private bool mIsAccepted = false;

		public bool isAccepted {
			get {
				return mIsAccepted;
			} set {
				mIsAccepted = value;
			}
		}
	}

	public static class Buttons {
		public const int None = 0;
		public const int Left = 0x1 << 0;
		public const int Right = 0x1 << 1;
		public const int Middle = 0x1 << 2;
	}

	public sealed class MouseEvent : Event {
		private int mButton = Buttons.Left;
		private Vector3 mPosition = Vector3.zero;
		private Vector3 mDelta = Vector3.zero;

		public int button {
			get {
				return mButton;
			} set {
				mButton = value;
			}
		}

		public Vector3 position {
			get {
				return mPosition;
			} set {
				mPosition = value;
			}
		}

		public Vector3 delta {
			get {
				return mDelta;
			} set {
				mDelta = value;
			}
		}
	}

	public sealed class KeyEvent : Event {
		public bool isDown(KeyCode keyCode) {
			return Input.GetKeyDown(keyCode);
		}

		public bool isHold(KeyCode keyCode) {
			return Input.GetKey(keyCode);
		}

		public bool isUp(KeyCode keyCode) {
			return Input.GetKeyUp(keyCode);
		}
	}
}