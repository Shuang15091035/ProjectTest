using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public class Constants {

		public static readonly Rect RectZero = new Rect(0.0f, 0.0f, 0.0f, 0.0f);
		public static readonly Bounds BoundsZero = new Bounds(Vector3.zero, Vector3.zero);
		public static readonly Ray RayZero = new Ray(Vector3.zero, Vector3.zero);
		
		public static RaycastResult RaycastResultInvalid = new RaycastResult() {
			gameObject = null,
			depth = -1,
		};

		public static DateTime DateTimeNull = new DateTime(1970, 1, 1, 0, 0, 0);
	}

}
