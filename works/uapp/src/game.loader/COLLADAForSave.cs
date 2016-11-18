/**
 * @file COLLADAForSave.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-11-10
 * @brief
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace uapp {

	public class COLLADAForSave {

		private COLLADA mCOLLADA;
		//没使用过
		//private GameObject mObject;
		private uapp.IFile mFile;
		//没使用过
		//private OnSceneSaveListener mListener;
		//private AsyncTask<bool> mTask;
		private GameObject mRoot;
		private DaeExtra mExtra;

		private Dictionary<string, UnityEngine.Material> mMaterials;
		private Dictionary<string, UnityEngine.Texture> mTextures;
		private Dictionary<string, UnityEngine.Mesh> mMeshes;
		private Dictionary<string, COLLADA.Newparam> mSampler2Ds;

		public COLLADA Result {
			get {
				return mCOLLADA;
			}
		}

		public COLLADAForSave(uapp.IFile file, OnSceneSaveListener listener, AsyncTask<bool> task) {
			mFile = file;
			//没使用过
			//mListener = listener;
			//mTask = task;
			if (file.Extra is DaeExtra) {
				mExtra = file.Extra as DaeExtra;
			} else {
				mExtra = new DaeExtra();
				mExtra.Obfuscated = false;
			}
		}

		public IEnumerator Build(GameObject[] objects) {
			if (objects == null) {
				yield break;
			}

			string sceneName = "uapp_scene";
			float meter = 1.0f; // TODO

			// 只有一个GameObject时，它就是根节点了
			mRoot = null;
			if (objects.Length == 1) {
				mRoot = objects[0];
			}
			sceneName = mRoot.name.ToUpper(); // NOTE 照顾OSX系统预览
			string sceneUrl = "#" + sceneName;

			mCOLLADA = new COLLADA();
			// <asset>
			var asset = new COLLADA.Asset();
			asset.up_axis = COLLADA.Asset.UpAxis.Y_UP; // TODO 验证是否正确
			// <unit>
			var unit = new COLLADA.Unit();
			unit.meter = meter;
			asset.unit = unit;
			// </unit>
			// <contributor>
			var contributor = new COLLADA.Contributor();
			contributor.hashcode = !mExtra.Obfuscated ? null : "2e294299c14110882c0584545a99addb"; // luojunwen19850618
			asset.contributor = contributor;
			// </contributor>
			mCOLLADA.asset = asset;
			// <asset>

			var scene = new COLLADA.Scene();
			var instance_visual_scene = new COLLADA.InstanceVisualScene();
			instance_visual_scene.url = sceneUrl;
			scene.instance_visual_scene = instance_visual_scene;
			mCOLLADA.scene = scene;
			var library_visual_scenes = new List<COLLADA.VisualScene>();
			var visual_scene = new COLLADA.VisualScene();
			visual_scene.id = sceneName;
			library_visual_scenes.Add(visual_scene);

			if (mRoot != null) {
				IEnumerator e = BuildNode(visual_scene, mRoot, null);
				while (e.MoveNext()) {
					yield return e.Current;
				}
			} else {
				foreach (var obj in objects) {
					IEnumerator e = BuildNode(visual_scene, obj, null);
					while (e.MoveNext()) {
						yield return e.Current;
					}
				}
			}
			mCOLLADA.library_visual_scenes = library_visual_scenes;

			// animation
			BuildAnimations(visual_scene);
		}

		public IEnumerator BuildNode(COLLADA.VisualScene visualScene, GameObject obj, COLLADA.Node parent) {
			if (obj == null) {
				yield break;
			}
			var node = new COLLADA.Node();
			//node.id = "node-" + obj.name; // 屎忽痕
			node.id = obj.name;
			node.name = obj.name;
			Transform transform = obj.transform;
			BuildTransform(transform, node);
			if (parent != null) {
				parent.AddChild(node);
			} else {
				visualScene.AddNode(node);
			}
			IEnumerator e = BuildGeometry(obj, node);
			while (e.MoveNext()) {
				yield return e.Current;
			}
			//BuildAnimation(obj, node);

			foreach (Transform ct in transform) {
				GameObject child = ct.gameObject;
				e = BuildNode(visualScene, child, node);
				while (e.MoveNext()) {
					yield return e.Current;
				}
			}

			node.gameObject = obj;
		}

		public void BuildTransform(Transform transform, COLLADA.Node node) {
			Vector3 position = transform.localPosition;
			float angle;
			Vector3 axis;
            //transform.localRotation.ToAngleAxis (out angle, out axis);
            //Vector4 rotate = new Vector4 (axis.x, axis.y, axis.z, -angle);
            QuaternionUtils.ToRightHandAngleAxis (transform.localRotation, out angle, out axis);
            Vector4 rotate = new Vector4 (axis.x, axis.y, axis.z, angle);
            Vector3 scale = transform.localScale;
            if (transform.gameObject != mRoot) { // NOTE 根节点不写入位置和旋转信息
				position = Vector3Utils.ToRightHand(position); // NOTE 转换为右手坐标系
				var colladaTranslate = new COLLADA.Translate();
				colladaTranslate.value = Vector3Utils.Format(position);
				node.AddTranslate(colladaTranslate);
				var colladaRotate = new COLLADA.Rotate();
				colladaRotate.value = Vector4Utils.Format(rotate); 
				node.AddRotate(colladaRotate);
			}
			node.scale = Vector3Utils.Format(scale);
		}

		public IEnumerator BuildGeometry(GameObject obj, COLLADA.Node node) {
			var meshFilters = obj.GetComponents<MeshFilter>();
			if (meshFilters == null || meshFilters.Length == 0) {
				yield break;
			}
			var meshRenderers = obj.GetComponents<MeshRenderer>();
			if (meshRenderers == null || meshRenderers.Length == 0) {
				yield break;
			}
			for (int meshFilerIndex = 0; meshFilerIndex < meshFilters.Length; meshFilerIndex++) {
				var meshFilter = meshFilters[meshFilerIndex];
				var engineMesh = meshFilter.sharedMesh;
				if (engineMesh == null) {
					continue;
				}
				var meshRenderer = meshRenderers[meshFilerIndex];
				var materials = meshRenderer.sharedMaterials;
				string engineMeshName = StringUtils.RemoveAll(engineMesh.name, " Instance");
				if (StringUtils.IsNullOrBlank(engineMeshName)) {
					engineMeshName = StringUtils.Standardize(obj.name);
				}
				string meshId = "geom-" + engineMeshName;
				string meshName = engineMeshName;

				// NOTE 防止重复build，如果找到id相同的但是mesh不一样的也需要build，id需要修改一下
				var aEngineMesh = getEngineMesh(meshId);
				if (aEngineMesh != null && aEngineMesh != engineMesh) {
					meshId += "_1";
				}

				IEnumerator e = null;
				var materialNames = new string[materials.Length];
				// <instance_geometry>
				var instance_geometry = new COLLADA.InstanceGeometry();
				instance_geometry.url = "#" + meshId;
				if (materials != null && materials.Length > 0) {
					var bind_material = new COLLADA.BindMaterial();
					var technique_common = new COLLADA.TechniqueCommon(); // TODO 其他Technique类型
					int materialIndex = 0;
					foreach (var material in materials) {
						var materialName = StringUtils.RemoveAll(material.name, " (Instance)");
						materialNames[materialIndex] = materialName;
						var materialId = materialName + "-material";
						// <instance_material>
						var instance_material = new COLLADA.InstanceMaterial();
						instance_material.symbol = materialName;
						instance_material.target = "#" + materialId;
						buildBindVertexInputFromShader(material.shader, instance_material, meshRenderer);
						e = BuildMaterial(material, materialId, materialName, meshRenderer);
						while (e.MoveNext()) {
							yield return e.Current;
						}
						technique_common.AddInstanceMaterial(instance_material);
						// </instance_material>
						materialIndex++;
					}
					bind_material.technique_common = technique_common;
					instance_geometry.bind_material = bind_material;
				}
				node.AddInstanceGeometry(instance_geometry);
				// </instance_geometry>

				e = BuildMesh(engineMesh, meshId, meshName, materialNames, meshRenderer);
				while (e.MoveNext()) {
					yield return e.Current;
				}
			}
		}

		private void buildBindVertexInputFromShader(Shader engineShader, COLLADA.InstanceMaterial instance_material, MeshRenderer meshRenderer) {
			if (StringUtils.Equals(engineShader.name, Shaders.Standard)) {
				if (meshRenderer.lightmapIndex >= 0) {
					// <bind_vertex_input>
					var bind_vertex_input = new COLLADA.BindVertexInput();
					bind_vertex_input.semantic = "CHANNEL2";
					bind_vertex_input.input_semantic = COLLADA.Input.Semantic.TEXCOORD;
					bind_vertex_input.input_set = 1;
					instance_material.AddBindVertexInput(bind_vertex_input);
					// </bind_vertex_input>
				}
			} else if (StringUtils.Equals(engineShader.name, Shaders.DiffuseLightmap)) {
				// <bind_vertex_input>
				var bind_vertex_input = new COLLADA.BindVertexInput();
				bind_vertex_input.semantic = "CHANNEL1";
				bind_vertex_input.input_semantic = COLLADA.Input.Semantic.TEXCOORD;
				bind_vertex_input.input_set = 0;
				instance_material.AddBindVertexInput(bind_vertex_input);
				// </bind_vertex_input>
				// <bind_vertex_input>
				bind_vertex_input = new COLLADA.BindVertexInput();
				bind_vertex_input.semantic = "CHANNEL2";
				bind_vertex_input.input_semantic = COLLADA.Input.Semantic.TEXCOORD;
				bind_vertex_input.input_set = 1;
				instance_material.AddBindVertexInput(bind_vertex_input);
				// </bind_vertex_input>
			} else if (StringUtils.Equals(engineShader.name, Shaders.TransparentLightmap)) {
				// <bind_vertex_input>
				var bind_vertex_input = new COLLADA.BindVertexInput();
				bind_vertex_input.semantic = "CHANNEL1";
				bind_vertex_input.input_semantic = COLLADA.Input.Semantic.TEXCOORD;
				bind_vertex_input.input_set = 0;
				instance_material.AddBindVertexInput(bind_vertex_input);
				// </bind_vertex_input>
				// <bind_vertex_input>
				bind_vertex_input = new COLLADA.BindVertexInput();
				bind_vertex_input.semantic = "CHANNEL2";
				bind_vertex_input.input_semantic = COLLADA.Input.Semantic.TEXCOORD;
				bind_vertex_input.input_set = 1;
				instance_material.AddBindVertexInput(bind_vertex_input);
				// </bind_vertex_input>
				// <bind_vertex_input>
				bind_vertex_input = new COLLADA.BindVertexInput();
				bind_vertex_input.semantic = "CHANNEL1";
				bind_vertex_input.input_semantic = COLLADA.Input.Semantic.TEXCOORD;
				bind_vertex_input.input_set = 0;
				instance_material.AddBindVertexInput(bind_vertex_input);
				// </bind_vertex_input>
			}
		}

		public IEnumerator BuildMaterial(Material engineMaterial, string materialId, string materialName, MeshRenderer meshRenderer) {
			if (hasEngineMaterial(materialId)) {
				yield break;
			}
			var material = new COLLADA.Material();
			material.id = materialId;
			material.name = materialName;
			// <instance_effect>
			var instance_effect = new COLLADA.InstanceEffect();
			instance_effect.url = "#" + materialName;
			material.instance_effect = instance_effect;
			// </instance_effect>
			// <library_effects><effect>
			IEnumerator e = BuildEffect(engineMaterial, materialName, meshRenderer);
			while (e.MoveNext()) {
				yield return e.Current;
			}
			// <library_effects></effect>
			mCOLLADA.AddMaterial(material);
			addEngineMaterial(materialId, engineMaterial);
		}

		public IEnumerator BuildEffect(Material engineMaterial, string effectId, MeshRenderer meshRenderer) {
			var engineShader = engineMaterial.shader;
			if (engineShader == null) {
				yield break;
			}

			var effect = new COLLADA.Effect();
			effect.id = effectId;

			if (mSampler2Ds != null) {
				mSampler2Ds.Clear();
			}
			IEnumerator e = null;
			// <profile_COMMON>
			var profile_COMMON = new COLLADA.ProfileCOMMON();
			effect.profile_COMMON = profile_COMMON;
			// <newparam>
			if (engineMaterial.HasProperty(Shaders.Textures.Diffuse)) {
				var texture = engineMaterial.GetTexture(Shaders.Textures.Diffuse);
				if (texture is Texture2D) {
					var texture2D = texture as Texture2D;
					BuildSampler2D(texture2D, Shaders.Textures.Diffuse, effect);
				}
			}
			// </newparam>
			// <newparam>
			var lightmap = getLightmapFromMaterialOrRenderer(engineMaterial, meshRenderer);
			if (lightmap != null) {
				var texture2D = lightmap;
				BuildSampler2D(texture2D, Shaders.Textures.Lightmap, effect);
			}
			// </newparam>
			// <newparam>
			if (engineMaterial.HasProperty(Shaders.Textures.Reflective)) {
				var texture = engineMaterial.GetTexture(Shaders.Textures.Reflective);
				if (texture is Texture2D) {
					var texture2D = texture as Texture2D;
					BuildSampler2D(texture2D, Shaders.Textures.Reflective, effect);
				}
			}
			// </newparam>
			// <newparam>
			if (engineMaterial.HasProperty(Shaders.Textures.Transparent)) {
				var texture = engineMaterial.GetTexture(Shaders.Textures.Transparent);
				if (texture is Texture2D) {
					var texture2D = texture as Texture2D;
					BuildSampler2D(texture2D, Shaders.Textures.Transparent, effect);
				}
			}
			// </newparam>
			// <newparam>
			if (engineMaterial.HasProperty(Shaders.Textures.Bump)) {
				var texture = engineMaterial.GetTexture(Shaders.Textures.Bump);
				if (texture is Texture2D) {
					var texture2D = texture as Texture2D;
					BuildSampler2D(texture2D, Shaders.Textures.Bump, effect);
				}
			}
			// </newparam>
			// <newparam>
			if (engineMaterial.HasProperty(Shaders.Textures.AO)) {
				var texture = engineMaterial.GetTexture(Shaders.Textures.AO);
				if (texture is Texture2D) {
					var texture2D = texture as Texture2D;
					BuildSampler2D(texture2D, Shaders.Textures.AO, effect);
				}
			}
			// </newparam>
			// <technique>
			e = BuildTechniqueType(engineMaterial, effect, meshRenderer);
			while (e.MoveNext()) {
				yield return e.Current;
			}
			// </technique>
			// </profile_COMMON>

			mCOLLADA.AddEffect(effect);
		}

		public void BuildSampler2D(Texture2D engineTexture, string textureType, COLLADA.Effect effect) {
			var profile_COMMON = effect.profile_COMMON;
			// <newparam>
			var newparam_surface = new COLLADA.Newparam();
			newparam_surface.sid = engineTexture.name + "-surface";
			// <surface>
			var surface = new COLLADA.Surface();
			surface.type = COLLADA.Surface.Types._2D;
			surface.init_from = engineTexture.name + "-tex";
			// <library_images><image>
			BuildImage(engineTexture, surface.init_from);
			// <library_images></image>
			newparam_surface.surface = surface;
			// </surface>
			profile_COMMON.AddNewparam(newparam_surface);
			// </newparam>

			// <newparam>
			var newparam_sampler = new COLLADA.Newparam();
			newparam_sampler.sid = engineTexture.name + "-sampler";
			// <sampler2D>
			var sampler2D = new COLLADA.Sampler2D();
			sampler2D.source = newparam_surface.sid;
			newparam_sampler.sampler2D = sampler2D;
			// </surface>
			profile_COMMON.AddNewparam(newparam_sampler);
			// </newparam>
			effect.profile_COMMON = profile_COMMON;

			addSampler2D(textureType, newparam_sampler);
		}

		public IEnumerator BuildTechniqueType(Material engineMaterial, COLLADA.Effect effect, MeshRenderer meshRenderer) {
			var profile_COMMON = effect.profile_COMMON;
			if (profile_COMMON == null) {
				yield break;
			}
			// TODO other techniques
			// <technique>
			var technique = new COLLADA.TechniqueFX();
			technique.sid = "common";
			// TODO other light models
			// <phong>
			var phong = new COLLADA.Phong();
			// <ambient>
			BuildLightMap(phong, engineMaterial, meshRenderer);
			// </ambient>
			// <diffuse>
			BuildTextureOrColor(phong, Shaders.Textures.Diffuse, "CHANNEL1", Shaders.Colors.Diffuse, engineMaterial);
			// </diffuse>
			// <reflective>
			BuildTextureOrColor(phong, Shaders.Textures.Reflective, "CHANNEL1", "", engineMaterial);
			// </reflective>
			// <transparent>
			BuildTextureOrColor(phong, Shaders.Textures.Transparent, "CHANNEL1", "", engineMaterial);
			// </transparent>
			technique.type = phong;
			// </phong>
			// <extra>
			var extra = BuildTechniqueFXExtra(engineMaterial);
			technique.extra = extra;
			// </extra>
			profile_COMMON.technique = technique;
			// </technique>
		}

		public void BuildLightMap(COLLADA.Phong phong, Material engineMaterial, MeshRenderer meshRenderer) {
			if (getLightmapFromMaterialOrRenderer(engineMaterial, meshRenderer) == null) {
				return;
			}
			var sampler2D = getSampler2D(Shaders.Textures.Lightmap);
			if (sampler2D != null) {
				var texture = new COLLADA.CommonColorOrTexture();
				texture.texture = new COLLADA.Texture();
				texture.texture.texture = sampler2D.sid;
				texture.texture.texcoord = "CHANNEL2";
				phong.ambient = texture;
			}
		}

		public void BuildTextureOrColor(COLLADA.Phong phong, string textureType, string texcoordChannel, string colorType, Material engineMaterial) {
			if (engineMaterial.HasProperty(textureType)) {
				var engineTexture = engineMaterial.GetTexture(textureType);
				if (engineTexture != null) {
					var tex = BuildTexture(textureType, texcoordChannel, engineTexture);
					if (tex != null) {
						var texture = new COLLADA.CommonColorOrTexture();
						texture.texture = tex;
						if (StringUtils.Equals(textureType, Shaders.Textures.Lightmap)) {
							phong.ambient = texture;
						} else if (StringUtils.Equals(textureType, Shaders.Textures.Diffuse)) {
							phong.diffuse = texture;
						} else if (StringUtils.Equals(textureType, Shaders.Textures.Reflective)) {
							phong.reflective = texture;
						} else if (StringUtils.Equals(textureType, Shaders.Textures.Transparent)) {
							phong.transparent = texture;
						}
					}
				} else {
					BuildColor(phong, colorType, engineMaterial);
				}
			} else {
				BuildColor(phong, colorType, engineMaterial);
			}
		}

		public COLLADA.Texture BuildTexture(string textureType, string texcoordChannel, Texture engineTexture) {
			var sampler2D = getSampler2D(textureType);
			if (sampler2D == null) {
				return null;
			}
			var texture = new COLLADA.Texture();
			texture.texture = sampler2D.sid;
			texture.texcoord = texcoordChannel;
			return texture;
		}

		public void BuildColor(COLLADA.Phong phong, string colorType, Material engineMaterial) {
			if (!engineMaterial.HasProperty(colorType)) {
				return;
			}
			var engineColor = engineMaterial.GetColor(colorType);
			var color = new COLLADA.CommonColorOrTexture();
			color.color = new COLLADA.Color();
			color.color.value = ColorUtils.Format(engineColor);
			phong.diffuse = color;
		}

		public bool BuildImage(Texture engineTexture, string textureId) {
			if (hasEngineTexture(textureId)) {
				return false;
			}
			var image = new COLLADA.Image();
			image.id = textureId;
            // NOTE 相对于导出的位置计算贴图路径
            //			var texturePath = Application.dataPath + StringUtils.RemoveFirst(UnityEditor.AssetDatabase.GetAssetPath(engineTexture), "Assets");
#if UNITY_EDITOR
            var textureFile = File.DataPath(StringUtils.RemoveFirst(UnityEditor.AssetDatabase.GetAssetPath(engineTexture), "Assets/"));
#else
            IFile textureFile = null; // TODO 待实现
#endif
            if (textureFile == null) {
                return false;
            }
            var copyTextureToDir = mExtra.copyTextureToDir;
			if (copyTextureToDir != null) {
				copyTextureToDir.Copy(textureFile);
				textureFile = File.FilePath(System.IO.Path.Combine(copyTextureToDir.RealPath, textureFile.FileName));
			}
			if (mExtra.willDoPlatformOptimization) {
				switch (mExtra.platformOptimization) {
					case RuntimePlatform.IPhonePlayer:
						{
							if (StringUtils.Equals(textureFile.Ext, ".exr")) { // NOTE EXR文件转换PVR会出现颜色严重错误
								break;
							}
							IFile pvrFile = TextureUtils.ConvertPVR(textureFile);
							if (pvrFile != null) {
								textureFile = pvrFile;
							}
							break;
						}
					default:
						{
							break;
						}
				}
			}
			image.init_from = PathUtils.RelativePath(mFile.Dir.RealPath, textureFile.RealPath, System.IO.Path.DirectorySeparatorChar, System.IO.Path.DirectorySeparatorChar, '/');
			mCOLLADA.AddImage(image);
			addEngineTexture(textureId, engineTexture);
			return true;
		}

		public COLLADA.Extra BuildTechniqueFXExtra(Material engineMaterial) {
			COLLADA.Extra extra = null;
			COLLADA.TechniqueCore techniqueU = null;
			if (engineMaterial.HasProperty(Shaders.Textures.Bump)) {
				var engineTexture = engineMaterial.GetTexture(Shaders.Textures.Bump);
				if (engineTexture != null) {
					if (extra == null) {
						extra = new COLLADA.Extra();
					}
					COLLADA.TechniqueCore techniqueBump = new COLLADA.TechniqueCore(COLLADA.TechniqueCore.Profile.FCOLLADA);
					var bump = new COLLADA.Bump();
					bump.texture = BuildTexture(Shaders.Textures.Bump, "CHANNEL1", engineTexture);
					techniqueBump.bump = bump;
					extra.AddTechnique(techniqueBump);
				}
			}
			if (engineMaterial.HasProperty(Shaders.Textures.AO)) {
				var engineTexture = engineMaterial.GetTexture(Shaders.Textures.AO);
				if (engineTexture != null) {
					if (extra == null) {
						extra = new COLLADA.Extra();
					}
					COLLADA.TechniqueCore techniqueAO = new COLLADA.TechniqueCore(COLLADA.TechniqueCore.Profile.UCOLLADA);
					var ao = new COLLADA.AO();
					ao.texture = BuildTexture(Shaders.Textures.AO, "CHANNEL1", engineTexture);
					techniqueAO.ao = ao;
					extra.AddTechnique(techniqueAO);
				}
			}
			// NOTE 如果同时具有diffuse_color和diffuse_texture，把diffuse_color导出到extra字段中
			if (engineMaterial.HasProperty(Shaders.Colors.Diffuse)) {
				var diffuseColor = engineMaterial.GetColor(Shaders.Colors.Diffuse);
				if (!ColorUtils.isWhite(diffuseColor)) {
					if (engineMaterial.HasProperty(Shaders.Textures.Diffuse)) {
						var diffuseTexture = engineMaterial.GetTexture(Shaders.Textures.Diffuse);
						if (diffuseTexture != null) {
							if (extra == null) {
								extra = new COLLADA.Extra();
							}
							if (techniqueU == null) {
								techniqueU = new COLLADA.TechniqueCore(COLLADA.TechniqueCore.Profile.UCOLLADA);
								extra.AddTechnique(techniqueU);
							}
							techniqueU.diffuseColor = new COLLADA.Color();
							techniqueU.diffuseColor.value = ColorUtils.Format(diffuseColor);
						}
					}
				}
			}
			if (engineMaterial.HasProperty(Shaders.Const.ReflAmount)) {
				if (extra == null) {
					extra = new COLLADA.Extra();
				}
				if (techniqueU == null) {
					techniqueU = new COLLADA.TechniqueCore(COLLADA.TechniqueCore.Profile.UCOLLADA);
					extra.AddTechnique(techniqueU);
				}
				techniqueU.reflAmount = engineMaterial.GetFloat(Shaders.Const.ReflAmount);
			}
			if (engineMaterial.HasProperty(Shaders.Rim.Color)) {
				if (extra == null) {
					extra = new COLLADA.Extra();
				}
				if (techniqueU == null) {
					techniqueU = new COLLADA.TechniqueCore(COLLADA.TechniqueCore.Profile.UCOLLADA);
					extra.AddTechnique(techniqueU);
				}
				techniqueU.rimColor = new COLLADA.Color();
				techniqueU.rimColor.value = ColorUtils.Format(engineMaterial.GetColor(Shaders.Rim.Color));
			}
			if (engineMaterial.HasProperty(Shaders.Rim.Power)) {
				if (extra == null) {
					extra = new COLLADA.Extra();
				}
				if (techniqueU == null) {
					techniqueU = new COLLADA.TechniqueCore(COLLADA.TechniqueCore.Profile.UCOLLADA);
					extra.AddTechnique(techniqueU);
				}
				techniqueU.rimPower = engineMaterial.GetFloat(Shaders.Rim.Power);
			}
			BuildTechniqueFXExtraTilingOffsets(engineMaterial, ref techniqueU, ref extra);
			return extra;
		}

		public void BuildTechniqueFXExtraTilingOffsets(Material engineMaterial, ref COLLADA.TechniqueCore techniqueCore, ref COLLADA.Extra extra) {
			foreach (var textureName in Shaders.Textures.AllTextures()) {
				BuildTechniqueFXExtraTilingOffset(engineMaterial, textureName, ref techniqueCore, ref extra);
			}
		}

		public void BuildTechniqueFXExtraTilingOffset(Material engineMaterial, string textureName, ref COLLADA.TechniqueCore techniqueCore, ref COLLADA.Extra extra) {
			if (engineMaterial.HasProperty(textureName)) {
				var tiling = engineMaterial.GetTextureScale(textureName);
				var offset = engineMaterial.GetTextureOffset(textureName);
				var tilingOffset = new Vector4(tiling.x, tiling.y, offset.x, offset.y);
				if (!Vector4Utils.IsDefaultTilingOffset(tilingOffset)) {
					if (extra == null) {
						extra = new COLLADA.Extra();
					}
					if (techniqueCore == null) {
						techniqueCore = new COLLADA.TechniqueCore(COLLADA.TechniqueCore.Profile.UCOLLADA);
						extra.AddTechnique(techniqueCore);
					}
					techniqueCore.SetTilingOffsetValue(textureName, Vector4Utils.Format(tilingOffset));
				}
			}
		}

		public IEnumerator BuildMesh(Mesh engineMesh, string meshId, string meshName, string[] materialNames, MeshRenderer meshRenderer) {
//			// NOTE 防止重复build
//			if (hasEngineMesh(meshId)) {
//				yield break;
//			}
			
			var positions = engineMesh.vertices;
			if (positions == null || positions.Length == 0) {
				yield break;
			}

			GameObject obj = meshRenderer.gameObject;
			string floatFormat = "0.###";
			float scaleThreshold = 99.0f; // NOTE 缩放大于一定程度，修改输出精度
			if (obj.transform.localScale.x > scaleThreshold || obj.transform.localScale.y > scaleThreshold || obj.transform.localScale.z > scaleThreshold) {
				floatFormat = "0.######";
			}

			// <geometry>
			var geometry = new COLLADA.Geometry();
			geometry.id = meshId;
			geometry.name = meshName;

			var mesh = new COLLADA.Mesh();
			COLLADA.Input input = null;
			int offset = 0;
			//没用过
			//int set = 0;
			var inputs = new List<COLLADA.Input>();

			// <source id="positions"> ---------------------------------------------------------------------------------------------------------
			var source = new COLLADA.Source();
			source.id = meshId + "-positions";
			// <float_array>
			var float_array = new COLLADA.FloatArray();
			float_array.id = source.id + "-array";
			float_array.count = positions.Length * 3;
			float_array.values = Vector3Utils.Join(positions, " ", floatFormat, (Vector3 origin) => {
				origin = Vector3Utils.ToRightHand(origin); //左手坐标系转右手坐标系
				if (mExtra.Obfuscated) {
					origin = Vector3Utils.Encode(origin);
				}
				return origin;
			});
			source.float_array = float_array;
			// </float_array>
			// <technique_common>
			var technique_common = new COLLADA.TechniqueCommon();
			// <accessor>
			var accessor = new COLLADA.Accessor();
			accessor.source = "#" + float_array.id;
			accessor.count = positions.Length;
			accessor.stride = 3;
			// <param>
			var param = new COLLADA.Param();
			param.name = "X";
			param.type = "float";
			accessor.AddParam(param);
			// </param>
			// <param>
			param = new COLLADA.Param();
			param.name = "Y";
			param.type = "float";
			accessor.AddParam(param);
			// </param>
			// <param>
			param = new COLLADA.Param();
			param.name = "Z";
			param.type = "float";
			accessor.AddParam(param);
			// </param>
			technique_common.accessor = accessor;
			// </accessor>
			source.technique_common = technique_common;
			// </technique_common>
			mesh.AddSource(source);
			// </source>

			// <vertices> ---------------------------------------------------------------------------------------------------------
			var vertices = new COLLADA.Vertices();
			vertices.id = meshId + "-vertices";
			// <input>
			var vertexInput = new COLLADA.Input();
			vertexInput.semantic = COLLADA.Input.Semantic.POSITION;
			vertexInput.source = "#" + source.id;
			// </input>
			vertices.AddInput(vertexInput);
			mesh.vertices = vertices;
			// </vertices>

			// <triangles><input> ---------------------------------------------------------------------------------------------------------
			input = new COLLADA.Input();
			input.semantic = COLLADA.Input.Semantic.VERTEX;
			input.source = "#" + vertices.id;
			input.offset = offset;
			//offset++;
			inputs.Add(input);
			// <triangles></input>

			// normals ---------------------------------------------------------------------------------------------------------
			var normals = engineMesh.normals;
			if (normals != null && normals.Length > 0) {
				// <source id="normals">
				source = new COLLADA.Source();
				source.id = meshId + "-normals";
				// <float_array>
				float_array = new COLLADA.FloatArray();
				float_array.id = source.id + "-array";
				float_array.count = normals.Length * 3;
				float_array.values = Vector3Utils.Join(normals, " ", floatFormat, (Vector3 origin) => {
					return Vector3Utils.ToRightHand(origin);
				});
				source.float_array = float_array;
				// </float_array>
				// <technique_common>
				technique_common = new COLLADA.TechniqueCommon();
				// <accessor>
				accessor = new COLLADA.Accessor();
				accessor.source = "#" + float_array.id;
				accessor.count = normals.Length;
				accessor.stride = 3;
				// <param>
				param = new COLLADA.Param();
				param.name = "X";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				// <param>
				param = new COLLADA.Param();
				param.name = "Y";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				// <param>
				param = new COLLADA.Param();
				param.name = "Z";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				technique_common.accessor = accessor;
				// </accessor>
				source.technique_common = technique_common;
				// </technique_common>
				mesh.AddSource(source);
				// </source>

				// <triangles><input>
				input = new COLLADA.Input();
				input.semantic = COLLADA.Input.Semantic.NORMAL;
				input.source = "#" + source.id;
				input.offset = offset;
				//offset++;
				inputs.Add(input);
				// <triangles></input>
			}

			var tangents = engineMesh.tangents;
			if (tangents != null && tangents.Length > 0) {
				const int numTangentComponents = 4;
				// <source id="tangents">
				source = new COLLADA.Source();
				source.id = meshId + "-tangents";
				// <float_array>
				float_array = new COLLADA.FloatArray();
				float_array.id = source.id + "-array";
				float_array.count = tangents.Length * numTangentComponents;
				if (mExtra.ExportBinormals) {
					float_array.values = Vector4Utils.Join3(tangents, " ", floatFormat, (Vector4 origin) => {
						return Vector4Utils.ToRightHand(origin);
					});
				} else {
					float_array.values = Vector4Utils.Join(tangents, " ", floatFormat, (Vector4 origin) => {
						return Vector4Utils.ToRightHand(origin);
					});
				}
				source.float_array = float_array;
				// </float_array>
				// <technique_common>
				technique_common = new COLLADA.TechniqueCommon();
				// <accessor>
				accessor = new COLLADA.Accessor();
				accessor.source = "#" + float_array.id;
				accessor.count = tangents.Length;
				accessor.stride = numTangentComponents;
				// <param>
				param = new COLLADA.Param();
				param.name = "X";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				// <param>
				param = new COLLADA.Param();
				param.name = "Y";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				// <param>
				param = new COLLADA.Param();
				param.name = "Z";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				if (numTangentComponents == 4) {
					// <param>
					param = new COLLADA.Param();
					param.name = "W";
					param.type = "float";
					accessor.AddParam(param);
					// </param>
				}
				technique_common.accessor = accessor;
				// </accessor>
				source.technique_common = technique_common;
				// </technique_common>
				mesh.AddSource(source);
				// </source>

				// <triangles><input>
				input = new COLLADA.Input();
				input.semantic = COLLADA.Input.Semantic.TANGENT;
				input.source = "#" + source.id;
				input.offset = offset;
				//offset++;
				inputs.Add(input);
				// <triangles></input>
			}

			if (mExtra.ExportBinormals) {
				if (normals != null && normals.Length > 0 && tangents != null && tangents.Length > 0) {
					Vector3[] binormals = new Vector3[normals.Length];
					for (int i = 0; i < binormals.Length; i++) {
						binormals[i] = Vector3.Cross(normals[i], tangents[i]) * tangents[i].w;
					}
					// <source id="binormals">
					source = new COLLADA.Source();
					source.id = meshId + "-binormals";
					// <float_array>
					float_array = new COLLADA.FloatArray();
					float_array.id = source.id + "-array";
					float_array.count = normals.Length * 3;
					float_array.values = Vector3Utils.Join(binormals, " ", floatFormat, (Vector3 origin) => {
						origin.x = -origin.x;
						return origin;
					});
					source.float_array = float_array;
					// </float_array>
					// <technique_common>
					technique_common = new COLLADA.TechniqueCommon();
					// <accessor>
					accessor = new COLLADA.Accessor();
					accessor.source = "#" + float_array.id;
					accessor.count = normals.Length;
					accessor.stride = 3;
					// <param>
					param = new COLLADA.Param();
					param.name = "X";
					param.type = "float";
					accessor.AddParam(param);
					// </param>
					// <param>
					param = new COLLADA.Param();
					param.name = "Y";
					param.type = "float";
					accessor.AddParam(param);
					// </param>
					// <param>
					param = new COLLADA.Param();
					param.name = "Z";
					param.type = "float";
					accessor.AddParam(param);
					// </param>
					technique_common.accessor = accessor;
					// </accessor>
					source.technique_common = technique_common;
					// </technique_common>
					mesh.AddSource(source);
					// </source>

					// <triangles><input>
					input = new COLLADA.Input();
					input.semantic = COLLADA.Input.Semantic.BINORMAL;
					input.source = "#" + source.id;
					input.offset = offset;
					//offset++;
					inputs.Add(input);
					// <triangles></input>
				}
			}

			// uvs ---------------------------------------------------------------------------------------------------------
#if UNITY_5
			int uvSets = 4;
#else
			int uvSets = 2;
#endif
			for (int i = 0; i < uvSets; i++) {
				Vector2[] uv = null;
				switch (i) {
					case 0:
						{
							uv = engineMesh.uv;
							break;
						}
					case 1:
						{
							uv = engineMesh.uv2;
							lightmapUVHandle(uv, meshRenderer); // lightmap for handle uv2
							break;
						}
#if UNITY_5
					case 2:
						{
							uv = engineMesh.uv3;
							break;
						}
					case 3:
						{
							uv = engineMesh.uv4;
							break;
						}
#endif
					default:
						break;
				}
				if (uv == null || uv.Length == 0) {
					break;
				}
				// <source id="map">
				source = new COLLADA.Source();
				source.id = meshId + "-map" + (i + 1);
				// <float_array>
				float_array = new COLLADA.FloatArray();
				float_array.id = source.id + "-array";
				float_array.count = uv.Length * 2;
				float_array.values = Vector2Utils.Join(uv, " ", (Vector2 origin) => {
					return origin;
				});
				source.float_array = float_array;
				// </float_array>
				// <technique_common>
				technique_common = new COLLADA.TechniqueCommon();
				// <accessor>
				accessor = new COLLADA.Accessor();
				accessor.source = "#" + float_array.id;
				accessor.count = uv.Length;
				accessor.stride = 2;
				// <param>
				param = new COLLADA.Param();
				param.name = "S";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				// <param>
				param = new COLLADA.Param();
				param.name = "T";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				technique_common.accessor = accessor;
				// </accessor>
				source.technique_common = technique_common;
				// </technique_common>
				mesh.AddSource(source);
				// </source>

				// <triangles><input>
				input = new COLLADA.Input();
				input.semantic = COLLADA.Input.Semantic.TEXCOORD;
				input.source = "#" + source.id;
				input.offset = offset;
				//offset++;
				input.set = i;
				inputs.Add(input);
				// <triangles></input>
			}

			var colors = engineMesh.colors;
			if (colors != null && colors.Length > 0) {
				// <source id="colors">
				source = new COLLADA.Source();
				source.id = meshId + "-colors";
				// <float_array>
				float_array = new COLLADA.FloatArray();
				float_array.id = source.id + "-array";
				float_array.count = colors.Length * 4;
				float_array.values = ColorUtils.Join(colors, " ");
				source.float_array = float_array;
				// </float_array>
				// <technique_common>
				technique_common = new COLLADA.TechniqueCommon();
				// <accessor>
				accessor = new COLLADA.Accessor();
				accessor.source = "#" + float_array.id;
				accessor.count = normals.Length;
				accessor.stride = 4;
				// <param>
				param = new COLLADA.Param();
				param.name = "R";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				// <param>
				param = new COLLADA.Param();
				param.name = "G";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				// <param>
				param = new COLLADA.Param();
				param.name = "B";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				// <param>
				param = new COLLADA.Param();
				param.name = "A";
				param.type = "float";
				accessor.AddParam(param);
				// </param>
				technique_common.accessor = accessor;
				// </accessor>
				source.technique_common = technique_common;
				// </technique_common>
				mesh.AddSource(source);
				// </source>

				// <triangles><input>
				input = new COLLADA.Input();
				input.semantic = COLLADA.Input.Semantic.COLOR;
				input.source = "#" + source.id;
				input.offset = offset;
				//offset++;
				inputs.Add(input);
				// <triangles></input>
			}

			for (int subMeshIndex = 0; subMeshIndex < engineMesh.subMeshCount; subMeshIndex++) {
				var engineTriangles = engineMesh.GetTriangles(subMeshIndex);
				// <triangles>
				var triangles = new COLLADA.Triangles();
				triangles.material = materialNames[subMeshIndex];
				triangles.count = engineTriangles.Length / 3;
				triangles.inputs = inputs;
				var values = new int[engineTriangles.Length];
				for (int i = 0; i < engineTriangles.Length; i += 3) {
					values[i] = engineTriangles[i + 1];
					values[i + 1] = engineTriangles[i];
					values[i + 2] = engineTriangles[i + 2];
					if (mExtra.Obfuscated) {
						values[i]++;
						values[i + 1]++;
						values[i + 2]++;
					}
				}
				//var values = engineTriangles;
				triangles.p = IntUtils.Join(values, " ", null);
				mesh.AddPrimitives(triangles);
				// </triangles>
			}

			geometry.mesh = mesh;
			mCOLLADA.AddGeometry(geometry);
			// </geometry>

			addEngineMesh(meshId, engineMesh);
		}

		public void BuildAnimations(COLLADA.VisualScene visualScene) {
			var nodes = visualScene.nodes;
			if (nodes == null) {
				return;
			}
			foreach (var node in nodes) {
				BuildAnimation(node.gameObject, node);
			}
		}

		public void BuildAnimation(GameObject obj, COLLADA.Node node) {
			#if UNITY_EDITOR
			Animation animation = obj.GetComponent<Animation>();
			if (animation != null && animation.enabled) {
				bool noAnimationClip = true;
				foreach (AnimationState animationState in animation) {
					noAnimationClip = false;
					AnimationClip animationClip = animationState.clip;
					if (animationClip == null) {
						continue;
					}

					bool hasAnimation = false;
					var animationMap = new Dictionary<string, AnimationPath>();

					//				var localPositionBindings = new EditorCurveBinding[3];
					//				var localPositionCurves = new AnimationCurve[3];
					//				int isLocalPositionAnimation = 0;
					//				var localRotationBindings = new EditorCurveBinding[4];
					//				var localRotationCurves = new AnimationCurve[4];
					//				int isLocalRotationAnimation = 0;

					var bindings = AnimationUtility.GetCurveBindings(animationClip);
					foreach (var binding in bindings) {
						AnimationCurve animationCurve = AnimationUtility.GetEditorCurve(animationClip, binding);
						AnimationPath animationPath = null;
						if (animationMap.ContainsKey(binding.path)) {
							animationPath = animationMap[binding.path];
						} else {
							animationPath = new AnimationPath();
							animationMap[binding.path] = animationPath;
						}
						if (binding.propertyName == "m_LocalPosition.x") {
							hasAnimation = true;
							animationPath.localPositionBindings[0] = binding;
							animationPath.localPositionCurves[0] = animationCurve;
							animationPath.isLocalPositionAnimation = Flags.Add(animationPath.isLocalPositionAnimation, 0x1);
						} else if (binding.propertyName == "m_LocalPosition.y") {
							hasAnimation = true;
							animationPath.localPositionBindings[1] = binding;
							animationPath.localPositionCurves[1] = animationCurve;
							animationPath.isLocalPositionAnimation = Flags.Add(animationPath.isLocalPositionAnimation, 0x1 << 1);
						} else if (binding.propertyName == "m_LocalPosition.z") {
							hasAnimation = true;
							animationPath.localPositionBindings[2] = binding;
							animationPath.localPositionCurves[2] = animationCurve;
							animationPath.isLocalPositionAnimation = Flags.Add(animationPath.isLocalPositionAnimation, 0x1 << 2);
						} else if (binding.propertyName == "m_LocalRotation.x") {
							hasAnimation = true;
							animationPath.localRotationBindings[0] = binding;
							animationPath.localRotationCurves[0] = animationCurve;
							animationPath.isLocalRotationAnimation = Flags.Add(animationPath.isLocalRotationAnimation, 0x1);
						} else if (binding.propertyName == "m_LocalRotation.y") {
							hasAnimation = true;
							animationPath.localRotationBindings[1] = binding;
							animationPath.localRotationCurves[1] = animationCurve;
							animationPath.isLocalRotationAnimation = Flags.Add(animationPath.isLocalRotationAnimation, 0x1 << 1);
						} else if (binding.propertyName == "m_LocalRotation.z") {
							hasAnimation = true;
							animationPath.localRotationBindings[2] = binding;
							animationPath.localRotationCurves[2] = animationCurve;
							animationPath.isLocalRotationAnimation = Flags.Add(animationPath.isLocalRotationAnimation, 0x1 << 2);
						} else if (binding.propertyName == "m_LocalRotation.w") {
							hasAnimation = true;
							animationPath.localRotationBindings[3] = binding;
							animationPath.localRotationCurves[3] = animationCurve;
							animationPath.isLocalRotationAnimation = Flags.Add(animationPath.isLocalRotationAnimation, 0x1 << 3);
						}
					}

					if (hasAnimation) {
						COLLADA.Animation colladaAnimation = new COLLADA.Animation();
						//var animationId = animationClip.name.Replace(' ', '_'); // NOTE 空格替换为下划线
						var animationId = animationClip.name;
						colladaAnimation.id = animationId;

						foreach (KeyValuePair<string, AnimationPath> kv in animationMap) {
							//foreach (AnimationPath animationPath in animationMap.Values) {
							string samplerIdMiddlePart = StringUtils.IsNullOrBlank(kv.Key) ? "-sampler" : "-sampler-" + kv.Key;
							AnimationPath animationPath = kv.Value;
							var localPositionBindings = animationPath.localPositionBindings;
							var localPositionCurves = animationPath.localPositionCurves;
							int isLocalPositionAnimation = animationPath.isLocalPositionAnimation;
							var localRotationBindings = animationPath.localRotationBindings;
							var localRotationCurves = animationPath.localRotationCurves;
							int isLocalRotationAnimation = animationPath.isLocalRotationAnimation;

							// translate animation
							if (Flags.Test(isLocalPositionAnimation, 0x7)) {

								var localPositionXFrames = localPositionCurves[0].keys;
								var localPositionYFrames = localPositionCurves[1].keys;
								var localPositionZFrames = localPositionCurves[2].keys;

								var colladaSampler = new COLLADA.AnimationSampler();
								colladaSampler.id = colladaAnimation.id + samplerIdMiddlePart + "-translate";

								// anim input
								// <source>
								var colladaSource = new COLLADA.Source();
								colladaSource.id = colladaSampler.id + "-input";
								var float_array = new COLLADA.FloatArray();
								float_array.count = localPositionXFrames.Length;
								int i = 0;
								foreach (var keyframe in localPositionXFrames) {
									//float time = (keyframe.time * animationClip.frameRate) / 1000.0f;
									//float time = keyframe.time * animationClip.frameRate;
									float time = keyframe.time;
									if (i == 0) {
										float_array.values = time.ToString("0.###");
									} else {
										float_array.values += " " + time.ToString("0.###");
									}
									i++;
								}
								float_array.id = colladaSource.id + "-array";
								colladaSource.float_array = float_array;
								var colladaTechniqueCommon = new COLLADA.TechniqueCommon();
								colladaTechniqueCommon.accessor = new COLLADA.Accessor();
								colladaTechniqueCommon.accessor.source = "#" + float_array.id;
								colladaTechniqueCommon.accessor.count = float_array.count;
								colladaTechniqueCommon.accessor.stride = 1;
								var colladaParam = new COLLADA.Param();
								colladaParam.name = "TIME";
								colladaParam.type = "float";
								colladaTechniqueCommon.accessor.AddParam(colladaParam);
								colladaSource.technique_common = colladaTechniqueCommon;
								colladaAnimation.AddSource(colladaSource);
								// </source>
								// <input>
								var colladaInput = new COLLADA.Input();
								colladaInput.semantic = COLLADA.Input.Semantic.INPUT;
								colladaInput.source = "#" + colladaSource.id;
								colladaSampler.AddInput(colladaInput);
								// </input>

								// anim output
								// <source>
								colladaSource = new COLLADA.Source();
								colladaSource.id = colladaSampler.id + "-output";
								float_array = new COLLADA.FloatArray();
								float_array.count = localPositionXFrames.Length * 3;
								for (i = 0; i < localPositionXFrames.Length; i++) {
                                    // NOTE 转换为右手坐标系
                                    var position = new Vector3 (localPositionXFrames[i].value, localPositionYFrames[i].value, localPositionZFrames[i].value);
                                    position = Vector3Utils.ToRightHand (position);
									if (i == 0) {
										float_array.values = Vector3Utils.Format(position.x, position.y, position.z);
									} else {
										float_array.values += " " + Vector3Utils.Format(position.x, position.y, position.z);
									}
								}
								float_array.id = colladaSource.id + "-array";
								colladaSource.float_array = float_array;
								colladaTechniqueCommon = new COLLADA.TechniqueCommon();
								colladaTechniqueCommon.accessor = new COLLADA.Accessor();
								colladaTechniqueCommon.accessor.source = "#" + float_array.id;
								colladaTechniqueCommon.accessor.count = float_array.count / 3;
								colladaTechniqueCommon.accessor.stride = 3;
								colladaParam = new COLLADA.Param();
								colladaParam.name = "X";
								colladaParam.type = "float";
								colladaTechniqueCommon.accessor.AddParam(colladaParam);
								colladaParam = new COLLADA.Param();
								colladaParam.name = "Y";
								colladaParam.type = "float";
								colladaTechniqueCommon.accessor.AddParam(colladaParam);
								colladaParam = new COLLADA.Param();
								colladaParam.name = "Z";
								colladaParam.type = "float";
								colladaTechniqueCommon.accessor.AddParam(colladaParam);
								colladaSource.technique_common = colladaTechniqueCommon;
								colladaAnimation.AddSource(colladaSource);
								// </source>
								// <input>
								colladaInput = new COLLADA.Input();
								colladaInput.semantic = COLLADA.Input.Semantic.OUTPUT;
								colladaInput.source = "#" + colladaSource.id;
								colladaSampler.AddInput(colladaInput);
								// </input>

								// anim interpolation
								// <source>
								colladaSource = new COLLADA.Source();
								colladaSource.id = colladaSampler.id + "-interpolation";
								var Name_array = new COLLADA.NameArray();
								Name_array.count = localPositionXFrames.Length;
								i = 0;
								foreach (var keyframe in localPositionXFrames) {
									if (i == 0) {
										Name_array.values = "LINEAR";
									} else {
										Name_array.values += " LINEAR";
									}
									i++;
								}
								Name_array.id = colladaSource.id + "-array";
								colladaSource.Name_array = Name_array;
								colladaTechniqueCommon = new COLLADA.TechniqueCommon();
								colladaTechniqueCommon.accessor = new COLLADA.Accessor();
								colladaTechniqueCommon.accessor.source = "#" + float_array.id;
								colladaTechniqueCommon.accessor.count = Name_array.count;
								colladaTechniqueCommon.accessor.stride = 1;
								colladaParam = new COLLADA.Param();
								colladaParam.name = "INTERPOLATION";
								colladaParam.type = "name";
								colladaTechniqueCommon.accessor.AddParam(colladaParam);
								colladaSource.technique_common = colladaTechniqueCommon;
								colladaAnimation.AddSource(colladaSource);
								// </source>
								// <input>
								colladaInput = new COLLADA.Input();
								colladaInput.semantic = COLLADA.Input.Semantic.INTERPOLATION;
								colladaInput.source = "#" + colladaSource.id;
								colladaSampler.AddInput(colladaInput);
								// </input>

								var colladaChannel = new COLLADA.Channel();
								colladaChannel.source = "#" + colladaSampler.id;
								var path = localPositionBindings[0].path;
								var colladaNode = node;
								if (StringUtils.IsNullOrBlank(path)) {
									path = mRoot.name;
								} else {
									path = mRoot.name + "/" + path;
									colladaNode = mCOLLADA.FindNodeByPath(path);
								}
								var colladaTranslate = new COLLADA.Translate();
								colladaTranslate.sid = "translateXYZ";
								colladaTranslate.value = Vector3Utils.Format(Vector3.zero);
								colladaNode.AddTranslate(colladaTranslate);
								colladaChannel.target = path + "/translateXYZ";

								colladaAnimation.AddSampler(colladaSampler);
								colladaAnimation.AddChannel(colladaChannel);
							}

							// rotate animation
							if (Flags.Test(isLocalRotationAnimation, 0xF)) {

								var localRotationXFrames = localRotationCurves[0].keys;
								var localRotationYFrames = localRotationCurves[1].keys;
								var localRotationZFrames = localRotationCurves[2].keys;
								var localRotationWFrames = localRotationCurves[3].keys;

								var colladaSamplers = new COLLADA.AnimationSampler[] {
									new COLLADA.AnimationSampler(),
									new COLLADA.AnimationSampler(),
									new COLLADA.AnimationSampler(),
								};
								colladaSamplers[0].id = colladaAnimation.id + samplerIdMiddlePart + "-rotateX";
								colladaSamplers[1].id = colladaAnimation.id + samplerIdMiddlePart + "-rotateY";
								colladaSamplers[2].id = colladaAnimation.id + samplerIdMiddlePart + "-rotateZ";
								var colladaOutputSources = new COLLADA.Source[]{ null, null, null };

								for (int axisIndex = 0; axisIndex < 3; axisIndex++) {
									// anim input
									// <source>
									var colladaSampler = colladaSamplers[axisIndex];
									var colladaSource = new COLLADA.Source();
									colladaSource.id = colladaSampler.id + "-input";
									var float_array = new COLLADA.FloatArray();
									float_array.count = localRotationXFrames.Length;
									int i = 0;
									foreach (var keyframe in localRotationXFrames) {
										//float time = (keyframe.time * animationClip.frameRate) / 1000.0f;
										//float time = keyframe.time * animationClip.frameRate;
										float time = keyframe.time;
										if (i == 0) {
											float_array.values = time.ToString("0.###");
										} else {
											float_array.values += " " + time.ToString("0.###");
										}
										i++;
									}
									float_array.id = colladaSource.id + "-array";
									colladaSource.float_array = float_array;
									var colladaTechniqueCommon = new COLLADA.TechniqueCommon();
									colladaTechniqueCommon.accessor = new COLLADA.Accessor();
									colladaTechniqueCommon.accessor.source = "#" + float_array.id;
									colladaTechniqueCommon.accessor.count = float_array.count;
									colladaTechniqueCommon.accessor.stride = 1;
									var colladaParam = new COLLADA.Param();
									colladaParam.name = "TIME";
									colladaParam.type = "float";
									colladaTechniqueCommon.accessor.AddParam(colladaParam);
									colladaSource.technique_common = colladaTechniqueCommon;
									colladaAnimation.AddSource(colladaSource);
									// </source>
									// <input>
									var colladaInput = new COLLADA.Input();
									colladaInput.semantic = COLLADA.Input.Semantic.INPUT;
									colladaInput.source = "#" + colladaSource.id;
									colladaSampler.AddInput(colladaInput);
									// </input>

									// anim output
									// <source>
									colladaSource = new COLLADA.Source();
									colladaOutputSources[axisIndex] = colladaSource;
									colladaSource.id = colladaSampler.id + "-output";
									float_array = new COLLADA.FloatArray();
									float_array.count = localRotationXFrames.Length;
									float_array.id = colladaSource.id + "-array";
									colladaSource.float_array = float_array;

									colladaTechniqueCommon = new COLLADA.TechniqueCommon();
									colladaTechniqueCommon.accessor = new COLLADA.Accessor();
									colladaTechniqueCommon.accessor.source = "#" + float_array.id;
									colladaTechniqueCommon.accessor.count = float_array.count;
									colladaTechniqueCommon.accessor.stride = 1;
									colladaParam = new COLLADA.Param();
									colladaParam.name = "ANGLE";
									colladaParam.type = "float";
									colladaTechniqueCommon.accessor.AddParam(colladaParam);
									colladaSource.technique_common = colladaTechniqueCommon;
									colladaAnimation.AddSource(colladaSource);
									// </source>
									// <input>
									colladaInput = new COLLADA.Input();
									colladaInput.semantic = COLLADA.Input.Semantic.OUTPUT;
									colladaInput.source = "#" + colladaSource.id;
									colladaSampler.AddInput(colladaInput);
									// </input>

									var colladaChannel = new COLLADA.Channel();
									colladaChannel.source = "#" + colladaSampler.id;
									var path = localRotationBindings[0].path;
									var colladaNode = node;
									if (StringUtils.IsNullOrBlank(path)) {
										path = mRoot.name;
									} else {
										path = mRoot.name + "/" + path;
										colladaNode = mCOLLADA.FindNodeByPath(path);
									}
									var colladaRotate = new COLLADA.Rotate();
									switch (axisIndex) {
										case 0:
											{
												colladaRotate.sid = "rotateX";
												colladaRotate.value = Vector4Utils.Format(1.0f, 0.0f, 0.0f, 0.0f);
												colladaChannel.target = path + "/rotateX.ANGLE";
												break;
											}
										case 1:
											{
												colladaRotate.sid = "rotateY";
												colladaRotate.value = Vector4Utils.Format(0.0f, 1.0f, 0.0f, 0.0f);
												colladaChannel.target = path + "/rotateY.ANGLE";
												break;
											}
										case 2:
											{
												colladaRotate.sid = "rotateZ";
												colladaRotate.value = Vector4Utils.Format(0.0f, 0.0f, 1.0f, 0.0f);
												colladaChannel.target = path + "/rotateZ.ANGLE";
												break;
											}
									}
									colladaNode.AddRotate(colladaRotate);
									//colladaChannel.target = path + "/rotate." + axisIndex;

									colladaAnimation.AddSampler(colladaSampler);
									colladaAnimation.AddChannel(colladaChannel);
								}

								for (int i = 0; i < localRotationXFrames.Length; i++) {
                                    //Quaternion q = new Quaternion(localRotationXFrames[i].value, localRotationYFrames[i].value, localRotationZFrames[i].value, localRotationWFrames[i].value);
                                    //var eulerAngles = q.eulerAngles;
                                    //// NOTE 转换为右手坐标系
                                    //eulerAngles.y = -eulerAngles.y;
                                    var q = QuaternionUtils.ToRightHand (new Quaternion (localRotationXFrames[i].value, localRotationYFrames[i].value, localRotationZFrames[i].value, localRotationWFrames[i].value));
                                    var eulerAngles = q.eulerAngles;
                                    for (int axisIndex = 0; axisIndex < 3; axisIndex++) {
										var colladaSource = colladaOutputSources[axisIndex];
										var float_array = colladaSource.float_array;
										if (i == 0) {
											float_array.values = eulerAngles[axisIndex].ToString("0.###");
										} else {
											float_array.values += " " + eulerAngles[axisIndex].ToString("0.###");
										}
									}
								}

								//						// anim interpolation
								//						// <source>
								//						colladaSource = new COLLADA.Source();
								//						colladaSource.id = colladaSampler.id + "-interpolation";
								//						var Name_array = new COLLADA.NameArray();
								//						Name_array.count = localPositionXFrames.Length;
								//						i = 0;
								//						foreach (var keyframe in localPositionXFrames) {
								//							if (i == 0) {
								//								Name_array.values = "LINEAR";
								//							} else {
								//								Name_array.values += " LINEAR";
								//							}
								//							i++;
								//						}
								//						Name_array.id = colladaSource.id + "-array";
								//						colladaSource.Name_array = Name_array;
								//						colladaTechniqueCommon = new COLLADA.TechniqueCommon();
								//						colladaTechniqueCommon.accessor = new COLLADA.Accessor();
								//						colladaTechniqueCommon.accessor.source = "#" + float_array.id;
								//						colladaTechniqueCommon.accessor.count = Name_array.count;
								//						colladaTechniqueCommon.accessor.stride = 1;
								//						colladaParam = new COLLADA.Param();
								//						colladaParam.name = "INTERPOLATION";
								//						colladaParam.type = "name";
								//						colladaTechniqueCommon.accessor.AddParam(colladaParam);
								//						colladaSource.technique_common = colladaTechniqueCommon;
								//						colladaAnimation.AddSource(colladaSource);
								//						// </source>
								//						// <input>
								//						colladaInput = new COLLADA.Input();
								//						colladaInput.semantic = COLLADA.Input.Semantic.INTERPOLATION;
								//						colladaInput.source = "#" + colladaSource.id;
								//						colladaSampler.AddInput(colladaInput);
								//						// </input>
							}
						}

						if (mCOLLADA.library_animations == null) {
							mCOLLADA.library_animations = new List<COLLADA.Animation>();
						}
						mCOLLADA.library_animations.Add(colladaAnimation);
					}
				}
				if (noAnimationClip) {
					Debug.LogError("No animation clips found in" + obj.name + ", maybe the animation is not legacy.");
				}
			}

			var children = node.chlidren;
			if (children != null) {
				foreach (var child in children) {
					BuildAnimation(child.gameObject, child);
				}
			}
			#endif
		}

		private Texture2D getLightmapFromMaterialOrRenderer(Material engineMaterial, MeshRenderer meshRenderer) {
			if (engineMaterial.HasProperty(Shaders.Textures.Lightmap)) {
				var texture = engineMaterial.GetTexture(Shaders.Textures.Lightmap);
				if (texture != null) {
					if (texture is Texture2D) {
						var texture2D = texture as Texture2D;
						return texture2D;
					}
				}
			}
			return getLightmapFromRenderer(meshRenderer);
		}

		private Texture2D getLightmapFromRenderer(MeshRenderer meshRenderer) {
			if (meshRenderer.lightmapIndex < 0) {
				return null;
			}
			var lightmapIndex = meshRenderer.lightmapIndex;
			var lightmap = LightmapSettings.lightmaps[lightmapIndex];
			if (lightmap != null) {
				var texture2D = lightmap.lightmapFar;
				if (texture2D != null) {
					return texture2D;
				}
				texture2D = lightmap.lightmapNear;
				if (texture2D != null) {
					return texture2D;
				}
			}
			return null;
		}

		private void lightmapUVHandle(Vector2[] uv, Renderer renderer) {
			if (uv == null || uv.Length == 0) {
				return;
			}
#if UNITY_5
			Vector4 tilingOffset = renderer.lightmapScaleOffset;
#else
			Vector4 tilingOffset = renderer.lightmapTilingOffset;
#endif
			for (int i = 0; i < uv.Length; i++) {
				uv[i].x = (uv[i].x * tilingOffset.x) + tilingOffset.z;
				uv[i].y = (uv[i].y * tilingOffset.y) + tilingOffset.w;
			}
		}

		private bool hasEngineMaterial(string materialId) {
			if (mMaterials == null || !mMaterials.ContainsKey(materialId)) {
				return false;
			}
			return true;
		}

		private void addEngineMaterial(string materialId, UnityEngine.Material material) {
			if (material == null) {
				return;
			}
			if (mMaterials == null) {
				mMaterials = new Dictionary<string, UnityEngine.Material>();
			}
			mMaterials[materialId] = material;
		}

		private bool hasEngineTexture(string textureId) {
			if (mTextures == null || !mTextures.ContainsKey(textureId)) {
				return false;
			}
			return true;
		}

		private void addEngineTexture(string textureId, UnityEngine.Texture texture) {
			if (texture == null) {
				return;
			}
			if (mTextures == null) {
				mTextures = new Dictionary<string, UnityEngine.Texture>();
			}
			mTextures[textureId] = texture;
		}

		private UnityEngine.Mesh getEngineMesh(string meshId) {
			if (mMeshes == null || !mMeshes.ContainsKey(meshId)) {
				return null;
			}
			return mMeshes[meshId];
		}

		private void addEngineMesh(string meshId, UnityEngine.Mesh mesh) {
			if (mesh == null) {
				return;
			}
			if (mMeshes == null) {
				mMeshes = new Dictionary<string, UnityEngine.Mesh>();
			}
			mMeshes[meshId] = mesh;
		}

		private COLLADA.Newparam getSampler2D(string samplerSid) {
			if (mSampler2Ds == null || !mSampler2Ds.ContainsKey(samplerSid)) {
				return null;
			}
			return mSampler2Ds[samplerSid];
		}

		private void addSampler2D(string samplerSid, COLLADA.Newparam sampler) {
			if (sampler == null) {
				return;
			}
			if (mSampler2Ds == null) {
				mSampler2Ds = new Dictionary<string, COLLADA.Newparam>();
			}
			mSampler2Ds[samplerSid] = sampler;
		}
	}

#if UNITY_EDITOR
    public sealed class AnimationPath {
		public string path;
		public EditorCurveBinding[] localPositionBindings = new EditorCurveBinding[3];
		public AnimationCurve[] localPositionCurves = new AnimationCurve[3];
		public int isLocalPositionAnimation = 0;
		public EditorCurveBinding[] localRotationBindings = new EditorCurveBinding[4];
		public AnimationCurve[] localRotationCurves = new AnimationCurve[4];
		public int isLocalRotationAnimation = 0;
	}
#endif
}
