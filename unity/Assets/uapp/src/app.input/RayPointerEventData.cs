using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace uapp {
	
	public class RayPointerEventData : PointerEventData {

		private Ray mRay;

		public RayPointerEventData(EventSystem eventSystem) : base(eventSystem) {
		
		}

		public Ray Ray {
			get {
				return mRay;
			}
			set {
				mRay = value;
			}
		}

	}
}
