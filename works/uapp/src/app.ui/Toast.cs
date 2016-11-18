using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uapp {
	
	public class Toast : MonoBehaviour {

		public static float Short = 1.5f;
		public static float Long = 3.0f;

		private Text mText;
		private CanvasRenderer[] mCanvasRenderers;
		private float mStartTime;
		private float mShowTime;
		private float mHoldTime;
		private float mHideTime;

		public delegate void OnDisappear();
		private OnDisappear mOnDisappear;

		public void Show(string text, float duration, OnDisappear onDisappear = null) {
			mText = gameObject.GetComponentInChildren<Text>();
			mText.text = text;
			mCanvasRenderers = gameObject.GetComponentsInChildren<CanvasRenderer>();
			foreach (var canvasRenderer in mCanvasRenderers) {
				canvasRenderer.SetAlpha(0.0f);
			}
			mStartTime = Time.time;
			mShowTime = duration * 0.4f;
			mHoldTime = duration * 0.6f;
			mHideTime = duration;
			gameObject.SetActive(true);

			mOnDisappear = onDisappear;
		}

		void FixedUpdate() {
			var dt = Time.fixedTime - mStartTime;
			if (dt < 0.0f) {
				return;
			}
			foreach (var canvasRenderer in mCanvasRenderers) {
				if (dt < mShowTime) {
					var a = dt / mShowTime;
					canvasRenderer.SetAlpha(a);
				} else if (dt < mHoldTime) {
					canvasRenderer.SetAlpha(1.0f);
				} else if (dt < mHideTime) {
					var a = 1.0f - ((dt - mHoldTime) / (mHideTime - mHoldTime));
					canvasRenderer.SetAlpha(a);
				} else {
					gameObject.SetActive(false);
					if (mOnDisappear != null) {
						mOnDisappear();
						mOnDisappear = null;
					}
				}
			}
		}
	}

}
