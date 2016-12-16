using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class LimnPlanInit : LimnPlanEdit {

		private Button btn_free;
		private Button btn_rect;
		private Button btn_delete_point;

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_free = findViewById<Button>("btn_free", null, lo_limn_plan_toolbar.gameObject);
			btn_free.onClick.AddListener(() => {
				SharedModel.Instance.CurrentRoomShape = RoomShape.Polygon;
				ParentMachine.ChangeState(States.LimnRoom);
			});

			btn_rect = findViewById<Button>("btn_rect", null, lo_limn_plan_toolbar.gameObject);
			btn_rect.onClick.AddListener(() => {
				SharedModel.Instance.CurrentRoomShape = RoomShape.Rectangle;
				ParentMachine.ChangeState(States.LimnRoom);
			});

			btn_delete_point = findViewById<Button>("btn_delete_point", null, lo_limn_plan_toolbar.gameObject);

			var btn_door = findViewById<Button>("btn_door", null, lo_limn_plan_toolbar.gameObject);
			btn_door.onClick.AddListener(() => {
				var door = new Door("door", 0, null);
				door.GameObject.SetActive(false);
				SharedModel.Instance.CurrentInWallItem = door;
				ParentMachine.ChangeState(States.LimnInWallItem);
			});

			var btn_window = findViewById<Button>("btn_window", null, lo_limn_plan_toolbar.gameObject);
			btn_window.onClick.AddListener(() => {
				var window = new Window("window", 0, null);
				window.GameObject.SetActive(false);
				SharedModel.Instance.CurrentInWallItem = window;
				ParentMachine.ChangeState(States.LimnInWallItem);
			});

			btn_done = findViewById<Button>("btn_done", null, App.Canvas);

			return view;
		}

//		protected override void onEditItemClick(EditItem editItem) {
//			base.onEditItemClick(editItem);
//			if (editItem is Room) {
//				SharedModel.Instance.CurrentRoom = editItem as Room;
//				ParentMachine.ChangeState(States.LimnRoomEdit);
//			} else if (editItem is Wall) {
//				SharedModel.Instance.CurrentWall = editItem as Wall;
//				ParentMachine.ChangeState(States.LimnScale);
//			}
//		}

		protected override void OnScreenMouseDoubleClick(uapp.MouseEvent e) {
			base.OnScreenMouseDoubleClick(e);

			Vector3 point;
			if (!Picker.RaycastGround(e.position, out point)) {
				return;
			}
			var editItem = mPlan.EditItemByPointXZ(point);
			onEditItemDoubleClick(editItem);
		}

		private void onEditItemDoubleClick(EditItem editItem) {
			if (editItem is Room) {
				SharedModel.Instance.CurrentRoom = editItem as Room;
				ParentMachine.ChangeState(States.LimnRoomInfo);
			} else if (editItem is Wall) {
				SharedModel.Instance.CurrentWall = editItem as Wall;
				ParentMachine.ChangeState(States.LimnScale);
			}
		}

		public override void OnStateIn() {
			base.OnStateIn();

			// 
			showLimnPlanToolbar();
			showStatusbar();
            btn_done.GetComponentInChildren<Text> ().text = "完成";

            // 
            btn_delete_point.onClick.RemoveAllListeners();
			btn_delete_point.onClick.AddListener(() => {
				deleteSelected();
			});

			// 
			btn_done.onClick.RemoveAllListeners();
			btn_done.onClick.AddListener (() => {
				ParentState.ParentMachine.ChangeState(States.PrototypeMatch, false);
			});
		}

        public override void OnStateOut () {
            base.OnStateOut ();
            btn_done.GetComponentInChildren<Text> ().text = "确定";
        }

		protected override void OnKey(uapp.KeyEvent e) {
			base.OnKey(e);

			if (e.isUp(KeyCode.Delete)) {
				deleteSelected();
			} else if (e.isUp(KeyCode.M)) {
                if (mSelectedRoom != null) {
                    if (mSelectedRoom.Floor.AutoAlignPoints(mSelectedRoom.Floor.SelectedPointIndex)) {
                        mSelectedRoom.Build();
                    }
                }
            }
		}

		private void deleteSelected() {
			if (mSelectedEditItem != null) {
				if (mSelectedEditItem is InWallItem) {
					var inWallItem = mSelectedEditItem as InWallItem;
					var wall = inWallItem.Wall;
					wall.DestroyInWallItem(inWallItem);
					wall.Build();
				} else if (mSelectedEditItem is Wall) {
					var wall = mSelectedEditItem as Wall;
					var room = wall.Room;
					room.DestroyWall(wall);
					if (room.IsEmpty) {
						SharedModel.Instance.CurrentPlan.DestroyRoom(room);
					} else {
						room.Build();
					}
				} else if (mSelectedEditItem is Room) {
					var room = mSelectedEditItem as Room;
					if (room.Floor.SelectedPointIndex < 0) {
						SharedModel.Instance.CurrentPlan.DestroyRoom(room);
					} else {
						room.RemovePoint(room.Floor.SelectedPointIndex);
						if (room.IsEmpty) {
							SharedModel.Instance.CurrentPlan.DestroyRoom(room);
						} else {
							room.Build();
						}
					}
				}
			} else if (mSelectedRoom != null) {
				if (mSelectedRoom.Floor.SelectedPointIndex < 0) {
					SharedModel.Instance.CurrentPlan.DestroyRoom(mSelectedRoom);
				} else {
					mSelectedRoom.RemovePoint(mSelectedRoom.Floor.SelectedPointIndex);
					if (mSelectedRoom.IsEmpty) {
						SharedModel.Instance.CurrentPlan.DestroyRoom(mSelectedRoom);
					} else {
						mSelectedRoom.Build();
					}
				}
			} 
		}

        //		protected override void OnKey(uapp.KeyEvent e) {
        //			base.OnKey(e);
        //
        //			if (e.isUp(KeyCode.P)) { // TODO 改成ui实现
        //				mPlan.DestroyRoom(mSelectedRoom);
        //				mSelectedRoom = null;
        //			}
        //		}

        //		protected override void OnScreenMouseMove(uapp.MouseEvent e) {
        //			base.OnScreenMouseMove(e);
        //
        //			var plan = SharedModel.Instance.CurrentPlan;
        //			if (plan == null) {
        //				return;
        //			}
        //			Vector3 point;
        //			if (!Picker.RaycastGround(e.position, out point)) {
        //				return;
        //			}
        //			plan.SetRoomStateByPoint(point, EditState.Highlight);
        //		}
        //
        //		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
        //			base.OnScreenMouseClick(e);
        //
        //			var plan = SharedModel.Instance.CurrentPlan;
        //			if (plan == null) {
        //				return;
        //			}
        //			Vector3 point;
        //			if (!Picker.RaycastGround(e.position, out point)) {
        //				return;
        //			}
        //			var room = plan.GetRoomByPoint(point);
        //			if (room != null) {
        //				SharedModel.Instance.CurrentRoom = room;
        //				ParentMachine.ChangeState(States.LimnRoomEdit);
        //			}
        //		}
    }
}
