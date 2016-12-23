using UnityEngine;
using System.Collections;

namespace project {
	
    public enum OrderListViewType {
        OrderListViewTypeArea = 0,
        OrderListViewTypeItem,

        OrderListViewTypeNum,

    }

    public class OrderListAdapter : uapp.ListAdapter<OrderObject> {

        public override int GetViewTypeAt (int position) {
            var orderObject = GetItemAt (position);
            if (orderObject is OrderArea) {
                return (int)OrderListViewType.OrderListViewTypeArea;
            } else if(orderObject is OrderItem){
                return (int)OrderListViewType.OrderListViewTypeItem;
			} else if(orderObject is OrderArchItem){
				return (int)OrderListViewType.OrderListViewTypeItem;
			} 
            return 0;
        }
    }
}
