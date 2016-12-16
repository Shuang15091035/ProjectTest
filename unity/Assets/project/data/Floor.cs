using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace project {

	public enum FloorPointMoveFunc {
		Free,
		HorizontalVertical, // 与前面或后面一个节点保持在水平线或垂直线上
	}

	public class Floor : RoomItem {

		public static float PointSize = 0.2f;

		private GameObject mFloorObject;
		private Mesh mMesh;
		private MeshRenderer mMeshRenderer;
		private Texture2D mTexture;
		private Texture2D mPointTexture;
		private Texture2D mPointSelectedTexture;

		private List<GameObject> mDesignPoints;
		private Mesh mDesignPointMesh;
		private Material mDesignPointMaterial;
		private TextMesh mAreaMesh;
		private TextMesh mAreaMesh2;

		private uapp.Polygon mData;
		private float mAreaSize;
		private int mSelectedPointIndex = -1;
		private FloorPointMoveFunc mPointMoveFunc;

		public Floor(Room room, int id, SKU sku, GameObject parent = null) : base(room, "floor", id, sku, parent) {
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
			mTexture = textureFile.GetContent<Texture2D>();

			mFloorObject = new GameObject("floor");
			uapp.ObjectUtils.SetParent(mFloorObject, mGameObject);
			mMesh = new Mesh();
			var meshFilter = mFloorObject.AddComponent<MeshFilter>();
			meshFilter.sharedMesh = mMesh;
			mData = new uapp.Polygon();

			mMeshRenderer = mFloorObject.AddComponent<MeshRenderer>();
			mMeshRenderer.sharedMaterial = new Material(Shader.Find(uapp.Shaders.DiffuseAlpha));
			mMeshRenderer.sharedMaterial.SetTexture(uapp.Shaders.Textures.Diffuse, mTexture);

			mPointTexture = uapp.File.Resource("img_floor_point_n").GetContent<Texture2D>();
			mPointSelectedTexture = uapp.File.Resource("img_floor_point_p").GetContent<Texture2D>();

			// NOTE Design阶段显示面积
			var mAreaObject = uapp.File.Resource("FloorAreaText").GetContent<GameObject>();
			mAreaObject.name = mGameObject.name + "@area";
			uapp.ObjectUtils.SetParent(mAreaObject, mFloorObject);
//			mAreaMesh = mAreaObject.GetComponent<TextMesh>();
			uapp.ObjectUtils.EnumComponent<TextMesh>(mAreaObject, (GameObject gameObject, TextMesh component) => {
				if (mAreaMesh == null) {
					mAreaMesh = component;
				} else if (mAreaMesh2 == null) {
					mAreaMesh2 = component;
				}
			}, true);

			var item = this.Item;
			item.Type = ItemType.Floor;
			item.EditItem = this;
		}

		public override void OnDelete() {
			base.OnDelete();
		}

		public FloorPointMoveFunc PointMoveFunc {
			get {
				return mPointMoveFunc;
			} set {
				mPointMoveFunc = value;
			}
		}

		public override EditState EditState {
			get {
				return base.EditState;
			}
			set {
				base.EditState = value;
				switch (mEditState) {
				case EditState.Normal:
					mMeshRenderer.sharedMaterial.SetColor(uapp.Shaders.Colors.Diffuse, uapp.ColorUtils.FromRbga(0x00000026));
					break;
				case EditState.Highlighted:
				case EditState.Selected:
					mMeshRenderer.sharedMaterial.SetColor(uapp.Shaders.Colors.Diffuse, uapp.ColorUtils.FromRbga(0x005aff33));
					break;
				}
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
							mMeshRenderer.sharedMaterial.SetTexture(uapp.Shaders.Textures.Diffuse, null);
							mAreaMesh.gameObject.SetActive(true);
							EditState = EditState.Normal;
							break;
						}
					case EditPhase.Edit:
						{
							mMeshRenderer.sharedMaterial.SetColor(uapp.Shaders.Colors.Diffuse, Color.white);
							mMeshRenderer.sharedMaterial.SetTexture(uapp.Shaders.Textures.Diffuse, mTexture);
							if (mDesignPoints != null) {
								foreach (var designPoint in mDesignPoints) {
									designPoint.gameObject.SetActive(false);
								}
							}
							mAreaMesh.gameObject.SetActive(false);
							break;
						}
				}
			}
		}

		public uapp.Polygon Data {
			get {
				return mData;
			}
		}

		public float AreaSize {
			get {
				return mAreaSize;
			}
		}

		public int SelectedPointIndex {
			get {
				return mSelectedPointIndex;
			} set {
				mSelectedPointIndex = value;
				updateDesignPoints();
			}
		}

		public override Area Area {
			get {
				return base.Area;
			}
			set {
				base.Area = value;
				updateAreaNameSizeText();
			}
		}

		private void updateAreaNameSizeText() {
			var area = Area;
			var areaName = area != null ? area.Name : "房间";
			mAreaMesh.text = areaName + "\n" + ActualAreaSize.ToString("F2") + "m²";
			mAreaMesh2.text = mAreaMesh.text;
		}

		public Vector3 Center {
			get {
				return mData.Center;
			}
		}

		public int PushPoint(Vector3 point) {
			point.y = Heights.FloorDefault;
			return mData.PushPoint(point);
		}

		public bool RemovePoint(int index) {
			if (mData == null) {
				return false;
			}
			return mData.RemovePoint(index);
		}

		public bool MovePoint(int index, Vector3 point) {
			point.y = Heights.FloorDefault;
			var b = false;
			switch (mPointMoveFunc) {
				case FloorPointMoveFunc.Free: {
						b = mData.SetPoint(index, point);
						break;
					}
				case FloorPointMoveFunc.HorizontalVertical: {
						var n = mData.NumPoints;
						var threshold = PointSize;
						var prevIndex = index - 1;
						if (prevIndex < 0) {
							prevIndex = mData.NumPoints - 1;
						}
						var nextIndex = index + 1;
						if (nextIndex > mData.NumPoints - 1) {
							nextIndex = 0;
						}

						if (prevIndex != index) {
							var prev = mData.PointAt(prevIndex);
							if (Mathf.Abs(point.z - prev.z) < threshold) {
								point.z = prev.z;
							} 
							if (Mathf.Abs(point.x - prev.x) < threshold) {
								point.x = prev.x;
							}
						}
						if (nextIndex != index && nextIndex != prevIndex) {
							var next = mData.PointAt(nextIndex);
							if (Mathf.Abs(point.z - next.z) < threshold) {
								point.z = next.z;
							}
							if (Mathf.Abs(point.x - next.x) < threshold) {
								point.x = next.x;
							}
						}
						b = mData.SetPoint(index, point);
						break;
					}
			}
			return b;
		}

        public bool AutoAlignPoints(int startIndex) {
            var n = mData.NumPoints;
            if (startIndex < 0 || startIndex > n - 1) {
                return false;
            }
            startIndex++;
            if (startIndex > n - 1) {
                startIndex = 0;
            }
            var stopIndex = startIndex - 1;
            if (stopIndex < 0) {
                stopIndex = n - 1;
            }
            while (true) {
                var prevIndex = startIndex - 1;
                if (prevIndex < 0) {
                    prevIndex = n - 1;
                }
                var point = mData.PointAt(startIndex);
                var prev = mData.PointAt(prevIndex);
                bool alignZorX = Mathf.Abs(point.z - prev.z) < Mathf.Abs(point.x - prev.x);
                if (alignZorX) {
                    point.z = prev.z;
                } else {
                    point.x = prev.x;
                }
                var changeIndex = startIndex;
                startIndex++;
                if (startIndex > n - 1) {
                    startIndex = 0;
                }
                if (startIndex == stopIndex) {
                    break;
                }
                mData.SetPoint(changeIndex, point);
            }
            {
                var lastIndex = stopIndex - 1;
                if (lastIndex < 0) {
                    lastIndex = n - 1;
                }
                var prevIndex = lastIndex - 1;
                if (prevIndex < 0) {
                    prevIndex = n - 1;
                }
                var nextIndex = lastIndex + 1;
                if (nextIndex > n - 1) {
                    nextIndex = 0;
                }
                var point = mData.PointAt(lastIndex);
                var prev = mData.PointAt(prevIndex);
                var next = mData.PointAt(nextIndex);
                bool alignPrevOrNext = Mathf.Abs (point.x - prev.x) < Mathf.Abs (point.x - next.x);
                if (alignPrevOrNext) {
                    point.x = prev.x;
                } else {
                    point.x = next.x;
                }
                alignPrevOrNext = Mathf.Abs (point.z - prev.z) < Mathf.Abs (point.z - next.z);
                if (alignPrevOrNext) {
                    point.z = prev.z;
                } else {
                    point.z = next.z;
                }
                mData.SetPoint(lastIndex, point);
            }
            return true;
        }

		public override bool IsPointInXZ(Vector3 point) {
			base.IsPointInXZ(point);
			return mData.IsPointInXZ(point);
		}

		public bool Build() {
            // TODO 去掉冗余点
			if (mEditPhase == EditPhase.Design) {
				if (mDesignPoints == null) {
					mDesignPoints = new List<GameObject>();
				}
				if (mDesignPoints.Count < mData.NumPoints) {
					for (var i = mDesignPoints.Count; i < mData.NumPoints; i++) {
						var dp = createDesignPoint();
						mDesignPoints.Add(dp);
					}
				} else if (mDesignPoints.Count > mData.NumPoints) {
					for (var i = mData.NumPoints; i < mDesignPoints.Count; i++) {
						var dp = mDesignPoints[i];
						dp.SetActive(false);
					}
				}
				updateDesignPoints();
			}

			if (mData.NumPoints < 3) {
				// 少于3个顶点，不显示地板
				mFloorObject.SetActive(false);
				return true;
			}
			return trianglulateMesh();
		}

		private void updateDesignPoints() {
			if (mDesignPoints == null) {
				return;
			}
			var n = mDesignPoints.Count < mData.NumPoints ? mDesignPoints.Count : mData.NumPoints;
			for (var i = 0; i < n; i++) {
				var dp = mDesignPoints[i];
				var mat = uapp.ObjectUtils.GetMaterial(dp);
				if (i == mSelectedPointIndex) {
					mat.mainTexture = mPointSelectedTexture;
				} else {
					mat.mainTexture = mPointTexture;
				}
				dp.SetActive(true);
				var p = mData.PointAt(i);
				dp.transform.position = p;
				dp.transform.Translate(0.0f, 0.01f, 0.0f);
			}
		}

		private GameObject createDesignPoint() {
			var designPointObject = new GameObject("design point");
			uapp.ObjectUtils.SetParent(designPointObject, mGameObject);

            if (mDesignPointMesh == null) {
                var mesh = uapp.MeshUtils.RectXZ (Floor.PointSize, Floor.PointSize, uapp.MeshUtils.RectPivot.Center, Heights.FloorPoint);
                mDesignPointMesh = mesh;
            }

            // 单独材质
            var mat = new Material (Shader.Find (uapp.Shaders.DiffuseAlpha));
            mat.mainTexture = mPointTexture;

            uapp.MeshUtils.AddMeshComponentPure(designPointObject, mDesignPointMesh, mat);

			return designPointObject;
		}

		private Poly2Tri.Polygon mPolygon;
		private IList<Poly2Tri.PolygonPoint> mPolygonPoints;
		protected bool canTrianglulation() {
			if (mData.isEdgeIntersect) {
				return false;
			}
			if (mPolygonPoints == null) {
				mPolygonPoints = new List<Poly2Tri.PolygonPoint>();
			}
			mPolygonPoints.Clear();
			foreach (var point in mData.Points) {
				mPolygonPoints.Add(new Poly2Tri.PolygonPoint(point.x, point.z));
			}

			mPolygon = new Poly2Tri.Polygon(mPolygonPoints);
			try {
				Poly2Tri.P2T.Triangulate(mPolygon);
			} catch (System.Exception ex) {
				return false;
			}

			return mPolygon.Triangles.Count > 0;
		}

		protected bool trianglulateMesh() {
			if (!canTrianglulation()) {
				mFloorObject.SetActive(false);
				return false;
			}
			var triangles = mPolygon.Triangles;
			var vertices = new Vector3[triangles.Count * 3];
			var indices = new int[triangles.Count * 3];
			var i = 0;
			var j = 0;
			mAreaSize = 0.0f; // NOTE 顺便计算面积
			foreach (var triangle in triangles) {
				indices[j++] = i;
				var x0 = triangle.Points[2].Xf;
				var y0 = triangle.Points[2].Yf;
				var x1 = triangle.Points[1].Xf;
				var y1 = triangle.Points[1].Yf;
				var x2 = triangle.Points[0].Xf;
				var y2 = triangle.Points[0].Yf;
				vertices[i++] = new Vector3(x0, Heights.FloorDefault, y0);
				indices[j++] = i;
				vertices[i++] = new Vector3(x1, Heights.FloorDefault, y1);
				indices[j++] = i;
				vertices[i++] = new Vector3(x2, Heights.FloorDefault, y2);
				mAreaSize += uapp.MathUtils.AreaOfTriangle(x0, y0, x1, y1, x2, y2); // NOTE 顺便计算面积
			}
			var center = Vector3.zero;
			foreach (var vertex in vertices) {
				center += vertex;
			}
			var uvs = new Vector2[triangles.Count * 3];
			var k = 0;
			center /= vertices.Length;
			foreach (var vertex in vertices) {
				var uv = Vector2.zero;
				uv.x = vertex.x - center.x;
				uv.y = vertex.z - center.z;
				uvs[k++] = uv;
			}
			if (mEditPhase == EditPhase.Design) {
				updateAreaNameSizeText();
				mAreaMesh.transform.position = center;
				mAreaMesh.transform.Translate(0.0f, Heights.FloorAreaText - center.y, 0.0f, Space.World);
				mAreaMesh.gameObject.SetActive(true);
			} else {
				mAreaMesh.gameObject.SetActive(false);
			}

			mMesh.Clear();
			mMesh.vertices = vertices;
			mMesh.uv = uvs;
			mMesh.triangles = indices;
			mFloorObject.gameObject.SetActive(true);
			return true;
		}

		public override float ActualAreaSize {
			get {
				var area = AreaSize;
				var scale = PlanScale;
				return area / scale;
			}
		}

		public override void onPlanScaleChanged(float scale) {
			base.onPlanScaleChanged(scale);
			updateAreaNameSizeText();
		}
	}
}
