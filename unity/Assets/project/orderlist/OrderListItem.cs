using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace project {
	
    public class OrderListItem : uapp.ListItem<OrderObject> {

		public static int OrderListEventDelete = 1;
		public static int OrderListEventReplace = 2;
		
        public Image img_image;
        public Text tv_name;
		public Text tv_category;
		public Text tv_brand;
		public Text tv_model;
		public Text tv_spec;
        public Button btn_delete;
		public Button btn_replace;

        public override void SetRecord (OrderObject record) {
			if (record is OrderItem) {
				var orderItem = record as OrderItem;
				setItem(orderItem.Item);
			} else if (record is OrderArchItem) {
				var orderArchItem = record as OrderArchItem;
				if (orderArchItem.Items == null || orderArchItem.Items.Count == 0) {
					return;
				}
				var item = orderArchItem.Items[0];
				setItem(item, orderArchItem);
			}

			sendEventOnClick(OrderListEventDelete, record, btn_delete);
			sendEventOnClick(OrderListEventReplace, record, btn_replace);
        }

		private void setItem(Item item, OrderArchItem orderArchItem = null) {
			var sku = item.SKU;
			if (sku == null) {
				// TODO
				return;
			}
			var source = sku.Source;
			if (source == null) {
				// TODO
				return;
			}

			Sprite sprite = null;
			if (sku.Preview != null) {
				sprite = sku.Preview.GetContent<Sprite>();
			}
			img_image.sprite = sprite != null ? sprite : SharedModel.Instance.PreviewNotFound.GetContent<Sprite>();

			tv_name.text = sku.Name;
			tv_category.text = sku.Category;
			tv_brand.text = sku.Brand;
			tv_model.text = sku.Model;
			switch (item.Type) {
				case ItemType.Item:
					tv_spec.text = source.RealL.ToString() + "×" + source.RealW.ToString() + "×" + source.RealH.ToString();
					break;
				case ItemType.Floor:
				case ItemType.Wall:
					tv_spec.text = orderArchItem.AreaSize.ToString("F2") + "m²";
					break;
			}
		}

    }
}