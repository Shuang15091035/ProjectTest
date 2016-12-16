using UnityEngine;
using System.IO;
using System.Collections;

using UnityEditor;

namespace uapp {
	
	public class Utils : Editor {

		[MenuItem("Utils/Inverse texture")]
		static void InverseTexture() {
			var textures = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets);
			if (textures == null || textures.Length == 0) {
				return;
			}

			foreach (Texture2D texture in textures) {
				var tex = uapp.TextureUtils.Inverse(texture);
				var path = AssetDatabase.GetAssetPath(texture);
				path = Path.GetDirectoryName(path) + "/" + Path.GetFileNameWithoutExtension(path) + "_i.png"; 
				var bytes = tex.EncodeToPNG();
				System.IO.File.WriteAllBytes(path, bytes);
				GameObject.DestroyImmediate(tex);
			}
		}
	}

}
