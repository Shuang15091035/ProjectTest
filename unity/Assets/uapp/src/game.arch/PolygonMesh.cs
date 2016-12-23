using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public class PolygonMesh {

		public delegate Vector3 CastVertexDelegate(Vector3 v);
		public delegate Vector2 CastUVDelegate(Vector2 uv);

		public static bool TrianglulateXZ(Polygon polygon, IEnumerable<Polygon> holes, float height, Mesh mesh, bool clockwise, CastVertexDelegate castVertex = null, CastUVDelegate castUV = null) {
			return trianglulateMesh(polygon, holes, height, mesh, clockwise, castVertex, castUV);
		}

		private static Poly2Tri.Polygon mPolygon;
		private static IList<Poly2Tri.PolygonPoint> mPolygonPoints;

		private static Poly2Tri.Polygon castPolygon(Polygon polygon) {
			if (mPolygonPoints == null) {
				mPolygonPoints = new List<Poly2Tri.PolygonPoint>();
			}
			mPolygonPoints.Clear();
			foreach (var point in polygon.Points) {
				mPolygonPoints.Add(new Poly2Tri.PolygonPoint(point.x, point.z));
			}
			return new Poly2Tri.Polygon(mPolygonPoints);
		}

		private static bool canTrianglulation(Polygon polygon, IEnumerable<Polygon> holes) {
			if (polygon.isEdgeIntersect) {
				return false;
			}

			mPolygon = castPolygon(polygon);
			if (holes != null) {
				foreach (var hole in holes) {
					mPolygon.AddHole(castPolygon(hole));
				}
			}
			try {
				Poly2Tri.P2T.Triangulate(mPolygon);
			} catch (System.Exception) {
				return false;
			}

			return mPolygon.Triangles.Count > 0;
		}

		protected static bool trianglulateMesh(Polygon polygon, IEnumerable<Polygon> holes, float height, Mesh mesh, bool clockwise, CastVertexDelegate castVertex, CastUVDelegate castUV) {
			if (!canTrianglulation(polygon, holes)) {
				return false;
			}
			var triangles = mPolygon.Triangles;
			var vertices = new Vector3[triangles.Count * 3];
			var indices = new int[triangles.Count * 3];
			var i = 0;
			var j = 0;
			if (!clockwise) {
				j = indices.Length - 1;
			}
			foreach (var triangle in triangles) {
				
				var x0 = triangle.Points[2].Xf;
				var y0 = triangle.Points[2].Yf;
				var x1 = triangle.Points[1].Xf;
				var y1 = triangle.Points[1].Yf;
				var x2 = triangle.Points[0].Xf;
				var y2 = triangle.Points[0].Yf;

				if (clockwise) {
					indices[j++] = i;
				} else {
					indices[j--] = i;
				}
				var v0 = new Vector3(x0, height, y0);
				vertices[i++] = v0;

				if (clockwise) {
					indices[j++] = i;
				} else {
					indices[j--] = i;
				}
				var v1 = new Vector3(x1, height, y1);
				vertices[i++] = v1;

				if (clockwise) {
					indices[j++] = i;
				} else {
					indices[j--] = i;
				}
				var v2 = new Vector3(x2, height, y2);
				vertices[i++] = v2;
			}

			var center = Vector3.zero;
			foreach (var vertex in vertices) {
				center += vertex;
			}
			var uvs = new Vector2[triangles.Count * 3];
			var k = 0;
			center /= vertices.Length;
			foreach (var vertex in vertices) {
				var uv = Vector2.zero;
				uv.x = vertex.x - center.x;
				uv.y = vertex.z - center.z;
				if (castUV != null) {
					uv = castUV(uv);
				}
				uvs[k++] = uv;
			}
			if (castVertex != null) {
				var n = vertices.Length;
				for (i = 0; i < n; i++) {
					var v = vertices[i];
					vertices[i] = castVertex(v);
				}
			}

			mesh.Clear();
			mesh.vertices = vertices;
			mesh.uv = uvs;
			mesh.triangles = indices;
			return true;
		}

	}
}
