using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace project {
	
	public class ItemMaterialListItem : uapp.ListItem<SKU> {

		public Button img_image;
		public Text tv_name;

		public override void SetRecord (SKU record) {
			var preview = record.Preview;
			if (preview != null) {
				img_image.image.sprite = preview.GetContent<Sprite>();
			}
			tv_name.text = record.Name;
			listenOnClick (img_image, record);
		}

	}
}
