using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public enum ArchitectureBuildResult {
		Ok,
		Failed,
	}

	public class ArchitectureBuilder : UObject {
		
		private IArchitecture mArch;
		private SelfIncreaseList<IWallElement> mWallElements;
		private SelfIncreaseList<IFloorElement> mFloorElements;
		private SelfIncreaseList<ICeilElement> mCeilElements;

		private int mCurrentWallElementIndex;
		private int mCurrentFloorElementIndex;
		private int mCurrentCeilElementIndex;
		private List<IWallElement> mTempRoomInnerWalls;
		private List<IWallElement> mTempRoomOuterWalls;

		public ArchitectureBuilder(IArchitecture arch) {
			mArch = arch;
		}

		public ArchitectureBuildResult Build() {
			var arch = mArch;
			if (arch == null) {
				return ArchitectureBuildResult.Failed;
			}
			var rooms = arch.Rooms;
			if (rooms == null) {
				return ArchitectureBuildResult.Failed;
			}

			ArchitectureBuildResult result = ArchitectureBuildResult.Ok;
			if (mWallElements == null) {
				mWallElements = new SelfIncreaseList<IWallElement>();
			}
			mCurrentWallElementIndex = 0;
			if (mFloorElements == null) {
				mFloorElements = new SelfIncreaseList<IFloorElement>();
			}
			mCurrentFloorElementIndex = 0;
			if (mCeilElements == null) {
				mCeilElements = new SelfIncreaseList<ICeilElement>();
			}
			mCurrentCeilElementIndex = 0;

			foreach (var room in rooms) {
				result = buildRoom(room);
				if (result != ArchitectureBuildResult.Ok) {
					return result;
				}
			}

			return ArchitectureBuildResult.Ok;
		}

		private ArchitectureBuildResult buildRoom(IRoom room) {
			if (room == null) {
				return ArchitectureBuildResult.Ok;
			}

			var roomData = room.Data;
			if (roomData.NumPoints <= 1) {
				return ArchitectureBuildResult.Ok;
			}
			if (roomData.NumPoints == 2) {
				return buildOneWall(room);
			}

			return buildWalls(room);
		}

		private ArchitectureBuildResult buildOneWall(IRoom room) {
			var P0 = room.Data.PointAt(0);
			var P1 = room.Data.PointAt(1);
			if (Vector3Utils.Approximate(P0, P1)) {
				return ArchitectureBuildResult.Ok;
			}

			var w = room.WallAt(0);
			var h = w.Height;
			var t = w.Thickness;
			var ht = t * 0.5f;
			var W = P1 - P0;
			var N = Vector3.Cross(Vector3.up, W).normalized;

			// 内墙
			var innerWall = createWallElement(room, w);
			innerWall.Start = P0 + (ht * N);
			innerWall.End = P1 + (ht * N);
			innerWall.Height = h;
			innerWall.Solid = true;
			innerWall.Build();
			innerWall.Parent = mArch.Root;

			// 外墙
			var outerWall = createWallElement(room, w);
			outerWall.Start = P1 - (ht * N);
			outerWall.End = P0 - (ht * N);
			outerWall.Height = h;
			innerWall.Solid = false;
			outerWall.Build();
			outerWall.Parent = mArch.Root;

			return ArchitectureBuildResult.Ok;
		}

		private ArchitectureBuildResult buildWalls(IRoom room) {
			if (room.Data.isEdgeIntersect) { // NOTE 有交叉不能出现房间
				return ArchitectureBuildResult.Ok;
			}

			if (mTempRoomInnerWalls == null) {
				mTempRoomInnerWalls = new List<IWallElement>();
			}
			mTempRoomInnerWalls.Clear();
			if (mTempRoomOuterWalls == null) {
				mTempRoomOuterWalls = new List<IWallElement>();
			}
			mTempRoomOuterWalls.Clear();

			var roomData = room.Data;
			var n = roomData.NumPoints;
			for (var i = 0; i < n; i++) {
//				var pi = i - 1;
//				if (pi < 0) {
//					pi = n - 1;
//				}
//				var P0 = roomData.PointAt(pi);
//
//				var P1 = roomData.PointAt(i);
//				if (Vector3Utils.Approximate(P0, P1)) {
//					continue;
//				}
//
//				var ni = i + 1;
//				if (ni >= n) {
//					ni = 0;
//				}
//				var P2 = roomData.PointAt(ni);
//
//				var W1 = P1 - P0;
//				var W2 = P2 - P1;
//				var N1 = W1.normalized;
//				var N2 = W2.normalized;
//				var w1 = room.WallAt(pi);
//				var w2 = room.WallAt(i);
//				var h1 = w1.Height;
//				var h2 = w2.Height;
//				var t1 = w1.Thickness;
//				var t2 = w2.Thickness;
//				var ht1 = t1 * 0.5f;
//				var ht2 = t2 * 0.5f;
//
//				// 内墙
//				var PI = P1 - (ht1 * N1) + (ht2 * N2);
//				var innerWall = createWallElement(room, w2);
//				innerWall.Start = PI;
//				innerWall.Height = h2;
//				innerWall.Solid = true;
//				mTempRoomInnerWalls.Add(innerWall);
//
//				// 外墙
//				var PO = P1 + (ht1 * N1) - (ht2 * N2);
//				var outerWall = createWallElement(room, w2);
//				outerWall.End = PO;
//				outerWall.Height = h2;
//				outerWall.Solid = false;
//				mTempRoomOuterWalls.Add(outerWall);

				var pi = i - 1;
				if (pi < 0) {
					pi = n - 1;
				}
				var P0 = roomData.PointAt(pi);

				var P1 = roomData.PointAt(i);
				if (Vector3Utils.Approximate(P0, P1)) {
					continue;
				}

				var ni = i + 1;
				if (ni >= n) {
					ni = 0;
				}
				var P2 = roomData.PointAt(ni);

				var w1 = room.WallAt(pi);
				var w2 = room.WallAt(i);
				var h1 = w1.Height;
				var h2 = w2.Height;
				var t1 = w1.Thickness;
				var t2 = w2.Thickness;
				var ht1 = t1 * 0.5f;
				var ht2 = t2 * 0.5f;
				var W1 = P1 - P0;
				var W2 = P2 - P1;
				var N1 = ht1 * Vector3.Cross(Vector3.up, W1).normalized;
				var N2 = ht2 * Vector3.Cross(Vector3.up, W2).normalized;

				// 内墙，使用内线求交出内点
				Vector3 PI;
				if (!MathUtils.LineIntersect(P0 + N1, P1 + N1 + W1, P1 + N2 - W2, P2 + N2, out PI)) {
					PI = P1 + N2;
				}
				var innerWall = createWallElement(room, w2);
				w2.InnerWall = innerWall;
				innerWall.Start = PI;
				innerWall.Height = h2;
				innerWall.Solid = true;
				mTempRoomInnerWalls.Add(innerWall);

				// 外墙，使用外线求交出外点
				Vector3 PO;
				if (!MathUtils.LineIntersect(P0 - N1, P1 - N1 + W1, P1 - N2 - W2, P2 - N2, out PO)) {
					PO = P1 - N2;
				}
				var outerWall = createWallElement(room, w2);
				w2.OuterWall = outerWall;
				outerWall.End = PO;
				outerWall.Height = h2;
				outerWall.Solid = false;
				mTempRoomOuterWalls.Add(outerWall);
			}

			// 地板
			var floor = createFloorElement(room);
			floor.Data.ClearPoints();
			// 天花
			var ceil = createCeilElement(room);
			ceil.Data.ClearPoints();

			// 内墙
			n = mTempRoomInnerWalls.Count;
			for (var i = 0; i < n; i++) {
				var wall = mTempRoomInnerWalls[i];

				var ni = i + 1;
				if (ni >= n) {
					ni = 0;
				}
				var nextWall = mTempRoomInnerWalls[ni];

				wall.End = nextWall.Start;
				wall.Build();
				wall.Parent = mArch.Root;

				// NOTE 使用内墙数据构建地板
				floor.Data.PushPoint(wall.Start);
				// NOTE 使用内墙数据构建天花
				ceil.Data.PushPoint(wall.Start);
				ceil.Height = wall.Height;
			}

			// 构建地板
			floor.Build();
			floor.Parent = mArch.Root;
			// 构建天花
			ceil.Build();
			ceil.Parent = mArch.Root;

			// 外墙
			n = mTempRoomOuterWalls.Count;
			for (var i = 0; i < n; i++) {
				var wall = mTempRoomOuterWalls[i];

				var ni = i + 1;
				if (ni >= n) {
					ni = 0;
				}
				var nextWall = mTempRoomOuterWalls[ni];

				wall.Start = nextWall.End;
				wall.Build();
				wall.Parent = mArch.Root;
			}

			// 上下墙
			n = mTempRoomInnerWalls.Count;
			for (var i = 0; i < n; i++) {
				var innerWall = mTempRoomInnerWalls[i];
				var outerWall = mTempRoomOuterWalls[i];

				var topWall = createWallElement(room, null);
				topWall.Data.SetPoint(QuadrangleConer.TopLeft, outerWall.Data.PointAt(QuadrangleConer.TopRight));
				topWall.Data.SetPoint(QuadrangleConer.BottomLeft, innerWall.Data.PointAt(QuadrangleConer.TopLeft));
				topWall.Data.SetPoint(QuadrangleConer.BottomRight, innerWall.Data.PointAt(QuadrangleConer.TopRight));
				topWall.Data.SetPoint(QuadrangleConer.TopRight, outerWall.Data.PointAt(QuadrangleConer.TopLeft));
				topWall.Solid = false;
				topWall.Build();
				topWall.Parent = mArch.Root;

				// NOTE 下墙可忽略
//				var bottomWall = createWallElement(room, null);
//				bottomWall.Data.SetPoint(QuadrangleConer.TopLeft, innerWall.Data.PointAt(QuadrangleConer.BottomLeft));
//				bottomWall.Data.SetPoint(QuadrangleConer.BottomLeft, outerWall.Data.PointAt(QuadrangleConer.BottomRight));
//				bottomWall.Data.SetPoint(QuadrangleConer.BottomRight, outerWall.Data.PointAt(QuadrangleConer.BottomLeft));
//				bottomWall.Data.SetPoint(QuadrangleConer.TopRight, innerWall.Data.PointAt(QuadrangleConer.BottomRight));
//				bottomWall.Solid = false;
//				bottomWall.Build();
//				bottomWall.Parent = mArch.Root;

				// 补洞片
				var innerHoles = innerWall.GetBuildHoles();
				if (innerHoles != null) {
					var outerHoles = outerWall.GetBuildHoles();
				}
			}

			return ArchitectureBuildResult.Ok;
		}

		private IWallElement createWallElement(IRoom room, IWall wall) {
			var wallElement = mWallElements[mCurrentWallElementIndex];
			if (wallElement == null) {
				wallElement = new WallElement(room.WallTexture);
				mWallElements[mCurrentWallElementIndex] = wallElement;
			}
			wallElement.Arch = mArch;
			wallElement.Room = room;
			wallElement.Wall = wall;
			mCurrentWallElementIndex++;
			return wallElement;
		}

		private IFloorElement createFloorElement(IRoom room) {
			var floorElement = mFloorElements[mCurrentFloorElementIndex];
			if (floorElement == null) {
				floorElement = new FloorElement(room.FloorTexture);
				mFloorElements[mCurrentFloorElementIndex] = floorElement;
			}
			floorElement.Height = room.GroundHeight;
			floorElement.Arch = mArch;
			floorElement.Room = room;
			mCurrentFloorElementIndex++;
			return floorElement;
		}

		private ICeilElement createCeilElement(IRoom room) {
			var ceilElement = mCeilElements[mCurrentCeilElementIndex];
			if (ceilElement == null) {
				ceilElement = new CeilElement(room.CeilTexture);
				mCeilElements[mCurrentCeilElementIndex] = ceilElement;
			}
			ceilElement.Arch = mArch;
			ceilElement.Room = room;
			mCurrentCeilElementIndex++;
			return ceilElement;
		}
	}

}
