using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;
using System;
using System.Collections;
using Valve.VR;

namespace project {

    public class VRCamera : PlanEditBase {

        public GameObject leftController;
        public GameObject rightController;
        public GameObject cameraRig;
        private Plan mPlan;
        public GameObject head;
        public override bool OnPreCondition () {
            mPlan = SharedModel.Instance.CurrentPlan;
            var system = OpenVR.System;
            if (system == null) {
                return false;
            }
            return true;
        }

        public override GameObject OnCreateView (GameObject parentView) {
            var view = base.OnCreateView (parentView);
            VRSettings.enabled = false;
            var SteamVR = GameObject.Find ("[SteamVR]");
            Destroy (SteamVR);
            cameraRig.SetActive (false);

            return view;
        }
        public override void OnStateIn () {
            base.OnStateIn ();
            //cameraRig.SetActive(true);
            try {
                VRSettings.enabled = true;
                cameraRig.SetActive (true);
                SteamVRUtils.ManGoTo (head, cameraRig, new Vector3 (mPlan.FPSpx,cameraRig.transform.position.y, mPlan.FPSpz));

            } catch (Exception) {
            }
            SharedModel.Instance.CurrentPlan.LightsOnOff = false;
            SharedModel.Instance.Photographer.SetCurrentCameraEnabled (false);

            leftController.SetActive (true);
            rightController.SetActive (true);

            uapp.UiUtils.ToggleGroupOnValueChanged (lo_camera, (Toggle toggle, int toggleIndex, bool onOff) => {
                uapp.ToggleEx.SetToggleOnOff (toggle, onOff);
                if (onOff) {
                    switch (toggleIndex) {
                        case 3:
                            SharedModel.Instance.CurrentPlan.LightsOnOff = false;
                            SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (false);
                            SharedModel.Instance.Photographer.ChangeCamera (SharedModel.Instance.Photographer.BirdCamera);
                            ParentMachine.RevertState();
                            break;
                        case 2:
                            SharedModel.Instance.CurrentPlan.LightsOnOff = true;
                            SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (true);
                            SharedModel.Instance.Photographer.ChangeCamera (SharedModel.Instance.Photographer.FPSCamera);
                            ParentMachine.RevertState ();
                            break;
                        case 1:
                            SharedModel.Instance.CurrentPlan.LightsOnOff = true;
                            SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (true);
                            SharedModel.Instance.Photographer.EditorCamera.OnMoveVertical = (float dx, float dy) => {
                                var b = uapp.ObjectUtils.GetTransformBounds (SharedModel.Instance.CurrentPlan.Root);
                                SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (SharedModel.Instance.Photographer.EditorCamera.Camera.gameObject.transform.position.y < b.max.y);
                            };
                            SharedModel.Instance.Photographer.EditorCamera.OnMoveHorizontal = (float dx, float dz) => {
                                var b = uapp.ObjectUtils.GetTransformBounds (SharedModel.Instance.CurrentPlan.Root);
                                SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (SharedModel.Instance.Photographer.EditorCamera.Camera.gameObject.transform.position.y < b.max.y);
                            };
                            SharedModel.Instance.Photographer.ChangeCamera (SharedModel.Instance.Photographer.EditorCamera);
                            ParentMachine.RevertState ();
                            break;
                        //case 0:
                        //    SubMachine.ChangeState (States.VRCamera);
                        //    // TODO VR视角
                        //    break;
                        default:
                            break;
                    }
                }
            });

        }
        public override void OnStateOut () {
			try {
            	cameraRig.SetActive (false);
				VRSettings.enabled = false;
			} catch (Exception) {
				
			}

            leftController.SetActive (false);
            rightController.SetActive (false);
            var SteamVR = GameObject.Find ("[SteamVR]");
            Destroy (SteamVR);
            var light = GameObject.Find ("Directional_realtime");
            if (light != null) {
                light.gameObject.SetActive (true);
            }
            
            SharedModel.Instance.Photographer.SetCurrentCameraEnabled (true);
            base.OnStateOut ();
        }
    }
}