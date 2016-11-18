using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public interface IWall : IObject {

		float Thickness { get; set; }
		float Height { get; set; }
		IWallElement InnerWall { get; set; }
		IWallElement OuterWall { get; set; }
		IEnumerable<IWallItem> WallItems { get; }
		bool HasWallItems { get; }
		void AddWallItem(IWallItem wallItem);
	}
	
	public class Wall : UObject, IWall {

		private float mThickness = 0.5f;
		private float mHeight = 2.8f;
		private IWallElement mInnerWall;
		private IWallElement mOuterWall;
		private List<IWallItem> mWallItems;

		public Wall() {
			
		}

		public float Thickness {
			get {
				return mThickness;
			}
			set {
				mThickness = value;
			}
		}

		public float Height {
			get {
				return mHeight;
			}
			set {
				mHeight = value;
			}
		}

		public IWallElement InnerWall {
			get {
				return mInnerWall;
			}
			set {
				mInnerWall = value;
			}
		}

		public IWallElement OuterWall {
			get {
				return mOuterWall;
			}
			set {
				mOuterWall = value;
			}
		}

		public IEnumerable<IWallItem> WallItems { 
			get {
				return mWallItems;
			}
		}

		public bool HasWallItems {
			get {
				if (mWallItems == null || mWallItems.Count == 0) {
					return false;
				}
				return true;
			}
		}

		public void AddWallItem(IWallItem wallItem) {
			if (mWallItems == null) {
				mWallItems = new List<IWallItem>();
			}
			mWallItems.Add(wallItem);
		}
	}

}
