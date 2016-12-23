using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class Triangle {

		public static bool IsPointInsideXZ(Vector3 A, Vector3 B, Vector3 C, Vector3 P) {
			var ax = C.x - B.x;
			var ay = C.z - B.z;
			var bx = A.x - C.x;
			var by = A.z - C.z;
			var cx = B.x - A.x;
			var cy = B.z - A.z;
			var apx = P.x - A.x;
			var apy = P.z - A.z;
			var bpx = P.x - B.x;
			var bpy = P.z - B.z;
			var cpx = P.x - C.x;
			var cpy = P.z - C.z;

			var aCROSSbp = ax * bpy - ay * bpx;
			var cCROSSap = cx * apy - cy * apx;
			var bCROSScp = bx * cpy - by * cpx;

//			return ((aCROSSbp >= 0.0f) && (bCROSScp >= 0.0f) && (cCROSSap >= 0.0f));
			return ((aCROSSbp > 0.0f) && (bCROSScp > 0.0f) && (cCROSSap > 0.0f));
		}
	}
}
