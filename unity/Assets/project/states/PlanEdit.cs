using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace project {
	
	public class PlanEdit : AppBase {

		private Button btn_back;
		protected Toggle btn_fps_camera;
		protected Toggle btn_editor_camera;

		private Plan mPlan;

		public override bool OnPreCondition() {
			mPlan = SharedModel.Instance.CurrentPlan;
			return mPlan != null;
		}
        
        public override GameObject OnCreateView (GameObject parentView) {
			var view = base.OnCreateView (parentView);

			btn_back = findViewById<Button>("btn_back", null, App.Canvas);
			btn_fps_camera = findViewById<Toggle>("btn_fps_camera", null, parentView);
			btn_editor_camera = findViewById<Toggle>("btn_editor_camera", null, parentView);

            return view;
        }

        public override void OnStateIn () {
            base.OnStateIn ();

			// 返回
			btn_back.onClick.RemoveAllListeners();
			btn_back.onClick.AddListener (() => {
				ParentMachine.RevertState();
			});
			btn_back.gameObject.SetActive(true);

			//
			showOrderListToolbar();
			lo_sidebar.gameObject.SetActive(false);
			lo_statusbar.gameObject.SetActive(false);

			var fpsCamera = SharedModel.Instance.Photographer.FPSCamera;
			fpsCamera.PositionXZ = new Vector2(mPlan.FPSpx, mPlan.FPSpz);
			fpsCamera.Height = mPlan.FPSh;
			fpsCamera.Angles = new Vector2(mPlan.FPSax, mPlan.FPSay);
			var birdCamera = SharedModel.Instance.Photographer.BirdCamera;
			birdCamera.PositionXZ = new Vector3(mPlan.BIRDpx, mPlan.BIRDpz);
			birdCamera.Height = mPlan.BIRDh;
			if (SharedModel.Instance.WillChangeToThisCamera == SharedModel.Instance.Photographer.FPSCamera) {
				btn_fps_camera.isOn = true;
				changeToFPSCamera();
			} else if (SharedModel.Instance.WillChangeToThisCamera == SharedModel.Instance.Photographer.EditorCamera) {
				btn_editor_camera.isOn = true;
				changeToEditorCamera();
			}

			if (SubMachine.CurrentState != SubMachine.GetState(States.PlanEditInit)) {
				SubMachine.RevertState();
			}
        }

		public override void OnStateOut() {
			uapp.CoreUtils.Delete(mPlan);
			mPlan = null;
			SharedModel.Instance.CurrentPlan = null;
			base.OnStateOut();
		}
    }
}
