using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public class RayPointerRaycaster : GraphicRaycaster {

		private Canvas mCanvas;

		protected Canvas canvas {
			get {
				if (mCanvas == null) {
					mCanvas = GetComponent<Canvas>();
				}
				return mCanvas;
			}
		}

		public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList) {
			
			var canvas = this.canvas;
			if (canvas == null) {
				return;
			}
			var rayPointerEventData = eventData as RayPointerEventData;
			if (rayPointerEventData == null) {
				return;
			}
			// 获取canvas的RectTransform以便转换为canvas坐标系
			RectTransform canvasTransform = canvas.gameObject.GetComponent<RectTransform>();

			var ray = rayPointerEventData.Ray;
			var graphics = GraphicRegistry.GetGraphicsForCanvas(canvas);
			for (var i = 0; i < graphics.Count; i++) {
				var graphic = graphics[i];
				if (graphic.depth < 0) {
					continue;
				}
				Vector3 worldPosition;
				if (RayIntersectsRectTransform(graphic.rectTransform, ray, out worldPosition)) {
					var screenPosition = canvasTransform.worldToLocalMatrix.MultiplyPoint(worldPosition);
                    //screenPosition = canvas.transform.localToWorldMatrix.MultiplyPoint (screenPosition);
					var result = new RaycastResult() {
						module = this,
						gameObject = graphic.gameObject,
						index = resultAppendList.Count,
						depth = graphic.depth,
						screenPosition = screenPosition,
						worldPosition = worldPosition,
						distance = Vector3.Distance(ray.origin, worldPosition),
					};
					resultAppendList.Add(result);
				}

				// 特殊result，用于获取canvas坐标
				if (RayIntersectsRectTransform(canvasTransform, ray, out worldPosition)) {
					var screenPosition = canvasTransform.worldToLocalMatrix.MultiplyPoint(worldPosition);
                    //screenPosition = canvas.transform.localToWorldMatrix.MultiplyPoint (screenPosition);
                    //Debug.Log ("cp:" + screenPosition);
                    var result = new RaycastResult() {
						module = this,
						gameObject = canvas.gameObject,
						index = resultAppendList.Count,
						depth = -1,
						screenPosition = screenPosition,
						worldPosition = worldPosition,
						distance = Vector3.Distance(ray.origin, worldPosition),
					};
					resultAppendList.Add(result);
				}
			}
//			resultAppendList.Sort(mRaycastResultComparer);
		}

		public bool RayIntersectsRectTransform(RectTransform rectTransform, Ray ray, out Vector3 worldPosition) {
			Vector3[] corners = new Vector3[4];
			rectTransform.GetWorldCorners(corners);
			Plane plane = new Plane(corners[0], corners[1], corners[2]);

			float enter;
			if (!plane.Raycast(ray, out enter)) {
				worldPosition = Vector3.zero;
				return false;
			}

			Vector3 intersection = ray.GetPoint(enter);

			Vector3 BottomEdge = corners[3] - corners[0];
			Vector3 LeftEdge = corners[1] - corners[0];
			float BottomDot = Vector3.Dot(intersection - corners[0], BottomEdge);
			float LeftDot = Vector3.Dot(intersection - corners[0], LeftEdge);
			if (BottomDot < BottomEdge.sqrMagnitude && // Can use sqrMag because BottomEdge is not normalized
			    LeftDot < LeftEdge.sqrMagnitude &&
			    BottomDot >= 0 &&
			    LeftDot >= 0) {
				worldPosition = corners[0] + LeftDot * LeftEdge / LeftEdge.sqrMagnitude + BottomDot * BottomEdge / BottomEdge.sqrMagnitude;
				return true;
			} else {
				worldPosition = Vector3.zero;
				return false;
			}
		}

//		private RaycastResultComparer mRaycastResultComparer = new RaycastResultComparer();
//		private class RaycastResultComparer : IComparer<RaycastResult> {
//
//			public int Compare(RaycastResult r1, RaycastResult r2) {
//				if (r1.depth == r2.depth) {
//					return 0;
//				}
//				return r1.depth > r2.depth ? -1 : 1;
//			}
//		}
	}
}
