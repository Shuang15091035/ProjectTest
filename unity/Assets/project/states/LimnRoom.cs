using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class LimnRoom : LimnRoomEdit {

		private Button btn_delete_point;

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_delete_point = findViewById<Button>("btn_delete_point", null, lo_limn_room_toolbar.gameObject);

			btn_done = findViewById<Button>("btn_done", null, App.Canvas);

			return view;
		}

		public override bool OnPreCondition() {
			var result = SharedModel.Instance.CurrentPlan.CreateRoom(null, out mRoom);
			return result == PlanEditResult.Ok;
		}

		public override void OnStateIn() {
			base.OnStateIn();

//			SharedModel.Instance.Photographer.SetCurrentCameraEnabled(false);

			// 
			showLimnRoomToolbar();
			showStatusbar();

			btn_delete_point.onClick.RemoveAllListeners();
			btn_delete_point.onClick.AddListener(() => {
				mRoom.RemovePoint(mRoom.Floor.SelectedPointIndex);
				if (mRoom.IsEmpty) {
					SharedModel.Instance.CurrentPlan.DestroyRoom(mRoom);
					mRoom = null;
					ParentMachine.RevertState();
				}
				mRoom.Build();
			});

			btn_done.onClick.RemoveAllListeners();
			btn_done.onClick.AddListener(() => {
				if (mRoom.Floor.Data.NumPoints < 3) {
					SharedModel.Instance.CurrentPlan.DestroyRoom(mRoom);
					mRoom = null;
					Debug.LogWarning("户型没有画完");
					ParentMachine.RevertState();
					return;
				}
				// TODO 第一次画完户型，提示输出墙体实际长度
				//				ParentMachine.ChangeState(States.LimnScale, false);
				ParentMachine.RevertState();
			});
				
			// Design阶段
			mRoom.EditPhase = EditPhase.Design;
			mRoom.Floor.EditState = EditState.Highlighted;
			SharedModel.Instance.CurrentRoom = mRoom;
			if (SharedModel.Instance.CurrentRoomShape == RoomShape.Rectangle) {
				var size = 2.0f;
				var halfSize = size * 0.5f;
				mRoom.PushPoint(new Vector3(-halfSize, 0.0f, halfSize));
				mRoom.PushPoint(new Vector3(halfSize, 0.0f, halfSize));
				mRoom.PushPoint(new Vector3(halfSize, 0.0f, -halfSize));
				mRoom.PushPoint(new Vector3(-halfSize, 0.0f, -halfSize));
				mRoom.Build();
			}
		}

		public override void OnStateOut() {
			base.OnStateOut();

			if (mRoom != null) {
				mRoom.Floor.EditState = EditState.Normal;
				mRoom.Floor.SelectedPointIndex = -1;
			}

            if (SharedModel.Instance.CurrentRoomShape == RoomShape.Polygon) {
                if (!mRoom.IsEmpty) {
                    if (mRoom.Floor.AutoAlignPoints(0)) {
                        mRoom.Build();
                    }
                }
            }
		}

		protected override void OnScreenMouseDoubleClick(uapp.MouseEvent e) {
			base.OnScreenMouseDoubleClick(e);
//		}
//		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
//			base.OnScreenMouseClick(e);

			Vector3 point;
			if (!Picker.RaycastGround(e.position, out point)) {
				return;
			}
			mRoom.PushPoint(point);
			mRoom.Build();
		}

//		protected override void OnKey(uapp.KeyEvent e) {
//			base.OnKey(e);
//			if (e.isDown(KeyCode.D)) { // TODO 之后改为使用ui删除
//				mRoom.RemovePoint(mRoom.Floor.SelectedPointIndex);
//				if (mRoom.IsEmpty) {
//					SharedModel.Instance.CurrentPlan.DestroyRoom(mRoom);
//					ParentMachine.RevertState();
//				}
//				mRoom.Build();
//			}
//		}
	}
}
