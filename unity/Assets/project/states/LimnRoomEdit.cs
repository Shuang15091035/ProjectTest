using UnityEngine;
using System.Collections;

namespace project {

	// 房间编辑状态，包括使用鼠标编辑房间的所有操作，可以作为一个基类
	public class LimnRoomEdit : AppBase {

		protected Room mRoom;
		protected int mMovePointIndex = -1;
		protected EditItem mMoveEditItem = null;
		private Vector3 mLastPoint;

		public override void OnStateIn() {
			base.OnStateIn();
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
				mMovePointIndex = mRoom.PointByPoint(point);
				if (mMovePointIndex < 0) {
					mMoveEditItem = mRoom.EditItemByPointXZ(point);
					mRoom.SetEditItemState(mMoveEditItem, EditState.Highlighted);
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
				mMovePointIndex = mRoom.PointByPoint(point);
				if (mMovePointIndex < 0) {
					mMoveEditItem = mRoom.EditItemByPointXZ(point);
					mRoom.SetEditItemState(mMoveEditItem, EditState.Highlighted);
				} else {
					mRoom.SetEditItemState(null, EditState.Highlighted);
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
					mRoom.FloorPointMoveFunc = FloorPointMoveFunc.HorizontalVertical;
					if (mRoom.MovePoint(mMovePointIndex, point)) {
						mRoom.Build();
					}
				} else if (mMoveEditItem != null) {
					var ff = mRoom.FloorPointMoveFunc;
					if (mMoveEditItem is Wall) {
						var wall = mMoveEditItem as Wall;
						mRoom.SetEditItemState(wall, EditState.Highlighted);
						mRoom.FloorPointMoveFunc = FloorPointMoveFunc.Free;
						if (mRoom.MoveWall(wall, point)) {
							mRoom.Build();
						}
					} else if (mMoveEditItem is Floor) {
						var floor = mMoveEditItem as Floor;
						mRoom.SetEditItemState(floor, EditState.Highlighted);
						if (mRoom.Offset(offset)) {
							mRoom.Build();
						}
					}
					mRoom.FloorPointMoveFunc = ff;
				}
			}
		}

		protected override void OnScreenMouseUp(uapp.MouseEvent e) {
			base.OnScreenMouseUp(e);

			mMovePointIndex = -1;
			if (mMoveEditItem != null) {
				mRoom.SetEditItemState(null, EditState.Highlighted);
			}
			mMoveEditItem = null;
		}

		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
			base.OnScreenMouseClick(e);

			if (e.button == uapp.Buttons.Left) {
				Vector3 point;
				if (!Picker.RaycastGround(e.position, out point)) {
					return;
				}
				mRoom.Floor.SelectedPointIndex = mRoom.PointByPoint(point);
			}
		}

	}

}
