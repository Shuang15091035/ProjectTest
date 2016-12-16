using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {

//	public class PrototypeMatch : uapp.AppState {
//		
//		private Button btn_finish;
//
//		private Plan mPlan;
//
//		public override GameObject OnCreateView(GameObject parentView) {
//			var view = base.OnCreateView(parentView);
//			btn_finish = findViewById("btn_finish", btn_finish);
//			btn_finish.onClick.AddListener(() => {
//				ParentMachine.ChangeState(States.PlanEdit, false);
//			});
//			return view;
//		}
//
//		public override bool OnPreCondition() {
//			mPlan = SharedModel.Instance.CurrentPlan;
//			return mPlan != null;
//		}
//
//		public override void OnStateIn() {
//			base.OnStateIn();
//			// TODO 完成户型编辑进入场景后使用plan的scale对墙体重新bulid
//			mPlan.EditPhase = EditPhase.Edit;
//		}
//	}

	public class PrototypeMatch : PrototypeBase {

		protected Plan mPlan;

		public override bool OnPreCondition() {
			mPlan = SharedModel.Instance.CurrentPlan;
			return mPlan != null;
		}

		public override void OnStateIn() {
			base.OnStateIn();

            // 返回
            btn_back.onClick.RemoveAllListeners ();
            btn_back.onClick.AddListener (() => {
                ParentMachine.RevertState ();
            });
            btn_back.gameObject.SetActive (true);

            // 隐藏工具栏
            hideToolbar ();
			lo_sidebar.gameObject.SetActive(false);
			lo_statusbar.gameObject.SetActive(true);

			SharedModel.Instance.PlanBackgroundObject.SetActive(false);
			mPlan.EditPhase = EditPhase.Init;
		}

		protected override void onPlanSelected(Plan plan) {
			base.onPlanSelected(plan);
			loadPrototype(plan); // NOTE 此版本直接加载样板间作为匹配好的户型
		}

		protected void loadPrototype(Plan plan) {
			if (plan == null) {
				return;
			}
			var file = plan.File;
//			if (onlyArch) {
//				var archFile = file.Instantiate();
//				archFile.Path = archFile.Path + "/arch";
//				file = archFile;
//			}
			if (file == null) {
				return;
			}

//			fakeLoading("正在生成户型结构...", 5.0f);
//			ParentMachine.ChangeState(States.PlanEdit, false);
//			startLoading("正在生成户型结构...");
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
//				ParentMachine.ChangeState(States.PlanEdit, false);
//			}, (float progress) => {
//				loading(progress);
//			});

//			fakeLoading("正在生成户型结构...", 3.0f, () => {
//				startLoading("正在进行后处理...");
//				file.GetScene(true, (UnityEngine.SceneManagement.Scene scene) => {
//					var gameObjects = scene.GetRootGameObjects();
//					foreach (var gameObject in gameObjects) {
//						if (gameObject.name == "arch") {
//							gameObject.SetActive(true);
//							plan.HandlePrototypeObject(gameObject);
//						} else {
//							gameObject.SetActive(false);
//						}
//					}
//					finishLoading();
//					SharedModel.Instance.CurrentPlan = plan;
//					SharedModel.Instance.WillChangeToThisCamera = SharedModel.Instance.Photographer.EditorCamera;
//					ParentMachine.ChangeState(States.PlanEdit);
//				}, (float progress) => {
//					loading(progress);
//				});
//			});

			file.Path += "_kuangjia";
			fakeLoading("正在生成户型结构...", 3.0f, () => {
				startLoading("正在进行后处理...");
				file.GetScene(true, (UnityEngine.SceneManagement.Scene scene) => {
					var gameObjects = scene.GetRootGameObjects();
					foreach (var gameObject in gameObjects) {
						if (gameObject.name == "arch") {
							gameObject.SetActive(true);
							plan.HandlePrototypeObject(gameObject);
						} else {
							gameObject.SetActive(false);
						}
					}
					finishLoading();
					SharedModel.Instance.CurrentPlan = plan;
					SharedModel.Instance.WillChangeToThisCamera = SharedModel.Instance.Photographer.EditorCamera;
					ParentMachine.ChangeState(States.PlanEdit);
				}, (float progress) => {
					loading(progress);
				});
			});

		}

	}
}
