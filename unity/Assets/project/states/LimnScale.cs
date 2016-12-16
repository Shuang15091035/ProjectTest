using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class LimnScale : AppBase {

		private Image lo_scale;
		private InputField et_scale;
		private Button btn_scale;

		private Plan mCurrentPlan;
		private Wall mWall;
	
		public override bool OnPreCondition() {
			mCurrentPlan = SharedModel.Instance.CurrentPlan;
			mWall = SharedModel.Instance.CurrentWall;
			return mWall != null && mCurrentPlan != null;
		}

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			lo_scale = findViewById<Image>("lo_scale");
			lo_scale.gameObject.SetActive(false);

			et_scale = findViewById("et_scale", et_scale);
			btn_scale = findViewById("btn_scale", btn_scale);
			btn_scale.onClick.AddListener(() => {
				if (uapp.StringUtils.IsNullOrBlank(et_scale.text)) {
					ParentMachine.RevertState();
					return;
				}
				float actualLength;
				if (!float.TryParse(et_scale.text, out actualLength)) {
					return;
				}
				mCurrentPlan.Scale = mWall.GetScale(actualLength);
				Debug.Log("Scale: " + mCurrentPlan.Scale);
				ParentMachine.RevertState();
			});

			return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			//
			hideToolbar();
			hideStatusbar();

			mCurrentPlan = SharedModel.Instance.CurrentPlan;

			et_scale.text = "";
			updateEditPanel();
		}

		public override void OnStateOut() {

			mWall.EditState = EditState.Normal;
			lo_scale.gameObject.SetActive(false);

			base.OnStateOut();
		}

		private void updateEditPanel() {
			var p = SharedModel.Instance.Photographer.CurrentCamera.Camera.WorldToScreenPoint(mWall.Center);
			lo_scale.gameObject.transform.position = p;
			lo_scale.gameObject.SetActive(true);
		}

//		protected override void OnScreenMouseMove(uapp.MouseEvent e) {
//			base.OnScreenMouseMove(e);
//
//			if (e.button == uapp.Buttons.None) {
//				Vector3 point;
//				if (!Picker.RaycastGround(SharedModel.Instance.Photographer.CurrentCamera.Camera, e.position, out point)) {
//					return;
//				}
//				var wall = mRoom.WallByPoint(point);
//				mRoom.SetWallState(wall, EditState.Highlight);
//			}
//		}
//
//		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
//			base.OnScreenMouseClick(e);
//
//			if (e.button == uapp.Buttons.Left) {
//				Vector3 point;
//				if (!Picker.RaycastGround(SharedModel.Instance.Photographer.CurrentCamera.Camera, e.position, out point)) {
//					return;
//				}
//				mSelectedWall = mRoom.WallByPoint(point);
//				mRoom.SetWallState(mSelectedWall, EditState.Selected);
//				if (mSelectedWall != null) {
//					var p = SharedModel.Instance.Photographer.CurrentCamera.Camera.WorldToScreenPoint(mSelectedWall.Center);
//					lo_scale.gameObject.transform.position = p;
//					lo_scale.gameObject.SetActive(true);
//				} else {
//					lo_scale.gameObject.SetActive(false);
//				}
//			}
//		}
	}
}
