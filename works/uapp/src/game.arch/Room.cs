using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public interface IRoom : IObject {

		RealWorldTexture WallTexture { get; set; }
		RealWorldTexture FloorTexture { get; set; }
		RealWorldTexture CeilTexture { get; set; }
		float GroundHeight { get; set; }
		Polygon Data { get; }
		IEnumerable<IWall> Walls { get; }
		IWall WallAt(int index);
		void PushPoint(Vector3 point);
	}

	public class Room : UObject, IRoom {

		private RealWorldTexture mWallTexture;
		private RealWorldTexture mFloorTexture;
		private RealWorldTexture mCeilTexture;
		private float mGroundHeight;
		private Polygon mData = new Polygon();
		private List<IWall> mWalls;

		public RealWorldTexture WallTexture {
			get {
				return mWallTexture;
			}
			set {
				mWallTexture = value;
			}
		}

		public RealWorldTexture FloorTexture {
			get {
				return mFloorTexture;
			}
			set {
				mFloorTexture = value;
			}
		}

		public RealWorldTexture CeilTexture {
			get {
				return mCeilTexture;
			}
			set {
				mCeilTexture = value;
			}
		}

		public float GroundHeight {
			get {
				return mGroundHeight;
			}
			set {
				mGroundHeight = value;
			}
		}

		public Polygon Data {
			get {
				return mData;
			}
		}

		public IEnumerable<IWall> Walls {
			get {
				return mWalls;
			}
		}

		public IWall WallAt(int index) {
			if (mWalls == null) {
				return null;
			}
			if (index < 0 || index >= mWalls.Count) {
				return null;
			}
			return mWalls[index];
		}

		public void PushPoint(Vector3 point) {
			mData.PushPoint(point);
			if (mWalls == null) {
				mWalls = new List<IWall>();
			}
			var wall = new Wall();
			mWalls.Add(wall);
		}

	}

}
