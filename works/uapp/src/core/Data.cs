using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class Data {

		public static int IntBytes = 4;
		public static int FloatBytes = 4;
		public static int Vector2Bytes = FloatBytes * 2;
		public static int Vector3Bytes = FloatBytes * 3;
		public static int Vector4Bytes = FloatBytes * 4;

		public static int Kilobytes = 1024;
		public static int Megabytes = 1024 * 1024;
		public static int Gigabytes = 1024 * 1024 * 1024;

		public static string GetBytesDisplay(long bytes, string format = "0.##") {
			float fb = (float)bytes;
			float kb = 1024.0f;
			fb /= kb;
			if (fb < kb) {
				return fb.ToString(format) + "KB";
			}
			fb /= kb;
			if (fb < kb) {
				return fb.ToString(format) + "MB";
			}
			fb /= kb;
			if (fb < kb) {
				return fb.ToString(format) + "GB";
			}
			return "Large";
		}

	}
}
