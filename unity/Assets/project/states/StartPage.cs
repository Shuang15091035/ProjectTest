using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class StartPage : uapp.AppState {

		private Button btn_back = null;
		private Toggle btn_prototype = null;
		private Toggle btn_new_plan = null;

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_back = findViewById<Button>("btn_back", null, parentView);

            // 样板间
			btn_prototype = findViewById<Toggle>("btn_prototype");
			btn_prototype.onValueChanged.AddListener((bool onOff) => {
				if (onOff) {
					SubMachine.ChangeState(States.Prototype);
				}
			});

            // DIY
			btn_new_plan = findViewById<Toggle>("btn_new_plan");
			btn_new_plan.onValueChanged.AddListener((bool onOff) => {
				if (onOff) {
					SubMachine.ChangeState(States.NewPlan);
				}
			});

            return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			btn_back.onClick.RemoveAllListeners();
			btn_back.onClick.AddListener(() => {
				ParentMachine.RevertState();
			});
			btn_back.gameObject.SetActive(true);
		}

    }
}
