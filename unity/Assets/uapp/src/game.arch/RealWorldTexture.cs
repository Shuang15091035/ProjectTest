using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class RealWorldTexture : UObject {

		private Texture2D mTexture;
		private Vector2 mActualSize;

		public RealWorldTexture(Texture2D texture = null, float actualWidth = 1.0f, float actualHeight = 1.0f) {
			mTexture = texture;
			mActualSize.x = actualWidth;
			mActualSize.y = actualHeight;
		}

		public Texture2D Texture {
			get {
				return mTexture;
			}
			set {
				mTexture = value;
			}
		}

		public Vector2 ActualSize {
			get {
				return mActualSize;
			}
			set {
				mActualSize = value;
			}
		}

		public float ActualWidth {
			get {
				return mActualSize.x;
			}
			set {
				mActualSize.x = value;
			}
		}

		public float ActualHeight {
			get {
				return mActualSize.y;
			}
			set {
				mActualSize.y = value;
			}
		}

		public void Assign(RealWorldTexture other) {
			if (other == null) {
				return;
			}
			mTexture = other.mTexture;
			mActualSize = other.mActualSize;
		}

	}
}
