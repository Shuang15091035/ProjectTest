using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class PlanEditBase : AppBase {

		protected Button btn_back;
        protected Button btn_order_list;
        protected Button btn_save;
		protected Button btn_share;
		protected ToggleGroup lo_camera;
        
        protected uapp.ToggleEx btn_add_item;

		private bool mGroundHit = false;
		private Vector3 mLastGroundPosition = Vector3.zero;

		protected bool getGroundOffset(Vector3 screenPosition, out Vector3 offset) {
			offset = Vector3.zero;
			if (!mGroundHit) {
				return false;
			}
			Vector3 point;
			if (!Picker.RaycastGround(screenPosition, out point)) {
				return false;
			}
			offset = point - mLastGroundPosition;
			mLastGroundPosition = point;
			return true;
		}

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_back = findViewById<Button>("btn_back", null, App.Canvas);
            btn_order_list = findViewById<Button> ("btn_order_list", null, lo_order_list_toolbar.gameObject);

            lo_camera = findViewById<ToggleGroup>("lo_camera", null, parentView);

			btn_save = findViewById<Button>("btn_save", null, lo_order_list_toolbar.gameObject);
			btn_share = findViewById<Button>("btn_share", null, lo_order_list_toolbar.gameObject);
			btn_add_item = findViewById<uapp.ToggleEx> ("btn_add_item", null, parentView);

			return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			btn_back.gameObject.SetActive(true);
			lo_order_list_toolbar.gameObject.SetActive(true);

            btn_order_list.onClick.RemoveAllListeners ();
            btn_order_list.onClick.AddListener (() => {
                ParentMachine.ChangeState (States.OrderList, false);
            });

            btn_save.onClick.RemoveAllListeners();
			btn_save.onClick.AddListener(() => {
				ParentMachine.ChangeState(States.SavePlan, false);
			});

			btn_share.onClick.RemoveAllListeners();
			btn_share.onClick.AddListener(() => {
				ParentMachine.ChangeState(States.Share, false);
			});

			btn_add_item.onValueChanged.RemoveAllListeners ();
            btn_add_item.onValueChanged.AddListener ((bool onOff) => {
                uapp.ToggleEx.SetToggleOnOff (btn_add_item, onOff);
                if (onOff) {
                    ParentMachine.ChangeState (States.ItemList);
                }
            });
			btn_add_item.gameObject.SetActive(true);

            uapp.UiUtils.ToggleGroupOnValueChanged (lo_camera, (Toggle toggle, int toggleIndex, bool onOff) => {
                uapp.ToggleEx.SetToggleOnOff (toggle, onOff);
                if (onOff) {
                    switch (toggleIndex) {
                        case 3:
                            SharedModel.Instance.CurrentPlan.LightsOnOff = false;
                            SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (false);
                            SharedModel.Instance.Photographer.ChangeCamera (SharedModel.Instance.Photographer.BirdCamera);
                            break;
                        case 2:
							changeToFPSCamera();
                            break;
                        case 1:
							changeToEditorCamera();
                            break;
                        case 0:
                            ParentMachine.ChangeState (States.VRCamera);
                            // TODO VR视角
                            break;
                        default:
                            break;
                    }
                }
            });
            lo_camera.gameObject.SetActive(true);

        }

		protected override void OnScreenMouseDown(uapp.MouseEvent e) {
			base.OnScreenMouseDown(e);

			if (e.button == uapp.Buttons.Left) {
				Vector3 point;
				if (Picker.RaycastGround(e.position, out point)) {
					mGroundHit = true;
					mLastGroundPosition = point;
					return;
				}
			}
			mGroundHit = false;
		}

		// NOTE 显示可选物件
//		protected override void OnScreenMouseMove(uapp.MouseEvent e) {
//			base.OnScreenMouseMove(e);
//
//			if (e.button == uapp.Buttons.None) {
//				var item = Picker.RaycastItem(e.position);
//				SharedModel.Instance.CurrentPlan.SetItemState(item, EditState.Highlighted);
//			}
//		}

		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
			base.OnScreenMouseClick(e);
			if (e.button == uapp.Buttons.Left) {
				var item = Picker.RaycastItem(e.position);
				onItemClick(item);
			}
		}

		protected virtual void onItemClick(Item item) {
			SharedModel.Instance.CurrentPlan.SetItemState(item, EditState.Selected);
		}

	}
}
