using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace project {
	
	public class OrderArchItem : OrderObject {

		private ItemType mItemType;
		private List<Item> mItems;

		public ItemType ItemType {
			get {
				return mItemType;
			}
			set {
				mItemType = value;
			}
		}

		public List<Item> Items {
			get {
				return mItems;
			}
		}

		public void AddItem(Item item) {
			if (mItems ==  null) {
				mItems = new List<Item>();
			}
			mItems.Add(item);
		}

		public void RemoveItem(Item item) {
			if (mItems == null) {
				return;
			}
			mItems.Remove(item);
		}

		public float AreaSize {
			get {
				if (mItems == null) {
					return 0.0f;
				}
				float areaSize = 0.0f;
				foreach (var item in mItems) {
					var editItem = item.EditItem;
					if (editItem != null) {
						areaSize += editItem.ActualAreaSize;
					} else {
						areaSize += uapp.ObjectUtils.GetArea(item.gameObject, true); // TODO 返回实际大小
					}
				}
				return areaSize;
			}
		}

	}

}
