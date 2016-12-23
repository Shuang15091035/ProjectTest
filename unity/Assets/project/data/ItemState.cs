using UnityEngine;
using System.Collections;
using HighlightingSystem;

namespace project {
	
	public class ItemState : MonoBehaviour {

		public static Color HighlightColor = Color.red;
		public static Color SelectedColor = uapp.ColorUtils.FromRbga(0xd8ff00ff);

		private EditState mState = EditState.Normal;
		private Highlighter mHighlighter;

		public EditState State {
			get {
				return mState;
			}
			set {
				if (mState == value) {
					return;
				}
				if (value != EditState.Normal) {
					if (mHighlighter == null) {
						mHighlighter = uapp.ObjectUtils.AddComponentIfNotExists<Highlighter>(gameObject);
					}
					if (value == EditState.Highlighted) {
						mHighlighter.ConstantOn(HighlightColor);
					} else if (value == EditState.Selected) {
						mHighlighter.ConstantOn(SelectedColor);
					}
				} else {
					if (mHighlighter != null) {
						mHighlighter.ConstantOff();
					}
				}
				mState = value;
			}
		}

	}
}
