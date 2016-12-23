using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace project {
	
	public class ItemReplace : PlanEditBase {

		private Button btn_close;
		private ReplaceListView lv_replace;

		private Item mItem;
		private IList<Item> mItems;

		public override bool OnPreCondition() {
			mItem = SharedModel.Instance.CurrentItem;
			return mItem != null;
		}

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);

			btn_close = findViewById<Button>("btn_close");
			btn_close.onClick.AddListener(() => {
				ParentMachine.RevertState();
			});

			lv_replace = findViewById<ReplaceListView>("lv_replace");
			lv_replace.Adapter = new ReplaceListAdapter();
			lv_replace.Adapter.OnItemClick = (SKU sku, int position, GameObject v, uapp.ListView<SKU> listView) => {
				replaceItemBySKU(mItem, sku);
			};

			return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			var item = mItem;
			SharedModel.Instance.CurrentPlan.SetItemState(item, EditState.Selected);
//			var areas = SharedModel.Instance.LocalAreas;
//			if (areas == null || areas.Count == 0) {
//				return;
//			}
//			var categories = SharedModel.Instance.LocalCategoriesByItemType(item.Type);
//			if (categories == null || categories.Count == 0) {
//				return;
//			}
//			var area = item.Area != null ? item.Area.Name : areas[0];
//			var category = item.SKU != null ? item.SKU.Category : categories[0];
			var area = item.Area != null ? item.Area.Name : null;
			string category = null;
			var sku = item.SKU;
			if (sku != null) {
				category = sku.Category;
			} else {
				category = SharedModel.Instance.CategoryByItemType(item.Type);
			}
			var skus = SharedModel.Instance.GetSkuByAreaCategory(area, category);
			if (skus == null || skus.Count == 0) {
				if (item.IsGenericItem) {
					skus = SharedModel.Instance.GetSkuByAreaCategory(null, category);
				}
			}
			lv_replace.Adapter.Data = skus;
			lv_replace.Adapter.NotifyDataSetChanged();

			//
			hideStatusbar();
		}

		private Item replaceItemBySKU (Item item, SKU sku) {
			if (item == null || sku == null) {
				return null;
			}
			var newItem = SharedModel.Instance.CurrentPlan.ReplaceItemBySKU (item, sku);
			if (newItem != null && newItem != item) {
				SharedModel.Instance.CurrentItem = newItem;
				mItem = SharedModel.Instance.CurrentItem;
			}
			return newItem;
		}

		protected override void onItemClick(Item item) {
			base.onItemClick(item);
			if (item != mItem) {
				if (item == null || !item.CanEdit) {
					ParentMachine.RevertState();
					return;
				} else {
					SharedModel.Instance.CurrentItem = item;
					ParentMachine.ChangeState(States.ItemEdit, false);
					return;
				}
			}
		}
	}

}
