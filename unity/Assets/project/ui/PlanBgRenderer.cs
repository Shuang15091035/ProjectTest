using UnityEngine;
using System.Collections;

namespace project {
	
	public class PlanBgRenderer {

		private static Material mMaterial;
		private Mesh mMesh;
		private GameObject mGameObject;
	
		private Plan mPlan;
		public Plan Plan {
			get {
				return mPlan;
			} set {
				mPlan = value;
			}
		}

        public GameObject GameObject {
            get {
                return mGameObject;
            }
        }

		public void Build() {
			if (mPlan == null) {
				return;
			}
			var parent = mPlan.Root;
			if (mMaterial == null) {
				mMaterial = new Material(Shader.Find("uapp/Diffuse"));
			}
			if (mGameObject == null) {
				mGameObject = SharedModel.Instance.PlanBackgroundObject;
				uapp.ObjectUtils.SetParent(mGameObject, parent);

				mMesh = new Mesh();
				uapp.MeshUtils.AddMeshComponentPure(mGameObject, mMesh, mMaterial);
			}

			var texture = mPlan.Background.GetContent<Texture2D>();
			mMaterial.SetTexture(uapp.Shaders.Textures.Diffuse, texture);
			var w = texture.width;
			var h = texture.height;
			var hw = w * 0.5f;
			var hh = h * 0.5f;
			var s = 10.0f / w; // NOTE 图片一般为1000-2000大小，缩小倍数让它在场景中不至于过大，这里缩放到10大小
			hw *= s;
			hh *= s;

			mMesh.Clear();
			mMesh.vertices = new Vector3[] {
				new Vector3(-hw, 0.0f, hh),
				new Vector3(hw, 0.0f, hh),
				new Vector3(hw, 0.0f, -hh),
				new Vector3(-hw, 0.0f, -hh),
			};
			mMesh.uv = new Vector2[] {
				new Vector2(0.0f, 1.0f),
				new Vector2(1.0f, 1.0f),
				new Vector2(1.0f, 0.0f),
				new Vector2(0.0f, 0.0f),
			};
			mMesh.triangles = new int[] {
				0, 1, 2,
				2, 3, 0,
			};
		}

//		void OnPostRender() {
//			if (mPlan == null) {
//				return;
//			}
//			if (mMaterial == null) {
//				mMaterial = new Material(Shader.Find("uapp/Diffuse"));
//			}
//			mMaterial.SetTexture(uapp.Shaders.Textures.Diffuse, mPlan.Background.Content(typeof(Texture2D)) as Texture2D);
//			mMaterial.SetPass(0);
//			GL.Begin(GL.QUADS);
//			GL.Vertex3(-1.0f, 0.0f, 1.0f);
//			GL.TexCoord2(0.0f, 1.0f);
//			GL.Vertex3(1.0f, 0.0f, 1.0f);
//			GL.TexCoord2(1.0f, 1.0f);
//			GL.Vertex3(1.0f, 0.0f, -1.0f);
//			GL.TexCoord2(1.0f, 0.0f);
//			GL.Vertex3(-1.0f, 0.0f, -1.0f);
//			GL.TexCoord2(0.0f, 0.0f);
//			GL.End();
//		}
	}

}
