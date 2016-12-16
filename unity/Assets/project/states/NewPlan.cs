using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {

	public class NewPlan : AppBase {

		private Button btn_back;
		private Text tv_current_path;
		private PlanBgListView lv_plan_bgs;
		private PlanBgAdapter mPlanBgAdapter;
        private ToggleGroup tg_new_plan;

        private uapp.IFile mFile;
        
        public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_back = findViewById<Button>("btn_back", null, App.Canvas);

			tv_current_path = findViewById ("tv_current_path", tv_current_path);
			lv_plan_bgs = findViewById("lv_plan_bgs", lv_plan_bgs);
			mPlanBgAdapter = new PlanBgAdapter();
			lv_plan_bgs.Adapter = mPlanBgAdapter;
			tg_new_plan = findViewById ("lv_plan_bgs_contents", tg_new_plan);

            btn_done = findViewById<Button>("btn_done", null, parentView);

            return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			// 返回登录
			btn_back.onClick.RemoveAllListeners();
			btn_back.onClick.AddListener (() => {
				ParentState.ParentMachine.RevertState();
			});
			btn_back.gameObject.SetActive(true);

			// 
			showTitle();
			hideToolbar();
			lo_sidebar.gameObject.SetActive(true);
			showRefresh();

//			// 显示当前背景图文件夹路径
//			tv_current_path.text = SharedModel.Instance.PlanBgsDir.RealPath;

			// 更新背景图列表数据
			mPlanBgAdapter.Data = SharedModel.Instance.LocalPlanBgs;
			mPlanBgAdapter.NotifyDataSetChanged();

			// 刷新背景图文件夹
			btn_refresh.onClick.RemoveAllListeners();
			btn_refresh.onClick.AddListener (() => {
				var plansDir = SharedModel.Instance.PlanBgsDir;
				SharedModel.Instance.LocalPlanBgs = plansDir.ListFiles (new string[] { uapp.FilePatterns.PNG, uapp.FilePatterns.JPG }, false);
				mPlanBgAdapter.Data = SharedModel.Instance.LocalPlanBgs;
				mPlanBgAdapter.NotifyDataSetChanged ();
                uapp.UiUtils.ToggleGroupOnValueChanged (tg_new_plan, (Toggle toggle, int toggleIndex, bool onOff) => {
                    if (onOff) {
                        mFile = mPlanBgAdapter.GetItemAt (toggleIndex);
                    }
                });
            });

			// 点击背景图，记录选中的样板间
			mFile = null;
			uapp.UiUtils.ToggleGroupOnValueChanged(tg_new_plan, (Toggle toggle, int toggleIndex, bool onOff) => {
				if (onOff) {
					mFile = mPlanBgAdapter.GetItemAt (toggleIndex);
				}
			});

			// 进入临摹状态
			btn_done.onClick.RemoveAllListeners();
			btn_done.onClick.AddListener (() => {
				selectPlanBgToLimn (mFile);
			});
        }

		// 完成选择底图，切换状态
		private void selectPlanBgToLimn(uapp.IFile planBg) {
			if (planBg == null) {
				return;
			}
			SharedModel.Instance.CurrentPlanBg = planBg;
			ParentMachine.ChangeState(States.LimnPlan);
		}
	}
}