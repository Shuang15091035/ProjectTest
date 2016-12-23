using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uapp {
	
	public class FixKeyLicenseView : MonoBehaviour {

		private FixKeyLicense mLicense;
		private string mKey = "";
		private bool mVisible = false;

		void Start() {
//			gameObject.SetActive(false);
			mLicense = GetComponent<FixKeyLicense>();
			if (mLicense == null) {
				Debug.LogError("[FixKeyLicense] FixKeyLicense cannot be found.");
				return;
			}
			mLicense.OnExpire = () => {
//				gameObject.SetActive(true);
				mVisible = true;
			};
		}

		void OnGUI() {
			if (!mVisible) {
				return;
			}
			GUILayout.BeginVertical();
			GUILayout.Label("验证码过期");
			mKey = GUILayout.TextField(mKey);
			if (GUILayout.Button("确认")) {
				if (!StringUtils.IsNullOrBlank(mKey)) {
					mVisible = !mLicense.CheckExpire(mKey);
				}
			}
			GUILayout.EndVertical();
		}
	}
}
