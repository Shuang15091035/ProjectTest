using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace project {
    
	public class ItemList : ItemListBase {

		private GameObject mItemObject;
		private Vector3 mItemOffset;

		public override void OnStateIn() {
			base.OnStateIn();

			CategoryItemType = ItemType.Item;

			btn_add_item.onValueChanged.RemoveAllListeners ();
			btn_add_item.onValueChanged.AddListener ((bool onOff) => {
				uapp.ToggleEx.SetToggleOnOff (btn_add_item, onOff);
				if (!onOff) {
					ParentMachine.RevertState ();
				}
			});

			//
			hideStatusbar();
		}

		public override void OnStateOut() {
			base.OnStateOut();

            btn_add_item.onValueChanged.RemoveAllListeners ();
            btn_add_item.isOn = false;
			uapp.ToggleEx.SetToggleOnOff(btn_add_item, false);
		}

		protected override void onSKUSelected(SKU sku) {
			base.onSKUSelected(sku);
			if (mItemObject != null) {
				GameObject.Destroy(mItemObject);
			}
			mItemObject = createObject (sku);
		}

		protected override void OnScreenMouseMove(uapp.MouseEvent e) {
			base.OnScreenMouseMove(e);

			if (e.button == uapp.Buttons.None || e.button == uapp.Buttons.Left) {
				if (mItemObject != null) {
					Vector3 point;
					if (Picker.RaycastGround(e.position, out point)) {
//						var bounds = uapp.ObjectUtils.GetScaleBounds (mItemObject);
//						var offset = uapp.BoundsUtils.BottomCenter (bounds);
//						offset = -offset;
//						print(offset);
////						offset.y = 0.0f;
//						mItemObject.transform.position = point + offset;
						mItemObject.transform.position = point + mItemOffset;
					}
//					Vector3 offset;
//					if (getGroundOffset(e.position, out offset)) {
//						mItemObject.transform.position += offset;
//					}
				}
			}
		}

//		protected override void OnScreenMouseUp(uapp.MouseEvent e) {
//			base.OnScreenMouseUp(e);
//
//			if (e.button == uapp.Buttons.Left) {
//				if (mItemObject != null) {
//					SharedModel.Instance.CurrentPlan.AddItemByObject(mItemObject);
//					mItemObject = null;
//				} else {
//					var item = Picker.RaycastItem(e.position);
//					if (item != null) {
//						if (item.Type == ItemType.Unknown) {
//							return;
//						}
//						SharedModel.Instance.CurrentItem = item;
//						//						if (item.IsArch) {
//						//							ParentMachine.ChangeState (States.ItemMaterialEdit, false);
//						//						} else if (item.Type == ItemType.Item) {
//						//							ParentMachine.ChangeState (States.ItemEdit, false);
//						//						}
//						ParentMachine.ChangeState (States.ItemEdit, false);
//					} else {
//						ParentMachine.RevertState();
//					}
//				}
//			}
//		}

		protected override void onItemClick(Item item) {
			if (mItemObject != null) {
				SharedModel.Instance.CurrentPlan.AddItemByObject(mItemObject);
				mItemObject = null;
				return;
			}
			base.onItemClick(item);
			if (item == null || !item.CanEdit) {
				ParentMachine.RevertState();
				return;
			}
			SharedModel.Instance.CurrentItem = item;
			ParentMachine.ChangeState (States.ItemEdit, false);
		}

//		protected override void OnScreenMouseClick(uapp.MouseEvent e) {
//			base.OnScreenMouseClick(e);
//
//			if (e.button == uapp.Buttons.Left) {
//				if (mItemObject == null) { // 没有拖动物件时点到物件，等于编辑物件
//					var item = Picker.RaycastItem(e.position);
//					if (item != null) {
//						if (item.Type == ItemType.Unknown) {
//							return;
//						}
//						SharedModel.Instance.CurrentItem = item;
////						if (item.IsArch) {
////							ParentMachine.ChangeState (States.ItemMaterialEdit, false);
////						} else if (item.Type == ItemType.Item) {
////							ParentMachine.ChangeState (States.ItemEdit, false);
////						}
//						ParentMachine.ChangeState (States.ItemEdit, false);
//					}
//				}
//			}
//		}

		private GameObject createObject(SKU sku) {
			mItemOffset = Vector3.zero;
			if (sku == null) {
				return null;
			}
			var source = sku.Source;
			if (source == null || source.File == null) {
				return null;
			}
            if (source.Type == SourceType.Material) {
                return null;
            }
			var itemObject = source.File.GetContent<GameObject>();
			if (itemObject == null) {
				Debug.Log("[ItemNotFound] " + source.File.Path);
			}
			var bounds = uapp.ObjectUtils.GetTransformBounds(itemObject);
			var offset = uapp.BoundsUtils.BottomCenter(bounds);
			mItemOffset = -offset;
			return itemObject;
        }

	}
}
