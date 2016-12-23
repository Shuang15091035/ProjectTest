using UnityEngine;
using System.Collections;

namespace project {
	
	public class RoomItem : EditItem {

		protected Room mRoom;

		protected RoomItem(Room room, string name, int id, SKU sku, GameObject parent = null) : base(name, id, sku, parent) {
			mRoom = room;
		}

		public Room Room {
			get {
				return mRoom;
			}
		}

		public float PlanScale {
			get {
				if (mRoom == null) {
					return 1.0f;
				}
				var plan = mRoom.Plan;
				if (plan == null) {
					return 1.0f;
				}
				return plan.Scale;
			}
		}

		public virtual void onPlanScaleChanged(float scale) {
			
		}

	}
}
