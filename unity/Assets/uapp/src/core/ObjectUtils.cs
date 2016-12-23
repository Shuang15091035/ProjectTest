using UnityEngine;
using System;
using System.Collections;

namespace uapp {

	public static class ObjectUtils {

		public static GameObject GetParent(GameObject gameObject) {
			if (gameObject == null) {
				return null;
			}
			Transform parentTransform = gameObject.transform.parent;
			if (parentTransform == null) {
				return null;
			}
			return parentTransform.gameObject;
		}

		public static void SetParent(GameObject gameObject, GameObject parent) {
			if (gameObject == null) {
				return;
			}
			Transform parentTransform = parent == null ? null : parent.transform;
			gameObject.transform.parent = parentTransform;
		}

		public static T FindParent<T>(GameObject gameObject, bool includeSelf = false) where T : Component {
			if (gameObject == null) {
				return null;
			}
			GameObject parent = null;
			if (includeSelf) {
				parent = gameObject;
			} else {
				parent = GetParent(gameObject);
			}
			while (parent != null) {
				var comp = parent.GetComponent<T>();
				if (comp != null) {
					return comp;
				}
				parent = GetParent(parent);
			}
			return null;
		}

		public static GameObject FindChild(string name, GameObject parent, bool incusive = true) {
			if(parent == null) {
				return null;
			}
			GameObject c = null;
			Transform ct = parent.transform.FindChild(name);
			if(ct != null) {
				c = ct.gameObject;
				return c;
			}
			if(incusive) {
				foreach(Transform t in parent.transform) {
					c = FindChild(name, t.gameObject, incusive);
					if(c != null) {
						return c;
					}
				}
			}
			return null;
		}

		public static void Clear(GameObject gameObject) {
			if (gameObject == null) {
				return;
			}
			foreach (Transform child in gameObject.transform) {
				GameObject.Destroy(child.gameObject);
			}
		}

		public static string GetPath(GameObject gameObject, string seperator = ".") {
			string path = gameObject.name;
			GameObject parent = GetParent(gameObject);
			while (parent != null) {
				path = parent.name + seperator + path;
				gameObject = parent;
				parent = GetParent(gameObject);
			}
			return path;
		}

		public static GameObject FindPath(string path, GameObject parent = null, string seperator = ".") {
			var parts = path.Split(new string[]{seperator}, StringSplitOptions.None);
			if (parts == null || parts.Length == 0) {
			}
			int startPart = 0;
			if (parent == null) {
				parent = GameObject.Find (parts[0]);
				startPart = 1;
			}
			if (parent == null) {
				return null;
			}
			for (int i = startPart; i < parts.Length; i++) {
				var part = parts[i];
				var child = FindChild (part, parent, false);
				if (child == null) {
					return null;
				}
				parent = child;
			}
			return parent;
		}

		public static void SetBehavioursEnabled(GameObject gameObject, bool enabled) {
			if (gameObject == null) {
				return;
			}
			var behaviours = gameObject.GetComponents<MonoBehaviour>();
			if (behaviours != null) {
				foreach (var behaviour in behaviours) {
					if (behaviour == null) {
						continue; // NOTE 妈逼Unity，神经病，给我返回的列表居然有空引用
					}
					behaviour.enabled = enabled;
				}
			}
			behaviours = gameObject.GetComponentsInChildren<MonoBehaviour>();
			if (behaviours != null) {
				foreach (var behaviour in behaviours) {
					if (behaviour == null) {
						continue; // NOTE 妈逼Unity，神经病，给我返回的列表居然有空引用
					}
					behaviour.enabled = enabled;
				}
			}
		}

		public static Bounds GetBounds(GameObject gameObject) {
			Bounds bounds = new Bounds();
			bool boundsIsNull = true;
			if (gameObject == null) {
				return bounds;
			}

			var meshFilters = gameObject.GetComponents<MeshFilter>();
			foreach (var meshFilter in meshFilters) {
				Mesh mesh = meshFilter.sharedMesh;
				if (mesh != null) {
					//mesh.RecalculateBounds ();
					if (boundsIsNull) {
						bounds.min = mesh.bounds.min;
						bounds.max = mesh.bounds.max;
						boundsIsNull = false;
					} else {
						bounds.Encapsulate(mesh.bounds);
					}
				}
			}
			meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
			foreach (var meshFilter in meshFilters) {
				Mesh mesh = meshFilter.sharedMesh;
				if (mesh != null) {
					//mesh.RecalculateBounds ();
					if (boundsIsNull) {
						bounds.min = mesh.bounds.min;
						bounds.max = mesh.bounds.max;
						boundsIsNull = false;
					} else {
						bounds.Encapsulate(mesh.bounds);
					}
				}
			}
			return bounds;
		}

		public static Bounds GetScaleBounds(GameObject gameObject) {
//			var bounds = GetBounds(gameObject);
//			var lossyScale = gameObject.transform.lossyScale;
//			var center = bounds.center;
//			bounds.center = new Vector3(center.x * lossyScale.x, center.y * lossyScale.y, center.z * lossyScale.z);
//			var extents = bounds.extents;
//			bounds.extents = new Vector3(extents.x * lossyScale.x, extents.y * lossyScale.y, extents.z * lossyScale.z);
//			return bounds;

			Bounds bounds = new Bounds();
			bool boundsIsNull = true;
			if (gameObject == null) {
				return bounds;
			}

			var meshFilters = gameObject.GetComponents<MeshFilter>();
			foreach (var meshFilter in meshFilters) {
				Mesh mesh = meshFilter.sharedMesh;
				if (mesh != null) {
					var mb = BoundsUtils.Scale(mesh.bounds, meshFilter.gameObject.transform.localScale);
					if (boundsIsNull) {
						bounds.min = mb.min;
						bounds.max = mb.max;
						boundsIsNull = false;
					} else {
						bounds.Encapsulate(mb);
					}
				}
			}
			meshFilters = gameObject.GetComponentsInChildren<MeshFilter>();
			foreach (var meshFilter in meshFilters) {
				Mesh mesh = meshFilter.sharedMesh;
				if (mesh != null) {
					var mb = BoundsUtils.Scale(mesh.bounds, meshFilter.gameObject.transform.localScale);
					if (boundsIsNull) {
						bounds.min = mb.min;
						bounds.max = mb.max;
						boundsIsNull = false;
					} else {
						bounds.Encapsulate(mb);
					}
				}
			}
			return bounds;
		}

		public static Bounds GetTransformBounds(GameObject gameObject) {
//			var bounds = GetBounds(gameObject);
//			var matrix = gameObject.transform.localToWorldMatrix;
//			var min = bounds.min;
//			bounds.min = matrix.MultiplyPoint(min);
//			var max = bounds.max;
//			bounds.max = matrix.MultiplyPoint(max);
//			return bounds;

			Bounds bounds = new Bounds();
			bool boundsIsNull = true;
			if (gameObject == null) {
				return bounds;
			}

			var meshRenderers = gameObject.GetComponents<MeshRenderer>();
			foreach (var meshRenderer in meshRenderers) {
				if (boundsIsNull) {
					bounds.min = meshRenderer.bounds.min;
					bounds.max = meshRenderer.bounds.max;
					boundsIsNull = false;
				} else {
					bounds.Encapsulate(meshRenderer.bounds);
				}
			}
			meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
			foreach (var meshRenderer in meshRenderers) {
				if (boundsIsNull) {
					bounds.min = meshRenderer.bounds.min;
					bounds.max = meshRenderer.bounds.max;
					boundsIsNull = false;
				} else {
					bounds.Encapsulate(meshRenderer.bounds);
				}
			}
			return bounds;
		}

		public static Mesh GetMesh(GameObject gameObject) {
			var meshFilter = gameObject.GetComponent<MeshFilter>();
			if (meshFilter == null) {
				return null;
			}
			return meshFilter.sharedMesh;
		}

		public static bool SetMesh(GameObject gameObject, Mesh mesh) {
			var meshFilter = gameObject.GetComponent<MeshFilter>();
			if (meshFilter == null) {
				return false;
			}
			meshFilter.sharedMesh = mesh;
			return true;
		}

		public static void SetMeshCollider(GameObject gameObject, bool includeChildren = false) {
			if (gameObject == null) {
				return;
			}

			var mesh = GetMesh(gameObject);
			MeshCollider meshCollider = null;
			var collider = gameObject.GetComponent<Collider>();
			if (collider == null) {
				if(mesh != null) {
					meshCollider = gameObject.AddComponent<MeshCollider>();
					meshCollider.sharedMesh = mesh;
				}
			} else if (!(collider is MeshCollider)) {
				GameObject.Destroy(collider);
				if(mesh != null) {
					meshCollider = gameObject.AddComponent<MeshCollider>();
					meshCollider.sharedMesh = mesh;
				}
			} else {
				meshCollider = collider as MeshCollider;
				meshCollider.sharedMesh = mesh;
			}

			if (includeChildren) {
				foreach(Transform childTransform in gameObject.transform) {
					var child = childTransform.gameObject;
					SetMeshCollider(child, includeChildren);
				}
			}

		}

		public static Material GetMaterial(GameObject gameObject, bool shared = true) {
			var meshRenderer = gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer == null) {
				return null;
			}
			if (shared) {
				return meshRenderer.sharedMaterial;
			}
			return meshRenderer.material;
		}

		public static void SetMaterial(GameObject gameObject, Material material, bool shared = true) {
			var meshRenderer = gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer == null) {
				return;
			}
			if (shared) {
				meshRenderer.sharedMaterial = material;
			}
			meshRenderer.material = material;
		}

		public static void SetColor(GameObject gameObject, Color color, bool includeChildren = false) {
			if (gameObject == null) {
				return;
			}
			var meshRenderer = gameObject.GetComponent<MeshRenderer>();
			if (meshRenderer != null) {
				var material = meshRenderer.material;
				material.color = color;
			}
			if (includeChildren) {
				EnumChild (gameObject, (GameObject child) => {
					meshRenderer = child.GetComponent<MeshRenderer> ();
					if (meshRenderer != null) {
						var material = meshRenderer.material;
						material.color = color;
					}
					return false;
				}, includeChildren);
			}
		}

		public delegate bool OnChildDelegate(GameObject child);
		public static void EnumChild(GameObject gameObject, OnChildDelegate onChild, bool recursive = false) {
			enumChild(gameObject, onChild, recursive);
		}

		private static bool enumChild(GameObject gameObject, OnChildDelegate onChild, bool recursive = false) {
			if (gameObject == null || onChild == null) {
				return false;
			}
			var b = false;
			foreach (Transform childTransform in gameObject.transform) {
				var child = childTransform.gameObject;
				b = onChild(child);
				if (b) {
					break;
				}
				if (recursive) {
					b = enumChild(child, onChild, recursive);
				}
				if (b) {
					break;
				}
			}
			return b;
		}

		public delegate void OnMeshDelegate(GameObject gameObject, Mesh mesh);
		public static void EnumMesh(GameObject gameObject, OnMeshDelegate onMesh, bool includeChildren = false) {
			if (onMesh == null) {
				return;
			}
			var meshFilter = gameObject.GetComponent<MeshFilter>();
			var meshRenderer = gameObject.GetComponent<MeshRenderer>();
			if (meshFilter != null && meshRenderer != null) {
				Mesh mesh = meshFilter.sharedMesh;
				if (mesh != null && onMesh != null) {
					onMesh(gameObject, mesh);
				}
			}
			if (includeChildren) {
				foreach (Transform childTransform in gameObject.transform) {
					var child = childTransform.gameObject;
					EnumMesh(child, onMesh, includeChildren);
				}
			}
		}

		public delegate void OnSubMeshDelegate(GameObject gameObject, Mesh mesh, int submeshIndex, Material material);
		public static void EnumSubMesh(GameObject gameObject, OnSubMeshDelegate onSubMesh, bool includeChildren = false) {
			if (onSubMesh == null) {
				return;
			}
			var meshFilter = gameObject.GetComponent<MeshFilter>();
			var meshRenderer = gameObject.GetComponent<MeshRenderer>();
			if (meshFilter != null && meshRenderer != null) {
				Mesh mesh = meshFilter.sharedMesh;
				var materials = meshRenderer.sharedMaterials;
				if (mesh != null && materials != null && materials.Length > 0 && mesh.subMeshCount == materials.Length && onSubMesh != null) {
					for (var i = 0; i < mesh.subMeshCount; i++) {
						Material material = materials[i];
						onSubMesh(gameObject, mesh, i, material);
					}
				}
			}
			if (includeChildren) {
				foreach (Transform childTransform in gameObject.transform) {
					var child = childTransform.gameObject;
					EnumSubMesh(child, onSubMesh, includeChildren);
				}
			}
		}

		public delegate void OnComponentDelegate<T>(GameObject gameObject, T component);
		public static void EnumComponent<T>(GameObject gameObject, OnComponentDelegate<T> onComponent, bool includeChildren = false) {
			if (onComponent == null) {
				return;
			}
			var components = gameObject.GetComponents<T>();
			if (components != null) {
				foreach (var component in components) {
					onComponent(gameObject, component);
				}
			}
			if (includeChildren) {
				foreach (Transform childTransform in gameObject.transform) {
					var child = childTransform.gameObject;
					EnumComponent<T>(child, onComponent, includeChildren);
				}
			}
		}

		public static T GetComponentInChildren<T>(GameObject gameObject) where T : Component {
			var comps = gameObject.GetComponentsInChildren<T>(); // NOTE GetComponentsInChildren是包括自身的，故做一个不包含自身的方法
			if (comps == null || comps.Length == 0) {
				return null;
			}
			var myComp = gameObject.GetComponent<T>();
			T result = null;
			foreach (var comp in comps) {
				if (comp != myComp) {
					result = comp;
					break;
				}
			}
			return result;
		}

		public static bool HasComponentInChildren<T>(GameObject gameObject) where T : Component {
			var b = false;
			EnumChild(gameObject, (GameObject child) => {
				b = child.GetComponent<T>() != null;
				return b;
			}, true);
			return b;
		}

		public static T AddComponentIfNotExists<T>(GameObject gameObject) where T : Component {
			if (gameObject == null) {
				return null;
			}
			var component = gameObject.GetComponent<T>();
			if (component == null) {
				component = gameObject.AddComponent<T>();
			}
			return component;
		}

		public static float GetArea(GameObject gameObject, bool includeChildren = true) {
			if (gameObject == null) {
				return 0.0f;
			}
			var mesh = GetMesh(gameObject);
			var area = MeshUtils.GetArea(mesh);
			if (includeChildren) {
				EnumMesh(gameObject, (GameObject go, Mesh m) => {
					area += MeshUtils.GetArea(m);
				}, includeChildren);
			}
			return area;
		}
	}

}
