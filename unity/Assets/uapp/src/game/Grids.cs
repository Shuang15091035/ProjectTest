/**
 * @file Grid.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class Grids : MonoBehaviour {

		public int StartRow = -10;
		public int StartColumn = -10;
		public int NumRows = 20;
		public int NumColumns = 20;
		public float GridWidth = 1.0f;
		public float GridHeight = 1.0f;
		public float GridSize {
			set {
				GridWidth = value;
				GridHeight = value;
			}
		}
		public Color GridColor = Color.white;
		public Material GridMaterial;

		public void OnRenderObject() {
			Material material = gridMaterial;
			material.SetPass(0);

			GL.PushMatrix();
			GL.MultMatrix(transform.localToWorldMatrix);

			GL.Begin(GL.LINES);
			////			if (GridMaterial == null) {
			////				GL.Color(GridColor);
			////			} else {
			//				GL.Color(Color.white);
			//			//}
			GL.Color(GridColor);

			int lineRows = NumRows + 1;
			int lineColumns = NumColumns + 1;
			int EndRow = StartRow + NumRows;
			int EndColumn = StartColumn + NumColumns;
			float startX = StartColumn * GridWidth;
			float endX = EndColumn * GridWidth;
			float startZ = StartRow * GridHeight;
			float endZ = EndRow * GridHeight;
			float x = 0.0f;
			float z = 0.0f;
			for (int i = 0; i < lineRows; i++) {
				z = startZ + (i * GridHeight);
				GL.Vertex3(startX, 0.0f, z);
				GL.Vertex3(endX, 0.0f, z);
			}
			for (int i = 0; i < lineColumns; i++) {
				x = startX + (i * GridWidth);
				GL.Vertex3(x, 0.0f, startZ);
				GL.Vertex3(x, 0.0f, endZ);
			}

			GL.End();
			GL.PopMatrix();
		}

		private Material gridMaterial {
			get {
				if (GridMaterial == null) {
					// simple color
					Shader shader = Shader.Find("Hidden/Internal-Colored");
					GridMaterial = new Material(shader);
					GridMaterial.hideFlags = HideFlags.HideAndDontSave;
					// Turn on alpha blending
					GridMaterial.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
					GridMaterial.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
					// Turn backface culling off
					GridMaterial.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
					// Turn off depth writes
					GridMaterial.SetInt ("_ZWrite", 0);
				}
				return GridMaterial;
			}
		}
	}
}

