using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace project {
	
    public class OrderListAreaItem : uapp.ListItem<OrderObject> {

		public Text tv_name;

        public override void SetRecord (OrderObject record) {
			var orderArea = record as OrderArea;
            //tv_name.text = uapp.StringUtils.SeparateBy (orderArea.Name, "   ");
            tv_name.text = orderArea.Name;
        }
    }
}
