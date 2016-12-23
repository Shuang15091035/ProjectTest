using UnityEngine;
using System.Collections;

namespace project {

	// 户型编辑状态，包括使用鼠标编辑户型中所有房间的所有操作，可以作为一个基类
	public class LimnPlanEdit : AppBase {

		protected Plan mPlan;
		protected Room mMoveRoom = null;
		protected int mMovePointIndex = -1;
		protected EditItem mMoveEditItem = null;
		protected Room mSelectedRoom = null;
		protected EditItem mSelectedEditItem = null;
		private Vector3 mLastPoint;

		public override bool OnPreCondition() {
			mPlan = SharedModel.Instance.CurrentPlan;
			return mPlan != null;
		}

		public override void OnStateIn() {
			base.OnStateIn();
			mMoveRoom = null;
			mMovePointIndex = -1;
			mMoveEditItem = null;
		}

		protected override void OnScreenMouseDown(uapp.MouseEvent e) {
			base.OnScreenMouseDown(e);

			if (e.button == uapp.Buttons.Left) {
				Vector3 point;
				if (!Picker.RaycastGround(e.position, out point)) {
					return;
				}
				mLastPoint = point;
				if (!mPlan.PointByPoint(point, out mMoveRoom, out mMovePointIndex)) {
					mMoveEditItem = mPlan.EditItemByPointXZ(point);
					mPlan.SetEditItemState(mMoveEditItem, EditState.Highlighted);
				}
			}
		}

		protected override void OnScreenMouseMove(uapp.MouseEvent e) {
			base.OnScreenMouseMove(e);

			if (e.button == uapp.Buttons.None) {
				Vector3 point;
				if (!Picker.RaycastGround(e.position, out point)) {
					return;
				}
				if (!mPlan.PointByPoint(point, out mMoveRoom, out mMovePointIndex)) {
					mMoveEditItem = mPlan.EditItemByPointXZ(point);
					mPlan.SetEditItemState(mMoveEditItem, EditState.Highlighted);
				} else {
					mPlan.SetRoomSelectedPointIndex(mMoveRoom, mMovePointIndex);
					mPlan.SetEditItemState(null, EditState.Highlighted);
				}
			} else if (e.button == uapp.Buttons.Left) {
				if (mMovePointIndex < 0 && mMoveEditItem == null) {
					return;
				}
				Vector3 point;
				if (!Picker.RaycastGround(e.position, out point)) {
					return;
				}
				var offset = point - mLastPoint;
				mLastPoint = point;
				if (mMovePointIndex >= 0) {
					Room touchRoom;
					int touchRoomPointIndex; 
					mPlan.PointByPoint(point, out touchRoom, out touchRoomPointIndex);
					mPlan.SetRoomSelectedPointIndex(mMoveRoom, mMovePointIndex);
					mPlan.SetRoomSelectedPointIndex(touchRoom, touchRoomPointIndex);

					if (touchRoomPointIndex >= 0 && touchRoom != mMoveRoom && touchRoomPointIndex != mMovePointIndex) { // 点吸附
						var p = touchRoom.Floor.Data.PointAt(touchRoomPointIndex);
						if (mMoveRoom.MovePoint(mMovePointIndex, p)) {
							mMoveRoom.Build();
						}
					} else {
						var w = mPlan.WallByPointXZ(point);
						if (w != null && !mMoveRoom.ContainsWall(w)) { // 墙吸附
							var p = w.PointByPointXZ(point);
							if (mMoveRoom.MovePoint(mMovePointIndex, p)) {
								mMoveRoom.Build();
							}
						} else {
							mMoveRoom.FloorPointMoveFunc = FloorPointMoveFunc.HorizontalVertical; // 水平垂直对齐
							if (mMoveRoom.MovePoint(mMovePointIndex, point)) {
								mMoveRoom.Build();
							}
						}
					}
				} else if (mMoveEditItem != null) {
					if (mMoveEditItem is Wall) {
						var wall = mMoveEditItem as Wall;
						var room = wall.Room;
						room.SetEditItemState(wall, EditState.Highlighted);
						var ff = room.FloorPointMoveFunc;
						room.FloorPointMoveFunc = FloorPointMoveFunc.Free;
						if (room.MoveWall(wall, point)) {
							room.Build();
						}
						room.FloorPointMoveFunc = ff;
					} else if (mMoveEditItem is Room) {
						var room = mMoveEditItem as Room;
						room.SetEditItemState(room, EditState.Highlighted);
						//var ff = room.FloorPointMoveFunc;
						if (room.Offset(offset)) {
							room.Build();
						}

                        // 点吸附
                        //var n = room.Floor.Data.NumPoints;
                        //bool needToBuild = false;
                        //for (var i = 0; i < n; i++) {
                        //    Room touchRoom;
                        //    int touchRoomPointIndex;
                        //    var p = room.Floor.Data.PointAt(i);
                        //    mPlan.PointByPointExcept (p, room, out touchRoom, out touchRoomPointIndex);

                        //    if (touchRoomPointIndex >= 0) { // 点吸附
                        //        var tp = touchRoom.Floor.Data.PointAt (touchRoomPointIndex);
                        //        if (room.MovePoint (i, tp)) {
                        //            needToBuild = true;
                        //        }
                        //    }
                        //}
                        //if (needToBuild) {
                        //    room.Build();
                        //}

						//room.FloorPointMoveFunc = ff;
					} else if (mMoveEditItem is InWallItem) {
						var inWallItem = mMoveEditItem as InWallItem;
						var wall = inWallItem.Wall;
						if (wall.MoveInWallItemXZ(inWallItem, point)) {
							wall.Build();
						}
					}
				}
			}
		}

		protected override void OnScreenMouseUp(uapp.MouseEvent e) {
			base.OnScreenMouseUp(e);

			mMovePointIndex = -1;
			if (mMoveEditItem != null) {
				mPlan.SetEditItemState(null, EditState.Highlighted);
			}
			mMoveRoom = null;
			mMoveEditItem = null;
		}

		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
			base.OnScreenMouseClick(e);

			if (e.button == uapp.Buttons.Left) {
				Vector3 point;
				if (!Picker.RaycastGround(e.position, out point)) {
					return;
				}

				mSelectedEditItem = null;
				mSelectedRoom = null;
				mPlan.ClearEditItemState(true);

				var selectedPointIndex = -1;
				mPlan.PointByPoint(point, out mSelectedRoom, out selectedPointIndex);
				mPlan.SetRoomSelectedPointIndex(mSelectedRoom, selectedPointIndex);
				if (selectedPointIndex < 0) {
					mSelectedEditItem = mPlan.EditItemByPointXZ(point);
					mPlan.SetEditItemState(mSelectedEditItem, EditState.Selected);
//					if (mSelectedEditItem != null) {
//						mSelectedRoom = mPlan.RoomByPointXZ(point);
//						mPlan.SetEditItemState(mSelectedRoom, EditState.Selected);
//					}
				}
			}
		}

	}
}
