using UnityEngine;
using System.Collections;

namespace uapp {

// *** build in names ***
//	_Color
//	_MainTex
//	_Cutoff
//	_Glossiness
//	_Metallic
//	_MetallicGlossMap
//	_BumpScale
//	_BumpMap
//	_Parallax
//	_ParallaxMap
//	_OcclusionStrength
//	_OcclusionMap
//	_EmissionColor
//	_EmissionMap
//	_DetailMask
//	_DetailAlbedoMap
//	_DetailNormalMapScale
//	_DetailNormalMap
//	_UVSec
//	_EmissionScaleUI
//	_EmissionColorUI
//	_Mode
//	_SrcBlend
//	_DstBlend
//	_ZWrite

	public class Shaders {

		public static string Standard = "uapp/Standard";
		public static string Diffuse = "uapp/Diffuse";
		public static string DiffuseAlpha = "uapp/Diffuse+Alpha";
		public static string DiffuseLightmap = "uapp/Diffuse+LightMap";
		public static string DiffuseReflective2D = "uapp/Diffuse+Reflective2D";
		public static string DiffuseReflective2DLightMap = "uapp/Diffuse+Reflective2D+LightMap";
		public static string TransparentLightmap = "uapp/Transparent+LightMap";

		public static class Textures {

			public static string Diffuse = "_MainTex";
			public static string DiffuseST = "_MainTex_ST";
			public static string Lightmap = "_LightMap";
            public static string Reflective = "_Reflective2D";
			public static string Transparent = "_Transparent";
			public static string Bump = "_BumpMap";
			public static string AO = "_AO";
			public static string Metallic = "_MetallicGlossMap";
			public static string DetailDiffuse = "_DetailAlbedoMap";
			public static string DetailBump = "_DetailNormalMap";

			private static string[] allTextures = null;
			public static string[] AllTextures() {
				if (allTextures == null) {
					allTextures = new string[]{
						Diffuse,
						Lightmap,
						Reflective,
						Transparent,
						Bump,
						AO,
					};
				}
				return allTextures;
			}
		}

		public static class Colors {

			public static string Diffuse = "_Color";

			private static string[] allColors= null;
			public static string[] AllColors() {
				if (allColors == null) {
					allColors = new string[]{
						Diffuse,
					};
				}
				return allColors;
			}
		}

		public static class Rim {

			public static string Color = "_RimColor";
			public static string Power = "_RimPower";
		}

		public static class Const {
			
			public static string ReflAmount = "_ReflAmount";
		}

		public class Properties {

			public Color DiffuseColor = Color.white;
			public Texture DiffuseTexture;
			public Texture LightmapTexture;
			public Texture TransparentTexture;

			public void setupMaterial(Material material) {
				if (material.HasProperty(Colors.Diffuse)) {
					material.SetColor(Colors.Diffuse, DiffuseColor);
				}
				if (material.HasProperty(Textures.Diffuse) && DiffuseTexture != null) {
					material.SetTexture(Textures.Diffuse, DiffuseTexture);
				}
				if (material.HasProperty(Textures.Lightmap) && LightmapTexture != null) {
					material.SetTexture(Textures.Lightmap, LightmapTexture);
				}
				if (material.HasProperty(Textures.Transparent) && TransparentTexture != null) {
					material.SetTexture(Textures.Transparent, TransparentTexture);
				}
			}

			public string ToShader() {
				int flag = 0;
				if (DiffuseTexture != null) {
					flag = Flags.Add(flag, 0x1 << 0);
				}
				if (LightmapTexture != null) {
					flag = Flags.Add(flag, 0x1 << 1);
				}
				if (TransparentTexture != null) {
					flag = Flags.Add(flag, 0x1 << 2);
				}
				switch (flag) {
					case 0x0: // 000
						return Shaders.Standard;
					case 0x1: // 001
					case 0x2: // 010
					case 0x3: // 011
						return Shaders.DiffuseLightmap;
					case 0x4: // 100
					case 0x5: // 101
					case 0x6: // 110
					case 0x7: // 111
						return Shaders.TransparentLightmap;
				}
				return Shaders.Standard;
			}
		}
	}
}
