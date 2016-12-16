using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace project {

	public interface ISharedModel {

		uapp.IFile CurrentPlanBg { get; set; }
		Plan CurrentPlan { get; set; }
		RoomShape CurrentRoomShape { get; set; }
		Room CurrentRoom { get; set; }
		Wall CurrentWall { get; set; }
		InWallItem CurrentInWallItem { get; set; }
		Item CurrentItem { get; set; }
		Photographer Photographer { get; }
		uapp.ICameraPrefab WillChangeToThisCamera { get; set; }
		GameObject PlanBackgroundObject { get; }
	
		List<string> DefaultAreaNames { get; }
		SKU DefaultFloor { get; }
		SKU DefaultWall { get; }
		SKU DefaultDoor { get; }

		uapp.IFile LocalAssetsDir { get; }
		uapp.IFile LocalProjectDir { get; }
		IList<Plan> LocalPrototypes { get; }
        uapp.IFile PlanBgsDir { get; }

        IList<Plan> LocalPlans { get; }
		IList<uapp.IFile> LocalPlanBgs { get; set; }
		IList<string> LocalAreas { get; }
//		IList<string> LocalCategories { get; }
		IList<string> LocalCategoriesByArea(string area);
//		IList<string> LocalCategoriesByItemType(ItemType itemType);
		string CategoryByItemType(ItemType itemType);
		IList<SKU> LocalSKUs { get; }
		IList<Source> LocalSources { get; }

		string CurrentArea { get; set; }
        string CurrentCategory { get; set; }
		IList<SKU> GetSkuByAreaCategory (string area, string category);

        SKU GetLocalSKU(string name);
        Item BindSKU (Item item);
        string ReplaceSkuAreaType { get; set; }

		uapp.IFile PreviewNotFound { get; }
    }

	public class SharedModel : ISharedModel {

		private static SharedModel sInstance = null;
		public static ISharedModel Instance {
			get {
				if (sInstance == null) {
					sInstance = new SharedModel();
					sInstance.initLocals();
				}
				return sInstance;
			}
		}
        
		private uapp.IFile mCurrentPlanBg;
		private Plan mCurrentPlan;
		private Room mCurrentRoom;
		private RoomShape mCurrentRoomShape = RoomShape.Polygon;
		private Wall mCurrentWall;
		private InWallItem mCurrentInWallItem;
		private Item mCurrentItem;
		private Photographer mPhotographer;
		private uapp.ICameraPrefab mWillChangeToThisCamera;
		private GameObject mPlanBackgroundObject;
        private uapp.IFile mPlanBgsDir = new uapp.File (uapp.FileType.Data, "../house/", true);  //todo 在创建显示他的真实路径
        private uapp.IFile mLocalAssetsDir = uapp.File.DataPath("project/assets/", true);
		private uapp.IFile mLocalProjectDir = uapp.File.DataPath("project/assets/project/", true);
		private IList<Plan> mLocalPrototypes;
		private IList<Plan> mLocalPlans;
		private IList<uapp.IFile> mLocalPlanBgs;
		private IList<string> mLocalAreas;
		private IList<string> mLocalCategories;
		private Dictionary<string, IList<string>> mLocalAreaCategories;
		private IList<SKU> mLocalSKUs;
		private IList<Source> mLocalSources;
		private string mCurrentArea;
        private string mCurrentCategory;
        private string mReplaceSkuAreaType;
		private uapp.IFile mPreviewNotFound = uapp.File.Resource("preview_not_found");

        private SharedModel() {
			
		}

        public string ReplaceSkuAreaType {
            get {
                return mReplaceSkuAreaType;
            }set {
                mReplaceSkuAreaType = value;
            }
        }
			
        public uapp.IFile CurrentPlanBg {
			get {
				return mCurrentPlanBg;
			}
			set {
				mCurrentPlanBg = value;
			}
		}

		public Plan CurrentPlan {
			get {
				return mCurrentPlan;
			}
			set {
				mCurrentPlan = value;
			}
		}

		public Room CurrentRoom {
			get {
				return mCurrentRoom;
			}
			set {
				mCurrentRoom = value;
			}
		}

		public RoomShape CurrentRoomShape {
			get {
				return mCurrentRoomShape;
			}
			set {
				mCurrentRoomShape = value;
			}
		}

		public Wall CurrentWall {
			get {
				return mCurrentWall;
			}
			set {
				mCurrentWall = value;
			}
		}

		public InWallItem CurrentInWallItem {
			get {
				return mCurrentInWallItem;
			} set {
				mCurrentInWallItem = value;
			}
		}

		public Item CurrentItem {
			get {
				return mCurrentItem;
			} set {
				mCurrentItem = value;
			}
		}

		public Photographer Photographer {
			get {
				if (mPhotographer == null) {
					mPhotographer = new Photographer();
				}
				return mPhotographer;
			}
		}

		public uapp.ICameraPrefab WillChangeToThisCamera {
			get {
				return mWillChangeToThisCamera;
			}
			set {
				mWillChangeToThisCamera = value;
			}
		}

		public GameObject PlanBackgroundObject { 
			get {
				if (mPlanBackgroundObject == null) {
					mPlanBackgroundObject = new GameObject("plan_background");
				}
				return mPlanBackgroundObject;
			}
		}

		private List<string> mDefaultAreaNames;
		public List<string> DefaultAreaNames { 
			get {
				if (mDefaultAreaNames == null) {
					mDefaultAreaNames = new List<string>();
				}
				mDefaultAreaNames.Add("客厅");
				mDefaultAreaNames.Add("主卧");
				mDefaultAreaNames.Add("次卧");
				mDefaultAreaNames.Add("厨房");
				mDefaultAreaNames.Add("卫生间");
				mDefaultAreaNames.Add("书房");
				return mDefaultAreaNames;
			}
		}

		private SKU mDefaultFloor;
		public SKU DefaultFloor {
			get {
				if (mDefaultFloor == null) {
					mDefaultFloor = new SKU();
					mDefaultFloor.Source = new Source();
					mDefaultFloor.Source.Type = SourceType.Material;
					mDefaultFloor.Source.File = new uapp.File(uapp.FileType.Data, "project/assets/texture/floor.jpg");
				}
				return mDefaultFloor;
			}
		}

		private SKU mDefaultWall;
		public SKU DefaultWall {
			get {
				if (mDefaultWall == null) {
					mDefaultWall = new SKU();
					mDefaultWall.Source = new Source();
					mDefaultWall.Source.Type = SourceType.Material;
					mDefaultWall.Source.File = new uapp.File(uapp.FileType.Data, "project/assets/texture/wall.jpg");
				}
				return mDefaultWall;
			}
		}

		private SKU mDefaultDoor;
		public SKU DefaultDoor {
			get {
				if (mDefaultDoor == null) {
					mDefaultDoor = new SKU();
					mDefaultDoor.Source = new Source();
					mDefaultDoor.Source.Type = SourceType.Model;
					mDefaultDoor.Source.RealL = 1000;
					mDefaultDoor.Source.RealH = 1000;
				}
				return mDefaultDoor;
			}
		}

		public uapp.IFile LocalAssetsDir { 
			get {
				return mLocalAssetsDir;
			}
		}

		public uapp.IFile LocalProjectDir { 
			get {
				return mLocalProjectDir;
			}
		}

		private void initLocals() {
			var sourceFile = uapp.File.DataPath(System.IO.Path.Combine(mLocalProjectDir.Path, "sources.fit"));
			SourceTable sourceTable = new SourceTable();
			mLocalSources = sourceTable.LoadFile(sourceFile);

			var skusFile = uapp.File.DataPath(System.IO.Path.Combine(mLocalProjectDir.Path, "skus.fit"));
			SKUTable skuTable = new SKUTable();
			mLocalSKUs = skuTable.LoadFile(skusFile);
			skuTable.Build(mLocalSKUs, mLocalSources);

			if (mLocalSKUs != null) {
				mLocalAreas = new List<string>();
				mLocalAreaCategories = new Dictionary<string, IList<string>>();
				foreach (var sku in mLocalSKUs) {
					var area = sku.Area;
					var category = sku.Category;
					IList<string> categories = null;
					if (area != null && !mLocalAreaCategories.ContainsKey(area)) {
						categories = new List<string>();
						mLocalAreaCategories[area] = categories;
						mLocalAreas.Add(area);
					} else {
						categories = mLocalAreaCategories[area];
					}
					if (categories != null) {
						if (category != null && !categories.Contains(category)) {
							if (category == "墙纸" || category == "地板" || category == "天花" || category == "窗帘") { // NOTE 先忽略这些分类
								continue;
							}
							categories.Add(category);
						}
					}
				}
			}

			var prototypeFile = uapp.File.DataPath(System.IO.Path.Combine(mLocalProjectDir.Path, "prototypes.fit"));
			PlanTable prototypeTable = new PlanTable();
			mLocalPrototypes = prototypeTable.LoadFile(prototypeFile);

			// TODO 其他表

			//var plansDir = new uapp.File(uapp.FileType.Data, "../house/", true);
			mLocalPlanBgs = mPlanBgsDir.ListFiles(new string[]{ uapp.FilePatterns.PNG, uapp.FilePatterns.JPG }, false);
		}

        public IList<Plan> LocalPrototypes { 
			get {
				return mLocalPrototypes;
			}
		}

		public IList<Plan> LocalPlans { 
			get {
				return mLocalPlans;
			}
		}

        public uapp.IFile PlanBgsDir {
            get {
                return mPlanBgsDir;
            }
        }

        public IList<uapp.IFile> LocalPlanBgs { 
			get {
				return mLocalPlanBgs;
            }
            set {
                if (mLocalPlanBgs != value) {
                    mLocalPlanBgs = value;
                }
            }
		}

//		public IList<string> LocalAreas {
//			get {
//				if (mLocalAreas == null) {
//					mLocalAreas = new List<string>();
//					mLocalAreas.Add("客厅");
//					mLocalAreas.Add("卧室");
//					mLocalAreas.Add("厨房");
//					mLocalAreas.Add("卫生间");
//					mLocalAreas.Add("书房");
//				}
//				return mLocalAreas;
//			}
//		}
//
//		public IList<string> LocalCategories {
//			get {
//				if (mLocalCategories == null) {
//					mLocalCategories = new List<string>();
//					mLocalCategories.Add("家具");
//					mLocalCategories.Add("饰品");
//					mLocalCategories.Add("墙纸");
//					mLocalCategories.Add("地板");
//					mLocalCategories.Add("窗帘");
//				}
//				return mLocalCategories;
//			}
//		}

		public IList<string> LocalAreas {
			get {
				if (mLocalAreaCategories == null) {
					return null;
				}
//				return mLocalAreaCategories.Keys;
				return mLocalAreas;
			}
		}

		public IList<string> LocalCategoriesByArea(string area) {
			if (mLocalAreaCategories == null) {
				return null;
			}
			if (!mLocalAreaCategories.ContainsKey(area)) {
				return null;
			}
			return mLocalAreaCategories[area];
		}

		private List<string> mTempLocalCategoriesByItemType;
		public IList<string> LocalCategoriesByItemType(ItemType itemType) {
			if (mTempLocalCategoriesByItemType == null) {
				mTempLocalCategoriesByItemType = new List<string>();
			}
			mTempLocalCategoriesByItemType.Clear();
			switch (itemType) {
				case ItemType.Item: {
						mTempLocalCategoriesByItemType.Add("家具");
						mTempLocalCategoriesByItemType.Add("饰品");
						break;
					}
				case ItemType.Floor: {
						mTempLocalCategoriesByItemType.Add("地板");
						break;
					}
				case ItemType.Wall: {
						mTempLocalCategoriesByItemType.Add("墙纸");
						break;
					}
				case ItemType.Ceil: {
						mTempLocalCategoriesByItemType.Add("天花");
						break;
					}
				case ItemType.Curtain: {
						mTempLocalCategoriesByItemType.Add("窗帘");
						break;
					}
			}
			return mTempLocalCategoriesByItemType;
		}

		public string CategoryByItemType(ItemType itemType) {
			string category = null;
			switch (itemType) {
				case ItemType.Floor:
					category = "地板";
					break;
				case ItemType.Wall:
					category = "墙纸";
					break;
				case ItemType.Ceil: 
					category = "天花";
					break;
				case ItemType.Curtain: 
					category = "窗帘";
					break;
				case ItemType.DoorStone://TODO 要换过门石
					category = "地板";
					break;
				default:
					break;
			}
			return category;
		}

		public IList<SKU> LocalSKUs { 
			get {
				return mLocalSKUs;
			}
		}

		public IList<Source> LocalSources { 
			get {
				return mLocalSources;
			}
		}

		public string CurrentArea {
			get {
				return mCurrentArea;
			}set {
				mCurrentArea = value;
			}
		}

		public string CurrentCategory {
			get {
				return mCurrentCategory;
			}set {
				mCurrentCategory = value;
			}
		}

		private IList<SKU> mTempSKUs;
		public IList<SKU> GetSkuByAreaCategory (string area, string category) {
			if (mLocalSKUs == null) {
				return null;
			}
			if (mTempSKUs == null) {
				mTempSKUs = new List<SKU>();
			}
			mTempSKUs.Clear();
            if (area == "主卧" || area == "次卧") {
                area = "卧室";
            }
			foreach (var sku in mLocalSKUs) {
//				if (sku.Area == area && sku.Category == category) {
//					mTempSKUs.Add(sku);
//				}
				if (uapp.StringUtils.IsNullOrBlank(area)) { // NOTE 区域为空时，只用分类返回
					if (category == sku.Category) {
						if (mTempSKUs.Contains(sku)) { // NOTE 去除忽略区域信息中的重复项
							continue;
						}
						mTempSKUs.Add(sku);
					}
				} else if (area == sku.Area && category == sku.Category) {
					mTempSKUs.Add(sku);
				}
			}
			return mTempSKUs;
		}

		public Item BindSKU(Item item) {
			if (mLocalSKUs == null) {
				return item;
			}

			foreach (var sku in mLocalSKUs) {
				if (item.SKUId == sku.Id) {
					item.SKU = sku;
					break;
				}
			}
			return item;
		}

        public SKU GetLocalSKU(string name) {
			var localSKUs = LocalSKUs;
			foreach (var sku in localSKUs) {
				if (uapp.StringUtils.Equals(sku.Name, name)) {
					return sku;
				}
			}
			return null;
		}

		public uapp.IFile PreviewNotFound {
			get {
				return mPreviewNotFound;
			}
		}
	}
}
