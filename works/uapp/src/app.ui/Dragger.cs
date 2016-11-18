using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;

namespace uapp {
	
	public class Dragger : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {

		public delegate GameObject OnPickDelegate(GameObject dragItem);
		public delegate void OnDropDelegate();

		private RectTransform mCanvas;
		private RectTransform mDragObject;
		private Vector2 mOffset;
		private OnPickDelegate mOnPick;
		private OnDropDelegate mOnDrop;

		public OnPickDelegate OnPick {
			get {
				return mOnPick;
			}
			set {
				mOnPick = value;
			}
		}

		public OnDropDelegate OnDrop {
			get {
				return mOnDrop;
			}
			set {
				mOnDrop = value;
			}
		}

		void Start() {
			var canvas = UiUtils.FindCanvas(gameObject);
			if (canvas == null) {
				throw new Exception("[Dragger] Canvas not found.");
			}
			mCanvas = canvas.gameObject.GetComponent<RectTransform>();
		}

		public void OnBeginDrag(PointerEventData eventData) {
			Vector2 localPoint = Vector2.zero;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas, eventData.position, eventData.enterEventCamera, out localPoint)) {
				GameObject dragObject = null;
				if (mOnPick != null) {
					dragObject = mOnPick(gameObject);
				}
				if (dragObject == null) {
					dragObject = GameObject.Instantiate(gameObject, mCanvas) as GameObject;
				}
				mDragObject = dragObject.gameObject.GetComponent<RectTransform>();
				if (mDragObject == null) {
					throw new Exception("[Dragger] RectTransform not found in drag object.");
				}
				mOffset = mDragObject.anchoredPosition - localPoint;
			}
		}

		public void OnDrag(PointerEventData eventData) {
			if (mDragObject == null) {
				return;
			}
			Vector2 localPoint = Vector2.zero;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(mCanvas, eventData.position, eventData.enterEventCamera, out localPoint)) {
				mDragObject.anchoredPosition = localPoint + mOffset;
			}
		}

		public void OnEndDrag(PointerEventData eventData) {
			if (mDragObject != null) {
				GameObject.Destroy(mDragObject.gameObject);
				mDragObject = null;
				if (mOnDrop != null) {
					mOnDrop();
				}
			}
			mOffset = Vector2.zero;
		}

	}

}
