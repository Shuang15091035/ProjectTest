using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace project {
	
	public class App : uapp.App {

        private Button btn_quit = null;

        public override GameObject OnCreateView (GameObject parentView) {
			var view = base.OnCreateView (parentView);
			btn_quit = findViewById ("btn_quit", btn_quit);
			btn_quit.onClick.AddListener (() => {
                Application.Quit ();
            });
			return view;
        }

		public override void OnStateIn() {
			base.OnStateIn();
			SharedModel.Instance.Photographer.ChangeCamera(SharedModel.Instance.Photographer.EditorCamera);
		}
    }
}
