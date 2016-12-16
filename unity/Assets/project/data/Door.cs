using UnityEngine;
using System.Collections;

namespace project {
	
	public class Door : InWallItem {

		private static Texture2D img_door_n = null;
		private static Texture2D img_door_p = null;

		private MeshRenderer mMeshRenderer;
		private MeshCollider mCollider;

		public Door(string name, int id, SKU sku, GameObject parent = null) : base(name, id, sku, parent) {
			mDesignObject = new GameObject();
			uapp.ObjectUtils.SetParent(mDesignObject, mGameObject);
			if (img_door_n == null) {
				img_door_n = uapp.File.Resource("img_door_n").GetContent<Texture2D>();
			}
			if (img_door_p == null) {
				img_door_p = uapp.File.Resource("img_door_p").GetContent<Texture2D>();
			}
			var material = new Material(Shader.Find(uapp.Shaders.DiffuseAlpha));
			material.SetTexture(uapp.Shaders.Textures.Diffuse, img_door_n);

			mData = new Rect(Vector2.zero, new Vector2(0.5f, 0.5f)); // NOTE 先写死
			uapp.MeshUtils.RectXZObject(mDesignObject, material, mData.width, mData.height, new Vector2(0.0f, 0.05f), Heights.WallItem);

			mMeshRenderer = mDesignObject.GetComponent<MeshRenderer>();

			mCollider = mDesignObject.AddComponent<MeshCollider>();
			mCollider.sharedMesh = uapp.ObjectUtils.GetMesh(mDesignObject);
		}

		public override void OnDelete() {
			base.OnDelete();
		}

		public override EditState EditState {
			get {
				return base.EditState;
			}
			set {
				base.EditState = value;
				switch (mEditState) {
					case EditState.Normal:
						mMeshRenderer.sharedMaterial.SetTexture(uapp.Shaders.Textures.Diffuse, img_door_n);
						break;
					case EditState.Highlighted:
					case EditState.Selected:
						mMeshRenderer.sharedMaterial.SetTexture(uapp.Shaders.Textures.Diffuse, img_door_p);
						break;
				}
			}
		}

		public override bool IsPointInXZ(Vector3 point) {
			var ray = new Ray(new Vector3(point.x, 1.0f, point.z), Vector3.down);
			RaycastHit result;
			return mCollider.Raycast(ray, out result, 100.0f);
		}
	}

}
