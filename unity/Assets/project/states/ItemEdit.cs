using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {

	public class ItemEdit : PlanEditBase {
		
		private LayoutGroup lo_edit; 
		private Vector3 lo_edit_offset = new Vector3 (0, 1.2f, 0);
		private Button btn_rotate;
		private Button btn_replace;
		private Button btn_delete;
		private Button btn_palette;
        private Image lo_change_color;

		private Item mItem;

        public override bool OnPreCondition () {
            mItem = SharedModel.Instance.CurrentItem;
            return mItem != null;
        }

        public override GameObject OnCreateView (GameObject parentView) {
            var view = base.OnCreateView (parentView);

			lo_edit = findViewById<LayoutGroup>("lo_edit");
            lo_change_color = findViewById<Image> ("lo_change_color");
            lo_change_color.gameObject.SetActive (false);

            // 旋转
            btn_rotate = findViewById<Button>("btn_rotate");
			btn_rotate.onClick.AddListener(() => {
				mItem.gameObject.transform.Rotate (0, -45.0f, 0);
			});

			// 替换物件
			btn_replace = findViewById<Button>("btn_replace");
			btn_replace.onClick.AddListener(() => {
				ParentMachine.ChangeState(States.ItemReplace);
			});

			// 删除物件
			btn_delete = findViewById<Button>("btn_delete");
			btn_delete.onClick.AddListener(() => {
				SharedModel.Instance.CurrentPlan.DestroyItem (mItem);
				mItem = null;
				ParentMachine.RevertState();
			});

			// 调色板
			btn_palette = findViewById<Button>("btn_palette");
			btn_palette.onClick.AddListener(() => {
                // TODO
                lo_change_color.gameObject.SetActive (!lo_change_color.gameObject.activeInHierarchy);
				var btn_change_color1 = findViewById<Button>("btn_change_color1", null, lo_change_color.gameObject);
				btn_change_color1.onClick.RemoveAllListeners();
				btn_change_color1.onClick.AddListener(() => {
					changeColor(mItem, uapp.ColorUtils.FromRbga(0x7b7b76ff));
				});
				var btn_change_color2 = findViewById<Button>("btn_change_color2", null, lo_change_color.gameObject);
				btn_change_color2.onClick.RemoveAllListeners();
				btn_change_color2.onClick.AddListener(() => {
					changeColor(mItem, uapp.ColorUtils.FromRbga(0xdfb5b4ff));
				});
				var btn_change_color3 = findViewById<Button>("btn_change_color3", null, lo_change_color.gameObject);
				btn_change_color3.onClick.RemoveAllListeners();
				btn_change_color3.onClick.AddListener(() => {
					changeColor(mItem, uapp.ColorUtils.FromRbga(0xf1eedeff));
				});
				var btn_change_color4 = findViewById<Button>("btn_change_color4", null, lo_change_color.gameObject);
				btn_change_color4.onClick.RemoveAllListeners();
				btn_change_color4.onClick.AddListener(() => {
					changeColor(mItem, uapp.ColorUtils.FromRbga(0x4a2d0aff));
				});
				var btn_change_color5 = findViewById<Button>("btn_change_color5", null, lo_change_color.gameObject);
				btn_change_color5.onClick.RemoveAllListeners();
				btn_change_color5.onClick.AddListener(() => {
					changeColor(mItem, uapp.ColorUtils.FromRbga(0x834e29ff));
				});
				var btn_change_color6 = findViewById<Button>("btn_change_color6", null, lo_change_color.gameObject);
				btn_change_color6.onClick.RemoveAllListeners();
				btn_change_color6.onClick.AddListener(() => {
					changeColor(mItem, uapp.ColorUtils.FromRbga(0xbe5d25ff));
				});
            });

			return view;
        }

        public override void OnStateIn() {
            base.OnStateIn();

			updateEditMenu();

            //
            hideStatusbar();
        }

		public override void OnStateOut() {
			base.OnStateOut();

			SharedModel.Instance.CurrentPlan.ClearItemState(true);
		}

        protected override void OnScreenMouseMove (uapp.MouseEvent e) {
            base.OnScreenMouseMove (e);

			if (mItem != null && !mItem.IsMaterialItem) {
	            if (uapp.Flags.Test (e.button, uapp.Buttons.Left)) {
	                if (uapp.Flags.Test (e.button, uapp.Buttons.Right)) {
	                    mItem.transform.Rotate (new Vector3 (0.0f, e.delta.x, 0.0f));
	                } else {
//	                    Vector3 point;
//	                    if (Picker.RaycastGround (e.position, out point)) {
//	                        var bounds = uapp.ObjectUtils.GetScaleBounds (mItem.gameObject);
//	                        var offset = uapp.BoundsUtils.BottomCenter (bounds);
//	                        offset = -offset;
//	                        offset.y = 0.0f;
//                            var oldPosition = mItem.transform.position;
//	                        mItem.transform.position = point + offset;
//
//                            updateEditMenu ();
//                        }
						Vector3 offset;
						if (getGroundOffset(e.position, out offset)) {
							mItem.transform.position += offset;
						}
	                }
	            }
			}

//            if (e.button == uapp.Buttons.Right) {
//                updateEditMenu ();
//            }
        }
			
//        protected override void OnScreenMouseClick (uapp.MouseEvent e) {
//            base.OnScreenMouseClick (e);
//            Vector3 point;
//            Picker.RaycastGround (e.position, out point);
//            if (e.button == uapp.Buttons.Left) {
//                mItem = Picker.RaycastItem (e.position);
//                if (mItem == null) {
//                    ParentMachine.RevertState ();
//                } else {
//                    SharedModel.Instance.CurrentItem = mItem;
//                    updateEditMenu ();
//                }
//            }
//        }

		protected override void onItemClick(Item item) {
			base.onItemClick(item);
			mItem = item;
			if (item == null) {
				ParentMachine.RevertState();
			} else {
				SharedModel.Instance.CurrentItem = item;
				updateEditMenu();
			}
		}

//		protected override void OnKey(uapp.KeyEvent e) {
//			base.OnKey (e);
//
//			if (e.isUp(KeyCode.Alpha1)) {
//				changeColor(mItem, new Color(0.1f, 0.1f, 0.1f));
//			} else if (e.isUp(KeyCode.Alpha2)) {
//				changeColor(mItem, new Color(0.5f, 0.5f, 0.5f));
//			} else if (e.isUp(KeyCode.Alpha3)) {
//				changeColor(mItem, new Color(0.9f, 0.9f, 0.9f));
//			} else if (e.isUp(KeyCode.Alpha4)) {
//				changeColor(mItem, Color.red);
//			} else if (e.isUp(KeyCode.Alpha5)) {
//				changeColor(mItem, Color.yellow);
//			} else if (e.isUp(KeyCode.Alpha6)) {
//				changeColor(mItem, Color.blue);
//			} else if (e.isUp(KeyCode.Alpha7)) {
//				changeColor(mItem, Color.green);
//			}
//		}

        private void updateEditMenu() {
			if (mItem == null) {
				return;
			}

			if (mItem.IsModelItem) {
				btn_rotate.gameObject.SetActive(true);
				btn_palette.gameObject.SetActive(false);
				lo_change_color.gameObject.SetActive(false);
			} else if (mItem.IsMaterialItem) {
				btn_rotate.gameObject.SetActive(false);
				if (mItem.IsColorItem) {
					btn_palette.gameObject.SetActive(true);
				} else {
					btn_palette.gameObject.SetActive(false);
					lo_change_color.gameObject.SetActive(false);
				}
			}

            //lo_edit.transform.position = SharedModel.Instance.Photographer.CurrentCamera.Camera.WorldToScreenPoint (mItem.gameObject.transform.position + lo_edit_offset);
            //lo_edit.transform.position = SharedModel.Instance.Photographer.CurrentCamera.Camera.WorldToScreenPoint (point + lo_edit_offset);
            //var center = uapp.ObjectUtils.GetBounds (mItem.gameObject).center;
            //center = mItem.gameObject.transform.localToWorldMatrix.MultiplyPoint (center);
            //lo_edit.transform.position = SharedModel.Instance.Photographer.CurrentCamera.Camera.WorldToScreenPoint (center);
        }

		private void changeColor(Item item, Color color) {
			uapp.ObjectUtils.SetColor(item.gameObject, color, true);
		}
    }
}
