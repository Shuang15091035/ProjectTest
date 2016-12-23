using UnityEngine;
using System.Collections;

namespace project {
	
	public class ItemListBase : PlanEditBase {

		protected AreaListView lv_areas;
		protected CategoryListView lv_categories;
		protected SKUListView lv_skus;

		private ItemType mCategoryItemType;

		protected ItemType CategoryItemType {
			get {
				return mCategoryItemType;
			} set {
				mCategoryItemType = value;
			}
		}

		// 区域列表被选中
		protected virtual void onAreaSelected(string area) {}
		// 分类列表被选中
		protected virtual void onCategorySelected(string category) {}
		// sku列表被选中
		protected virtual void onSKUSelected(SKU sku) {}

		public override bool OnPreCondition() {
			if (SharedModel.Instance.CurrentPlan == null) {
				return false;
			}
			return true;
		}

		public override GameObject OnCreateView(GameObject parentView) {
			var view = base.OnCreateView(parentView);
			setupUi(parentView);
			return view;
		}

		public override void OnStateIn() {
			base.OnStateIn();

			lv_areas.Adapter.OnItemClick = (string area, int position, GameObject v, uapp.ListView<string> listView) => {
				showCategories(area);
				hideSkus();
				SharedModel.Instance.CurrentArea = area;
				onAreaSelected(area);
			};

			lv_categories.Adapter.OnItemClick = (string category, int position, GameObject v, uapp.ListView<string> listView) => {
				showSkus(SharedModel.Instance.CurrentArea, category);
				SharedModel.Instance.CurrentCategory = category;
				onCategorySelected(category);
			};

			lv_skus.Adapter.OnItemClick = (SKU sku, int position, GameObject v, uapp.ListView<SKU> listView) => {
				onSKUSelected(sku);
			};

			lv_areas.Adapter.Data = SharedModel.Instance.LocalAreas;
			lv_areas.Adapter.NotifyDataSetChanged();
		}

		protected void setupUi(GameObject parentView) {
			// 一级菜单
			lv_areas = findViewById<AreaListView>("lv_areas", null, parentView);
			if (lv_areas.Adapter == null) {
				lv_areas.Adapter = new AreaListAdapter();
			}
			lv_areas.gameObject.SetActive(true);

			// 二级菜单
			lv_categories = findViewById<CategoryListView>("lv_categories", null, parentView);
			if (lv_categories.Adapter == null) {
				lv_categories.Adapter = new CategoryListAdapter();
			}
			lv_categories.gameObject.SetActive(false);

			// 三级菜单
			lv_skus = findViewById<SKUListView>("lv_skus", null, parentView);
			if (lv_skus.Adapter == null) {
				lv_skus.Adapter = new SKUListAdapter();
			}
			lv_skus.gameObject.SetActive(false);
		}

		protected void resetSKUUi() {
			var areas = SharedModel.Instance.LocalAreas;
			lv_areas.Adapter.Data = areas;
			lv_areas.Adapter.NotifyDataSetChanged();
			lv_areas.gameObject.SetActive(true);
			lv_categories.gameObject.SetActive(false);
			SharedModel.Instance.CurrentArea = null;
			SharedModel.Instance.CurrentCategory = null;
		}

		protected void showCategories(string area) {
			var categories = SharedModel.Instance.LocalCategoriesByArea(area);
			if (categories == null || categories.Count == 0) {
				return;
			}
			lv_categories.Adapter.Data = categories;
			lv_categories.Adapter.NotifyDataSetChanged();
			lv_categories.gameObject.SetActive(true);
		}

		protected void hideCategories() {
			lv_categories.gameObject.SetActive(false);
			lv_skus.gameObject.SetActive(false);
		}

		protected void showSkus(string area, string category) {
			if (area == null || category == null) {
				return;
			}
			var needToShowSkus = true;
			if (area != SharedModel.Instance.CurrentArea || category != SharedModel.Instance.CurrentCategory) { // 需要刷新sku列表
				var skus = SharedModel.Instance.GetSkuByAreaCategory(area, category);
				if (skus == null || skus.Count == 0) {
					needToShowSkus = false;
				} else {
					var skuListAdapter = lv_skus.Adapter;
                    if (skus.Count%2 == 1) {
                        SKU sku = new SKU();
                        skus.Add(sku);
                    }
					skuListAdapter.Data = skus;
					skuListAdapter.NotifyDataSetChanged();
				}
			}
			if (needToShowSkus) {
				lv_categories.gameObject.SetActive(true);
			}
			lv_skus.gameObject.SetActive(needToShowSkus);
			SharedModel.Instance.CurrentArea = area;
			SharedModel.Instance.CurrentCategory = category;
		}

		protected void hideSkus() {
			lv_skus.gameObject.SetActive (false);
		}

	}

}
