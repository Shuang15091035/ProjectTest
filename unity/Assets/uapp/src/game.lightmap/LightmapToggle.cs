using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class LightmapToggle {

		private bool mOnOff = true;
		private LightmapData[] mSaveLightmaps;
		private LightProbes mSaveProbes;

		public bool OnOff {
			get {
				return mOnOff;
			}
			set {
				if (!value) {
					mSaveLightmaps = LightmapSettings.lightmaps;
					mSaveProbes = LightmapSettings.lightProbes;
					LightmapSettings.lightmaps = null;
					LightmapSettings.lightProbes = null;
				} else {
					if (mSaveLightmaps != null) {
						LightmapSettings.lightmaps = mSaveLightmaps;
					}
					if (mSaveProbes != null) {
						LightmapSettings.lightProbes = mSaveProbes;
					}
				}
				mOnOff = value;
			}
		}

		public void Clear() {
			mSaveLightmaps = null;
			mSaveProbes = null;
		}
	}
}
