using UnityEngine;
using System.Collections;

namespace project {

	public class SavePlan : AppBase {

		public override void OnStateIn() {
			base.OnStateIn();

//			showDialog(null, "保存成功", "确定", null, () => {
//				ParentMachine.RevertState();
//			}, () => {
//				ParentMachine.RevertState();
//			});
			showToast("保存成功", uapp.Toast.Short, () => {
				ParentMachine.RevertState();
			});
		}
    }
}