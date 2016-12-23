/**
 * @file CoreUtils.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace uapp {

	public static class CoreUtils {

		public static void Delete(IObject obj) {
			if (obj == null) {
				return;
			}
			obj.OnDelete();
		}

		public static void DeleteList<T>(IList<T> list) where T : IObject {
			if (list == null) {
				return;
			}
			foreach (var i in list) {
				Delete(i);
			}
			list.Clear();
		}

		public static int ParseInt(string str, int defaultValue) {
			int result = defaultValue;
			if (int.TryParse(str, out result)) {
				return result;
			}
			return defaultValue;
		}

		public static float ParseFloat(string str, float defaultValue) {
			float result = defaultValue;
			if (float.TryParse(str, out result)) {
				return result;
			}
			return defaultValue;
		}

		public static bool ObjectEquals(object obj1, object obj2) {
			if (obj1 == null) {
				if (obj2 == null) {
					return true;
				}
				return false;
			}
			if (obj2 == null) {
				return false;
			}
			return obj1.Equals(obj2);
		}

		public static bool ListAddUnique<T>(IList<T> list, T item) {
			if (list == null || list.Contains(item)) {
				return false;
			}
			list.Add(item);
			return true;
		}

		public static string ExecuteCommand(string filename, string arguments) {
			var processInfo = new ProcessStartInfo(filename, arguments);
			processInfo.CreateNoWindow = true;
			processInfo.UseShellExecute = false;
			processInfo.RedirectStandardOutput = true;
			processInfo.RedirectStandardError = true;
			var process = Process.Start(processInfo);
			process.WaitForExit();

			// debug
			//			var output = process.StandardOutput.ReadToEnd();
			//			UnityEngine.Debug.Log("[COMMAND] " + filename + " " + arguments);
			//			if (!StringUtils.IsNullOrBlank(output)) {
			//				UnityEngine.Debug.LogWarning("[OUTPUT ] " + output);
			//			}
			var error = process.StandardError.ReadToEnd();
			// debug
			//			if (!StringUtils.IsNullOrBlank(error)) {
			//				UnityEngine.Debug.LogError("[ERROR  ] " + error);
			//			}

			process.Close();
			return error;
		}
	}

	public static class ArrayUtils {
		
		public static int IndexOf<T>(T[] array, T value) {
			if (array == null || array.Length == 0) {
				return -1;
			}
			int index = 0;
			foreach (T item in array) {
				if (EqualityComparer<T>.Default.Equals(item, value)) {
					return index;
				}
				index++;
			}
			return -1;
		}

		public static bool Contains<T>(T[] array, T value) {
			return IndexOf(array, value) >= 0;
		}
	}

	public static class TypeUtils {

		public static bool Equals(Type t1, Type t2) {
			// .NET
			//			return t1 == t2;
			// Mono version
			return t1.FullName == t2.FullName;
		}

		public static bool Equals<T1, T2>() {
			return Equals(typeof(T1), typeof(T2));
		}
	}

	public enum ClockDirection {
		Collinear,
		Clockwise,
		CounterClockwise,
	}

	public static class MathUtils {

		public const float Epsilon = 0.001f;

		public static bool FloatEquals(float l, float r) {
			return Mathf.Abs(l - r) < Epsilon;
		}

		public static bool IsPowerOf2(int x) {
			return ((x != 0) && (x & (x - 1)) == 0);
		}

		public static int Log2(int x) {
			if (x == 1) {
				return 0;
			} else {
				return 1 + Log2(x >> 1);
			}
		}

		public static int NextPowerOf2(int x) {
			var k = x;
			if (k == 0) {
				return 1;
			}

			k--;
			const int b = sizeof(int) * 8;
			for (int i = 1; i < b; i <<= 1) {
				k |= k >> i;
			}

			return k + 1;
		}

		public static int PreviousPowerOf2(int x) {
			return 1 << Log2(x);
		}

		public static int NearestPowerOf2(int x) {
			var np = NextPowerOf2(x);
			var pp = PreviousPowerOf2(x);
			return (np - x) < (x - pp) ? np : pp;
		}

		public static float Deg2Rad(float degrees) {
			return degrees * Mathf.Deg2Rad;
		}

		public static float Rad2Deg(float radians) {
			return radians * Mathf.Rad2Deg;
		}

		public static ClockDirection ClockDir(float lx, float ly, float rx, float ry) {
			float c = (ly * rx) - (lx * ry);
			if (c == 0.0f) {
				return ClockDirection.Collinear;
			}
			if (c > 0.0f) {
				return ClockDirection.Clockwise;
			}
			return ClockDirection.CounterClockwise;
		}

		public static float Radians(float lx, float ly, float rx, float ry) {
			float d = (lx * rx) + (ly * ry);
			float r = Mathf.Acos(d);
			if (ClockDir(lx, ly, rx, ry) == ClockDirection.CounterClockwise) {
				r = -r;
			}
			return r;
		}

		public static float Degrees(float lx, float ly, float rx, float ry) {
			return Rad2Deg(Radians(lx, ly, rx, ry));
		}

		public static float ManhattanDistance(float x0, float y0, float x1, float y1) {
			return Mathf.Abs(x0 - x1) + Mathf.Abs(y0 - y1);
		}

		public static float ManhattanDistance(Vector3 p0, Vector3 p1) {
			return Mathf.Abs(p0.x - p1.x) + Mathf.Abs(p0.y - p1.y) + Mathf.Abs(p0.z - p1.z);
		}

		public static float SquareLength(float x, float y, float z) {
			return (x * x) + (y * y) + (z * z);
		}

		public static float SquareLength(Vector3 v) {
			return SquareLength(v.x, v.y, v.z);
		}

		public static float Length(float x, float y, float z) {
			return Mathf.Sqrt(SquareLength(x, y, z));
		}

		public static float Length(Vector3 v) {
			return Length(v.x, v.y, v.z);
		}

		public static float SquareDistance(float x0, float y0, float z0, float x1, float y1, float z1) {
			return SquareLength(x1 - x0, y1 - y0, z1 - z0);
		}

		public static float SquareDistance(Vector3 p0, Vector3 p1) {
			return  SquareDistance(p0.x, p0.y, p0.z, p1.z, p1.y, p1.z);
		}

		public static float Distance(float x0, float y0, float z0, float x1, float y1, float z1) {
			return  Mathf.Sqrt(SquareDistance(x0, y0, z0, x1, y1, z1));
		}

		public static float Distance(Vector3 p0, Vector3 p1) {
			return  Distance(p0.x, p0.y, p0.z, p1.x, p1.y, p1.z);
		}

		public static float AreaOfTriangle(float x0, float y0, float x1, float y1, float x2, float y2) {
			float area = Mathf.Abs((x1 * (y2 - y0)) + (x2 * (y0 - y1)) + (x0 * (y1 - y2))) / 2.0f;
			return area;
		}

		public static float AreaOfTriangle(Vector3 A, Vector3 B, Vector3 C) {
			var AB = B - A;
			var AC = C - A;
			float area = Length(Vector3.Cross(AB, AC)) * 0.5f;
			return area;
		}

		public static Vector3 Lerp(Vector3 from, Vector3 to, float t) {
			var tsf = to - from;
			var tsfmt = tsf * t;
			return from + tsfmt;
		}

		public static float SquareDistancePointToLineSegment(Vector3 point, Vector3 start, Vector3 end, out float u, out Vector3 upoint) {
			u = 0.0f;
			upoint = Vector3.zero;
			float lx0 = start.x;
			float ly0 = start.y;
			float lz0 = start.z;
			float lx1 = end.x;
			float ly1 = end.y;
			float lz1 = end.z;
			float px = point.x;
			float py = point.y;
			float pz = point.z;

			float lx = lx1 - lx0;
			float ly = ly1 - ly0;
			float lz = lz1 - lz0;
			float som = lx * lx + ly * ly + lz * lz;
			if (som == 0.0f) {
				return SquareDistance(lx0, ly0, lz0, px, py, pz);
			}
			u = ((px - lx0) * lx + (py - ly0) * ly + (pz - lz0) * lz) / som;
			if (u > 1.0f) {
				u = 1.0f;
			} else if (u < 0.0f) {
				u = 0.0f;
			}
			float x = lx0 + u * lx;
			float y = ly0 + u * ly;
			float z = lz0 + u * lz;
			upoint.x = x;
			upoint.y = y;
			upoint.z = z;
			return SquareDistance(x, y, z, px, py, pz);

//			var l = end - start;
//			if (l == Vector3.zero) {
//				return SquareDistance(point, start);
//			}
//			var ld = SquareLength(l);
//			var u = Vector3.Dot(point - start, l);
//			u *= u;
//			u /= ld;
//			if (u > 1.0f) {
//				u = 1.0f;
//			} else if (u < 0.0f) {
//				u = 0.0f;
//			}
//			var p = start + u * l.normalized;
//			return SquareDistance(point, p);
		}

		public static float GetScalar(Vector3 A, Vector3 B) {
			var AB = Vector3.Dot(A, B);
			var B2 = SquareLength(B);
			if (B2 == 0.0f) {
				return 0.0f;
			}
			return AB / B2;
		}

		public static bool LineIntersect(Vector3 line1Start, Vector3 line1End, Vector3 line2Start, Vector3 line2End, out Vector3 point) {
			point = Vector3.zero;
			var p = line1Start;
			var r = line1End - line1Start;
			var q = line2Start;
			var s = line2End - line2Start;
			var rxs = Vector3.Cross(r, s);
			var qp = q - p;
			var qpxr = Vector3.Cross(qp, r);
			if (rxs == Vector3.zero) {
				if (qpxr == Vector3.zero) {
					// TODO
					return false;
				} else {
					return false;
				}
			} else {
				var u = GetScalar(qpxr, rxs);
				if (u <= 0.0f || u >= 1.0f) {
					return false;
				}
				var qpxs = Vector3.Cross(qp, s);
				var t = GetScalar(qpxs, rxs);
				if (t <= 0.0f || t >= 1.0f) {
					return false;
				}
				// TODO
				point = Lerp(line1Start, line1End, t);
				return true;
			}
		}
	}

	public static class StringUtils {

		public static string Standardize(string str) {
			if (str == null) {
				return null;
			}
			string sta = str.Trim();
			return sta.Replace(" ", "_");
		}

		public static bool IsNullOrBlank(string str) {
			return String.IsNullOrEmpty(str) || str.Trim().Length == 0;
		}

		public static string RemoveFirst(string str, string removal) {
			if (str == null) {
				return null;
			}
			string result = str;
			int index = result.IndexOf(removal);
			if (index < 0) {
				return result;
			}
			int count = removal.Length;
			result = result.Remove(index, count);
			return result;
		}

		public static string RemoveLast(string str, string removal) {
			if (str == null) {
				return null;
			}
			string result = str;
			int index = result.LastIndexOf(removal);
			if (index < 0) {
				return result;
			}
			int count = removal.Length;
			result = result.Remove(index, count);
			return result;
		}

		public static string RemoveAll(string str, string removal) {
			if (str == null) {
				return null;
			}
			string result = str;
			while (true) {
				int index = result.IndexOf(removal);
				if (index < 0) {
					break;
				}
				int count = removal.Length;
				result = result.Remove(index, count);
			}
			return result;
		}

		public static string Quote(string str) {
			if (str == null) {
				return null;
			}
			return "\"" + str + "\"";
		}

		private static StringBuilder SeparateByStringBuilder = new StringBuilder();
		public static string SeparateBy(string str, string separator) {
			if (str == null || separator == null) {
				return str;
			}
			SeparateByStringBuilder.Length = 0;
			foreach (var c in str) {
				SeparateByStringBuilder.Append(c);
				SeparateByStringBuilder.Append(separator);
			}
			SeparateByStringBuilder.Remove(SeparateByStringBuilder.Length - 1, 1);
			return SeparateByStringBuilder.ToString();
		}
	}

	public static class PathUtils {

		public static string RelativePath(string absPath, string relTo, char absSeparator, char relSeparator, char pathSeparator) {
			string[] absDirs = absPath.Split(absSeparator);
			string[] relDirs = relTo.Split(relSeparator);
			int len = absDirs.Length < relDirs.Length ? absDirs.Length : relDirs.Length; 
			int lastCommonRoot = -1;
			int index; 
			for (index = 0; index < len; index++) {
				if (absDirs[index] == relDirs[index]) {
					lastCommonRoot = index;
				} else {
					break;
				}
			} 
			if (lastCommonRoot == -1) {
				//throw new ArgumentException("Paths do not have a common base");
				return relTo;
			} 
			StringBuilder relativePath = new StringBuilder();
			for (index = lastCommonRoot + 1; index < absDirs.Length; index++) {
				if (absDirs[index].Length > 0) {
					relativePath.Append(".." + pathSeparator);
				}
			}
			for (index = lastCommonRoot + 1; index < relDirs.Length - 1; index++) {
				relativePath.Append(relDirs[index] + pathSeparator);
			}
			relativePath.Append(relDirs[relDirs.Length - 1]);
			return relativePath.ToString();
		}

		public static string RelativePath(string absPath, string relTo) {
			return RelativePath(absPath, relTo, '/', '/', '/'); // NOTE unity默认返回都是'/'，跟系统无关，系统返回的路径就跟系统有关
		}
			
		#if UNITY_EDITOR
		public static string GetProjectRelativePath(string path) {
			path = path.Replace("\\", "/"); // NOTE 兼容windows
			return FileUtil.GetProjectRelativePath(path);
		}

		public static string GetAssetFullPath(UnityEngine.Object asset) {
			var assetPath = AssetDatabase.GetAssetPath(asset);
			if (assetPath == null) {
				return null;
			}
			var dataPath = Application.dataPath;
			dataPath = StringUtils.RemoveLast(dataPath, "Assets");
			return System.IO.Path.Combine(dataPath, assetPath);
		}
		#endif
	}

	public static class IntUtils {

		public delegate int OnIntDelegate(int origin);

		public static string Join(int[] ints, string separator, OnIntDelegate onInt) {
			string[] intStrings = new string[ints.Length];
			int i = 0;
			if (onInt == null) {
				foreach (var ii in ints) {
					intStrings[i++] = ii.ToString();
				}
			} else {
				foreach (var ii in ints) {
					var iii = onInt(ii);
					intStrings[i++] = iii.ToString();
				}
			}
			return string.Join(separator, intStrings);
		}
	}

	public static class Vector2Utils {

		public static Vector2 Encode(Vector2 v) {
			return new Vector2(v.y * 19.85f, v.x * 6.18f);
		}
		
		public static Vector2 Decode(Vector2 v) {
			return new Vector2(v.y / 6.18f, v.x / 19.85f);
		}

		public delegate Vector2 OnVectorDelegate(Vector2 origin);

		public static string Join(Vector2[] vectors, string separator, OnVectorDelegate onVector) {
			string[] vectorStrings = new string[vectors.Length * 2];
			int i = 0;
			if (onVector == null) {
				foreach (var vector in vectors) {
					vectorStrings[i++] = vector.x.ToString("0.###");
					vectorStrings[i++] = vector.y.ToString("0.###");
				}
			} else {
				foreach (var vector in vectors) {
					var v = onVector(vector);
					vectorStrings[i++] = v.x.ToString("0.###");
					vectorStrings[i++] = v.y.ToString("0.###");
				}
			}
			return string.Join(separator, vectorStrings);
		}
	}

	public static class Vector3Utils {

		public static float Epsilon = 0.001f;

		public static bool ValueEquals(Vector3 l, Vector3 r) {
			if (!MathUtils.FloatEquals(l.x, r.x) || !MathUtils.FloatEquals(l.y, r.y) || !MathUtils.FloatEquals(l.z, r.z)) {
				return false;
			}
			return true;
		}

		public static bool Approximate(Vector3 l, Vector3 r, float e = 0.001f) {
			if (!(Mathf.Abs(l.x - r.x) > e) || !(Mathf.Abs(l.y - r.y) > e) || !(Mathf.Abs(l.z - r.z) > e)) {
				return false;
			}
			return true;
		}

		public static string Format(float x, float y, float z, string separator = " ", string format = "0.###") {
			var formatText = "{0:" + format + "}" + separator + "{1:" + format + "}" + separator + "{2:" + format + "}";
			return string.Format(formatText, x, y, z);
		}

		public static string Format(Vector3 v, string separator = " ", string format = "0.###") {
			return Format(v.x, v.y, v.z, separator, format);
		}

		public static Vector3 Deformat(string str) {
			return Deformat(str, Vector3.zero);
		}

		public static Vector3 Deformat(string str, Vector3 defaultValue) {
			if (str == null) {
				return defaultValue;
			}
			string[] vs = str.Split(new char[] { ' ' });
			if (vs == null) {
				return Vector3.zero;
			}
			Vector3 v = new Vector3();
			if (vs.Length > 0) {
				v.x = float.Parse(vs[0]);
			}
			if (vs.Length > 1) {
				v.y = float.Parse(vs[1]);
			}
			if (vs.Length > 2) {
				v.z = float.Parse(vs[2]);
			}
			return v;
		}

		public static Vector3 Encode(Vector3 v) {
			return new Vector3(v.y * 19.85f, v.z * 6.18f, v.x * 3.19f);
		}

		public static Vector3 Decode(Vector3 v) {
			return new Vector3(v.z / 3.19f, v.x / 19.85f, v.y / 6.18f);
		}

		public delegate Vector3 OnVectorDelegate(Vector3 origin);

		public static string Join(Vector3[] vectors, string separator, string format = "0.###", OnVectorDelegate onVector = null) {
			string[] vectorStrings = new string[vectors.Length * 3];
			int i = 0;
			if (onVector == null) {
				foreach (var vector in vectors) {
					vectorStrings[i++] = vector.x.ToString(format);
					vectorStrings[i++] = vector.y.ToString(format);
					vectorStrings[i++] = vector.z.ToString(format);
				}
			} else {
				foreach (var vector in vectors) {
					var v = onVector(vector);
					vectorStrings[i++] = v.x.ToString(format);
					vectorStrings[i++] = v.y.ToString(format);
					vectorStrings[i++] = v.z.ToString(format);
				}
			}
			return string.Join(separator, vectorStrings);
		}

		public static Vector3 ToRightHand(Vector3 v) {
			v.z = -v.z;
			return v;
		}
	}

	public static class Vector4Utils {

		public static string Format(float x, float y, float z, float w) {
			return string.Format("{0:0.###} {1:0.###} {2:0.###} {3:0.###}", x, y, z, w);
		}

		public static string Format(Vector4 v) {
			return Format(v.x, v.y, v.z, v.w);
		}

		public static Vector4 Deformat(string str) {
			return Deformat(str, Vector4.zero);
		}

		public static Vector4 Deformat(string str, Vector4 defaultValue) {
			if (str == null) {
				return defaultValue;
			}
			string[] vs = str.Split(new char[] { ' ' });
			if (vs == null) {
				return Vector4.zero;
			}
			Vector4 v = new Vector4();
			if (vs.Length > 0) {
				v.x = float.Parse(vs[0]);
			}
			if (vs.Length > 1) {
				v.y = float.Parse(vs[1]);
			}
			if (vs.Length > 2) {
				v.z = float.Parse(vs[2]);
			}
			if (vs.Length > 3) {
				v.w = float.Parse(vs[3]);
			}
			return v;
		}

		public static Vector4 Encode(Vector4 v) {
			return new Vector4(v.y * 19.92f, v.z * 3.19f, v.x * 19.85f, v.w * 6.18f);
		}

		public static Vector4 Decode(Vector4 v) {
			return new Vector4(v.z / 19.85f, v.x / 19.92f, v.y / 3.19f, v.w / 6.18f);
		}

		public delegate Vector4 OnVectorDelegate(Vector4 origin);

		public static string Join(Vector4[] vectors, string separator, string format = "0.###", OnVectorDelegate onVector = null) {
			string[] vectorStrings = new string[vectors.Length * 4];
			int i = 0;
			if (onVector == null) {
				foreach (var vector in vectors) {
					vectorStrings[i++] = vector.x.ToString(format);
					vectorStrings[i++] = vector.y.ToString(format);
					vectorStrings[i++] = vector.z.ToString(format);
					vectorStrings[i++] = vector.w.ToString(format);
				}
			} else {
				foreach (var vector in vectors) {
					var v = onVector(vector);
					vectorStrings[i++] = v.x.ToString(format);
					vectorStrings[i++] = v.y.ToString(format);
					vectorStrings[i++] = v.z.ToString(format);
					vectorStrings[i++] = v.w.ToString(format);
				}
			}
			return string.Join(separator, vectorStrings);
		}

		public static string Join3(Vector4[] vectors, string separator, string format = "0.###", OnVectorDelegate onVector = null) {
			string[] vectorStrings = new string[vectors.Length * 3];
			int i = 0;
			if (onVector == null) {
				foreach (var vector in vectors) {
					vectorStrings[i++] = vector.x.ToString(format);
					vectorStrings[i++] = vector.y.ToString(format);
					vectorStrings[i++] = vector.z.ToString(format);
				}
			} else {
				foreach (var vector in vectors) {
					var v = onVector(vector);
					vectorStrings[i++] = v.x.ToString(format);
					vectorStrings[i++] = v.y.ToString(format);
					vectorStrings[i++] = v.z.ToString(format);
				}
			}
			return string.Join(separator, vectorStrings);
		}

		public static bool IsDefaultTilingOffset(Vector4 tilingOffset) {
			if (tilingOffset.x != 1.0f) {
				return false;
			}
			if (tilingOffset.y != 1.0f) {
				return false;
			}
			if (tilingOffset.z != 0.0f) {
				return false;
			}
			if (tilingOffset.w != 0.0f) {
				return false;
			}
			return true;
		}

		public static Vector4 ToRightHand(Vector4 v) {
			v.x = -v.x;
			return v;
		}
	}

	public static class QuaternionUtils {

		public static string Format(float w, float x, float y, float z, string separator = " ", string format = "0.###") {
			return string.Format("{0:" + format + "}" + separator + "{1:" + format + "}" + separator + "{2:" + format + "}" + separator + "{3:" + format + "}", w, x, y, z);
		}

		public static string Format(Quaternion q, string separator = " ", string format = "0.###") {
			return Format(q.w, q.x, q.y, q.z, separator, format);
		}

		public static Quaternion Deformat(string str) {
			if (str == null) {
				return Quaternion.identity;
			}
			string[] qs = str.Split(new char[] { ' ' });
			if (qs == null) {
				return Quaternion.identity;
			}
			Quaternion q = new Quaternion();
			if (qs.Length > 0) {
				q.w = float.Parse(qs[0]);
			}
			if (qs.Length > 1) {
				q.x = float.Parse(qs[1]);
			}
			if (qs.Length > 2) {
				q.y = float.Parse(qs[2]);
			}
			if (qs.Length > 3) {
				q.z = float.Parse(qs[3]);
			}
			return q;
		}

		public static string Encode(Quaternion q) {
			return Format(q.z, q.w * 2.0f, q.x * 3.0f, q.y * 4.0f);
		}

		public static Quaternion Decode(string str) {
			Quaternion sq = Deformat(str);
			Quaternion q = new Quaternion();
			q.w = sq.x / 2.0f;
			q.x = sq.y / 3.0f;
			q.y = sq.z / 4.0f;
			q.z = sq.w;
			return q;
		}

		public static Quaternion ToRightHand(Quaternion q) {
			q.x = -q.x;
			q.y = -q.y;
			return q;
		}

		public static void ToRightHandAngleAxis(Quaternion q, out float angle, out Vector3 axis) {
			Quaternion rq = ToRightHand(q);
			rq.ToAngleAxis(out angle, out axis);
		}
	}

	public static class Matrix4Utils {

		//		public static string Format(float w, float x, float y, float z) {
		//			return string.Format("{0:0.000} {1:0.000} {2:0.000} {3:0.000}", w, x, y, z);
		//		}
		//
		//		public static string Format(Quaternion q) {
		//			return Format(q.w, q.x, q.y, q.z);
		//		}

		public static Matrix4x4 Deformat(string str) {
			if (str == null) {
				return Matrix4x4.identity;
			}
			string[] qs = str.Split(new char[] { ' ' });
			if (qs == null) {
				return Matrix4x4.identity;
			}
			Matrix4x4 m = new Matrix4x4();
			if (qs.Length > 0) {
				m.m00 = float.Parse(qs[0]);
			}
			if (qs.Length > 1) {
				m.m01 = float.Parse(qs[1]);
			}
			if (qs.Length > 2) {
				m.m02 = float.Parse(qs[2]);
			}
			if (qs.Length > 3) {
				m.m03 = float.Parse(qs[3]);
			}
			if (qs.Length > 4) {
				m.m10 = float.Parse(qs[4]);
			}
			if (qs.Length > 5) {
				m.m11 = float.Parse(qs[5]);
			}
			if (qs.Length > 6) {
				m.m12 = float.Parse(qs[6]);
			}
			if (qs.Length > 7) {
				m.m13 = float.Parse(qs[7]);
			}
			if (qs.Length > 8) {
				m.m20 = float.Parse(qs[8]);
			}
			if (qs.Length > 9) {
				m.m21 = float.Parse(qs[9]);
			}
			if (qs.Length > 10) {
				m.m22 = float.Parse(qs[10]);
			}
			if (qs.Length > 11) {
				m.m23 = float.Parse(qs[11]);
			}
			if (qs.Length > 12) {
				m.m30 = float.Parse(qs[12]);
			}
			if (qs.Length > 13) {
				m.m31 = float.Parse(qs[13]);
			}
			if (qs.Length > 14) {
				m.m32 = float.Parse(qs[14]);
			}
			if (qs.Length > 15) {
				m.m33 = float.Parse(qs[15]);
			}
			return m;
		}

		public static Vector3 GetPosition(Matrix4x4 matrix) {
			var x = matrix.m03;
			var y = matrix.m13;
			var z = matrix.m23;
			return new Vector3(x, y, z);
		}

		public static Quaternion GetRotation(Matrix4x4 matrix) {
			var qw = Mathf.Sqrt(1f + matrix.m00 + matrix.m11 + matrix.m22) / 2;
			var w = 4 * qw;
			var qx = (matrix.m21 - matrix.m12) / w;
			var qy = (matrix.m02 - matrix.m20) / w;
			var qz = (matrix.m10 - matrix.m01) / w;
			return new Quaternion(qx, qy, qz, qw);
		}

		public static Vector3 GetScale(Matrix4x4 matrix) {
			var x = Mathf.Sqrt(matrix.m00 * matrix.m00 + matrix.m01 * matrix.m01 + matrix.m02 * matrix.m02);
			var y = Mathf.Sqrt(matrix.m10 * matrix.m10 + matrix.m11 * matrix.m11 + matrix.m12 * matrix.m12);
			var z = Mathf.Sqrt(matrix.m20 * matrix.m20 + matrix.m21 * matrix.m21 + matrix.m22 * matrix.m22);
			return new Vector3(x, y, z);
		}
	}

	public static class ColorUtils {

		public static Color FromRbga(uint rgba) {
			float r = (float)((rgba & 0xff000000) >> 24) / 255.0f;
			float g = (float)((rgba & 0x00ff0000) >> 16) / 255.0f;
			float b = (float)((rgba & 0x0000ff00) >> 8 ) / 255.0f;
			float a = (float) (rgba & 0x000000ff) / 255.0f;
			return new Color(r, g, b, a);
		}

		public static uint ToRgba(Color color) {
			uint cr = (uint)(color.r * 255.0f);
			uint cg = (uint)(color.g * 255.0f);
			uint cb = (uint)(color.b * 255.0f);
			uint ca = (uint)(color.a * 255.0f);
			return (cr << 24) | (cg << 16) | (cb << 8) | ca;
		}

		private static uint RgbaGetA(uint rgba) {
			return rgba & 0x000000ff;
		}

		private static uint RgbaSetA(uint rgba, uint a) {
			rgba &= 0xffffff00;
			rgba |= a;
			return rgba;
		}

		public static Color Inverse(Color color) {
			var rgba = ToRgba(color);
			var a = RgbaGetA(rgba);
			rgba = ~rgba;
			rgba = RgbaSetA(rgba, a);
			return FromRbga(rgba);
		}

		public static bool isWhite(float r, float g, float b, float a) {
			return r == 1.0f && g == 1.0f && b == 1.0f;
		}

		public static bool isWhite(Color c) {
			return isWhite(c.r, c.g, c.b, c.a);
		}
		
		public static string Format(float r, float g, float b, float a) {
			return string.Format("{0:0.##} {1:0.##} {2:0.##} {3:0.##}", r, g, b, a);
		}
		
		public static string Format(Color c) {
			return Format(c.r, c.g, c.b, c.a);
		}
		
		public static Color Deformat(string str) {
			return Deformat(str, Color.white);
		}
		
		public static Color Deformat(string str, Color defaultValue) {
			if (str == null) {
				return defaultValue;
			}
			string[] vs = str.Split(new char[] { ' ' });
			if (vs == null) {
				return Vector4.zero;
			}
			Color c = new Color();
			if (vs.Length > 0) {
				c.r = float.Parse(vs[0]);
			}
			if (vs.Length > 1) {
				c.g = float.Parse(vs[1]);
			}
			if (vs.Length > 2) {
				c.b = float.Parse(vs[2]);
			}
			if (vs.Length > 3) {
				c.a = float.Parse(vs[3]);
			}
			return c;
		}
		
		public static string Encode(Color c) {
			return Format(c.r, c.g * 2.0f, c.b * 3.0f, c.a * 4.0f);
		}
		
		public static Color Decode(string str) {
			Color sv = Deformat(str);
			Color v = new Color();
			v.r = sv.b / 3.0f;
			v.g = sv.r;
			v.b = sv.g / 2.0f;
			v.a = sv.a / 4.0f;
			return v;
		}

		public static string Join(Color[] colors, string separator) {
			string[] colorStrings = new string[colors.Length * 4];
			int i = 0;
			foreach (var color in colors) {
				colorStrings[i++] = color.r.ToString("0.###");
				colorStrings[i++] = color.g.ToString("0.###");
				colorStrings[i++] = color.b.ToString("0.###");
				colorStrings[i++] = color.a.ToString("0.###");
			}
			return string.Join(separator, colorStrings);
		}
	}

	public static class BoundsUtils {

		public static Vector3 BottomCenter(Bounds bounds) {
			return new Vector3(bounds.center.x, bounds.min.y, bounds.center.z);
		}

		public static Bounds Scale(Bounds bounds, Vector3 scale) {
			var b = bounds;
			var center = bounds.center;
			b.center = new Vector3(center.x * scale.x, center.y * scale.y, center.z * scale.z);
			var extents = bounds.extents;
			b.extents = new Vector3(extents.x * scale.x, extents.y * scale.y, extents.z * scale.z);
			return b;
		}
	}


	public class TextureUtils {

		public static Texture2D Inverse(Texture2D texture) {
			if (texture == null) {
				return null;
			}
			var colors = texture.GetPixels();
			if (colors == null || colors.Length == 0) {
				return null;
			}

			for (var i = 0; i < colors.Length; i++) {
				var color = colors[i];
				colors[i] = ColorUtils.Inverse(color);
			}

			var newTexture = new Texture2D(texture.width, texture.height);
			newTexture.SetPixels(colors);
			return newTexture;
		}

		public static IFile ConvertPVR(IFile src) {
			#if UNITY_EDITOR_OSX
			var texturetool = "/Applications/Xcode.app/Contents/Developer/Platforms/iPhoneOS.platform/Developer/usr/bin/texturetool";
			var opt = "--channel-weighting-linear";
			//var bpp = "--bits-per-pixel-2"
			var bpp = "--bits-per-pixel-4";
			var generateMipmap = true;
			var inputFile = src.RealPath;
			var outputFile = StringUtils.RemoveLast(inputFile, src.Ext) + ".pvr";
			string arguments = null;
			if (generateMipmap) {
				arguments = "-f PVR -m -e PVRTC " + opt + " " + bpp + " -o " + outputFile + " " + inputFile;
			} else {
				arguments = "-f PVR -e PVRTC " + opt + " " + bpp + " -o " + outputFile + " " + inputFile;
			}
			var error = CoreUtils.ExecuteCommand(texturetool, arguments);
			if (!StringUtils.IsNullOrBlank(error)) {
				UnityEngine.Debug.LogError("[" + src.Path + "] " + error);
			}
			return File.FilePath(outputFile);
			#else
			return null;
			#endif
		}

		#if UNITY_EDITOR
		private delegate void getWidthAndHeightDelegate(TextureImporter importer, ref int width, ref int height);
		private static getWidthAndHeightDelegate getWidthAndHeight;

		public static Size GetOriginSize(Texture texture) {
			var path = AssetDatabase.GetAssetPath(texture);
			var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
			if (textureImporter == null) {
				throw new Exception("Failed to get Texture importer for " + path);
			}
			if (getWidthAndHeight == null) {
				var method = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
				getWidthAndHeight = Delegate.CreateDelegate(typeof(getWidthAndHeightDelegate), null, method) as getWidthAndHeightDelegate;
			}
			var size = new Size();
			getWidthAndHeight(textureImporter, ref size.width, ref size.height);
			return size;
		}

		public static TextureImporterFormat GetOriginFormat(Texture texture) {
			var path = AssetDatabase.GetAssetPath(texture);
			var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
			if (textureImporter == null) {
				throw new Exception("Failed to get Texture importer for " + path);
			}
			int maxTextureSize = 0;
			TextureImporterFormat textureImporterFormat = TextureImporterFormat.AutomaticCompressed;
			textureImporter.GetPlatformTextureSettings("iPhone", out maxTextureSize, out textureImporterFormat);
			return textureImporterFormat;
		}
		#endif
	}
}

