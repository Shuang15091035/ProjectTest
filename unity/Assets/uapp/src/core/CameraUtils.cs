using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class CameraUtils {

		public static Texture2D CameraShot(Camera camera) {
			if (camera == null) {
				return null;
			}
			int width = (int)camera.pixelWidth;
			int height = (int)camera.pixelHeight;
			// 创建纹理，存放截图数据
			Texture2D tex = new Texture2D(width, height, TextureFormat.RGB24, false);
			// 创建RTT，改变Camera的渲染目标
			RenderTexture rt = new RenderTexture(width, height, 24);
			camera.targetTexture = rt;
			// 强制渲染
			camera.Render();
			// 设置为active的RTT可以是Texture2D的ReadPixels方法读取的不是屏幕数据而是RTT的数据
			RenderTexture.active = rt;
			tex.ReadPixels(camera.pixelRect, 0, 0);
			// 渲染目标还原
			camera.targetTexture = null;
			// 清除RTT
			RenderTexture.active = null;
			GameObject.DestroyImmediate(rt);
			return tex;
		}
	}

}
