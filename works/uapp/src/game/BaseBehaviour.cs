/**
 * @file BaseBehaviour.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace uapp {

	public class BaseBehaviour : MonoBehaviour {

		private KeyEvent mKeyEvent = new KeyEvent();

		protected virtual void OnAwake() {}
		protected virtual void OnStart() {}
		protected virtual void OnUpdate() {}
		protected virtual void OnScreenMouseDown(MouseEvent e) {}
		protected virtual void OnScreenMouseMove(MouseEvent e) {}
		protected virtual void OnScreenMouseUp(MouseEvent e) {}
		protected virtual void OnScreenMouseScroll(MouseEvent e) {}
		protected virtual void OnScreenMouseClick(MouseEvent e) {}
		protected virtual void OnScreenMouseDoubleClick(MouseEvent e) {}
		protected virtual void OnKey(KeyEvent e) {}
			
		void Awake() {
			Guard.Run(OnAwake);
		}

		void Start() {
			UiScreen.Instance.RegisterEvents(this);
			Guard.Run(OnStart);
		}

		void OnDestroy() {
			UiScreen.Instance.UnregisterEvents(this);
		}
			
		void Update() {

			if (Cursor.lockState == CursorLockMode.Locked) {
				handleMouseMove();
			} else {
				// mouse move
				if (!Input.GetMouseButton(0) && !Input.GetMouseButton(1) && !Input.GetMouseButton(2)) {
					float deltaX = Input.GetAxis("Mouse X");
					float deltaY = Input.GetAxis("Mouse Y");
					if (deltaX != 0.0f || deltaY != 0.0f) {
						MouseEvent e = new MouseEvent();
						e.button = Buttons.None;
						e.position = Input.mousePosition;
						e.delta = new Vector3(deltaX, deltaY, 0.0f);
						Guard.Run(() => OnScreenMouseMove(e));
					}
				}
			}

			// key
			Guard.Run(() => OnKey(mKeyEvent));
			Guard.Run(OnUpdate);
		}

		public void ScreenPointerDown(PointerEventData eventData) {
			// mouse down
			int mouseDown = Buttons.None;
			if (Input.GetMouseButtonDown(0)) {
				mouseDown = Flags.Add(mouseDown, Buttons.Left);
			}
			if (Input.GetMouseButtonDown(1)) {
				mouseDown = Flags.Add(mouseDown, Buttons.Right);
			}
			if (mouseDown > 0) {
				MouseEvent e = new MouseEvent();
				e.button = mouseDown;
				e.position = Input.mousePosition;
				Guard.Run(() => OnScreenMouseDown(e));
			}
		}

		public void ScreenPointerMove(PointerEventData eventData) {
			// mouse move
			int mouseMove = Buttons.None;
			if (Input.GetMouseButton(0)) {
				mouseMove = Flags.Add(mouseMove, Buttons.Left);
			}
			if (Input.GetMouseButton(1)) {
				mouseMove = Flags.Add(mouseMove, Buttons.Right);
			}
			if (mouseMove > 0) {
				MouseEvent e = new MouseEvent();
				e.button = mouseMove;
				e.position = Input.mousePosition;
				// TODO 与使用以下方法获取的delta数值有区别，可能更适用，看实际情况来调整
				float deltaX = Input.GetAxis("Mouse X");
				float deltaY = Input.GetAxis("Mouse Y");
				e.delta = new Vector3(deltaX, deltaY, 0.0f);
//				e.delta = new Vector3(eventData.delta.x, eventData.delta.y, 0.0f);
				Guard.Run(() => OnScreenMouseMove(e));
			}
		}

		private void handleMouseMove() {
			// mouse move
			int mouseMove = Buttons.None;
			if (Input.GetMouseButton(0)) {
				mouseMove = Flags.Add(mouseMove, Buttons.Left);
			}
			if (Input.GetMouseButton(1)) {
				mouseMove = Flags.Add(mouseMove, Buttons.Right);
			}
			if (mouseMove > 0) {
				MouseEvent e = new MouseEvent();
				e.button = mouseMove;
				e.position = Input.mousePosition;
				float deltaX = Input.GetAxis("Mouse X");
				float deltaY = Input.GetAxis("Mouse Y");
				e.delta = new Vector3(deltaX, deltaY, 0.0f);
				Guard.Run(() => OnScreenMouseMove(e));
			}
		}

		public void ScreenPointerUp(PointerEventData eventData) {
			// mouse up
			int mouseUp = Buttons.None;
			if (Input.GetMouseButtonUp(0)) {
				mouseUp = Flags.Add(mouseUp, Buttons.Left);
			}
			if (Input.GetMouseButtonUp(1)) {
				mouseUp = Flags.Add(mouseUp, Buttons.Right);
			}
			if (mouseUp > 0) {
				MouseEvent e = new MouseEvent();
				e.button = mouseUp;
				e.position = Input.mousePosition;
				e.delta = new Vector3(eventData.delta.x, eventData.delta.y, 0.0f);
				Guard.Run(() => OnScreenMouseUp(e));
			}
		}

		public void ScreenPointerClick(PointerEventData eventData) {
			int mouseUp = Buttons.None;
			if (Input.GetMouseButtonUp(0)) {
				mouseUp = Flags.Add(mouseUp, Buttons.Left);
			}
			if (Input.GetMouseButtonUp(1)) {
				mouseUp = Flags.Add(mouseUp, Buttons.Right);
			}
			if (mouseUp > 0) {
				MouseEvent e = new MouseEvent();
				e.button = mouseUp;
				e.position = Input.mousePosition;
				e.delta = new Vector3(eventData.delta.x, eventData.delta.y, 0.0f);
				if (eventData.clickCount == 1) {
					// mouse click
					Guard.Run(() => OnScreenMouseClick(e));
				} else if (eventData.clickCount == 2) {
					// mouse double click
					Guard.Run(() => OnScreenMouseDoubleClick(e));
				}
			}
		}

		public void ScreenScroll(PointerEventData eventData) {
			MouseEvent e = new MouseEvent();
			e.button = Buttons.Middle;
			e.delta = new Vector3(0.0f, 0.0f, eventData.scrollDelta.y);
			Guard.Run(() => OnScreenMouseScroll(e));		
			// TODO 与使用以下方法获取的delta数值有区别，可能更适用，看实际情况来调整
//			float z = Input.GetAxis("Mouse ScrollWheel");
//			if (z != 0.0f) {
//				MouseEvent e = new MouseEvent();
//				e.button = Buttons.Middle;
//				e.delta = new Vector3(0.0f, 0.0f, z);
//				Guard.Run(() => OnScreenMouseScroll(e));
//			}
		}

	}

}