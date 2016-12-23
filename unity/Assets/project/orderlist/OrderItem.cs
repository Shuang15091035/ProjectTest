using UnityEngine;
using System.Collections;

namespace project {
	
	public class OrderItem : OrderObject {

		private Item mItem;

		public OrderItem(Item item) {
			mItem = item;
		}

		public override void OnDelete() {
			base.OnDelete();
			mItem = null;
		}

		public Item Item {
			get {
				return mItem;
			} set {
				mItem = value;
			}
		}
	}
}
