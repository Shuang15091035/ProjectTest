using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class RectUtils {

		public static bool SmallerThan(Rect l, Rect r) {
			return l.width < r.width || l.height < r.height;
		}

	}

}
