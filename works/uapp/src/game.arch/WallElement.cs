using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public interface IWallElement : IArchElement {

		IWall Wall { get; set; }
		Quadrangle Data { get; }
		Vector3 Start { get; set; }
		Vector3 End { get; set; }
		float Height { get; set; }
		float Length { get; }
		bool Solid { get; set; }
		ArchitectureBuildResult Build();
		GameObject Parent { get; set; }

		IList<Polygon> GetBuildHoles();
	}
	
	public class WallElement : ArchElement, IWallElement {

		private IWall mWall;
		private Quadrangle mData = new Quadrangle(new Vector3(0.0f, 2.8f, 0.0f), Vector3.zero, Vector3.zero, new Vector3(0.0f, 2.8f, 0.0f));
		private bool mSolid = true;

		private GameObject mGameObject;
		private Mesh mMesh;
		private Material mMaterial;
		private RealWorldTexture mTexture = new RealWorldTexture();

		private List<Polygon> mBuildHoles;

		public WallElement(RealWorldTexture texture) {
			mGameObject = new GameObject("wall");

			mMesh = new Mesh();
			var mf = mGameObject.AddComponent<MeshFilter>();
			mf.sharedMesh = mMesh;

			mMaterial = new Material(Shader.Find(Shaders.DiffuseAlpha));
			mTexture.Assign(texture);
			mMaterial.mainTexture = mTexture.Texture;
			var mr = mGameObject.AddComponent<MeshRenderer>();
			mr.sharedMaterial = mMaterial;
		}

		public IWall Wall {
			get {
				return mWall;
			}
			set {
				mWall = value;
			}
		}

		public Quadrangle Data {
			get {
				return mData;
			}
		}

		public Vector3 Start {
			get {
				return mData.PointAt(QuadrangleConer.BottomLeft);
			}
			set {
				var bl = mData.PointAt(QuadrangleConer.BottomLeft);
				var tl = mData.PointAt(QuadrangleConer.TopLeft);
				bl.x = value.x;
				bl.z = value.z;
				tl.x = value.x;
				tl.z = value.z;
				mData.SetPoint(QuadrangleConer.BottomLeft, bl);
				mData.SetPoint(QuadrangleConer.TopLeft, tl);
			}
		}

		public Vector3 End {
			get {
				return mData.PointAt(QuadrangleConer.BottomRight);
			}
			set {
				var br = mData.PointAt(QuadrangleConer.BottomRight);
				var tr = mData.PointAt(QuadrangleConer.TopRight);
				br.x = value.x;
				br.z = value.z;
				tr.x = value.x;
				tr.z = value.z;
				mData.SetPoint(QuadrangleConer.BottomRight, br);
				mData.SetPoint(QuadrangleConer.TopRight, tr);
			}
		}

		public float Height {
			get {
				return mData.PointAt(QuadrangleConer.TopLeft).y - mData.PointAt(QuadrangleConer.BottomLeft).y;
			}
			set {
				var tl = mData.PointAt(QuadrangleConer.TopLeft);
				var tr = mData.PointAt(QuadrangleConer.TopRight);
				tl.y = mData.PointAt(QuadrangleConer.BottomLeft).y + value;
				tr.y = mData.PointAt(QuadrangleConer.BottomRight).y + value;
				mData.SetPoint(QuadrangleConer.TopLeft, tl);
				mData.SetPoint(QuadrangleConer.TopRight, tr);
			}
		}

		public Vector3 Center {
			get {
				return MathUtils.Lerp(Start, End, 0.5f);
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
				var A = mData.PointAt(QuadrangleConer.BottomLeft);
				var B = mData.PointAt(QuadrangleConer.BottomRight);
				var C = mData.PointAt(QuadrangleConer.TopRight);
				var BC = C - B;
				var BA = A - B;
				var normal = Vector3.Cross(BC, BA);
				return normal.normalized;
			}
		}

		public float Length {
			get {
				return MathUtils.Distance(Start, End);
			}
		}

		public float ActualLength {
			get {
				var length = Length;
				var scale = mArch.Scale;
				return length / scale;
			}
		}

		public float ActualHeight {
			get {
				var height = Height;
				var scale = mArch.Scale;
				return height / scale;
			}
		}

		public bool Solid {
			get {
				return mSolid;
			}
			set {
				mSolid = value;
				if (mSolid) {
					mMaterial.mainTexture = mTexture.Texture;
					mMaterial.color = Color.white;
				} else {
					mMaterial.mainTexture = null;
					var color = Color.white;
					color.a = 0.5f;
					mMaterial.color = color;
				}
			}
		}

		public ArchitectureBuildResult Build() {
			if (mWall != null && mWall.HasWallItems) {
				var wallItems = mWall.WallItems;

				var wallPoly = new Polygon();
				var l = Length;
				var h = Height;
				var O = Start;
				var D = Direction;
				wallPoly.PushPoint(new Vector3(0.0f, 0.0f, h));
				wallPoly.PushPoint(new Vector3(l, 0.0f, h));
				wallPoly.PushPoint(new Vector3(l, 0.0f, 0.0f));
				wallPoly.PushPoint(new Vector3(0.0f, 0.0f, 0.0f));

				// NOTE wallItem以内墙坐标系定义
				if (mBuildHoles == null) {
					mBuildHoles = new List<Polygon>();
				}
				mBuildHoles.Clear();
				if (this == mWall.InnerWall) {
					foreach (var wallItem in wallItems) {
						var wallItemPoly = new Polygon();
						var wallItemPolyMin = wallItem.Data.min;
						var wallItemPolyMax = wallItem.Data.max;
						wallItemPoly.PushPoint(new Vector3(wallItemPolyMin.x, 0.0f, wallItemPolyMax.y));
						wallItemPoly.PushPoint(new Vector3(wallItemPolyMax.x, 0.0f, wallItemPolyMax.y));
						wallItemPoly.PushPoint(new Vector3(wallItemPolyMax.x, 0.0f, wallItemPolyMin.y));
						wallItemPoly.PushPoint(new Vector3(wallItemPolyMin.x, 0.0f, wallItemPolyMin.y));
						mBuildHoles.Add(wallItemPoly);
					}
				} else {
					// 外墙与内墙长度不一致
					var li = mWall.InnerWall.Length;
					var d = ((l - li) * 0.5f);
					foreach (var wallItem in wallItems) {
						var wallItemPoly = new Polygon();
						var wallItemPolyMin = new Vector3(li - wallItem.Data.max.x + d, wallItem.Data.min.y, 0.0f);
						var wallItemPolyMax = new Vector3(li - wallItem.Data.min.x + d, wallItem.Data.max.y, 0.0f);
						wallItemPoly.PushPoint(new Vector3(wallItemPolyMin.x, 0.0f, wallItemPolyMax.y));
						wallItemPoly.PushPoint(new Vector3(wallItemPolyMax.x, 0.0f, wallItemPolyMax.y));
						wallItemPoly.PushPoint(new Vector3(wallItemPolyMax.x, 0.0f, wallItemPolyMin.y));
						wallItemPoly.PushPoint(new Vector3(wallItemPolyMin.x, 0.0f, wallItemPolyMin.y));
						mBuildHoles.Add(wallItemPoly);
					}
				}

				float us = 1.0f / mArch.Scale / mTexture.ActualWidth;
				float vs = 1.0f / mArch.Scale / mTexture.ActualHeight;
				PolygonMesh.TrianglulateXZ(wallPoly, mBuildHoles, 0.0f, mMesh, true, (Vector3 v) => {
					return Start + v.x * D + v.z * Vector3.up;
				}, (Vector2 uv) => {
					uv.x *= us;
					uv.y *= vs;
					return uv;
				});

				return ArchitectureBuildResult.Ok;

			} else {
				mMesh.vertices = mData.Points;

				var uv = new Vector2[4];
				var l = ActualLength;
				var h = ActualHeight;
				var u = l / mTexture.ActualWidth;
				var v = h / mTexture.ActualHeight;
				uv[(int)QuadrangleConer.TopLeft] = new Vector2(0.0f, v);
				uv[(int)QuadrangleConer.BottomLeft] = new Vector2(0.0f, 0.0f);
				uv[(int)QuadrangleConer.BottomRight] = new Vector2(u, 0.0f);
				uv[(int)QuadrangleConer.TopRight] = new Vector2(u, v);
				mMesh.uv = uv;
				mMesh.triangles = new int[] { 
					0, 2, 1,
					2, 0, 3,
				};

				return ArchitectureBuildResult.Ok;
			}
		}

		public GameObject Parent {
			get {
				return ObjectUtils.GetParent(mGameObject);
			}
			set {
				ObjectUtils.SetParent(mGameObject, value);
			}
		}

		public IList<Polygon> GetBuildHoles() {
			return mBuildHoles;
		}
	}
}
