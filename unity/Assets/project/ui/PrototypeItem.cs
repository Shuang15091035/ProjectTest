using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class PrototypeItem : uapp.ListItem<Plan> {

		public Button btn_image;
		public Image img_image;
        public Text tv_name;
        public Text tv_price;

		public override void SetRecord(Plan record) {
			var preview = record.Preview;
			if (preview != null) {
				img_image.sprite = preview.GetContent<Sprite>();
			}
			tv_name.text = record.Name;
			tv_price.text = "￥" + record.PackagePrice.ToString("F0") + "/m²";

			listenOnClick (btn_image, record);
        }

	}
}
