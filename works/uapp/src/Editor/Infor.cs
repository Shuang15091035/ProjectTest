using UnityEngine;
using System.Collections;

using UnityEditor;

namespace uapp {
	
	public class Infor {
		
	}

	public class ObjectInfor : EditorWindow {

		[MenuItem("Infor/Object Info")]
		static void Show() {
			EditorWindow.GetWindow<ObjectInfor>();
		}

		void OnGUI() {
			var gameObject = Selection.activeGameObject;
			if (gameObject == null) {
				return;
			}
			var bounds = ObjectUtils.GetTransformBounds(gameObject);
			EditorGUILayout.BeginVertical();

			var min = bounds.min;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("min:");
			EditorGUILayout.LabelField(Vector3Utils.Format(min, ", "));
			EditorGUILayout.EndHorizontal();

			var max = bounds.max;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("max:");
			EditorGUILayout.LabelField(Vector3Utils.Format(max, ", "));
			EditorGUILayout.EndHorizontal();

			var center = bounds.center;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("ctr:");
			EditorGUILayout.LabelField(Vector3Utils.Format(center, ", "));
			EditorGUILayout.EndHorizontal();

			var extents = bounds.extents;
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("ext:");
			EditorGUILayout.LabelField(Vector3Utils.Format(extents, ", "));
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.EndVertical();
		}
	}
}
