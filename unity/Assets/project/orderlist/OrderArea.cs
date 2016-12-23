using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace project {
	
    public class OrderArea : OrderObject {

		private string mName;
		private IList<OrderObject> mOrders = new List<OrderObject> ();
		private float mTotalPrice;

		public OrderArea(string name) {
			mName = name;
        }

		public override void OnDelete() {
			base.OnDelete();
			mOrders.Clear();
		}

		public string Name {
			get {
				return mName;
			}
		}

		public IList<OrderObject> Orders {
			get {
				return mOrders;
			}
		}

		public void AddItem(Item item) {
			if (item == null) {
                return;
            }
			if (item.IsArch) {					
				var orderArchItem = getArchByItem(item);
				if (orderArchItem == null) {
					orderArchItem = new OrderArchItem();
					orderArchItem.ItemType = item.Type;
					mOrders.Add(orderArchItem);
				}
				orderArchItem.AddItem(item);
			} else {
				var orderItem = new OrderItem(item);
				mOrders.Add(orderItem);
			}
        }

		public void RemoveItem(Item item) {
			if (item == null || mOrders == null) {
                return;
            }
			if (item.IsArch) {
				var orderArchItem = getArchByItem(item);
				if (orderArchItem == null) {
					return;
				}
				mOrders.Remove(orderArchItem);
			} else {
	            var foundItem = getOrderByItem(item);
	            if (foundItem == null) {
	                return;
	            }
				mOrders.Remove(foundItem);
			}
        }

		public bool ReplaceItem(Item oldItem, Item newItem) {
			if (oldItem == null || newItem == null || mOrders == null) {
				return false;
			}
			foreach (var orderItem in mOrders) {
				if (orderItem is OrderItem) {
					var oi = orderItem as OrderItem;
					if (oi.Item == oldItem) {
						oi.Item = newItem;
						return true;
					}
				}
			}
			return false;
		}

		private OrderItem getOrderByItem (Item item) {
            OrderItem foundItem = null;
            foreach (var orderItem in mOrders) {
				if (orderItem is OrderItem) {
					var oi = orderItem as OrderItem;
					if (oi.Item == item) {
						foundItem = oi;
						break;
					}
				}
            }
            return foundItem;
        }

		private OrderArchItem getArchByItem(Item item) {
			OrderArchItem foundArchItem = null;
			foreach (var order in mOrders) {
				if (order is OrderArchItem) {
					var orderArchItem = order as OrderArchItem;
					if (orderArchItem.ItemType == item.Type) {
						foundArchItem = orderArchItem;
						break;
					}
				}
			}
			return foundArchItem;
		}

//		public float TotalPrice () {
//            if (mOrders == null) {
//                return 0.0f;
//            }
//            float total = 0.0f;
//			foreach (var orderItem in mOrders) {
//				var sku = orderItem.Item.SKU;
//				if (sku == null) {
//					continue;
//				}
//				total += sku.price;
//            }
//            return total;
//        }
    }
}