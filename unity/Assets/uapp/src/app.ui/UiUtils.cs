using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;

namespace uapp {
	
    public sealed class UiUtils {

        public static void AddEventTrigger (GameObject gameObject, EventTriggerType triggerType, UnityAction<BaseEventData> action) {
            // Create a new TriggerEvent and add a listener
            EventTrigger.TriggerEvent trigger = new EventTrigger.TriggerEvent ();
			trigger.AddListener ((BaseEventData eventData) =>  action(eventData)); // ignore event data

            // Create and initialise EventTrigger.Entry using the created TriggerEvent
            EventTrigger.Entry entry = new EventTrigger.Entry () { callback = trigger, eventID = triggerType };

            // Add the EventTrigger.Entry to delegates list on the EventTrigger
            EventTrigger eventTrigger = gameObject.GetComponent<EventTrigger> ();
            if (eventTrigger == null) {
                eventTrigger = gameObject.AddComponent<EventTrigger> ();
            }
#if UNITY_5
            eventTrigger.triggers.Add (entry);
#else
		eventTrigger.delegates.Add(entry);
#endif
        }

		public static T FindViewById<T>(string viewId, GameObject parent) where T : Component {
			if (parent == null) {
				return null;
			}
			T view = null;
			foreach (Transform childTranform in parent.transform) {
				var childObject = childTranform.gameObject;
				if (uapp.StringUtils.Equals(childObject.name, viewId)) {
					view = childObject.GetComponent<T>();
					if (view != null) {
						break;
					}
				}
				view = FindViewById<T>(viewId, childObject);
				if (view != null) {
					break;
				}
			}
			// NOTE 默认添加UiElement
//			if (view != null && view.GetComponent<UiElement>() == null) {
//				view.gameObject.AddComponent<UiElement>();
//			}
			return view;
		}

		public delegate void OnToggleGroupValueChangedDelegate(Toggle toggle, int toggleIndex, bool onOff);
		public class ToggleGroupOnValueChangedObject {
			public ToggleGroup toggleGroup;
			public Toggle toggle;
		}
		public static void ToggleGroupOnValueChanged(ToggleGroup toggleGroup, OnToggleGroupValueChangedDelegate onToggleGroupValueChanged) {
			if (toggleGroup == null || onToggleGroupValueChanged == null) {
				return;
			}
//			var toggles = toggleGroup.ActiveToggles; // fuck unity, always empty list
			var toggles = toggleGroup.gameObject.GetComponentsInChildren<Toggle> ();
			if (toggles == null || toggles.Length == 0) {
				return;
			}
			foreach (var toggle in toggles) {
				toggle.onValueChanged.RemoveAllListeners();
//				toggle.onValueChanged.AddListener ((bool onOff) => {
//					var toggleIndex = CoreUtils.ArrayIndexOf(toggles, toggle);
//					onToggleGroupValueChanged(toggle, toggleIndex, onOff);
//				});
				ToggleGroupOnValueChangedObject obj = new ToggleGroupOnValueChangedObject();
				obj.toggleGroup = toggleGroup;
				obj.toggle = toggle;
				toggle.onValueChanged.AddListener ((bool onOff) => {
					var toggles2 = obj.toggleGroup.gameObject.GetComponentsInChildren<Toggle> ();
					var toggleIndex = ArrayUtils.IndexOf(toggles2, obj.toggle);
					onToggleGroupValueChanged(obj.toggle, toggleIndex, onOff);
				});
			}
		}

        public static void ToggleGroupSetAllTogglesOnOff(ToggleGroup toggleGroup, bool onOff, Toggle exceptToggle = null) {
            var toggles = toggleGroup.gameObject.GetComponentsInChildren<Toggle> ();
            if (toggles == null || toggles.Length == 0) {
                return;
            }
            foreach (var toggle in toggles) {
                if (toggle == exceptToggle) {
                    continue;
                }
                ToggleEx.SetToggleOnOff(toggle, onOff);
            }
        }

		public static Canvas FindCanvas(GameObject gameObject) {
			return ObjectUtils.FindParent<Canvas>(gameObject);
		}

        public static Color GetImagePixelByWorldPosition (Image image, Vector3 worldPosition) {
            if (image == null) {
                return Color.clear;
            }
            var sprite = image.sprite;
            if (sprite == null) {
                return Color.clear;
            }
            var texture = sprite.texture;
            if (texture == null) {
                return Color.clear;
            }
            // 转换到image坐标系
            var i = image.rectTransform.worldToLocalMatrix.MultiplyPoint (worldPosition);
            var ix = i.x + (image.rectTransform.sizeDelta.x * 0.5f);
            var iy = i.y + (image.rectTransform.sizeDelta.y * 0.5f);
            //Debug.Log ("i:" + ix + "," + iy);
            // 转换到texture坐标系
            var px = (int)((ix / image.rectTransform.sizeDelta.x) * (float)texture.width);
            var py = (int)((iy / image.rectTransform.sizeDelta.y) * (float)texture.height);
            //Debug.Log ("p:" + px + "," + py);
            return texture.GetPixel (px, py);
        }
    }
}
