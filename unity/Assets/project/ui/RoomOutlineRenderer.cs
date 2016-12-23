using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

namespace project {
	
	public class RoomOutlineRenderer : MonoBehaviour {

		//		private static Material mWallOutlineMaterial;
				private static Material mFloorPointMaterial;

		private Room mRoom;
		public Room Room {
			get {
				return mRoom;
			}
			set {
				mRoom = value;
			}
		}

		//		void OnPostRender() {
		//			if (mRoom == null) {
		//				return;
		//			}
		//			var floor = mRoom.Floor;
		//			if (floor == null) {
		//				return;
		//			}
		//			var walls = mRoom.Walls;
		//			if (mWallOutlineMaterial == null) {
		////				mWallOutlineMaterial = new Material(Shader.Find("uapp/VertexColor"));
		//				var shader = Shader.Find ("Hidden/Internal-Colored");
		//				var mat = new Material (shader);
		//				mat.hideFlags = HideFlags.HideAndDontSave;
		////				mat.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
		////				mat.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
		////				// Turn off backface culling, depth writes, depth test.
		////				mat.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		////				mat.SetInt ("_ZWrite", 0);
		////				mat.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
		//				mWallOutlineMaterial = mat;
		//			}
		//			if (walls != null) {
		//				mWallOutlineMaterial.SetPass(0);
		//				GL.Begin(GL.LINES);
		//				foreach (var wall in walls) {
		//					if (wall.EditState == EditState.Highlight) {
		//						GL.Color(Color.red);
		//					} else if (wall.EditState == EditState.Selected) {
		//						GL.Color(Color.green);
		//					} else {
		//						GL.Color(Color.yellow);
		//					}
		//					GL.Vertex(wall.Start);
		//					GL.Vertex(wall.End);
		//				}
		//				GL.End();
		//			}
		//
		//			if (mFloorPointMaterial == null) {
		//				//				mWallOutlineMaterial = new Material(Shader.Find("uapp/VertexColor"));
		//				var shader = Shader.Find ("Hidden/Internal-Colored");
		//				var mat = new Material (shader);
		//				mat.hideFlags = HideFlags.HideAndDontSave;
		////				mat.SetInt ("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusDstColor);
		////				mat.SetInt ("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
		////				// Turn off backface culling, depth writes, depth test.
		////				mat.SetInt ("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
		////				mat.SetInt ("_ZWrite", 0);
		////				mat.SetInt ("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
		//				mFloorPointMaterial = mat;
		//			}
		//			var floorData = floor.Data;
		//			var floorPoints = floorData.Points;
		//			if (floorPoints != null) {
		//				var pointHalfSize = Floor.PointSize * 0.5f;
		//				mFloorPointMaterial.SetPass(0);
		//				GL.Begin(GL.QUADS);
		//				GL.Color(Color.green);
		//				foreach (var point in floorPoints) {
		//					GL.Vertex3(-pointHalfSize + point.x,  point.y, pointHalfSize + point.z);
		//					GL.Vertex3(pointHalfSize + point.x, point.y, pointHalfSize + point.z);
		//					GL.Vertex3(pointHalfSize + point.x, point.y, -pointHalfSize + point.z);
		//					GL.Vertex3(-pointHalfSize + point.x, point.y, -pointHalfSize + point.z);
		//				}
		//				GL.End();
		//			}
		//		}

		private VectorLine mWallLine;
		private List<Color> mWallLineColors;

		//		void Start() {
		//			var points = new Vector3[] {
		//				new Vector3(0.0f, 0.0f, 0.0f),
		//				new Vector3(1.0f, 1.0f, 1.0f),
		//				new Vector3(3.0f, 4.0f, 5.0f),
		//				new Vector3(-3.0f, -4.0f, 5.0f),
		//			};
		//			VectorLine.SetCamera3D(SharedModel.Instance.Photographer.CurrentCamera.Camera);
		//			line = VectorLine.SetLine3D(Color.green, points);
		//			line.lineWidth = 3f;
		//			line.joins = Joins.Fill;
		//			line.points3[1] = Vector3.zero;
		//		}

		void Update() {
			if (mRoom == null) {
				return;
			}
			var floor = mRoom.Floor;
			if (floor == null) {
				return;
			}
			var floorData = floor.Data;
			var numPoints = floorData.NumPoints;
			if (numPoints < 2) {
				VectorLine.Destroy(ref mWallLine);
				return;
			}
			var numWalls = mRoom.NumWalls;

			var lineColor = uapp.ColorUtils.FromRbga(0x5093e1ff);
			var lineHighlightColor = uapp.ColorUtils.FromRbga(0xff6c00ff);
			var lineSelectedColor = uapp.ColorUtils.FromRbga(0xff6c00ff);
			VectorLine.SetCamera3D(SharedModel.Instance.Photographer.CurrentCamera.Camera);
			if (mWallLineColors == null) {
				mWallLineColors = new List<Color>();
			}
			if (mWallLine == null || mWallLine.points3.Count != numPoints + 1) {
				VectorLine.Destroy(ref mWallLine);

				mWallLine = VectorLine.SetLine3D(lineColor, floorData.ToPointArray());
				mWallLine.points3.Add(floorData.PointAt(0));
				updateWallLinePoints(floorData);

				mWallLine.lineWidth = Wall.LineThickness;
				mWallLine.joins = Joins.Fill;
				mWallLineColors.Clear();
				for (var i = 0; i < numWalls; i++) {
//					var wall = mRoom.Walls[i];
//					switch (wall.EditState) {
//						case EditState.Normal:
//							{
//								mLine.SetColor(lineColor, i);
//								break;
//							}
//						case EditState.Highlight:
//							{
//								mLine.SetColor(lineHighlightColor, i);
//								break;
//							}
//						case EditState.Selected:
//							{
//								mLine.SetColor(lineSelectedColor, i);
//								break;
//							}
//					}
					mWallLineColors.Add(lineColor);
				}
			} else {
				updateWallLinePoints(floorData);
				for (var i = 0; i < numWalls; i++) {
					var wall = mRoom.Walls[i];
					switch (wall.EditState) {
						case EditState.Normal:
							{
//								mLine.SetColor(lineColor, i);
								mWallLineColors[i] = lineColor;
								break;
							}
						case EditState.Highlighted:
							{
//								mLine.SetColor(lineHighlightColor, i);
								mWallLineColors[i] = lineHighlightColor;
								break;
							}
						case EditState.Selected:
							{
//								mLine.SetColor(lineSelectedColor, i);
								mWallLineColors[i] = lineSelectedColor;
								break;
							}
					}
				}
				mWallLine.SetColors(mWallLineColors);
				mWallLine.Draw3D();
			}
		}

		private void updateWallLinePoints(uapp.Polygon floorData) {
			var numPoints = floorData.NumPoints;
			var point = Vector3.zero;
			for (var i = 0; i < numPoints; i++) {
				point = floorData.PointAt(i);
				point.y = Heights.FloorPoint;
				mWallLine.points3[i] = point;
			}
			point = floorData.PointAt(0);
			point.y = Heights.FloorPoint;
			mWallLine.points3[numPoints] = point;
		}

		public void Clear() {
			VectorLine.Destroy(ref mWallLine);
			mRoom = null;
		}

//		void OnPostRender() {
//			if (mRoom == null) {
//				return;
//			}
//			var floor = mRoom.Floor;
//			if (floor == null) {
//				return;
//			}
//
//			if (mFloorPointMaterial == null) {
//				var shader = Shader.Find(uapp.Shaders.Diffuse);
//				var mat = new Material(shader);
////				mat.hideFlags = HideFlags.HideAndDontSave;
//				var texture = uapp.File.Resource("spot").GetContent<Texture2D>();
//				mat.mainTexture = texture;
//				mFloorPointMaterial = mat;
//			}
//			var floorData = floor.Data;
//			var floorPoints = floorData.Points;
//			if (floorPoints != null) {
//				var pointHalfSize = Floor.PointSize * 0.5f;
//				mFloorPointMaterial.SetPass(0);
//				GL.Begin(GL.QUADS);
//				foreach (var point in floorPoints) {
//					GL.TexCoord2(0.0f, 1.0f);
//					GL.Vertex3(-pointHalfSize + point.x, point.y, pointHalfSize + point.z);
//					GL.TexCoord2(1.0f, 1.0f);
//					GL.Vertex3(pointHalfSize + point.x, point.y, pointHalfSize + point.z);
//					GL.TexCoord2(1.0f, 0.0f);
//					GL.Vertex3(pointHalfSize + point.x, point.y, -pointHalfSize + point.z);
//					GL.TexCoord2(0.0f, 0.0f);
//					GL.Vertex3(-pointHalfSize + point.x, point.y, -pointHalfSize + point.z);
//				}
//				GL.End();
//			}
//		}

	}
}
