using UnityEngine;
using System.Collections;

namespace uapp {

	public enum WallItemPlace {
		InWall,
		OnWall,
	}

	public enum WallItemPointTo {
		InRoom,
		OutRoom,
	}

	public interface IWallItem : IArchElement {

		IWall Wall { get; set; }
		Rect Data { get; set; }
		WallItemPlace Place { get; set; }
		WallItemPointTo PointTo { get; set; }
	}
	
	public class WallItem : ArchElement, IWallItem {

		protected IWall mWall;
		protected Rect mData; // NOTE 基于墙面坐标系，以墙的左下角为原点，墙的方式为x正方向，向上为y正方向
		protected WallItemPlace mPlace;
		protected WallItemPointTo mPointTo;

		public WallItem(WallItemPlace place, Rect data) {
			mPlace = place;
			mData = data;
		}

		public WallItem(WallItemPlace place = WallItemPlace.InWall) : this(place, Constants.RectZero) {
			
		}

		public IWall Wall {
			get {
				return mWall;
			}
			set {
				mWall = value;
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

		public WallItemPlace Place {
			get {
				return mPlace;
			}
			set {
				mPlace = value;
			}
		}

		public WallItemPointTo PointTo {
			get {
				return mPointTo;
			}
			set {
				mPointTo = value;
			}
		}

	}

}
