using UnityEngine;
using System.Collections;

namespace project {
	
	public class LimnInWallItem : AppBase {

		private InWallItem mInWallItem;
		private Wall mWall;

		public override bool OnPreCondition() {
			mInWallItem = SharedModel.Instance.CurrentInWallItem;
			return mInWallItem != null;
		}

//		protected override void OnScreenMouseMove(uapp.MouseEvent e) {
//			base.OnScreenMouseMove(e);
//
//			var plan = SharedModel.Instance.CurrentPlan;
//			if (plan == null) {
//				return;
//			}
//			Vector3 point;
//			if (!Picker.RaycastGround(e.position, out point)) {
//				return;
//			}
//			mWall = plan.WallByPoint(point);
//			plan.SetEditItemState(mWall, EditState.Highlight);
//		}

		public override void OnStateIn() {
			base.OnStateIn();

			//
			hideToolbar();
			lo_statusbar.gameObject.SetActive(false);
		}

		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
			base.OnScreenMouseClick(e);

			var plan = SharedModel.Instance.CurrentPlan;
			if (plan == null) {
				ParentMachine.RevertState();
				return;
			}
			Vector3 point;
			if (!Picker.RaycastGround(e.position, out point)) {
				ParentMachine.RevertState();
				return;
			}
			mWall = plan.WallByPointXZ(point);
			if (mWall != null) {
				var result = mWall.AddInWallItemXZ(mInWallItem, point);
				switch (result) {
					case Wall.WallEditResult.NotEnoughSpace:
						showToast("没有足够的空间！", uapp.Toast.Short);
						break;
					default:
						break;
				}
				if (result == Wall.WallEditResult.Ok) {
					mWall.Build();
				}
			}
			mWall = null;
			ParentMachine.RevertState();
		}
	}

}
