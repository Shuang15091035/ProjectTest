using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace uapp {
	
	public class UiBehaviour : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler, IPointerClickHandler, IScrollHandler {

		protected virtual void onPointerDown(PointerEventData eventData) {}
		protected virtual void onPointerMove(PointerEventData eventData) {}
		protected virtual void onPointerUp(PointerEventData eventData) {}
		protected virtual void onPointerClick(PointerEventData eventData) {}
		protected virtual void onScroll(PointerEventData eventData) {}

		public void OnPointerDown(PointerEventData eventData) {
			onPointerDown(eventData);
		}

		public void OnDrag(PointerEventData eventData) {
			onPointerMove(eventData);
		}

		public void OnPointerUp(PointerEventData eventData) {
			onPointerUp(eventData);
		}

		public void OnPointerClick(PointerEventData eventData) {
			onPointerClick(eventData);
		}

		public void OnScroll(PointerEventData eventData) {
			onScroll(eventData);
		}

	}

}
