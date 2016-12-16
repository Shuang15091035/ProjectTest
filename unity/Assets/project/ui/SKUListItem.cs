using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace project {

    public class SKUListItem : uapp.ListItem<SKU> {

		public Button img_image;
		public Text tv_name;
        public Text tv_brand;
		public Text tv_model;
		public Text tv_spec;
//        public Text tv_price;

        public override void SetRecord (SKU record) {
            if (record.Source == null) {
                img_image.gameObject.SetActive(false);
                tv_brand.gameObject.SetActive(false);
                tv_model.gameObject.SetActive(false);
                tv_name.gameObject.SetActive(false);
                tv_spec.gameObject.SetActive(false);
            }
			Sprite sprite = null;
			var preview = record.Preview;
			if (preview != null) {
				sprite = preview.GetContent<Sprite>();
			}
			img_image.image.sprite = sprite != null ? sprite : SharedModel.Instance.PreviewNotFound.GetContent<Sprite>();

			tv_name.text ="名称："+ record.Name;
            tv_brand.text ="品牌："+ record.Brand;
			tv_model.text ="型号："+ record.Model;
			var source = record.Source;
			if (source == null) {
				tv_spec.text ="规格：未知";
			} else {
				tv_spec.text ="规格：" + source.RealL + "×" + source.RealW + "×" + source.RealH;
			}
//            tv_price.text ="价格：￥"+ record.price.ToString("F2");
			listenOnClick (img_image, record);
        }
    }
}
