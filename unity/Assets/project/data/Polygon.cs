using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

//	public enum PolygonSimpleTriangulationMode {
//		CounterClockwise,
//		Clockwise,
//	}
	
	public class Polygon {

		private List<Vector3> mPoints;
//		private List<int> mResultIndices;
//		private List<int> mLastSimpleTrianglulateXZResult;

		public IEnumerable<Vector3> Points {
			get {
				return mPoints;
			}
		}

		public Vector3[] ToPointArray() {
			if (mPoints == null) {
				return null;
			}
			return mPoints.ToArray();
		}

		public int NumPoints {
			get {
				if (mPoints == null) {
					return 0;
				}
				return mPoints.Count;
			}
		}

		public Vector3 PointAt(int index) {
			if (mPoints == null) {
				return Vector3.zero;
			}
			if (index < 0 || index >= mPoints.Count) {
				return Vector3.zero;
			}
			return mPoints[index];
		}

		public void InsertHead(Vector3 point) {
			if (mPoints == null) {
				mPoints = new List<Vector3>();
			}
			mPoints.Insert(0, point);
		}

		public int PushPoint(Vector3 point) {
			if (mPoints == null) {
				mPoints = new List<Vector3>();
			}
			mPoints.Add(point);
			return mPoints.Count - 1;
		}

		public bool RemovePoint(int index) {
			if (mPoints == null) {
				return false;
			}
			if (index < 0 || index >= mPoints.Count) {
				return false;
			}
			mPoints.RemoveAt(index);
			return true;
		}

		public void PopPoint() {
			if (mPoints == null) {
				return;
			}
			mPoints.RemoveAt(mPoints.Count - 1);
		}

		public void ClearPoints() {
			if (mPoints == null) {
				return;
			}
			mPoints.Clear();
		}

		public bool SetPoint(int index, Vector3 point) {
			if (mPoints == null) {
				return false;
			}
			if (index < 0 || index >= mPoints.Count) {
				return false;
			}
			mPoints[index] = point;
			return true;
		}

		public Vector3 Center {
			get {
				var center = Vector3.zero;
				if (mPoints == null || mPoints.Count == 0) {
					return center;
				}
				foreach (var point in mPoints) {
					center += point;
				}
				center /= mPoints.Count;
				return center;
			}
		}

		public bool IsPointInXZ(Vector3 point) {
			var nc = NumPoints;
			if (nc < 3) {
				return false;
			}
			var px = point.x;
			var py = point.z;
			var i = 0;
			var j = nc - 1;
			bool c = false;
			for (; i < nc; j = i++) {
				var ni = mPoints[i];
				var nj = mPoints[j];
				var ix = ni.x;
				var iy = ni.z;
				var jx = nj.x;
				var jy = nj.z;
				if(((iy > py) != (jy > py)) && (px < (jx - ix) * (py - iy) / (jy - iy) + ix)) {
					c = !c;
				}
			}
			return c;
		}

		public float AreaProjectXZ {
			get {
				var A = 0.0f;
				if (mPoints == null || mPoints.Count == 0) {
					return A;
				}
				var n = mPoints.Count;
				for (int p = n - 1, q = 0; q < n; p = q++) {
					var P = mPoints[p];
					var Q = mPoints[q];
					A += P.x * Q.z - Q.x * P.z;
				}
				A *= 0.5f;
				return A;
			}
		}

		public bool isEdgeIntersect {
			get {
				if (mPoints == null || mPoints.Count == 0) {
					return false;
				}
				var n = mPoints.Count;
				for (var i = 0; i < n; i++) {
					var iis = i;
					var ie = i + 1;
					if (ie == n) {
						ie = 0;
					}
					for (var j = i + 1; j < n; j++) {
						var js = j;
						var je = j + 1;
						if (je == n) {
							je = 0;
						}
						var P1 = mPoints[iis];
						var Q1 = mPoints[ie];
						var P2 = mPoints[js];
						var Q2 = mPoints[je];
						Vector3 p;
						if (MathUtils.LineIntersect(P1, Q1, P2, Q2, out p)) {
							return true;
						}
					}
				}
				return false;
			}
		}

//		private IList<int> mTempIndices;
//		public bool SimpleTrianglulateXZ(PolygonSimpleTriangulationMode mode, IList<int> result) {
//			if (result == null) {
//				if (mResultIndices == null) {
//					mResultIndices = new List<int>();
//				}
//				result = mResultIndices;
//			}
//			result.Clear();
//
//			int n = NumPoints;
//			if (n < 3) {
//				return false;
//			}
//
//			if (mTempIndices == null) {
//				mTempIndices = new List<int>();
//			}
//			mTempIndices.Clear();
//			var isCounterClockwise = AreaProjectXZ > 0.0f;
//			if (isCounterClockwise) {
//				for (var v = 0; v < n; v++) {
//					mTempIndices.Add(v);
//				}
//			} else {
//				for (var v = 0; v < n; v++) {
//					mTempIndices.Add((n - 1) - v);
//				}
//			}
//
//			var nv = n;
//			var count = 2 * nv;
//
//			for (var v = nv - 1; nv > 2;) {
//				if (0 >= (count--)) {
//					return false;
//				}
//
//				var u = v;
//				if (nv <= u) {
//					u = 0;
//				}
//				v = u + 1;
//				if (nv <= v) {
//					v = 0;
//				}
//				var w = v + 1;
//				if (nv <= w) {
//					w = 0;
//				}
//
//				if (simpleTriangulateXZSnip(u, v, w, nv)) {
//					var a = mTempIndices[u];
//					var b = mTempIndices[v];
//					var c = mTempIndices[w];
//
//					switch (mode) {
//						case PolygonSimpleTriangulationMode.Clockwise: {
//								result.Add(c);
//								result.Add(b);
//								result.Add(a);
//								break;
//							}
//						case PolygonSimpleTriangulationMode.CounterClockwise: {
//								result.Add(a);
//								result.Add(b);
//								result.Add(c);
//								break;
//							}
//					}
//
//					for (int s = v, t = v + 1; t < nv; s++, t++) {
//						mTempIndices[s] = mTempIndices[t];
//					}
//					nv--;
//
//					count = 2 * nv;
//				}
//			}
//
//			return true;
//		}
//
//		public List<int> SimpleTrianglulateXZ(PolygonSimpleTriangulationMode mode) {
//			mLastSimpleTrianglulateXZResult = null;
//			if (!SimpleTrianglulateXZ(mode, null)) {
//				return null;
//			}
//			mLastSimpleTrianglulateXZResult = mResultIndices;
//			return mResultIndices;
//		}
//
//		public List<int> LastSimpleTrianglulateXZResult {
//			get {
//				return mLastSimpleTrianglulateXZResult;
//			}
//		}
//
//		bool simpleTriangulateXZSnip(int u, int v, int w, int n) {
//			u = mTempIndices[u];
//			v = mTempIndices[v];
//			w = mTempIndices[w];
//			var A = mPoints[u];
//			var B = mPoints[v];
//			var C = mPoints[w];
//
//			var EPSILON = 0.000001f;
//			var e = ((B.x - A.x) * (C.z - A.z)) - ((B.z - A.z) * (C.x - A.x));
//			if (e < EPSILON) {
//				return false;
//			}
//
//			for (var p = 0; p < n; p++) {
//				if ((p == u) || (p == v) || (p == w)) {
//					continue;
//				}
//				var P = mPoints[mTempIndices[p]];
//				if (Triangle.IsPointInsideXZ(A, B, C, P)) {
//					return false;
//				}
//			}
//			return true;
//		}
	}
}
