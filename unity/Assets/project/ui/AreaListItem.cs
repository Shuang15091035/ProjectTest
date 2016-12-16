using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace project {

    public class AreaListItem : uapp.ListItem<string> {

		public Text tv_name;
		public uapp.ToggleEx btn_name;

		public override void SetRecord (string record) {
			tv_name.text = record;
			listenOnClick (btn_name, record);
        }
    }
}
