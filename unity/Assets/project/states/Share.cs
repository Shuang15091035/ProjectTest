using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class Share : AppBase {

		private Button btn_back;
		private Button btn_close;
		private Image img_qrcode;

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_back = findViewById<Button>("btn_back", null, App.Canvas);

			btn_close = findViewById<Button>("btn_close");
			btn_close.onClick.AddListener(() => {
				ParentMachine.RevertState();
			});

			img_qrcode = findViewById<Image>("img_qrcode");

			return view;
		}

		public override bool OnPreCondition() {
			return SharedModel.Instance.CurrentPlan != null;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			btn_back.gameObject.SetActive(false);
			lo_order_list_toolbar.gameObject.SetActive(false);

			var qrcodeFile = SharedModel.Instance.CurrentPlan.QRCode;
			if (qrcodeFile != null) {
				var sprite = qrcodeFile.GetContent<Sprite>();
				img_qrcode.sprite = sprite;
			}
		}
	}

}
