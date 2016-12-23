using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class Lib {

		public static GameObject GlobalObject {
			get {
				var name = "uapp.object";
				var obj = GameObject.Find(name);
				if (obj == null) {
					obj = new GameObject(name);
				}
				return obj;
			}
		}
	}
}
