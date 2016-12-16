using UnityEngine;
using System.Collections;

namespace project {
	
	public class LimnBaseState : AppBase {
	
		protected override void OnScreenMouseMove(uapp.MouseEvent e) {
			base.OnScreenMouseMove(e);

			var plan = SharedModel.Instance.CurrentPlan;
			if (plan == null) {
				return;
			}
			Vector3 point;
			if (!Picker.RaycastGround(e.position, out point)) {
				return;
			}
			var editItem = plan.EditItemByPointXZ(point);
			plan.SetEditItemState(editItem, EditState.Highlighted);
		}

//		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
//			base.OnScreenMouseClick(e);
//
//			var plan = SharedModel.Instance.CurrentPlan;
//			if (plan == null) {
//				return;
//			}
//			Vector3 point;
//			if (!Picker.RaycastGround(e.position, out point)) {
//				return;
//			}
//			var editItem = plan.EditItemByPointXZ(point);
//			onEditItemClick(editItem);
//		}
//
//		protected virtual void onEditItemClick(EditItem editItem) {
//			
//		}

	}
}
