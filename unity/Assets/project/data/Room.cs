using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace project {

	public enum RoomShape {
		Polygon,
		Rectangle,
	}
	
	public class Room : EditItem {

		private Plan mPlan;
		private Area mArea;
		private Floor mFloor;
		private IList<Wall> mWalls;
		private SKU mWallSKU;

		private RoomOutlineRenderer mRoomOutlineRenderer;

		public Room(Plan plan, Area area, SKU floorSKU, SKU wallSKU) : base("room", 0, null, plan.Root) {
			mPlan = plan;
			mArea = area;
			mFloor = new Floor(this, mPlan.GetFloorId(), floorSKU, mGameObject);
			mPlan.AddArchItem(mFloor.Item);
			mWallSKU = wallSKU;

			// 只有该视角能编辑户型，所以这样创建
			mRoomOutlineRenderer = SharedModel.Instance.Photographer.LimnCamera.Camera.gameObject.AddComponent<RoomOutlineRenderer>();
			mRoomOutlineRenderer.Room = this;
		}

		public override void OnDelete() {
			mRoomOutlineRenderer.Clear();
			GameObject.Destroy(mRoomOutlineRenderer);
			mRoomOutlineRenderer = null;
			mPlan.RemoveArchItem(mFloor.Item);
			uapp.CoreUtils.Delete(mFloor);
			if (mWalls != null) {
				foreach (var wall in mWalls) {
					mPlan.RemoveArchItem(wall.Item);
					uapp.CoreUtils.Delete(wall);
				}
				mWalls.Clear();
				mWalls = null;
			}
			mArea = null;
			mPlan = null;
			base.OnDelete();
		}

		public Plan Plan {
			get {
				return mPlan;
			}
		}

//		private GameObject mGameObject;
//
//		public Room(Plan plan, Area area, SKU floorSKU, SKU wallSKU) {
//			mGameObject = new GameObject("room");
//			uapp.ObjectUtils.SetParent(mGameObject, plan.Root);
//			mPlan = plan;
//			mArea = area;
//			mFloor = new Floor(mPlan.GetFloorId(), area, floorSKU, mGameObject);
//			mPlan.AddArchItem(mFloor.Item);
//			mWallSKU = wallSKU;
//		}

		public override EditPhase EditPhase {
			get {
				return mFloor.EditPhase;
			}
			set {
				mFloor.EditPhase = value;
				if (mWalls != null) {
					foreach (var wall in mWalls) {
						wall.EditPhase = value;
					}
				}
				switch (value) {
					case EditPhase.Init: {
							mRoomOutlineRenderer.Clear();
							mRoomOutlineRenderer.enabled = false;
							break;
						}
					case EditPhase.Design:
						{
							mRoomOutlineRenderer.enabled = true;
							break;
						}
					case EditPhase.Edit:
						{
							mRoomOutlineRenderer.Clear();
							mRoomOutlineRenderer.enabled = false;
							break;
						}
				}
			}
		}

		public override Area Area {
			get {
				return mArea;
			}
			set {
				mArea = value;
			}
		}

		public string AreaName {
			get {
				if (mArea == null) {
					return null;
				}
				return mArea.Name;
			}
		}

		public PlanEditResult SetAreaName(string areaName) {
			if (mArea != null && uapp.StringUtils.Equals(mArea.Name, areaName)) {
				return PlanEditResult.Ok;
			}
			Area area;
			var result = mPlan.AddArea(areaName, out area);
//			if (result != PlanEditResult.Ok) {
//				return result;
//			}
			mArea = area;
			mFloor.Area = area;
			if (mWalls != null) {
				foreach (var wall in mWalls) {
					wall.Area = area;
				}
			}
			mGameObject.name = "room@" + mArea.Name;
			return PlanEditResult.Ok;
		}

		public Floor Floor {
			get {
				return mFloor;
			}
		}

		public IList<Wall> Walls {
			get {
				return mWalls;
			}
		}

		public int NumWalls {
			get {
				if (mWalls == null) {
					return 0;
				}
				return mWalls.Count;
			}
		}

		public Vector3 Center {
			get {
				return mFloor.Center;
			}
		}

		public int PushPoint(Vector3 point) {
			var pointIndex = mFloor.PushPoint(point);
			if (pointIndex < 0) {
				return -1;
			}
//			if (mFloor.Data.NumPoints > 1) {
			if (mWalls == null) {
				mWalls = new List<Wall>();
			}
			var wall = new Wall(this, mPlan.GetWallId(), mWallSKU, mGameObject);
			wall.EditPhase = EditPhase;
			mWalls.Add(wall);
			mPlan.AddArchItem(wall.Item);
//			}
//			if (mWalls != null) {
//				for (var i = 0; i < mWalls.Count; i++) {
//					var wall = mWalls[i];
//					var start = mFloor.Data.PointAt(i);
//					var end = mFloor.Data.PointAt(i + 1);
//					wall.Start = start;
//					wall.End = end;
//					wall.Build();
//				}
//			}
			return pointIndex;
		}

		public bool RemovePoint(int index) {
			if (!mFloor.RemovePoint(index)) {
				return false;
			}
			if (mWalls == null) {
				return true;
			}
			index--;
			if (index < 0) {
				index = mWalls.Count - 1;
			}
			if (index > mWalls.Count - 1) {
				return true;
			}
			var wall = mWalls[index];
			mWalls.RemoveAt(index);
			mPlan.AddArchItem(wall.Item);
			uapp.CoreUtils.Delete(wall);
			return true;
		}

		public void DestroyWall(Wall wall) {
			var wallIndex = wallIndexOf(wall);
			if (wallIndex < 0) {
				return;
			}
			mFloor.RemovePoint(wallIndex);
			uapp.CoreUtils.Delete(wall);
			mWalls.RemoveAt(wallIndex);
		}

		private int wallIndexOf(Wall wall) {
			if (mWalls == null) {
				return -1;
			}
			return mWalls.IndexOf(wall);
		}

		public FloorPointMoveFunc FloorPointMoveFunc {
			get {
				return mFloor.PointMoveFunc;
			} set {
				mFloor.PointMoveFunc = value;
			}
		}

		public bool MovePoint(int index, Vector3 point) {
			if (!mFloor.MovePoint(index, point)) {
				return false;
			}
			// 联动墙体
			var l = mFloor.Data.NumPoints - 1;
			if (l < 2) {
				return true;
			}
			var p = index - 1;
			if (p < 0) {
				p = l;
			}
			var n = index;
			if (n > l) {
				n = 0;
			}
			var pw = mWalls[p];
			var nw = mWalls[n];
			pw.Start = point;
			nw.End = point;
			return true;
		}

		public bool MoveWall(Wall wall, Vector3 point) {
			if (wall == null || mWalls == null) {
				return false;
			}
			var wallIndex = mWalls.IndexOf(wall);
			if (wallIndex < 0) {
				return false;
			}
			var n = mFloor.Data.NumPoints;
			var startIndex = wallIndex;
			var endIndex = wallIndex + 1;
			if (endIndex > n - 1) {
				endIndex = 0;
			}
			var prevWallIndex = wallIndex - 1;
			if (prevWallIndex < 0) {
				prevWallIndex = n - 1;
			}
			var nextWallIndex = wallIndex + 1;
			if (nextWallIndex > n - 1) {
				nextWallIndex = 0;
			}
			var wallNormal = wall.Normal;
			var startPoint = mFloor.Data.PointAt(startIndex);
			var endPoint = mFloor.Data.PointAt(endIndex);
			startPoint = startPoint + Vector3.Dot(point - startPoint, wallNormal) * wallNormal;
			endPoint = endPoint + Vector3.Dot(point - endPoint, wallNormal) * wallNormal;
			mFloor.MovePoint(startIndex, startPoint);
			mFloor.MovePoint(endIndex, endPoint);
			// NOTE 以地板顶点为数据基础，墙体位置由地板顶点决定，调用bulid后同步，无需做以下处理
//			wall.Start = startPoint;
//			wall.End = endPoint;
//			Wall prevWall = mWalls[prevWallIndex];
//			if (prevWall != wall) {
//				prevWall.Start = endPoint;
//			}
//			Wall nextWall = mWalls[nextWallIndex];
//			if (nextWall != wall && nextWall != prevWall) {
//				nextWall.End = startPoint;
//			}
			return true;
		}

		public bool Move(Vector3 point) {
			var floorData = mFloor.Data;
			var n = floorData.NumPoints;
			var center = floorData.Center;
			var offset = point - center;
			for (var i = 0; i < n; i++) {
				var p = floorData.PointAt(i);
				p += offset;
				floorData.SetPoint(i, p);
			}
			return true;
		}

		public bool Offset(Vector3 offset) {
			var floorData = mFloor.Data;
			var n = floorData.NumPoints;
			for (var i = 0; i < n; i++) {
				var p = floorData.PointAt(i);
				p += offset;
				floorData.SetPoint(i, p);
			}
			return true;
		}

		public bool IsEmpty {
			get {
				var n = mFloor.Data.NumPoints;
				return n <= 0;
			}
		}

		public bool Build() {
			if (!mFloor.Build()) {
				return false;
			}
			if (mWalls != null) {
				var nw = mWalls.Count;
				var nf = mFloor.Data.NumPoints;
				for (var i = 0; i < nw; i++) {
					var wall = mWalls[i];
					// NOTE 为了让墙发现朝内，故意把start和end调换（跟顶点顺序相反）
					var s = i + 1;
					var e = i;
					if (s >= nf) {
						s = 0;
					}
					var start = mFloor.Data.PointAt(s);
					var end = mFloor.Data.PointAt(e);
					wall.Start = start;
					wall.End = end;
					wall.Height = Heights.WallDefault * mPlan.Scale;
					wall.Build();
				}
			}

			return true;
		}

		public override bool IsPointInXZ(Vector3 point) {
			return mFloor.IsPointInXZ(point);
		}

		public int PointByPoint(Vector3 point) {
			var n = mFloor.Data.NumPoints;
			for (var i = 0; i < n; i++) {
				var p = mFloor.Data.PointAt(i);
				if (uapp.MathUtils.ManhattanDistance(p, point) < Floor.PointSize) {
					return i;
				}
			}
			return -1;
		}

		public EditItem EditItemByPointXZ(Vector3 point) {
//			var editItem = WallItemsByPointXZ(point);
//			if (editItem != null) {
//				return editItem;
//			}
			if (mFloor.IsPointInXZ(point)) {
				return mFloor;
			}
			return WallByPointXZ(point);
		}

		public Wall WallByPointXZ(Vector3 point) {
			if (mWalls == null) {
				return null;
			}
			var n = mWalls.Count;
			for (var i = 0; i < n; i++) {
				var wall = mWalls[i];
				if (wall.IsPointInXZ(point)) {
					return wall;
				}
			}
			return null;
		}

		public EditItem WallItemsByPointXZ(Vector3 point) {
			if (mWalls == null) {
				return null;
			}
			var n = mWalls.Count;
			for (var i = 0; i < n; i++) {
				var wall = mWalls[i];
				var editItem = wall.EditItemByPointXZ(point);
				if (editItem != null) {
					return editItem;
				}
			}
			return null;
		}

		public EditItem WallAndItemsByPointXZ(Vector3 point) {
			if (mWalls == null) {
				return null;
			}
			var n = mWalls.Count;
			for (var i = 0; i < n; i++) {
				var wall = mWalls[i];
				var editItem = wall.EditItemByPointXZ(point);
				if (editItem != null) {
					return editItem;
				}
				if (wall.IsPointInXZ(point)) {
					return wall;
				}
			}
			return null;
		}

		public void SetEditItemState(EditItem editItem, EditState state, bool clearSelected = false) {
			if (editItem == null) {
				ClearEditItemState(clearSelected);
				return;
			}
			if (editItem == this) {
				editItem = mFloor;
			}
			if (editItem is Floor) {
				var floor = editItem as Floor;
				if (mFloor == floor) {
					if (clearSelected) {
						mFloor.EditState = state;
					} else {
						if (mFloor.EditState != EditState.Selected) {
							mFloor.EditState = state;
						}
					}
				} else {
					mFloor.EditState = EditState.Normal;
				}
				ClearWallState(true);
				ClearWallItemsState(true);
			} else if (editItem is Wall) {
				var wall = editItem as Wall;
				SetWallState(wall, state, clearSelected);
				ClearFloorState(true);
				ClearWallItemsState(true);
			} else if (editItem is InWallItem) {
				var inWallItem = editItem as InWallItem;
				var wall = inWallItem.Wall;
				wall.SetEditItemState(inWallItem, state, clearSelected);
				ClearFloorState(true);
				ClearWallState(true);
			}
		}

//		public void SetItemStateByPoint(Vector3 point, EditState state) {
//			var p = PointByPoint(point);
//			if (p >= 0) {
//				// TODO 高亮顶点
//			} else {
//				var w = WallByPoint(point);
//				if (w < 0) {
//					return;
//				}
//				var n = mWalls.Count;
//				for (var i = 0; i < n; i++) {
//					var wall = mWalls[i];
//					if (wall.EditState == EditState.Selected) {
//						continue;
//					}
//					if (i == w) {
//						wall.EditState = state;
//					} else {
//						wall.EditState = EditState.Normal;
//					}
//				}
//			}
//		}

//		public Wall WallAt(int i) {
//			if (mWalls == null) {
//				return null;
//			}
//			var n = mWalls.Count;
//			if (i < 0 || i > n - 1) {
//				return null;
//			}
//			return mWalls[i];
//		}

		public void SetWallState(Wall wall, EditState state, bool clearSelected = false) {
			if (mWalls == null) {
				return;
			}
			foreach (var w in mWalls) {
				if (!clearSelected) {
					if (w.EditState == EditState.Selected) {
						continue;
					}
				}
				if (w == wall) {
					w.EditState = state;
				} else {
					w.EditState = EditState.Normal;
				}
			}
		}

		public void ClearEditItemState(bool clearSelected = false) {
			ClearFloorState(clearSelected);
			ClearWallState(clearSelected);
			ClearWallItemsState(clearSelected);
		}

		public void ClearFloorState(bool clearSelected = false) {
			if (!clearSelected) {
				if (mFloor.EditState == EditState.Selected) {
					return;
				}
			}
			mFloor.EditState = EditState.Normal;
		}

		public void ClearWallState(bool clearSelected = false) {
			if (mWalls != null) {
				foreach (var wall in mWalls) {
					if (!clearSelected) {
						if (wall.EditState == EditState.Selected) {
							continue;
						}
					}
					wall.EditState = EditState.Normal;
				}
			}
		}

		public void ClearWallItemsState(bool clearSelected = false) {
			if (mWalls != null) {
				foreach (var wall in mWalls) {
					wall.ClearEditItemState(clearSelected);
				}
			}
		}

		public bool ContainsWall(Wall wall) {
			if (mWalls == null) {
				return false;
			}
			return mWalls.Contains(wall);
		}

//		public bool IsPointIn(Vector3 point) {
//			return mFloor.IsPointIn(point);
//		}

		public void onPlanScaleChanged(float scale) {
			mFloor.onPlanScaleChanged(scale);
			if (mWalls != null) {
				foreach (var wall in mWalls) {
					wall.onPlanScaleChanged(scale);
				}
			}
		}

	}
}
