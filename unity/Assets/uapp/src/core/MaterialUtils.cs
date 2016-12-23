using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace uapp {
	
	public static class MaterialUtils {

		public static Texture GetTextureEx(Material material, string textureName) {
			if (!material.HasProperty(textureName)) {
				return null;
			}
			return material.GetTexture(textureName);
		}

		public delegate void OnTexture(Texture texture, string propertyName);
		public static void EnumTexture(Material material, OnTexture onTexture) {
			#if UNITY_EDITOR
			if (material == null) {
				return;
			}
			var shader = material.shader;
			if (shader == null) {
				return;
			}
			int propertyCount = ShaderUtil.GetPropertyCount(shader);
			for (int i = 0; i < propertyCount; i++) {
				var propertyType = ShaderUtil.GetPropertyType(shader, i);
				if (propertyType == ShaderUtil.ShaderPropertyType.TexEnv) {
					var propertyName = ShaderUtil.GetPropertyName(shader, i);
					var texture = material.GetTexture(propertyName);
					if (texture != null && onTexture != null) {
						onTexture(texture, propertyName);
					}
				}
			}
			#else
			foreach (var shaderTexture in uapp.Shaders.Textures.AllTextures()) {
				if (material.HasProperty(shaderTexture)) {
					var texture = material.GetTexture(shaderTexture);
					if (texture != null && onTexture != null) {
						onTexture(texture,shaderTexture);
					}
				}
			}
			#endif
		}

	}

}
