using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {

	public class PlanEditInit : PlanEditBase {

		public override void OnStateIn() {
			base.OnStateIn();

//			btn_editor_camera.isOn = true;
//			SharedModel.Instance.Photographer.ChangeCamera(SharedModel.Instance.Photographer.EditorCamera);

			//
			hideStatusbar();

            btn_order_list.onClick.RemoveAllListeners ();
            btn_order_list.onClick.AddListener (() => {
                ParentMachine.ChangeState (States.OrderList);
            });

            btn_save.onClick.RemoveAllListeners ();
            btn_save.onClick.AddListener (() => {
                ParentMachine.ChangeState (States.SavePlan);
            });

            btn_share.onClick.RemoveAllListeners ();
            btn_share.onClick.AddListener (() => {
                ParentMachine.ChangeState (States.Share);
            });

            SharedModel.Instance.CurrentPlan.ClearItemState();
		}

		protected override void onItemClick(Item item) {
			base.onItemClick(item);
			if (item == null || !item.CanEdit) {
				return;
			}
			SharedModel.Instance.CurrentItem = item;
			ParentMachine.ChangeState(States.ItemEdit);
		}

    }

}