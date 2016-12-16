using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace project {
	
	public class Wall : RoomItem {

		public enum WallEditResult {
			Ok,
			Failed,
			NotEnoughSpace,
		}

		public static float Thickness = 0.05f;
		public static float LineThickness = 5.0f;
		private List<InWallItem> mInWallItems;

		private Mesh mMesh;
		private MeshRenderer mMeshRenderer;
//		private LineRenderer mLineRenderer;
		private TextMesh mLengthMesh;
        private TextMesh mLengthMesh2;

        private uapp.Quadrangle mData;
		private Vector2[] mUVs = new Vector2[4];

		public Wall(Room room, int id, SKU sku, GameObject parent = null) : base(room, "wall", id, sku, parent) {
			if (sku == null) {
				return;
			}
			var source = sku.Source;
			if (source == null || source.Type != SourceType.Material) {
				return;
			}
			var textureFile = source.File;
			if (textureFile == null) {
				return;
			}
			var texture = textureFile.GetContent<Texture2D>();

			mMesh = new Mesh();
			var meshFilter = mGameObject.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = mMesh;
			var tl = new Vector3(0.0f, Heights.FloorDefault + Heights.WallDefault, 0.0f);
			var bl = new Vector3(0.0f, Heights.FloorDefault, 0.0f);
			var br = new Vector3(0.0f, Heights.FloorDefault, 0.0f);
			var tr = new Vector3(0.0f, Heights.FloorDefault + Heights.WallDefault, 0.0f);
			mData = new uapp.Quadrangle(tl, bl, br, tr);

			mMeshRenderer = mGameObject.AddComponent<MeshRenderer>();
			mMeshRenderer.sharedMaterial = new Material(Shader.Find(uapp.Shaders.Diffuse));
			mMeshRenderer.sharedMaterial.SetTexture(uapp.Shaders.Textures.Diffuse, texture);

            //			mLineRenderer = mGameObject.AddComponent<LineRenderer>();
            // NOTE Design阶段显示长度
            //var mLengthObject = uapp.File.Resource("WallLengthText").GetContent<GameObject>();
            var mLengthObject = uapp.File.Resource ("FloorAreaText").GetContent<GameObject> ();
            mLengthObject.name = mGameObject.name + "@length";
			uapp.ObjectUtils.SetParent(mLengthObject, mGameObject);
			//mLengthMesh = mLengthObject.GetComponent<TextMesh>();
            uapp.ObjectUtils.EnumComponent<TextMesh> (mLengthObject, (GameObject gameObject, TextMesh component) => {
                if (mLengthMesh == null) {
                    mLengthMesh = component;
                } else if (mLengthMesh2 == null) {
                    mLengthMesh2 = component;
                }
            }, true);

            var item = this.Item;
			item.Type = ItemType.Wall;
			item.EditItem = this;
		}

		public override void OnDelete() {
			base.OnDelete();
			if (mInWallItems != null) {
				foreach (var inWallItem in mInWallItems) {
					uapp.CoreUtils.Delete(inWallItem);
				}
				mInWallItems.Clear();
				mInWallItems = null;
			}
		}

		public override EditPhase EditPhase {
			get {
				return base.EditPhase;
			}
			set {
				base.EditPhase = value;
				switch (mEditPhase) {
					case EditPhase.Init: {
							mGameObject.SetActive(false);
							break;
						}
					case EditPhase.Design:
						{
							mMeshRenderer.enabled = false;
//							mLineRenderer.enabled = true;
//							mLineRenderer.SetColors(Color.blue, Color.blue);
//							mLineRenderer.SetWidth(2.0f, 2.0f);
							mLengthMesh.gameObject.SetActive(true);
							break;
						}
					case EditPhase.Edit:
						{
							mMeshRenderer.enabled = true;
//							mLineRenderer.enabled = false;
							mLengthMesh.gameObject.SetActive(false);
							break;
						}
				}
			}
		}

		public Vector3 Start {
			get {
				return mData.PointAt(uapp.QuadrangleConer.BottomLeft);
			}
			set {
				var bl = mData.PointAt(uapp.QuadrangleConer.BottomLeft);
				var tl = mData.PointAt(uapp.QuadrangleConer.TopLeft);
				bl.x = value.x;
				bl.z = value.z;
				tl.x = value.x;
				tl.z = value.z;
				mData.SetPoint(uapp.QuadrangleConer.BottomLeft, bl);
				mData.SetPoint(uapp.QuadrangleConer.TopLeft, tl);
			}
		}

		public Vector3 End {
			get {
				return mData.PointAt(uapp.QuadrangleConer.BottomRight);
			}
			set {
				var br = mData.PointAt(uapp.QuadrangleConer.BottomRight);
				var tr = mData.PointAt(uapp.QuadrangleConer.TopRight);
				br.x = value.x;
				br.z = value.z;
				tr.x = value.x;
				tr.z = value.z;
				mData.SetPoint(uapp.QuadrangleConer.BottomRight, br);
				mData.SetPoint(uapp.QuadrangleConer.TopRight, tr);
			}
		}

		public float Height {
			get {
				return mData.PointAt(uapp.QuadrangleConer.TopLeft).y - mData.PointAt(uapp.QuadrangleConer.BottomLeft).y;
			}
			set {
				var tl = mData.PointAt(uapp.QuadrangleConer.TopLeft);
				var tr = mData.PointAt(uapp.QuadrangleConer.TopRight);
				tl.y = mData.PointAt(uapp.QuadrangleConer.BottomLeft).y + value;
				tr.y = mData.PointAt(uapp.QuadrangleConer.BottomRight).y + value;
				mData.SetPoint(uapp.QuadrangleConer.TopLeft, tl);
				mData.SetPoint(uapp.QuadrangleConer.TopRight, tr);
			}
		}

		public Vector3 Center {
			get {
				return uapp.MathUtils.Lerp(Start, End, 0.5f);
			}
		}

		public Vector3 Direction {
			get {
				var dir = End - Start;
				return dir.normalized;
			}
		}

		public Vector3 Normal {
			get {
				var A = mData.PointAt(uapp.QuadrangleConer.BottomLeft);
				var B = mData.PointAt(uapp.QuadrangleConer.BottomRight);
				var C = mData.PointAt(uapp.QuadrangleConer.TopRight);
				var BC = C - B;
				var BA = A - B;
				var normal = Vector3.Cross(BC, BA);
				return normal.normalized;
			}
		}

		public float Length {
			get {
				return uapp.MathUtils.Length(End - Start);
			}
		}

		private float pointInWallXZ(Vector3 point, out float u, out Vector3 upoint) {
			return uapp.MathUtils.SquareDistancePointToLineSegment(point, Start, End, out u, out upoint);
		}

		public override bool IsPointInXZ(Vector3 point) {
			base.IsPointInXZ(point);
			float u;
			Vector3 upoint;
			if (pointInWallXZ(point, out u, out upoint) > Wall.Thickness) {
				return false;
			}
			return true;
		}

		public Vector3 PointByPointXZ(Vector3 point) {
			float u;
			Vector3 upoint;
			if (pointInWallXZ(point, out u, out upoint) > Wall.Thickness) {
				return Vector3.zero;
			}
			var wallDir = Direction;
			var wallStart = Start;
			return wallStart + (u * Length * wallDir);
		}

		public bool Build() {
			mUVs[0] = new Vector2(0.0f, 1.0f);
			mUVs[1] = new Vector2(0.0f, 0.0f);
			mUVs[2] = new Vector2(1.0f, 0.0f);
			mUVs[3] = new Vector2(1.0f, 1.0f);
			mMesh.vertices = mData.Points;
			mMesh.uv = mUVs;
			mMesh.triangles = new int[] { 
				0, 2, 1,
				2, 0, 3,
			};
			if (mInWallItems != null) {
				var wallDir = Direction;
				var wallStart = Start;
				var wallEnd = End;
				var degrees = uapp.MathUtils.Degrees(1.0f, 0.0f, wallDir.x, wallDir.z);
//				degrees += 180.0f; // NOTE 由于room的关系，wall的start和end是调换的（跟顶点顺序相反），故这样处理一下
				foreach (var inWallItem in mInWallItems) {
					var itemObject = inWallItem.GameObject;
					var itemRect = inWallItem.Data;
					var itemPos = wallStart + (itemRect.position.x * wallDir) + (itemRect.position.y * Vector3.up);
//					var itemPos = wallEnd - (itemRect.position.x * wallDir) + (itemRect.position.y * Vector3.up);
					itemObject.transform.position = itemPos;
					itemObject.transform.eulerAngles = new Vector3(0.0f, degrees, 0.0f);
					itemObject.SetActive(true);
				}
			}
//			mLineRenderer.SetPosition(0, Start);
//			mLineRenderer.SetPosition(1, End);
			if (mEditPhase == EditPhase.Design) {
				//mLengthMesh.text = ActualLength.ToString("F2") + "m";
				//var center = Center;
				//mLengthMesh.transform.position = center;
				//mLengthMesh.transform.Translate(0.0f, Heights.WallLengthText - center.y, 0.0f, Space.World);
				//mLengthMesh.gameObject.SetActive(true);
                updateLengthText();
			} else {
				mLengthMesh.gameObject.SetActive(false);
			}
			return true;
		}

        private void updateLengthText () {
            mLengthMesh.text = ActualLength.ToString ("F2") + "m";
            mLengthMesh2.text = mLengthMesh.text;
            var center = Center;
            mLengthMesh.transform.position = center;
            mLengthMesh.transform.Translate (0.0f, Heights.WallLengthText - center.y, 0.0f, Space.World);
            mLengthMesh.gameObject.SetActive (true);
        }

        //		private Vector3 getItemRectWorldPosition(Rect itemRect) {
        //			var wallDir = Direction;
        //			var wallStart = Start;
        //			var wallEnd = End;
        //			var itemPos = wallEnd + (itemRect.position.x * wallDir) + (itemRect.position.y * Vector3.up);
        //			return itemPos;
        //		}

        public float GetScale(float actualLength) {
			if (actualLength == 0.0f) {
				return 1.0f;
			}
			var wallLength = uapp.MathUtils.Distance(Start, End);
			if (wallLength == 0.0f) {
				return 1.0f;
			}
			return wallLength / actualLength;
		}

		public float ActualLength {
			get {
				var wallLength = uapp.MathUtils.Distance(Start, End);
				var scale = PlanScale;
				return wallLength / scale;
			}
		}

		public override float ActualAreaSize {
			get {
				return ActualLength * Height;
			}
		}

		public override void onPlanScaleChanged(float scale) {
			base.onPlanScaleChanged(scale);
			mLengthMesh.text = ActualLength.ToString("F2") + "m";
		}

		public Rect WallRect {
			get {
				float distance = uapp.MathUtils.Distance(Start, End);
				var rect = new Rect(0.0f, 0.0f, distance, Height);
				return rect;
			}
		}

		public List<InWallItem> InWallItems {
			get {
				return mInWallItems;
			}
		}

		public WallEditResult AddInWallItemXZ(InWallItem inWallItem, Vector3 position) {
//			if (inWallItem == null) {
//				return WallEditResult.Failed;
//			}
////			var item = inWallItem.Item;
////			if (item == null) {
////				return WallEditResult.Failed;
////			}
////			var sku = item.SKU;
////			if (sku == null) {
////				return WallEditResult.Failed;
////			}
////			var source = sku.Source;
////			if (source == null || source.Type != SourceType.Model) {
////				return WallEditResult.Failed;
////			}
//			if (!IsPointInXZ(position)) {
//				return WallEditResult.Failed;
//			}
//			var wallRect = WallRect;
//			var itemRect = inWallItem.Data;
//			if (uapp.RectUtils.SmallerThan(wallRect, itemRect)) {
//				return WallEditResult.NotEnoughSpace;
//			}
//
//			float u;
//			Vector3 upoint;
//			pointInWallXZ(position, out u, out upoint);
////			var itemPosToAdd3 = Start + u * (End - Start);
//			var itemPosToAdd = new Vector2(u * Length, 0.0f); // TODO 先写死高度
//			inWallItem.Position = itemPosToAdd;
//			if (mInWallItems == null) {
//				mInWallItems = new List<InWallItem>();
//			}
//			mInWallItems.Add(inWallItem);
//			inWallItem._notifyWall(this);
//			return WallEditResult.Ok;

			if (inWallItem == null) {
				return WallEditResult.Failed;
			}
			if (!IsPointInXZ(position)) {
				return WallEditResult.Failed;
			}
			var result = placeInWallItemXZ(inWallItem, position);
			if (result != WallEditResult.Ok) {
				return result;
			}

			if (mInWallItems == null) {
				mInWallItems = new List<InWallItem>();
			}
			mInWallItems.Add(inWallItem);
			inWallItem._notifyWall(this);
			return WallEditResult.Ok;
		}

		public void RemoveInWallItem(InWallItem item) {
			if (mInWallItems == null) {
				return;
			}
			mInWallItems.Remove(item);
		}

		public void DestroyInWallItem(InWallItem inWallItem) {
			if (mInWallItems == null) {
				return;
			}
			if (!mInWallItems.Contains(inWallItem)) {
				return;
			}
			uapp.CoreUtils.Delete(inWallItem);
			mInWallItems.Remove(inWallItem);
		}

		public bool MoveInWallItemXZ(InWallItem inWallItem, Vector3 position) {
			return placeInWallItemXZ(inWallItem, position) == WallEditResult.Ok;
		}

		private WallEditResult placeInWallItemXZ(InWallItem inWallItem, Vector3 position) {
			var wallRect = WallRect;
			var itemRect = inWallItem.Data;
			if (uapp.RectUtils.SmallerThan(wallRect, itemRect)) {
				return WallEditResult.NotEnoughSpace;
			}

			float u;
			Vector3 upoint;
			pointInWallXZ(position, out u, out upoint);
			itemRect.position = new Vector2(u * wallRect.width, 0.0f); // TODO 先写死高度
			itemRect = makeSureItemRectInWall(itemRect, wallRect);

			if (isItemRectOverlapOther(itemRect, inWallItem)) {
				const float gap = 0.02f;
				bool ok = false;
				foreach (var iw in mInWallItems) {
					var rect = iw.Data;
					float x = rect.x + rect.width + gap;
					float y = 0.0f;
					itemRect.position = new Vector2(x, y);
					itemRect = makeSureItemRectInWall(itemRect, wallRect);
					if (isItemRectOverlapOther(itemRect, inWallItem)) {
						x = rect.x - gap - rect.width;
						itemRect.position = new Vector2(x, y);
						itemRect = makeSureItemRectInWall(itemRect, wallRect);
						if (!isItemRectOverlapOther(itemRect, inWallItem)) {
							ok = true;
							break;
						}
					} else {
						ok = true;
						break;
					}
				}
				if (ok) {
					inWallItem.Data = itemRect;
					return WallEditResult.Ok;
				} else {
					return WallEditResult.NotEnoughSpace;
				}
			} else {
				inWallItem.Data = itemRect;
				return WallEditResult.Ok;
			}
		}

		private Rect makeSureItemRectInWall(Rect itemRect, Rect wallRect) {
			var itemRectPosition = itemRect.position;
			if (itemRectPosition.x < 0.0f) {
				itemRectPosition.x = 0.0f;
			}
			if (itemRectPosition.x + itemRect.width > wallRect.width) {
				itemRectPosition.x = wallRect.width - itemRect.width;
			}
			itemRect.position = itemRectPosition;
			return itemRect;
		}

		private bool isItemRectOverlapOther(Rect itemRect, InWallItem inWallItem) {
			if (mInWallItems != null) {
				foreach (var iw in mInWallItems) {
					if (iw == inWallItem) {
						continue;
					}
					if (itemRect.Overlaps(iw.Data)) {
						return true;
					}
				}
			}
			return false;
		}

		public EditItem EditItemByPointXZ(Vector3 point) {
			if (mInWallItems != null) {
				foreach (var inWallItem in mInWallItems) {
					if (inWallItem.IsPointInXZ(point)) {
						return inWallItem;
					}
				}
			}
			return null;
		}

		public void SetEditItemState(EditItem editItem, EditState state, bool clearSelected = false) {
			if (editItem == null) {
				ClearEditItemState();
				return;
			}
			if (mInWallItems != null) {
				foreach (var inWallItem in mInWallItems) {
					if (!clearSelected) {
						if (inWallItem.EditState == EditState.Selected) {
							continue;
						}
					}
					if (inWallItem == editItem) {
						inWallItem.EditState = state;
					} else {
						inWallItem.EditState = EditState.Normal;
					}
				}
			}
		}

		public void ClearEditItemState(bool clearSelected = false) {
			if (mInWallItems != null) {
				foreach (var inWallItem in mInWallItems) {
					if (!clearSelected) {
						if (inWallItem.EditState == EditState.Selected) {
							continue;
						}
					}
					inWallItem.EditState = EditState.Normal;
				}
			}
		}

	}
}
