using UnityEngine;
using System.Collections;

namespace project {
	
	public class Heights {

		public static float Gap = 0.01f;
		public static float PlanBackground = -0.1f;
		public static float Ground = 0.0f;
		public static float FloorDefault = Ground + Gap;
		public static float WallLine = FloorDefault + Gap;
		public static float WallItem = WallLine + Gap;
		public static float FloorPoint = WallItem + Gap;
		public static float FloorAreaText = FloorPoint + Gap;
		public static float WallLengthText = FloorAreaText;
		public static float WallDefault = 2.8f;
	}
}
