using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class LimnPlan : AppBase {

		private Button btn_back;
		private uapp.ICameraPrefab mCamera;
		private PlanBgRenderer mPlanBgRenderer;
		private Plan mPlan;

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_back = findViewById<Button>("btn_back", null, App.Canvas);

			var btn_visible = findViewById<Toggle>("btn_visible", null, lo_limn_plan_toolbar.gameObject);
//			btn_visible.onClick.AddListener(() => {
//				mPlanBgRenderer.GameObject.SetActive(!mPlanBgRenderer.GameObject.activeInHierarchy);
//			});
			btn_visible.onValueChanged.AddListener((bool onOff) => {
				uapp.ToggleEx.SetToggleOnOff(btn_visible, onOff, false);
				mPlanBgRenderer.GameObject.SetActive(!mPlanBgRenderer.GameObject.activeInHierarchy);
			});

			return view;
		}

		public override bool OnPreCondition() {
			if (SharedModel.Instance.CurrentPlanBg == null) {
				return false;
			}
			return true;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			// 返回首页
			btn_back.onClick.RemoveAllListeners();
			btn_back.onClick.AddListener (() => {
				ParentMachine.RevertState();
			});
			btn_back.gameObject.SetActive(true);

			// 
			showLimnPlanToolbar();
			lo_sidebar.gameObject.SetActive(false);
            showStatusbar();
            btn_done.GetComponentInChildren<Text> ().text = "完成";

			// 创建新户型
			mPlan = new Plan();
			mPlan.Background = SharedModel.Instance.CurrentPlanBg;
			SharedModel.Instance.CurrentPlan = mPlan;

			// 切换视角
			SharedModel.Instance.Photographer.LimnCamera.Camera.clearFlags = CameraClearFlags.SolidColor;
			SharedModel.Instance.Photographer.LimnCamera.Camera.backgroundColor = uapp.ColorUtils.FromRbga(0xFAFBF1FF);
			SharedModel.Instance.Photographer.ChangeCamera(SharedModel.Instance.Photographer.LimnCamera);
			SharedModel.Instance.Photographer.SetCurrentCameraEnabled(true);

			// 渲染背景图
			mPlanBgRenderer = new PlanBgRenderer();
			mPlanBgRenderer.Plan = mPlan;
			mPlanBgRenderer.Build();
		}

		public override void OnStateOut() {

            btn_done.GetComponentInChildren<Text> ().text = "确定";

            uapp.CoreUtils.Delete(mPlan);
			mPlan = null;
			SharedModel.Instance.CurrentPlan = null;
			base.OnStateOut();
		}

//        protected override void OnKey (uapp.KeyEvent e) {
//            base.OnKey (e);
//            if (e.isDown (KeyCode.E)) {
//                if (mPlanBgRenderer.GameObject.activeInHierarchy == true) {
//                    mPlanBgRenderer.GameObject.SetActive (false);
//                } else {
//                    mPlanBgRenderer.GameObject.SetActive (true);
//                }
//            }
//        }

    }
}
