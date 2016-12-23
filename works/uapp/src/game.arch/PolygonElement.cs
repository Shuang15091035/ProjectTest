using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public interface IPolygonElement : IArchElement {
		
		Polygon Data { get; }
		float Height { get; set; }
		ArchitectureBuildResult Build();
		GameObject Parent { get; set; }
	}

	public class PolygonElement : ArchElement, IPolygonElement {

		protected Polygon mData = new Polygon();
		protected float mPolygonHeight;

		protected GameObject mGameObject;
		protected Mesh mMesh;
		protected Material mMaterial;
		protected RealWorldTexture mTexture = new RealWorldTexture();

		protected PolygonElement(string name, RealWorldTexture texture) {
			mGameObject = new GameObject(name);

			mMesh = new Mesh();
			var mf = mGameObject.AddComponent<MeshFilter>();
			mf.sharedMesh = mMesh;

			mMaterial = new Material(Shader.Find(Shaders.Diffuse));
			mTexture.Assign(texture);
			mMaterial.mainTexture = mTexture.Texture;
			var mr = mGameObject.AddComponent<MeshRenderer>();
			mr.sharedMaterial = mMaterial;
		}

		public Polygon Data {
			get {
				return mData;
			}
		}

		public float Height {
			get {
				return mPolygonHeight;
			}
			set {
				mPolygonHeight = value;
			}
		}

		public virtual ArchitectureBuildResult Build() {			
			return trianglulate(true);
		}

		protected ArchitectureBuildResult trianglulate(bool clockwise) {
			float u = 1.0f / mArch.Scale / mTexture.ActualWidth;
			float v = 1.0f / mArch.Scale / mTexture.ActualHeight;
			var b = PolygonMesh.TrianglulateXZ(mData, null, mPolygonHeight, mMesh, clockwise, null, (Vector2 uv) => {
				uv.x *= u;
				uv.y *= v;
				return uv;
			}); // TODO 详细返回
			mGameObject.SetActive(b);
			return ArchitectureBuildResult.Ok;
		}

		public GameObject Parent {
			get {
				return ObjectUtils.GetParent(mGameObject);
			}
			set {
				ObjectUtils.SetParent(mGameObject, value);
			}
		}

	}
}
