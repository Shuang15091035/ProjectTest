using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class LoginPage : AppBase {

		private Button btn_back;
		private InputField et_username;
		private InputField et_password;
        private uapp.ToggleEx btn_region_init;
		private Button btn_login;

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_back = findViewById<Button>("btn_back", null, parentView);

			et_username = findViewById<InputField>("et_username");
			et_password = findViewById<InputField>("et_password");

            // 登录逻辑
			btn_login = findViewById<Button>("btn_login");
			btn_login.onClick.AddListener(() => {
				// 用户名，密码空检查
				if (uapp.StringUtils.IsNullOrBlank(et_username.text)) {
//					showDialog("输入有误", "必须填写用户名", "知道了", null, null, null);
					showToast("必须填写用户名", uapp.Toast.Short);
					return;
				} else if (uapp.StringUtils.IsNullOrBlank(et_password.text)) {
//					showDialog("输入有误", "必须填写密码", "知道了", null, null, null);
					showToast("必须填写密码", uapp.Toast.Short);
					return;
				}
				ParentMachine.ChangeState(States.StartPage);
			});

            // 地方选择
			var lo_region = findViewById<ToggleGroup>("lo_region");
			uapp.UiUtils.ToggleGroupOnValueChanged(lo_region, (Toggle toggle, int toggleIndex, bool onOff) => {
				uapp.ToggleEx.SetToggleOnOff(toggle, onOff);
			});
            btn_region_init = findViewById<uapp.ToggleEx>("btn_region3");
            btn_region_init.isOn = true;


            return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();
			btn_back.gameObject.SetActive(false);
		}

	}

}
