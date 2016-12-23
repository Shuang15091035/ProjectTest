/**
 * @file UiElement.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-26
 * @brief
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace uapp {

	/**
	 * NOTE 当控件不需要处理事件时但又不希望事件冒泡，可以直接添加这个脚本，unity只要找到处理事件的控件，就会停止冒泡
	 * 配合UiScreen和BaseBehaviour，实现全局屏幕事件和控件事件的处理，还有其他事件的处理，这个应该是最优雅的实现了
	 */
	public class UiElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IScrollHandler {

		public void OnPointerDown(PointerEventData eventData) {

		}

		public void OnPointerUp(PointerEventData eventData) {

		}

		public void OnScroll(PointerEventData eventData) {

		}

		public void OnPointerExit(PointerEventData eventData) {

		}

	}

}

