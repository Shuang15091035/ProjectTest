/**
 * @file UiScreen.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2016-8-17
 * @brief
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	/**
	 * 
	 */
	public class UiScreen : UiBehaviour {

		private static UiScreen instance;
		private List<BaseBehaviour> mBehaviours;

		public static UiScreen Instance {
			get {
				return instance;
			}
		}

		void Awake() {
			instance = this;
//			registerEvents(gameObject);
		}

		public void RegisterEvents(BaseBehaviour behaviour) {
			if (behaviour == null) {
				return;
			}
			if (mBehaviours == null) {
				mBehaviours = new List<BaseBehaviour>();
			}
			if (mBehaviours.Contains(behaviour)) {
				return;
			}
			mBehaviours.Add(behaviour);
		}

		public void UnregisterEvents(BaseBehaviour behaviour) {
			if (behaviour == null) {
				return;
			}
			if (mBehaviours == null) {
				return;
			}
			mBehaviours.Remove(behaviour);
		}

//		private void registerEvents(GameObject gameObject) {
//			if (gameObject == null) {
//				return;
//			}
//			if (mBehaviours == null) {
//				mBehaviours = new List<BaseBehaviour>();
//			}
//			var behaviour = gameObject.GetComponent<BaseBehaviour>();
//			if (behaviour != null) {
//				mBehaviours.Add(behaviour);
//			}
//			foreach (Transform childTransform in gameObject.transform) {
//				GameObject childObject = childTransform.gameObject;
//				registerEvents(childObject);
//			}
//		}

		protected override void onPointerDown(PointerEventData eventData) {
			base.onPointerDown(eventData);
			if (mBehaviours == null) {
				return;
			}
			foreach (var behaviour in mBehaviours) {
				if (behaviour.gameObject.activeInHierarchy) {
					behaviour.ScreenPointerDown(eventData);
				}
			}
		}

		protected override void onPointerMove(PointerEventData eventData) {
			base.onPointerMove(eventData);
			if (mBehaviours == null) {
				return;
			}
			foreach (var behaviour in mBehaviours) {
				if (behaviour.gameObject.activeInHierarchy) {
					behaviour.ScreenPointerMove(eventData);
				}
			}
		}

		protected override void onPointerUp(PointerEventData eventData) {
			base.onPointerUp(eventData);
			if (mBehaviours == null) {
				return;
			}
			foreach (var behaviour in mBehaviours) {
				if (behaviour.gameObject.activeInHierarchy) {
					behaviour.ScreenPointerUp(eventData);
				}
			}
		}

		protected override void onPointerClick(PointerEventData eventData) {
			base.onPointerClick(eventData);
			if (mBehaviours == null) {
				return;
			}
			foreach (var behaviour in mBehaviours) {
				if (behaviour.gameObject.activeInHierarchy) {
					behaviour.ScreenPointerClick(eventData);
				}
			}
		}

		protected override void onScroll(PointerEventData eventData) {
			base.onScroll(eventData);
			if (mBehaviours == null) {
				return;
			}
			foreach (var behaviour in mBehaviours) {
				if (behaviour.gameObject.activeInHierarchy) {
					behaviour.ScreenScroll(eventData);
				}
			}
		}

	}

}

