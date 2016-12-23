using UnityEngine;
using System.Collections;

namespace project {
	
	public class ItemMaterialEdit : uapp.AppState {

//		private ItemMaterialListAdapter mItemMaterialListAdapter;
//		private Item mItem;
//
//		public override bool OnPreCondition() {
//			mItem = SharedModel.Instance.CurrentItem;
//			return mItem != null;
//		}
//
//		public override GameObject OnCreateView(GameObject parentView) {
//			var view = base.OnCreateView(parentView);
//
//			mItemMaterialListAdapter = new ItemMaterialListAdapter();
//			var lv_items = findViewById<ItemListView>("lv_items");
//			lv_items.Adapter = mItemMaterialListAdapter;
//			uapp.BaseOnItemClickListener<SKU> itemListListener = new uapp.BaseOnItemClickListener<SKU>();
//			itemListListener.OnItemClickEvent = (SKU sku, int position, GameObject game, uapp.ListView<SKU> listView) => {
//				replaceItemMaterial(sku);
//			};
//			mItemMaterialListAdapter.OnItemClick = itemListListener;
//
//			return view;
//		}
//
//		private void replaceItemMaterial(SKU sku) {
//			switch (mItem.Type) {
//				case ItemType.Floor:
//					{
//						uapp.ObjectUtils.EnumMesh(mItem.gameObject, (GameObject gameObject, Mesh mesh, int submeshIndex, Material material) => {
//							var meshRenderer = gameObject.GetComponent<MeshRenderer>();
//
//						}, true);
//						break;
//					}
//				case ItemType.Wall:
//					{
//						var walls = SharedModel.Instance.CurrentPlan.FindItemsOfArea(mItem.Area);
//						if (walls != null) {
//							foreach (var wall in walls) {
//								
//							}
//						}
//						break;
//					}
//			}
//		}
	}
}
