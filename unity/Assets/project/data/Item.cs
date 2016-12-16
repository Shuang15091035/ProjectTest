using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace project {

	public enum ItemType {
		Unknown,
		Item,
		Wall,
		Floor,
		Ceil,
        Curtain,
		CeilItem,
		DoorStone,
    }

	public enum ItemPosition {
		Unknown,
		Ground,
		Top,
		OnItem,
		InWall,
		OnWall,
	}
	
	public class Item : MonoBehaviour {

		public ItemType Type = ItemType.Unknown;

		public bool IsValid {
			get {
				return Type != ItemType.Unknown;// && SKUId != 0;
			}
		}

		public bool IsArch {
			get {
				return Type == ItemType.Floor || Type == ItemType.Wall || Type == ItemType.Ceil|| Type == ItemType.DoorStone;
			}
		}

		public bool IsModelItem {
			get {
				return Type == ItemType.Item;
			}
		}

		public bool IsMaterialItem {
			get {
				return Type == ItemType.Floor || Type == ItemType.Wall || Type == ItemType.Ceil || Type == ItemType.CeilItem || Type == ItemType.Curtain|| Type == ItemType.DoorStone;
			}
		}

		public bool IsColorItem {
			get {
				return Type == ItemType.Floor || Type == ItemType.Wall || Type == ItemType.Ceil || Type == ItemType.CeilItem|| Type == ItemType.DoorStone;
			}
		}

		public bool IsGenericItem { // NOTE 泛型物件是指那些区域定义模糊的物件，例如窗帘、地板等，无需严格要求匹配区域信息
			get {
				return Type == ItemType.Floor || Type == ItemType.Wall || Type == ItemType.Ceil || Type == ItemType.Curtain|| Type == ItemType.DoorStone;
			}
		}

		public bool CanEdit {
			get {
				return !(Type == ItemType.Unknown || Type == ItemType.CeilItem);
			}
		}

		// for 预编辑资源
		public string AreaName;
		public int SKUId;

		// for 程序逻辑
		public int Id;
		private Area mArea;
		public SKU SKU;

		public IList<float> Data;
		public int DepId;
		public int Dep2Id;

		private EditItem mEditItem; // 表示该物件为DIY物件
		private Material mMaterial; // 物件的材质实例缓存

		public Item(int id, SKU sku = null) {
			this.Id = id;
			this.SKU = sku;
		}

		public Area Area {
			get {
				if (mEditItem != null && mEditItem is RoomItem) {
					var roomItem = mEditItem as RoomItem;
					var room = roomItem.Room;
					if (room != null) {
						return room.Area;
					}
				}
				return mArea;
			} set {
				mArea = value;
			}
		}

		public EditItem EditItem {
			get {
				return mEditItem;
			} set {
				mEditItem = value;
			}
		}

		public Material Material {
			get {
				return mMaterial;
			} set {
				mMaterial = value;
			}
		}

//		public void Copy(Item other) {
//			this.Type = other.Type;
//			this.Area = other.Area;
//			this.SKU = other.SKU;
//		}

	}
}
