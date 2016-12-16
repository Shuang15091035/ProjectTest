using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class AppBase : uapp.AppState {

		protected Image lo_toolbar;
		protected Image lo_limn_plan_toolbar;
		protected Image lo_limn_room_toolbar;
		protected Image lo_order_list_toolbar;
		protected Image lo_sidebar;
		protected Image lo_statusbar;
		protected Image lo_total;
        protected InputField et_unit_price;
        protected InputField et_area;
		protected Text tv_total;
		protected Button btn_done;
        protected Button btn_refresh;

		protected Button lo_dialog;
		protected Image lo_toast;
        protected Image lo_loading;
        protected Image img_logo;

        public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			img_logo = findViewById<Image> ("img_logo", null, App.Canvas);
			img_logo.gameObject.SetActive(true);
			lo_order_list_toolbar = findViewById<Image>("lo_order_list_toolbar", null, App.Canvas);
			lo_order_list_toolbar.gameObject.SetActive(false);

			lo_toolbar = findViewById<Image>("lo_toolbar", null, App.Canvas);
			lo_toolbar.gameObject.SetActive(false);
			lo_limn_plan_toolbar = findViewById<Image>("lo_limn_plan_toolbar", null, App.Canvas);
			lo_limn_room_toolbar = findViewById<Image>("lo_limn_room_toolbar", null, App.Canvas);

			lo_sidebar = findViewById<Image>("lo_sidebar", null, App.Canvas);

			lo_statusbar = findViewById<Image>("lo_statusbar", null, App.Canvas);
			lo_total = findViewById<Image>("lo_total", null, App.Canvas);
            lo_total.gameObject.SetActive(false);
            et_unit_price = findViewById<InputField> ("et_unit_price", null, lo_statusbar.gameObject);
            et_area = findViewById<InputField>("et_area", null, lo_statusbar.gameObject);
			tv_total = findViewById<Text>("tv_total", null, lo_statusbar.gameObject);
			btn_done = findViewById<Button>("btn_done", null, lo_statusbar.gameObject);
            btn_refresh = findViewById<Button> ("btn_refresh", null, lo_statusbar.gameObject);

            lo_dialog = findViewById<Button>("lo_dialog", null, App.Canvas);
			lo_dialog.gameObject.SetActive(false);

			lo_toast = findViewById<Image>("lo_toast", null, App.Canvas);
			lo_toast.gameObject.SetActive(false);

            lo_loading = findViewById<Image>("lo_loading", null, App.Canvas);
			lo_loading.gameObject.SetActive(false);

			return view;
		}

		protected void changeToFPSCamera() {
			SharedModel.Instance.CurrentPlan.LightsOnOff = true;
			SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (true);
			SharedModel.Instance.Photographer.ChangeCamera (SharedModel.Instance.Photographer.FPSCamera);
		}

		protected void changeToEditorCamera() {
			SharedModel.Instance.CurrentPlan.LightsOnOff = true;
			var b = SharedModel.Instance.CurrentPlan.TransformBounds;
			SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (SharedModel.Instance.Photographer.EditorCamera.Camera.gameObject.transform.position.y < b.max.y);
			SharedModel.Instance.Photographer.EditorCamera.OnMoveVertical = (float dx, float dy) => {
				var bb = SharedModel.Instance.CurrentPlan.TransformBounds;
				SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (SharedModel.Instance.Photographer.EditorCamera.Camera.gameObject.transform.position.y < bb.max.y);
			};
			SharedModel.Instance.Photographer.EditorCamera.OnMoveHorizontal = (float dx, float dz) => {
				var bb = SharedModel.Instance.CurrentPlan.TransformBounds;
				SharedModel.Instance.CurrentPlan.SetAllCeilItemsVisible (SharedModel.Instance.Photographer.EditorCamera.Camera.gameObject.transform.position.y < bb.max.y);
			};
			SharedModel.Instance.Photographer.ChangeCamera (SharedModel.Instance.Photographer.EditorCamera);
		}

		protected void showTitle() {
			img_logo.gameObject.SetActive(true);
			lo_order_list_toolbar.gameObject.SetActive(false);
		}

		protected void showLimnPlanToolbar() {
			lo_toolbar.gameObject.SetActive(true);
			lo_limn_plan_toolbar.gameObject.SetActive(true);
			lo_limn_room_toolbar.gameObject.SetActive(false);
			lo_order_list_toolbar.gameObject.SetActive(false);
		}

		protected void showLimnRoomToolbar() {
			lo_toolbar.gameObject.SetActive(true);
			lo_limn_plan_toolbar.gameObject.SetActive(false);
			lo_limn_room_toolbar.gameObject.SetActive(true);
			lo_order_list_toolbar.gameObject.SetActive(false);
		}

		protected void showOrderListToolbar() {
//			lo_toolbar.gameObject.SetActive(true);
//            lo_limn_plan_toolbar.gameObject.SetActive(false);
//			lo_limn_room_toolbar.gameObject.SetActive(false);
//			lo_order_list_toolbar.gameObject.SetActive(true);

			img_logo.gameObject.SetActive(false);
			lo_order_list_toolbar.gameObject.SetActive(true);
		}

		protected void hideToolbar() {
			lo_toolbar.gameObject.SetActive(false);
		}

        protected void showRefresh () {
            lo_total.gameObject.SetActive (false);
            btn_done.gameObject.SetActive (true);
            btn_refresh.gameObject.SetActive (true);
            lo_statusbar.gameObject.SetActive (true);
        }

		protected void showTotal(float unitPrice) {
			var tv_unit_price = findViewById<Text>("tv_unit_price", null, lo_total.gameObject);
			tv_unit_price.text = unitPrice.ToString("F0");
			lo_total.gameObject.SetActive(true);
            btn_done.gameObject.SetActive (true);
            btn_refresh.gameObject.SetActive (false);
            lo_statusbar.gameObject.SetActive(true);
		}

		protected void showStatusbar() {
			lo_total.gameObject.SetActive(false);
            btn_done.gameObject.SetActive (true);
            btn_refresh.gameObject.SetActive (false);
			lo_statusbar.gameObject.SetActive(true);
		}

		protected void hideStatusbar() {
			lo_statusbar.gameObject.SetActive(false);
		}

		protected delegate void OnDialogOk();
		protected delegate void OnDialogCancel();
		protected void showDialog(string title, string message, string ok, string cancel, OnDialogOk onOk, OnDialogCancel onCancel) {
			var tv_title = findViewById<Text>("tv_title", null, lo_dialog.gameObject);
			var tv_message = findViewById<Text>("tv_message", null, lo_dialog.gameObject);
			var btn_ok = findViewById<Button>("btn_ok", null, lo_dialog.gameObject);
			var tv_ok = btn_ok.GetComponentInChildren<Text>();
			var btn_cancel = findViewById<Button>("btn_cancel", null, lo_dialog.gameObject);
			var tv_cancel = btn_cancel.GetComponentInChildren<Text>();

			if (uapp.StringUtils.IsNullOrBlank(title)) {
				tv_title.gameObject.SetActive(false);
			} else {
				tv_title.text = title;
				tv_title.gameObject.SetActive(true);
			}
			tv_message.text = message;

			tv_ok.text = ok;
			btn_ok.onClick.RemoveAllListeners();
			btn_ok.onClick.AddListener(() => {
				if (onOk != null) {
					onOk();
				}
				lo_dialog.gameObject.SetActive(false);
			});

			if (cancel != null) {
				tv_cancel.text = cancel;
				btn_cancel.onClick.RemoveAllListeners();
				btn_cancel.onClick.AddListener(() => {
					if (onCancel != null) {
						onCancel();
					}
					lo_dialog.gameObject.SetActive(false);
				});
				btn_cancel.gameObject.SetActive(true);
			} else {
				btn_cancel.gameObject.SetActive(false);
			}

			lo_dialog.onClick.RemoveAllListeners();
			lo_dialog.onClick.AddListener(() => {
				if (onCancel != null) {
					onCancel();
				}
				lo_dialog.gameObject.SetActive(false);
			});
			lo_dialog.gameObject.SetActive(true);
		}

		protected void showToast(string text, float duration, uapp.Toast.OnDisappear onDisappear = null) {
			var t = lo_toast.GetComponent<uapp.Toast>();
			t.Show(text, duration, onDisappear);
		}

        protected void startLoading(string title) {
            var tv_loading = findViewById<Text>("tv_loading", null, lo_loading.gameObject);
            tv_loading.text = title;
            var tv_progress = findViewById<Text>("tv_progress", null, lo_loading.gameObject);
            tv_progress.text = "0%";
            lo_loading.gameObject.SetActive(true);
        }

        protected void loading(float progress) {
            var tv_progress = findViewById<Text>("tv_progress", null, lo_loading.gameObject);
            var percent = (int)(progress * 100.0f);
            tv_progress.text = percent + "%";
        }

        protected void finishLoading() {
            lo_loading.gameObject.SetActive(false);
        }

		protected delegate void OnFakeLoaded();
		protected void fakeLoading(string title, float duration, OnFakeLoaded onFakeLoaded = null) {
			StartCoroutine(fakeLoadTask(title, duration, onFakeLoaded));
		}

		protected IEnumerator fakeLoadTask(string title, float duration, OnFakeLoaded onFakeLoaded) {
			startLoading(title);
			for (var i = 0; i < 100; i++) {
				yield return new WaitForSeconds(duration / 100.0f);
				if (65 < i && i < 88) {
					continue;
				}
				loading((float)i / 100.0f);
			}
			finishLoading();
			yield return null;
			if (onFakeLoaded != null) {
				onFakeLoaded();
			}
		}
	}
}
