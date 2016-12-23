using UnityEngine;
using System.Collections;

namespace project {
	
	public class Picker {

		public static Plane Ground = new Plane(Vector3.up, Vector3.zero);

		public static bool RaycastGround(Camera camera, Vector3 screenPosition, out Vector3 point) {
			point = Vector3.zero;
			var ray = camera.ScreenPointToRay(screenPosition);
			var plane = Ground;
			float enter;
			if (!plane.Raycast(ray, out enter)) {
				return false;
			}
			point = ray.origin + ray.direction * enter;
			point.y = 0.0f;
			return true;
		}

		public static bool RaycastGround(Vector3 screenPosition, out Vector3 point) {
			return RaycastGround(SharedModel.Instance.Photographer.CurrentCamera.Camera, screenPosition, out point);
		}

		public static Item RaycastItem(Camera camera, Vector3 screenPosition) {
			var ray = camera.ScreenPointToRay(screenPosition);
			RaycastHit result;
			if (Physics.Raycast(ray, out result, 1000)) {
				//return Data.ItemFromObject(result.collider.gameObject);
				Debug.Log("[Object] " + result.collider.gameObject.name + " clicked.",result.collider.gameObject);
                var item = uapp.ObjectUtils.FindParent<Item>(result.collider.gameObject, true);
				if (item != null) {
					Debug.Log("[Item] " + item.gameObject.name + " clicked.");
					if (!item.IsValid) {
						item = null;
					}
				} else {
					Debug.Log("[Item] nothing clicked.");
				}
				return item;
            }
			return null;
		}

		public static Item RaycastItem(Vector3 screenPosition) {
			return RaycastItem(SharedModel.Instance.Photographer.CurrentCamera.Camera, screenPosition);
		}
	}
}
