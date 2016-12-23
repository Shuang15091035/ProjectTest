using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public class MouseRayPointerInputModule : RayPointerInputModule {
		
		protected override bool isDebug() {
			return base.isDebug();
		}

		protected override int getNumPointers() {
			// 鼠标左右键
			return 2;
		}

		protected override bool getPointerRay(int pointerId, out Ray ray) {
			// NOTE 测试用，只获取鼠标左键的射线
			ray = Constants.RayZero;
			if (pointerId > 0) {
				return false;
			}
			var camera = GameObject.Find("Camera").GetComponent<Camera>();
			ray = camera.ScreenPointToRay(Input.mousePosition);
			return true;
		}

		protected override void showPointerRay(int pointerId, Ray ray) {
			var rayObject = GameObject.Find("Cylinder");
			rayObject.transform.position = ray.origin;
			rayObject.transform.up = ray.direction;
		}

		protected override bool isPointerDown(int pointerId) {
			return Input.GetMouseButtonDown(pointerId);
		}

		protected override bool isPointerMove(int pointerId) {
			return Input.GetMouseButton(pointerId);
		}

		protected override bool isPointerUp(int pointerId) {
			return Input.GetMouseButtonUp(pointerId);
		}
	}
}
