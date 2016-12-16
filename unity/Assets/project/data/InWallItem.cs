using UnityEngine;
using System.Collections;

namespace project {
	
	public class InWallItem : EditItem {

		protected Wall mWall;
		protected Rect mData;

		protected GameObject mDesignObject;

		protected InWallItem(string name, int id, SKU sku, GameObject parent = null) : base(name, id, sku, parent) {
			mData = new Rect(Vector2.zero, Vector2.one);
		}

//		public void AddToWall(Wall wall) {
//			if (wall == mWall) {
//				return;
//			}
//			if (mWall != null) {
//				mWall.AddInWallItem(this);
//			}
//			mWall = wall;
//			if (mWall != null) {
//				mWall.RemoveInWallItem(this);
//			}
//		}
//
//		public bool Move(Vector3 point) {
//			if (mWall == null) {
//				return false;
//			}
//			var room = mWall.Room;
//			if (room == null) {
//				return false;
//			}
//			var plan = room.Plan;
//			if (plan == null) {
//				return false;
//			}
//			var editItem = plan.EditItemByPointXZ(point);
//			if (!(editItem is Wall)) {
//				return false;
//			}
//			var wall = editItem as Wall;
//			AddToWall(wall);
//			// 计算point投影到墙方向的位置，然后移动过去
//			var wallDirection = wall.Direction;
//			var p = Vector3.Dot(point, wallDirection);
//			var dst = wall.Start + p * wallDirection;
//			mGameObject.transform.position = dst;
//			return true;
//		}

		public Wall Wall {
			get {
				return mWall;
			}
		}

		public Rect Data {
			get {
				return mData;
			}
			set {
				mData = value;
			}
		}

		public Vector2 Position {
			get {
				return mData.position;
			}
			set {
				mData.position = value;
			}
		}

		public Vector2 Size {
			get {
				return mData.size;
			}
		}

		public void _notifyWall(Wall wall) {
			mWall = wall;
		}
	}

}
