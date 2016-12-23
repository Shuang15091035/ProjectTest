using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections;

namespace project {

	public class OrderList : AppBase {

		private Button btn_back;
        private Button btn_close;
		private Toggle btn_add_item;
		private OrderListView lv_order_list;
		protected ToggleGroup lo_camera;

		private Plan mPlan;

        public override GameObject OnCreateView (GameObject parentView) {
            var view = base.OnCreateView (parentView);

			btn_back = findViewById<Button>("btn_back", null, App.Canvas);
            btn_close = findViewById<Button>("btn_close");
            
			btn_add_item = findViewById<uapp.ToggleEx> ("btn_add_item", null, parentView);

			lv_order_list = findViewById<OrderListView>("lv_order_list");
			lv_order_list.Adapter = new OrderListAdapter ();
            btn_close.onClick.AddListener (() => {
                ParentMachine.RevertState ();
            });

			lo_camera = findViewById<ToggleGroup>("lo_camera", null, parentView);

            return view;
        }

		public override bool OnPreCondition() {
			mPlan = SharedModel.Instance.CurrentPlan;
			return mPlan != null;
		}

        public override void OnStateIn () {
            base.OnStateIn ();

			//
			btn_back.gameObject.SetActive(false);
			lo_order_list_toolbar.gameObject.SetActive(false);
			btn_add_item.gameObject.SetActive(false);

			updateOrderList();
			lv_order_list.Adapter.OnItemEvent = (int what, OrderObject orderObject, int position, GameObject v, uapp.ListView<OrderObject> listView) => {
				if (what == (int)OrderListItem.OrderListEventReplace) {
					var orderItem = orderObject as OrderItem;
					var item = orderItem.Item;
					SharedModel.Instance.CurrentItem = item;
					ParentMachine.ChangeState(States.ItemReplace);
				}
			};

			showTotal(mPlan.PackagePrice);
            float et_unit_price_text = 0.0f ;
            float area = 0.0f;
            et_unit_price.onValueChanged.RemoveAllListeners ();
            et_unit_price.onValueChanged.AddListener ((string text) => {
                if (uapp.StringUtils.IsNullOrBlank (text)) {
                    tv_total.text = " = ?";
                    return;
                }
                et_unit_price_text = uapp.CoreUtils.ParseFloat (text, 0.0f);
                tv_total.text = " = " + (et_unit_price_text * area).ToString ("F2");
                
            });
            et_area.onValueChanged.RemoveAllListeners();
			et_area.onValueChanged.AddListener((string text) => {
				if (uapp.StringUtils.IsNullOrBlank(text)) {
					tv_total.text = " = ?";
					return;
				}

                area = uapp.CoreUtils.ParseFloat(text, 0.0f);
				tv_total.text = " = " + (et_unit_price_text * area).ToString("F2");
			});
            
            btn_done.gameObject.SetActive (false);
			//btn_done.onClick.RemoveAllListeners();
			//btn_done.onClick.AddListener(() => {
			//	ParentMachine.RevertState();
			//});

			lo_camera.gameObject.SetActive(false);
        }

//		protected override void onSKUSelected(SKU sku) {
//			base.onSKUSelected(sku);
//			mReplaceItem = replaceItemBySKU (mReplaceItem, sku);
//			updateOrderList();
//		}

		private void updateOrderList() {
			var order = mPlan.PlanOrder;
			lv_order_list.Adapter.Data = order.AreaAndItems ();
			lv_order_list.Adapter.NotifyDataSetChanged();
//			tv_total.text = "合计：￥" + order.TotalPrice().ToString("F2");
		}
    }
}
