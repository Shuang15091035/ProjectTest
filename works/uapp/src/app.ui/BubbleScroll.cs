using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace uapp {
	
	public class BubbleScroll : MonoBehaviour, IScrollHandler {
	
		public void OnScroll(PointerEventData eventData) {
			ExecuteEvents.ExecuteHierarchy(transform.parent.gameObject, eventData, ExecuteEvents.scrollHandler);
		}
	}
}
