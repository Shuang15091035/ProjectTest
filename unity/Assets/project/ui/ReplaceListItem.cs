using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace project {

	public class ReplaceListItem : uapp.ListItem<SKU> {

		public Image img_image;
		public Text tv_name;
		public Text tv_category;
        public Text tv_brand;
		public Text tv_model;
		public Text tv_spec;
		public Button btn_replace;

        public override void SetRecord (SKU record) {
			Sprite sprite = null;
			var preview = record.Preview;
			if (preview != null) {
				sprite = preview.GetContent<Sprite>();
			}
			img_image.sprite = sprite != null ? sprite : SharedModel.Instance.PreviewNotFound.GetContent<Sprite>();

			tv_name.text = record.Name;
			tv_category.text = record.Category;
            tv_brand.text = record.Brand;
			tv_model.text = record.Model;
			var source = record.Source;
			if (source == null) {
				tv_spec.text ="未知";
			} else {
				tv_spec.text = source.RealL + "×" + source.RealW + "×" + source.RealH;
			}

			listenOnClick (btn_replace, record);
        }
    }
}