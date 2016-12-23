using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class PrototypeBase : AppBase {

		protected Button btn_back;
		protected PrototypeListView lv_prototypes;
		protected ToggleGroup tg_prototypes;

		protected Plan mSelectedPlan;

		protected virtual void onPlanSelected(Plan plan) {}

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_back = findViewById<Button>("btn_back", null, App.Canvas);

			lv_prototypes = findViewById<PrototypeListView>("lv_prototypes");
			lv_prototypes.Adapter = new PrototypeAdapter();

			tg_prototypes = findViewById<ToggleGroup>("lv_prototypes_contents");

			btn_done = findViewById<Button>("btn_done", null, parentView);

			return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			//// 返回首页
			//btn_back.onClick.RemoveAllListeners();
			//btn_back.onClick.AddListener (() => {
			//	ParentState.ParentMachine.RevertState();
			//});
			//btn_back.gameObject.SetActive(true);

			// 更新样板间列表数据
			lv_prototypes.Adapter.Data = SharedModel.Instance.LocalPrototypes;
			lv_prototypes.Adapter.NotifyDataSetChanged();

			// 点击样板间，记录选中的样板间
			uapp.UiUtils.ToggleGroupOnValueChanged(tg_prototypes, (Toggle toggle, int toggleIndex, bool onOff) => {
				mSelectedPlan = lv_prototypes.Adapter.GetItemAt(toggleIndex);

				//// test 
				//var plan = new Plan();
				//plan.File = uapp.File.Resource("Test");
				//mSelectedPlan = plan;
			});

			//
			showStatusbar();

			// 进入编辑状态
			btn_done.onClick.RemoveAllListeners();
			btn_done.onClick.AddListener (() => {
				onPlanSelected(mSelectedPlan);
			});
		}

	}
}
