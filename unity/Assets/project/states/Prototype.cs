using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace project {
	
	public class Prototype : PrototypeBase {

		public override void OnStateIn() {
			base.OnStateIn();

            // 返回首页
            btn_back.onClick.RemoveAllListeners ();
            btn_back.onClick.AddListener (() => {
                ParentState.ParentMachine.RevertState ();
            });
            btn_back.gameObject.SetActive (true);

            // 
            showTitle ();
			hideToolbar();
			lo_sidebar.gameObject.SetActive(true);
			lo_statusbar.gameObject.SetActive(true);
		}

		protected override void onPlanSelected(Plan plan) {
			base.onPlanSelected(plan);
			loadPrototype(plan);
		}

		protected void loadPrototype(Plan plan) {
			if (plan == null) {
				return;
			}
			var file = plan.File;
			if (file == null) {
				return;
			}

//			startLoading("玩命加载中...");
//			file.GetContent<GameObject>(true, (GameObject content) => {
//				plan.HandlePrototype(content);
//
//				//				var lightmap = Resources.Load<GameObject>(plan.File.Path + "/lightmap");
//				//				lightmap.GetComponent<uapp.PrefabLightmapData>().Load(plan.Root, (string path) => {
//				//					return Resources.Load<Texture2D>(plan.File.Path + "/" + path);
//				//				}, 0);
//
//				finishLoading();
//				SharedModel.Instance.CurrentPlan = plan;
//				ParentMachine.ChangeState(States.PlanEdit);
//			}, (float progress) => {
//				loading(progress);
//			});

			startLoading("玩命加载中...");
			file.GetScene(true, (UnityEngine.SceneManagement.Scene scene) => {
				plan.HandlePrototype(scene);
				finishLoading();
				SharedModel.Instance.CurrentPlan = plan;
				SharedModel.Instance.WillChangeToThisCamera = SharedModel.Instance.Photographer.FPSCamera;
				ParentMachine.ChangeState(States.PlanEdit);
			}, (float progress) => {
				loading(progress);
			});

		}

	}
}
