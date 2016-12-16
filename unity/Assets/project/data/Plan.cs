using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

namespace project {

	public enum PlanEditResult {
		Ok,
		NameConflict,
	}
	
	public class Plan : uapp.IObject {

		public int Id;
		public string Name;
		private float mScale = 1.0f;
		public uapp.IFile Background;
		public uapp.IFile File;
		public uapp.IFile Preview;
		public float PackagePrice;
		public uapp.IFile QRCode; // TODO 二维码文件，暂写死
		private PlanOrder mPlanOrder = new PlanOrder();

		// TODO 暂时这样
		public float FPSpx;
		public float FPSpz;
		public float FPSh = Photographer.DefaultFPSCameraHeight;
		public float FPSax;
		public float FPSay;
		public float BIRDpx;
		public float BIRDpz;
		public float BIRDh = Photographer.DefaultBirdCameraHeight;

		// 序列化到file
		private IList<Area> mAreas;
		private IList<Item> mArchItems;
		private IList<Item> mItems;

		//
		private GameObject mGameObject;
		private IList<Room> mRooms;

		private uapp.LightmapToggle mLightmapToggle;

		public void OnCreate() {
			
		}

		public void OnDelete() {
			if (mAreas != null) {
				mAreas.Clear();
			}
			if (mArchItems != null) {
				mArchItems.Clear();
			}
			if (mItems != null) {
				mItems.Clear();
			}
			if (mRooms != null) {
				foreach (var room in mRooms) {
					uapp.CoreUtils.Delete(room);
				}
				mRooms.Clear();
			}
			GameObject.Destroy(mGameObject);
			var file = File;
			if (file != null && file.Type == uapp.FileType.Scene) {
				UnityEngine.SceneManagement.SceneManager.UnloadScene(file.Path);
			}
			uapp.CoreUtils.Delete(mPlanOrder);
			if (mLightmapToggle != null) {
				mLightmapToggle.Clear();
				mLightmapToggle = null;
			}
		}

		public PlanOrder PlanOrder {
			get {
				return mPlanOrder;
			}
		}

		public GameObject Root {
			get {
				if (mGameObject == null) {
					mGameObject = new GameObject("plan");
				}
				return mGameObject;
			}
		}

		public float Scale {
			get {
				return mScale;
			}
			set {
				if (mScale == value) {
					return;
				}
				mScale = value;
				if (mRooms == null) {
					return;
				}
				foreach (var room in mRooms) {
					room.onPlanScaleChanged(mScale);
				}
			}
		}

		private int mCurrentFloorId = 1;
		public int GetFloorId() {
			int id = mCurrentFloorId;
			mCurrentFloorId++;
			return id;
		}

		private int mCurrentWallId = 1;
		public int GetWallId() {
			int id = mCurrentWallId;
			mCurrentWallId++;
			return id;
		}

//		public Floor AddFloor(SKU sku = null) {
//			if (sku == null) {
//				sku = SharedModel.Instance.DefaultFloor;
//			}
//			var source = sku.Source;
//			if (source == null || source.Type != SourceType.Material) {
//				return null;
//			}
//			if (mItems == null) {
//				mItems = new List<Item>();
//			}
//			var floor = new Floor(mCurrentFloorId, sku, Root);
//			mItems.Add(floor);
//			mCurrentFloorId++;
//			return floor;
//		}

		public PlanEditResult AddArea(string name, out Area outArea) {
			outArea = null;
			if (mAreas == null) {
				mAreas = new List<Area>();
			}
			foreach (var area in mAreas) {
				if (uapp.StringUtils.Equals(area.Name, name)) {
					outArea = area;
					return PlanEditResult.NameConflict;
				}
			}
			var newArea = new Area();
			newArea.Name = name;
			mAreas.Add(newArea);
			outArea = newArea;
			return PlanEditResult.Ok;
		}

		public PlanEditResult CreateRoom(string areaName, out Room outRoom) {
			outRoom = null;
			Area area = null;
			var result = PlanEditResult.Ok;
			if (areaName != null) { // 为空代表暂时先不定区域
				result = AddArea(areaName, out area);
				if (result != PlanEditResult.Ok) {
					return result;
				}
			}
			var floorSKU = SharedModel.Instance.DefaultFloor;
			var wallSKU = SharedModel.Instance.DefaultWall;
			if (mRooms == null) {
				mRooms = new List<Room>();
			}
			var room = new Room(this, area, floorSKU, wallSKU);
			mRooms.Add(room);
			outRoom = room;
			return result;
		}

		public void DestroyRoom(Room room) {
			if (mRooms == null) {
				return;
			}
			mRooms.Remove(room);
			uapp.CoreUtils.Delete(room);
		}

//		public Room RoomByPoint(Vector3 point) {
//			if (mRooms == null) {
//				return null;
//			}
//			foreach (var room in mRooms) {
//				if (room.IsPointIn(point)) {
//					return room;
//				}
//			}
//			return null;
//		}
//
//		public void SetRoomStateByPoint(Vector3 point, EditState state) {
//			if (mRooms == null) {
//				return;
//			}
//			foreach (var room in mRooms) {
//				if (room.IsPointIn(point)) {
//					room.Floor.EditState = EditState.Highlight;
//				} else {
//					room.Floor.EditState = EditState.Normal;
//				}
//			}
//		}

		public IList<Item> Items {
			get {
				return mItems;
			}
		}

		public bool PointByPoint(Vector3 point, out Room pointRoom, out int pointIndex) {
			return PointByPointExcept(point, null, out pointRoom, out pointIndex);
		}

        public bool PointByPointExcept (Vector3 point, Room exceptRoom, out Room pointRoom, out int pointIndex) {
            pointRoom = null;
            pointIndex = -1;
            if (mRooms == null) {
                return false;
            }
            foreach (var room in mRooms) {
                if (room == exceptRoom) {
                    continue;
                }
                pointIndex = room.PointByPoint (point);
                if (pointIndex >= 0) {
                    pointRoom = room;
                    return true;
                }
            }
            return false;
        }

        public EditItem EditItemByPointXZ(Vector3 point) {
			if (mRooms == null) {
				return null;
			}
			for (var i = mRooms.Count - 1; i >= 0; i--) {
				var room = mRooms[i];
				var editItem = room.WallItemsByPointXZ(point);
				if (editItem != null) {
					return editItem;
				}
			}
			//foreach (var room in mRooms) {
            for (var i = mRooms.Count - 1; i >= 0; i--) {
                var room = mRooms[i];
				var editItem = room.EditItemByPointXZ(point);
				if (editItem != null) {
					if (editItem == room.Floor) {
						editItem = room;
					}
					return editItem;
				}
			}
			return null;
		}

		public Room RoomByPointXZ(Vector3 point) {
			if (mRooms == null) {
				return null;
			}
			foreach (var room in mRooms) {
				if (room.IsPointInXZ(point)) {
					return room;
				}
			}
			return null;
		}

		public Wall WallByPointXZ(Vector3 point) {
			if (mRooms == null) {
				return null;
			}
			foreach (var room in mRooms) {
				var wall = room.WallByPointXZ(point);
				if (wall != null) {
					return wall;
				}
			}
			return null;
		}

		public void SetRoomSelectedPointIndex(Room room, int index) {
			if (mRooms == null) {
				return;
			}
			if (room == null) {
				foreach (var r in mRooms) {
					r.Floor.SelectedPointIndex = -1;
				}
			} else {
				foreach (var r in mRooms) {
					if (r == room) {
						r.Floor.SelectedPointIndex = index;
					} else {
						r.Floor.SelectedPointIndex = -1;
					}
				}
			}
		}

		public void SetEditItemState(EditItem editItem, EditState state, bool clearSelected = false) {
			if (mRooms == null) {
				return;
			}
			foreach (var room in mRooms) {
				room.SetEditItemState(editItem, state, clearSelected);
			}
		}

		public void ClearEditItemState(bool clearSelected = false) {
			if (mRooms == null) {
				return;
			}
			foreach (var room in mRooms) {
				room.ClearEditItemState(clearSelected);
			}
		}

		public EditPhase EditPhase {
			set {
				if (mRooms == null) {
					return;
				}
				foreach (var room in mRooms) {
					room.EditPhase = value;
				}
			}
		}

		public bool AddArchItem(Item item) {
			if (item == null || !item.IsArch) {
				return false;
			}
			if (mArchItems == null) {
				mArchItems = new List<Item>();
			}

			// NOTE 使它可点击
			var collider = item.GetComponent<MeshCollider>();
			if (collider == null) {
				collider = item.gameObject.AddComponent<MeshCollider>();
			}
			collider.sharedMesh = uapp.ObjectUtils.GetMesh(item.gameObject);

			mArchItems.Add(item);
			mPlanOrder.AddItem(item);
			return true;
		}

		public bool RemoveArchItem(Item item) {
			if (!item.IsArch) {
				return false;
			}
			if (mArchItems == null) {
				return true;
			}
			var b = mArchItems.Remove(item);
			if (b) {
				mPlanOrder.RemoveItem(item);
			}
			return b;
		}

		public Item AddItemByObject(GameObject itemObject) {
			// NOTE 外部加载物件必须添加Item
			var item = itemObject.GetComponent<Item>();
			if (item == null) {
				return null;
			}
			var sku = item.SKU;
			if (sku == null) {
				// 尝试查找sku
				SharedModel.Instance.BindSKU(item);
				sku = item.SKU;
				if (sku == null) {
					return null;
				}
			}
			var source = sku.Source;
			if (source == null || source.File == null) {
				return null;
			}
			addItemWithOrder(item);
			uapp.ObjectUtils.SetParent(itemObject, mGameObject);
			return item;
		}

		public Item AddItemBySKU(SKU sku) {
			if (sku == null) {
				return null;
			}
			var source = sku.Source;
			if (source == null || source.File == null) {
				return null;
			}
			var itemObject = source.File.GetContent<GameObject>(); // TODO 异步处理
			if (itemObject == null) {
				return null;
			}
			var item = Data.ItemFromObject(itemObject);
			item.SKU = sku;
			addItemWithOrder(item);
			return item;
		}

		private void addItemWithOrder(Item item) {
			if (addItem(item)) {
				mPlanOrder.AddItem(item);
			}
		}

		private bool addItem(Item item) {
			if (item == null || item.IsArch) {
				return false;
			}
			if (mItems == null) {
				mItems = new List<Item>();
			}

			// TODO 使它可点击，可以考虑换成MeshCollider
			var itemObject = item.gameObject;
            //var collider = itemObject.GetComponent<BoxCollider>();
            //if (collider == null) {
            //	collider = itemObject.AddComponent<BoxCollider>();
            //}
            //var bounds = uapp.ObjectUtils.GetBounds(itemObject);
            //collider.center = bounds.center;
            //collider.size = bounds.size;
            uapp.ObjectUtils.SetMeshCollider(itemObject, true);

            mItems.Add(item);

			var area = item.Area;
			if (area == null) { // TODO 实际情况应该是按照物件摆放的位置来决定它所属的区域，但逻辑还没有写，故这样处理一下
				if (!uapp.StringUtils.IsNullOrBlank (item.AreaName)) {
					AddArea (item.AreaName, out area);
					item.Area = area;
				}
			}
			return true;
		}

		public void DestroyItem(Item item) {
			destroyItemWithOrder(item);
		}

		private void destroyItemWithOrder(Item item) {
			if (destroyItem(item)) {
				mPlanOrder.RemoveItem(item);
			}
		}

		private bool destroyItem(Item item) {
			if (item == null) {
				return false;
			}
			if (mItems == null) {
				return false;
			}
			if (mItems.Remove(item)) {
				GameObject.Destroy(item.gameObject);
			}
			return true;
		}

		public void SetItemState(Item item, EditState state, bool clearSelected = false) {
			if (item == null) {
				ClearItemState(clearSelected);
			}
			if (mItems != null) {
				foreach (var it in mItems) {
					if (!it.CanEdit) {
						continue;
					}
					if (it == item) {
						var itemState = uapp.ObjectUtils.AddComponentIfNotExists<ItemState>(item.gameObject);
						if (clearSelected) {
							itemState.State = state;
						} else if (itemState.State != EditState.Selected) {
							itemState.State = state;
						}
					} else {
						var itemState = it.gameObject.GetComponent<ItemState>();
						if (itemState != null) {
							itemState.State = EditState.Normal;
						}
					}
				}
			}
			if (mArchItems != null) {
				foreach (var it in mArchItems) {
					if (!it.CanEdit) {
						continue;
					}
					if (it == item) {
						var itemState = uapp.ObjectUtils.AddComponentIfNotExists<ItemState>(item.gameObject);
						if (clearSelected) {
							itemState.State = state;
						} else if (itemState.State != EditState.Selected) {
							itemState.State = state;
						}
					} else {
						var itemState = it.gameObject.GetComponent<ItemState>();
						if (itemState != null) {
							itemState.State = EditState.Normal;
						}
					}
				}
			}
		}

		public void ClearItemState(bool clearSelected = false) {
			if (mItems != null) {
				foreach (var it in mItems) {
					var itemState = it.gameObject.GetComponent<ItemState>();
					if (itemState == null) {
						continue;
					}
					if (clearSelected) {
						itemState.State = EditState.Normal;
					} else if (itemState.State != EditState.Selected) {
						itemState.State = EditState.Normal;
					}
				}
			}
			if (mArchItems != null) {
				foreach (var it in mArchItems) {
					var itemState = it.gameObject.GetComponent<ItemState>();
					if (itemState == null) {
						continue;
					}
					if (clearSelected) {
						itemState.State = EditState.Normal;
					} else if (itemState.State != EditState.Selected) {
						itemState.State = EditState.Normal;
					}
				}
			}
		}

		public void ReplaceItem(Item oldItem, Item newItem) {
			destroyItem(oldItem);
			addItem(newItem);
			mPlanOrder.ReplaceItem(oldItem, newItem);
		}

		public Item ReplaceItemBySKU(Item oldItem, SKU sku) {
			if (oldItem == null || sku == null) {
				return null;
			}
			if (oldItem.SKU == sku) {
				return oldItem;
			}
			var source = sku.Source;
			if (source == null || source.File == null) {
				return oldItem;
			}
			Item item = null;
			switch (source.Type) {
				case SourceType.Model: {
						var itemObject = source.File.GetContent<GameObject>();
						if (itemObject == null) {
							item = oldItem;
							Debug.Log("[ItemNotFound] " + source.File.Path);
							break;
						}
						item = itemObject.GetComponent<Item>();
						// NOTE 外部加载物件必须添加Item
						if (item == null) {
							GameObject.Destroy(itemObject);
							item = oldItem;
							break;
						}

						item.Area = oldItem.Area;
						SharedModel.Instance.BindSKU(item);

						var oldItemObject = oldItem.gameObject;

                        // 新物件要放在旧物件的位置
//						var oldItemBottomCenter = uapp.BoundsUtils.BottomCenter(uapp.ObjectUtils.GetTransformBounds(oldItemObject)) - oldItemObject.transform.position;
//						var itemBottomCenter = uapp.BoundsUtils.BottomCenter(uapp.ObjectUtils.GetTransformBounds(itemObject));
						itemObject.transform.position = oldItemObject.transform.position;
                        //						itemObject.transform.Translate(-itemBottomCenter);
                        //						itemObject.transform.Translate(oldItemBottomCenter);

                        // 底部对齐旧物件
                        var oldItemBounds = uapp.ObjectUtils.GetTransformBounds(oldItemObject);
                        var oldItemBottomCenter = uapp.BoundsUtils.BottomCenter(oldItemBounds);
                        var itemBounds = uapp.ObjectUtils.GetTransformBounds(itemObject);
                        var itemBottomCenter = uapp.BoundsUtils.BottomCenter(itemBounds);
                        var itemPosition = itemObject.transform.position;
                        itemPosition.y += oldItemBottomCenter.y - itemBottomCenter.y;
                        itemObject.transform.position = itemPosition;

						// TODO 由于场景中有镜像物件，故这样处理一下，以后看需求
						var oldItemObjectScale = oldItemObject.transform.localScale;
						var itemObjectScale = itemObject.transform.localScale;
						itemObjectScale = Vector3.Scale(itemObjectScale, new Vector3(Mathf.Sign(oldItemObjectScale.x),Mathf.Sign(oldItemObjectScale.y),Mathf.Sign(oldItemObjectScale.z)));
						itemObject.transform.localScale = itemObjectScale;

						var oldItemEulerAngles = oldItemObject.gameObject.transform.eulerAngles;
						var itemEulerAngles = itemObject.gameObject.transform.eulerAngles;
						itemEulerAngles.y = oldItemEulerAngles.y;
						itemObject.gameObject.transform.eulerAngles = itemEulerAngles;

						uapp.ObjectUtils.SetParent(itemObject, Root);
//						DestroyItem(oldItem);
//						addItemWithOrder(item);
						ReplaceItem(oldItem, item);
						break;
					}
				case SourceType.Material: {
						var itemTexture = source.File.GetContent<Texture2D>();
						if (itemTexture == null) {
							item = oldItem;
							break;
						}
						item = oldItem;
						Material material = null;
						if (item.Material == null) {
							material = uapp.ObjectUtils.GetMaterial(item.gameObject);
							item.Material = GameObject.Instantiate(material);
							uapp.ObjectUtils.SetMaterial(item.gameObject, item.Material);
						}
						material = item.Material;
						if (material != null) {
							material.SetTexture(uapp.Shaders.Textures.Diffuse, itemTexture);
							itemTexture = source.File2 != null ? source.File2.GetContent<Texture2D>() : null;
							material.SetVector(uapp.Shaders.Textures.DiffuseST, new Vector4(source.TillingX, source.TillingY, 0.0f, 0.0f));
							material.SetTexture(uapp.Shaders.Textures.Bump, itemTexture);
							itemTexture = source.File3 != null ? source.File3.GetContent<Texture2D>() : null;
							material.SetTexture(uapp.Shaders.Textures.Metallic, itemTexture);
							itemTexture = source.File4 != null ? source.File4.GetContent<Texture2D>() : null;
							material.SetTexture(uapp.Shaders.Textures.DetailDiffuse, itemTexture);
							itemTexture = source.File5 != null ? source.File5.GetContent<Texture2D>() : null;
							material.SetTexture(uapp.Shaders.Textures.DetailBump, itemTexture);
						}
						item.SKU = sku;
						break;
					}
			}
			return item;
		}

		public void SetAllCeilItemsVisible(bool visible) {
			if (mArchItems != null) {
				foreach (var archItem in mArchItems) {
					if (archItem.Type == ItemType.Ceil) {
						archItem.gameObject.SetActive(visible);
					}
				}
			}
			if (mItems != null) {
				foreach (var item in mItems) {
					if (item.Type == ItemType.CeilItem) {
						item.gameObject.SetActive(visible);
					}
				}
			}
		}

		public void HandlePrototype(GameObject prototype) {
			if (prototype == null) {
				return;
			}
			mGameObject = prototype;
			mGameObject.name = "plan";
			uapp.ObjectUtils.EnumChild(mGameObject, (GameObject child) => {
				var item = child.GetComponent<Item>();
				if (item == null) {
					return false;
				}
				if (!uapp.StringUtils.IsNullOrBlank(item.AreaName)) {
					Area area;
					AddArea(item.AreaName, out area);
					item.Area = area;
				}
				//if (!uapp.StringUtils.IsNullOrBlank(item.SKUName)) {
				//	item.SKU = SharedModel.Instance.GetLocalSKU(item.SKUName);
				//}
                SharedModel.Instance.BindSKU (item);
				if (item.IsArch) {
					AddArchItem(item);
				} else {
                    addItemWithOrder (item);
				}
//				uapp.ObjectUtils.EnumMesh(item.gameObject, (GameObject gameObject, Mesh mesh, int submeshIndex, Material material) => {
//					
//				});
				return false;
			}, true);
		}

		public void HandlePrototype(Scene scene) {
			if (!scene.isLoaded) {
				return;
			}
			var gameObjects = scene.GetRootGameObjects();
			foreach (var gameObject in gameObjects) {
//				SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
				HandlePrototypeObject(gameObject);
			}
		}

		public void HandlePrototypeObject(GameObject gameObject) {
			uapp.ObjectUtils.EnumChild(gameObject, (GameObject child) => {
				var item = child.GetComponent<Item>();
				if (item == null || !item.IsValid) {
					GameObject.Destroy(item);
					return false;
				}

				// 父节点有Item，忽略自己
				var parent = uapp.ObjectUtils.GetParent(child);
				if (parent != null && parent.GetComponent<Item>() != null) {
					GameObject.Destroy(item);
					return false;
				}

				if (!uapp.StringUtils.IsNullOrBlank(item.AreaName)) {
					Area area;
					AddArea(item.AreaName, out area);
					item.Area = area;
				}
				SharedModel.Instance.BindSKU(item);
				if (item.IsArch) {
					AddArchItem(item);
				} else {
					addItemWithOrder(item);
				}
				return false;
			}, true);
		}

		public Area GetArea(string name) {
			if (mAreas == null) {
				return null;
			}
			foreach (var area in mAreas) {
				if (uapp.StringUtils.Equals(area.Name, name)) {
					return area;
				}
			}
			return null;
		}

		private IList<Item> mFindItems;
		public IList<Item> FindItemsOfArea(Area area) {
			if (area == null) {
				return null;
			}
			if (mFindItems == null) {
				mFindItems = new List<Item>();
			}
			mFindItems.Clear();
			uapp.ObjectUtils.EnumChild(mGameObject, (GameObject child) => {
				var item = child.GetComponent<Item>();
				if (item == null) {
					return false;
				}
				if (item.Area == area) {
					mFindItems.Add(item);
				}
				return false;
			});
			if (mFindItems.Count == 0) {
				return null;
			}
			return mFindItems;
		}
		
		public IList<SKU> GetOrderListSKU () {
            IList<SKU> skuOrderList = new List<SKU> ();
            foreach (var item in mItems) {
                if (item.SKU == null) {
					skuOrderList.Add( SharedModel.Instance.BindSKU (item).SKU);
                } else {
                    skuOrderList.Add (item.SKU);
                }
            }
            return skuOrderList;
        }

		public bool LightsOnOff {
			get {
				if (mLightmapToggle == null) {
					return false;
				}
				return mLightmapToggle.OnOff;
			}
			set {
				if (mLightmapToggle == null) {
					mLightmapToggle = new uapp.LightmapToggle();
				}
				mLightmapToggle.OnOff = value;
				uapp.SceneUtils.SetCurrentAllLightsOnOff(value);
				var file = File;
				if (file != null && file.Type == uapp.FileType.Scene) {
					uapp.SceneUtils.SetAllLightsOnOff(UnityEngine.SceneManagement.SceneManager.GetSceneByName(file.Path), value);
				}
			}
		}

		public Bounds TransformBounds {
			get {
				var file = File;
				if (file != null && file.Type == uapp.FileType.Scene) {
					var scene = UnityEngine.SceneManagement.SceneManager.GetSceneByName(file.Path);
					return uapp.SceneUtils.GetTransformBounds(scene);
				}
				return uapp.Constants.BoundsZero;
			}
		}
	}
}
