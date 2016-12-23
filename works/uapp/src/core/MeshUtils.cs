using UnityEngine;
using System;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace uapp {

	public class MeshUtils {

		public static void AddMeshComponentPure(GameObject gameObject, Mesh mesh, Material material) {
			var meshFilter = ObjectUtils.AddComponentIfNotExists<MeshFilter>(gameObject);
			meshFilter.sharedMesh = mesh;
			var meshRenderer = ObjectUtils.AddComponentIfNotExists<MeshRenderer>(gameObject);
			meshRenderer.sharedMaterial = material;
			meshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
		}

		public static Vector3 GetCenter(Mesh mesh) {
			var center = Vector3.zero;
			if (mesh == null) {
				return center;
			}
			var vertices = mesh.vertices;
			foreach (var vertex in vertices) {
				center.x += vertex.x;
				center.y += vertex.y;
				center.z += vertex.z;
			}
			center /= mesh.vertexCount;
			return center;
		}

		public static bool Scale(Mesh mesh, Vector3 scale) {
			if (mesh == null) {
				return false;
			}
			var newVertices = mesh.vertices;
			for (var i = 0; i < newVertices.Length; i++) {
				var vertex = newVertices[i];
				vertex.Scale(scale);
				newVertices[i] = vertex;
			}
			mesh.vertices = newVertices;
			mesh.RecalculateBounds();
			return true;
		}

		public static bool Translate(Mesh mesh, Vector3 offset) {
			if (mesh == null) {
				return false;
			}
			var newVertices = mesh.vertices;
			for (var i = 0; i < newVertices.Length; i++) {
				var vertex = newVertices[i];
				vertex.x += offset.x;
				vertex.y += offset.y;
				vertex.z += offset.z;
				newVertices[i] = vertex;
			}
			mesh.vertices = newVertices;
			mesh.RecalculateBounds();
			return true;
		}

		public static Mesh Clone(Mesh origin) {
			if (origin == null) {
				return null;
			}
			var newMesh = new Mesh();
			newMesh.name = origin.name;
			newMesh.vertices = origin.vertices;
			newMesh.triangles = origin.triangles;
			newMesh.uv = origin.uv;
			newMesh.uv2 = origin.uv2;
			newMesh.uv3 = origin.uv3;
			newMesh.uv4 = origin.uv4;
			newMesh.normals = origin.normals;
			newMesh.tangents = origin.tangents;
			newMesh.colors = origin.colors;
			return newMesh;
		}

		public class RectPivot {

			public static Vector2 Center = new Vector2(0.5f, 0.5f);
			public static Vector2 BottomCenter = new Vector2(0.0f, 0.5f);
			public static Vector2 BottomLeft = new Vector2(0.0f, 0.0f);
		}

		private static Vector2[] RectXZUVs = new Vector2[] {
			new Vector2(0.0f, 1.0f),
			new Vector2(1.0f, 1.0f),
			new Vector2(1.0f, 0.0f),
			new Vector2(0.0f, 0.0f),
		};
		private static int[] RectXZTriangles = new int[] {
			0, 1, 2,
			2, 3, 0,
		};

		public static Mesh RectXZ(float width, float height, Vector2 pivot, float ground = 0.0f) {
			Mesh mesh = new Mesh();
			RectXZ(mesh, width, height, pivot, ground);
			return mesh;
		}

		public static bool RectXZ(Mesh mesh, float width, float height, Vector2 pivot, float ground = 0.0f) {
			if (mesh == null) {
				return false;
			}
//			var hw = width * 0.5f;
//			var hh = height * 0.5f;
//			switch (pivot) {
//				case RectPivot.Center:
//					mesh.vertices = new Vector3[] {
//						new Vector3(-hw, ground, hh),
//						new Vector3(hw, ground, hh),
//						new Vector3(hw, ground, -hh),
//						new Vector3(-hw, ground, -hh),
//					};
//					break;
//				case RectPivot.BottomCenter:
//					mesh.vertices = new Vector3[] {
//						new Vector3(-hw, ground, height),
//						new Vector3(hw, ground, height),
//						new Vector3(hw, ground, 0.0f),
//						new Vector3(-hw, ground, 0.0f),
//					};
//					break;
//				case RectPivot.BottomLeft:
//					mesh.vertices = new Vector3[] {
//						new Vector3(0.0f, ground, height),
//						new Vector3(width, ground, height),
//						new Vector3(width, ground, 0.0f),
//						new Vector3(0.0f, ground, 0.0f),
//					};
//					break;
//			}
			var lw = width * pivot.x;
			var rw = width - lw;
			var dh = height * pivot.y;
			var uh = height - dh;
			mesh.vertices = new Vector3[] {
				new Vector3(-lw, ground, uh),
				new Vector3(rw, ground, uh),
				new Vector3(rw, ground, -dh),
				new Vector3(-lw, ground, -dh),
			};
			mesh.uv = RectXZUVs;
			mesh.triangles = RectXZTriangles;
			return true;
		}

		public static bool RectXZObject(GameObject gameObject, Material material, float width, float height, Vector2 pivot, float ground = 0.0f) {
			if (gameObject == null) {
				return false;
			}
			var mf = gameObject.AddComponent<MeshFilter>();
			mf.sharedMesh = RectXZ(width, height, pivot, ground);
			var mr = gameObject.AddComponent<MeshRenderer>();
			mr.sharedMaterial = material;
			return true;
		}

		public static float GetArea(Mesh mesh) {
			if (mesh == null) {
				return 0.0f;
			}
			float area = 0;
			var vertices = mesh.vertices;
			for(var i = 0; i < mesh.subMeshCount; i++) {
				var triangles = mesh.GetTriangles(i);
				for (var j = 0; j < triangles.Length; j += 3) {
					var A = vertices[triangles[j]];
					var B = vertices[triangles[j + 1]];
					var C = vertices[triangles[j + 2]];
					area += MathUtils.AreaOfTriangle(A, B, C);
				}
			}
			return area;
		}

		public static bool Save(Mesh mesh, string path) {
			#if UNITY_EDITOR
			if (mesh == null) {
				return false;
			}
			AssetDatabase.CreateAsset(mesh, "Assets/" + path);
			return true;
			#else
			throw new Exception("Save can only use in Editor.");
			#endif
		}
	}

}
