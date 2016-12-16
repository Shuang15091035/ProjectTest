using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace project {
	
	public class PlanOrder : uapp.IObject {
		
        public IList<OrderArea> mOrderAreas = new List<OrderArea>();
        public IList<OrderItem> mOrderItems = new List<OrderItem> ();
		public IList<OrderObject> mTempAreaAndItems = new List<OrderObject> ();

		public void OnCreate() {
			
		}

		public void OnDelete() {
			uapp.CoreUtils.DeleteList(mOrderAreas);
			uapp.CoreUtils.DeleteList(mOrderItems);
			mTempAreaAndItems.Clear();
		}

		public void AddItem(Item item) {
			if (item == null || item.Area == null) {
                return;
            }
			var sku = item.SKU;
			if (sku == null) {
				return;
			}
			OrderArea foundArea = getAreaByItem (item);
            if (foundArea == null) {
				foundArea = new OrderArea(item.Area.Name);
                mOrderAreas.Add(foundArea);
            }
			foundArea.AddItem(item);
        }

		public void RemoveItem(Item item) {
			if (item == null || mOrderAreas == null) {
                return;
            }
			OrderArea foundArea = getAreaByItem(item);
            if (foundArea == null) {
                return;
            }
			foundArea.RemoveItem(item);
        }

		public bool ReplaceItem(Item oldItem, Item newItem) {
			if (oldItem == null || oldItem.Area == null) {
				return false;
			}
			if (newItem == null || newItem.Area == null) {
				return false;
			}
			if (newItem.Area != oldItem.Area) {
				return false;
			}
			OrderArea area = getAreaByItem (oldItem);
			if (area == null) {
				return false;
			}
			return area.ReplaceItem(oldItem, newItem);
		}

		public OrderArea getAreaByItem(Item item) {
			if (item == null || item.Area == null) {
				return null;
			}
            OrderArea foundArea = null;
            foreach (var orderArea in mOrderAreas) {
				if (item.Area.Name == orderArea.Name) {
                    foundArea = orderArea;
                    break;
                }
            }
            return foundArea;
        }

//        public float TotalPrice() {
//            if (mOrderAreas == null) {
//                return 0.0f;
//            }
//            float total = 0.0f;
//            foreach (var orderArea in mOrderAreas) {
//				total += orderArea.TotalPrice ();
//            }
//            return total;
//        }

        public IList<OrderObject> AreaAndItems () {
            if (mOrderAreas == null) {
                return null;
            }
            mTempAreaAndItems.Clear ();
            foreach (var orderArea in mOrderAreas) {
                var orders = orderArea.Orders;
                if (orders == null || orders.Count == 0) {
                    continue;
                }
                mTempAreaAndItems.Add (orderArea);
                foreach (var order in orders) {
                    mTempAreaAndItems.Add (order);
                }
            }
            return mTempAreaAndItems;
        }
    }
}