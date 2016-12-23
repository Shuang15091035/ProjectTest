using UnityEngine;
using System.Collections;

namespace uapp {

	public enum QuadrangleConer {
		TopLeft,
		BottomLeft,
		BottomRight,
		TopRight,
	}
	
	public class Quadrangle {

		private Vector3[] mPoints = new Vector3[4];

		public Quadrangle(Vector3 topLeft, Vector3 bottomLeft, Vector3 bottomRight, Vector3 topRight) {
			mPoints[(int)QuadrangleConer.TopLeft] = topLeft;
			mPoints[(int)QuadrangleConer.BottomLeft] = bottomLeft;
			mPoints[(int)QuadrangleConer.BottomRight] = bottomRight;
			mPoints[(int)QuadrangleConer.TopRight] = topRight;
		}

		public Quadrangle() : this(Vector3.zero, Vector3.zero, Vector3.zero, Vector3.zero) {
			
		}

		public Vector3[] Points {
			get {
				return mPoints;
			}
		}

		public Vector3 PointAt(QuadrangleConer coner) {
			return mPoints[(int)coner];
		}

		public void SetPoint(QuadrangleConer coner, Vector3 point) {
			mPoints[(int)coner] = point;
		}

		public Polygon ToPolygon() {
			var polygon = new Polygon();
			foreach (var point in mPoints) {
				polygon.PushPoint(point);
			}
			return polygon;
		}

	}

}
