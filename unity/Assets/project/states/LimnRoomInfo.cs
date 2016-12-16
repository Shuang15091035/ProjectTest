using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class LimnRoomInfo : AppBase {

		private Image lo_edit;
		private Dropdown dd_area;
		private Button btn_delete;
		private Button btn_ok;

		private Room mRoom;
		private string mSelectedAreaName;

		public override bool OnPreCondition() {
			mRoom = SharedModel.Instance.CurrentRoom;
			return mRoom != null;
		}

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			lo_edit = findViewById<Image>("lo_edit");
			lo_edit.gameObject.SetActive(false);

			dd_area = findViewById<Dropdown>("dd_area");
			var areaNames = SharedModel.Instance.DefaultAreaNames;
			dd_area.ClearOptions();
			dd_area.AddOptions(areaNames);
			dd_area.onValueChanged.AddListener((int index) => {
				var areaName = areaNames[index];
				mSelectedAreaName = areaName;
			});

			btn_delete = findViewById<Button>("btn_delete");
			btn_delete.onClick.AddListener(() => {
				SharedModel.Instance.CurrentPlan.DestroyRoom(mRoom);
				mRoom = null;
				ParentMachine.RevertState();
			});

			btn_ok = findViewById<Button>("btn_ok");
			btn_ok.onClick.AddListener(() => {
				if (mSelectedAreaName != null) {
					mRoom.SetAreaName(mSelectedAreaName);
				}
				ParentMachine.RevertState();
			});

			return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			// 
			hideToolbar();
			hideStatusbar();

			var areaName = mRoom.AreaName;
			var areaNames = SharedModel.Instance.DefaultAreaNames;
			var areaIndex = areaNames.IndexOf(areaName);
			if (areaIndex < 0) {
				areaIndex = 0;
                mRoom.SetAreaName(areaNames[0]);
			}
			dd_area.value = areaIndex;
			mSelectedAreaName = null;

			updateEditPanel();
		}

		public override void OnStateOut() {

			if (mRoom != null) {
				mRoom.EditState = EditState.Normal;
			}
//			lo_edit.gameObject.SetActive(false);

			base.OnStateOut();
		}

		protected override void OnScreenMouseMove(uapp.MouseEvent e) {
			base.OnScreenMouseMove(e);
			updateEditPanel();
		}

		private void updateEditPanel() {
			if (mRoom == null) {
				return;
			}
			var p = SharedModel.Instance.Photographer.CurrentCamera.Camera.WorldToScreenPoint(mRoom.Center);
			lo_edit.gameObject.transform.position = p;
			lo_edit.gameObject.SetActive(true);
		}

//		protected override void OnKey(uapp.KeyEvent e) {
//			base.OnKey(e);
//
//			if (e.isDown(KeyCode.D)) { // TODO 之后改为使用ui删除
//				SharedModel.Instance.CurrentPlan.DestroyRoom(mRoom);
//				mRoom = null;
//				SharedModel.Instance.CurrentRoom = null;
//				ParentMachine.RevertState();
//			}
//		}
			
//		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
//			base.OnScreenMouseClick(e);
//
//			if (e.button == uapp.Buttons.Left) {
//				Vector3 point;
//				if (!Picker.RaycastGround(e.position, out point)) {
//					return;
//				}
//				if (!mRoom.IsPointIn(point)) {
//					ParentMachine.RevertState();
//				}
//			}
//		}

	}
}
