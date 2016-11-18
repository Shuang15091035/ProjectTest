/**
 * @file Guard.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-9-7
 * @brief
 */
using UnityEngine;
using System;
using System.Collections;

namespace uapp {

	public static class Guard {

		public static bool Enabled = false;

		public delegate void Code();

		public static void Run(Code code) {
			if (Debug.isDebugBuild && Enabled) {
				try {
					code();
				} catch (Exception e) {
					Debug.LogError(e.Message);
				}
			} else {
				code();
			}
		}

	}
}

