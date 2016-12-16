/**
 * @file COLLADAForLoad.cs
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

namespace uapp {

	public class COLLADAForLoad {

		private COLLADA mCOLLADA;
		private GameObject mObject;
		private uapp.IFile mFile;
		private OnSceneLoadListener mListener;
		private AsyncTask<GameObject> mTask;
		private DaeExtra mExtra;

		private Dictionary<string, UnityEngine.Material> mMaterials;
		//private Dictionary<string, UnityEngine.Mesh> mMeshes;

		private int mTotalNodeCount;
		private int mCurrentNodeCount;

		public GameObject Object {
			get {
				return mObject;
			}
		}

		public COLLADAForLoad(COLLADA collada, uapp.IFile file, OnSceneLoadListener listener, AsyncTask<GameObject> task) {
			mCOLLADA = collada;
			mFile = file;
			mListener = listener;
			mTask = task;
			if (file.Extra is DaeExtra) {
				mExtra = file.Extra as DaeExtra;
			} else {
				mExtra = new DaeExtra();
				mExtra.Obfuscated = false;
			}
			mCOLLADA.Obfuscated = mExtra.Obfuscated;
		}

		public IEnumerator Build(GameObject parent) {
			var scene = mCOLLADA.scene;
			if (scene == null || scene.instance_visual_scene == null) {
				mTask.Return(null);
				yield break;
			}

//			mObject = new GameObject();
//			mObject.name = System.IO.Path.GetFileNameWithoutExtension(mFile.Path);
//			if (parent != null) {
//				mObject.transform.parent = parent.transform;
//			}

			// axis
			//			if (StringUtils.Equals(asset.up_axis, "X_UP")) {
			//				//mObject.transform.up = Vector3.right;
			//				mObject.transform.Rotate(0.0f, 0.0f, 90.0f);
			//			} else if (StringUtils.Equals(asset.up_axis, "Y_UP")) {
			//				//mObject.transform.up = Vector3.up;
			//			} else if (StringUtils.Equals(asset.up_axis, "Z_UP")) {
			//				//mObject.transform.up = Vector3.forward;
			//				mObject.transform.Rotate(-90.0f, 0.0f, 0.0f);
			//			}

			// XXX 这个可能不是缩放,但这样处理效果是对的
//			var asset = mCOLLADA.asset;
//			mObject.transform.localScale = new Vector3(asset.unit.meter, asset.unit.meter, asset.unit.meter);

			// BuildScene(scene.instance_visual_scene, mObject);
			IEnumerator e = BuildScene(scene.instance_visual_scene, parent);
			while (e.MoveNext()) {
				yield return e.Current;
			}
		}

		private void setupRoot(GameObject root) {
			var asset = mCOLLADA.asset;
			root.transform.localScale = new Vector3(asset.unit.meter, asset.unit.meter, asset.unit.meter);
		}

		public IEnumerator BuildScene(COLLADA.InstanceVisualScene instanceVisualScene, GameObject parent) {
			var library_visual_scenes = mCOLLADA.library_visual_scenes;
			if (library_visual_scenes == null || library_visual_scenes.Count == 0) {
				mTask.Return(null);
				yield break;
			}
			if (instanceVisualScene.url == null) {
				mTask.Return(null);
				yield break;
			}
			var visualScene = mCOLLADA.getVisualScene(instanceVisualScene.url.Substring(1));
			if (visualScene == null) {
				mTask.Return(null);
				yield break;
			}
			var nodes = visualScene.nodes;
			if (nodes == null || nodes.Count == 0) {
				mTask.Return(null);
				yield break;
			}

			if (nodes.Count > 1) {
				mObject = new GameObject();
				mObject.name = System.IO.Path.GetFileNameWithoutExtension(mFile.Path);
				ObjectUtils.SetParent(mObject, parent);
				setupRoot(mObject);
				parent = mObject;
			}

			mTotalNodeCount = 0;
			mCurrentNodeCount = 0;
			foreach (var node in nodes) {
				mTotalNodeCount += getNodeCount(node);
			}
			foreach (var node in nodes) {
				//BuildNode(node, parent);
				IEnumerator e = BuildNode(node, parent);
				while (e.MoveNext()) {
					yield return e.Current;
				}
			}
		}

		private int getNodeCount(COLLADA.Node node) {
			if (node == null) {
				return 0;
			}
			if (node.chlidren == null) {
				return 1;
			}
			int nodeCount = 1 + node.chlidren.Count;
			foreach (var child in node.chlidren) {
				nodeCount += getNodeCount(child);
			}
			return nodeCount;
		}

		public IEnumerator BuildNode(COLLADA.Node node, GameObject parent) {
			if (node == null) {
				yield break;
			}
			GameObject nodeObject = new GameObject();
			if (mObject == null) {
				mObject = nodeObject;
				setupRoot(mObject);
			}
			if (node.name == null) {
				nodeObject.name = "unnamed_node";
			} else {
				nodeObject.name = node.name;
			}
			if (parent != null) {
				nodeObject.transform.parent = parent.transform;
			}
			if (node.matrix == null) {
				Vector3 t = Vector3.zero;// Vector3Utils.Deformat(node.translate);
				Vector4 r = Vector4.zero;// Vector4Utils.Deformat(node.rotate);
				Vector3 s = Vector3Utils.Deformat(node.scale, Vector3.one);
				nodeObject.transform.localPosition = t;
				nodeObject.transform.Rotate(new Vector3(r.x, r.y, r.z), r.w, Space.Self);
				nodeObject.transform.localScale = s;
			} else {
				Matrix4x4 m = Matrix4Utils.Deformat(node.matrix);
				nodeObject.transform.localPosition = Matrix4Utils.GetPosition(m);
				nodeObject.transform.localRotation = Matrix4Utils.GetRotation(m);
				nodeObject.transform.localScale = Matrix4Utils.GetScale(m);
			}
			var asset = mCOLLADA.asset;
			if (node.type == null || StringUtils.Equals(node.type, "NODE")) { // 场景节点处理
				// 标识up axis
				// NOTE 这样处理效果正确，但是不知道是否存在问题
				if (StringUtils.Equals(asset.up_axis, COLLADA.Asset.UpAxis.X_UP)) {
					nodeObject.transform.up = Vector3.up;
				} else if (StringUtils.Equals(asset.up_axis, COLLADA.Asset.UpAxis.Y_UP)) {
					//nodeObject.transform.up = Vector3.up;
				} else if (StringUtils.Equals(asset.up_axis, COLLADA.Asset.UpAxis.Z_UP)) {
					nodeObject.transform.up = Vector3.up;
				}
				// 处理geometry
				foreach (var instanceGeometry in node.instance_geometries) {
					//BuildGeometry(nodeObject, instanceGeometry);
					IEnumerator e = BuildGeometry(nodeObject, instanceGeometry);
					while (e.MoveNext()) {
						yield return e.Current;
					}
				}
				// 处理controller
				//				for (CCVColladaInstanceController* colladaInstanceController in colladaNode.instanceControllers) {
				//					[self buildController:nodeObject controller:colladaInstanceController];
				//				}

				// 处理子节点
				var children = node.chlidren;
				foreach (var childNode in children) {
					//BuildNode(childNode, nodeObject);
					IEnumerator e = BuildNode(childNode, nodeObject);
					while (e.MoveNext()) {
						yield return e.Current;
					}
				}
				// TODO
			}
			//return nodeObject;
			if (mListener != null) {
				mListener.OnSceneLoadObject(nodeObject);
				mCurrentNodeCount++;
				float progress = (float)mCurrentNodeCount / (float)mTotalNodeCount;
				mListener.OnSceneLoadingProgress(progress);
			}
		}

		public IEnumerator BuildGeometry(GameObject nodeObject, COLLADA.InstanceGeometry instanceGeometry) {
			var bindMaterial = instanceGeometry.bind_material;
			BuildBindMaterial(bindMaterial);

			string instanceGeometryUrl = instanceGeometry.url;
			if (instanceGeometryUrl == null) {
				yield break;
			}

			string meshName = instanceGeometryUrl.Substring(1);
			//BuildMesh(meshName, bindMaterial, nodeObject);
			IEnumerator e = BuildMesh(meshName, bindMaterial, nodeObject);
			while (e.MoveNext()) {
				yield return e.Current;
			}
		}

		public IEnumerator BuildMesh(string meshName, COLLADA.BindMaterial bindMaterial, GameObject obj) {
			//UnityEngine.Mesh mesh = null;
			var geometry = mCOLLADA.getGeometry(meshName);
			if (geometry == null) {
				yield break;
			}
			var mesh = geometry.mesh;
			if (mesh == null) {
				yield break;
			}
			UnityEngine.Mesh engineMesh = new UnityEngine.Mesh();
			engineMesh.name = geometry.id;
			IEnumerator e = mesh.Build(mCOLLADA);
			while (e.MoveNext()) {
				yield return e.Current;
			}
			if (mesh.buildMesh != null) {
				UnityEngine.Material[] materials = null;
				var buildMesh = mesh.buildMesh;
				engineMesh.subMeshCount = buildMesh.subMeshCount;
				engineMesh.vertices = mesh.buildMesh.positions.ToArray();
				MeshRenderer meshRenderer = obj.AddComponent<MeshRenderer>();
				if (buildMesh.uvs[0] != null) {
					engineMesh.uv = buildMesh.uvs[0].ToArray();
				}
				if (buildMesh.uvs[1] != null) {
					engineMesh.uv2 = buildMesh.uvs[1].ToArray();
				}
#if UNITY_5
				if (buildMesh.uvs[2] != null) {
					engineMesh.uv3 = buildMesh.uvs[2].ToArray();
				}
				if (buildMesh.uvs[3] != null) {
					engineMesh.uv4 = buildMesh.uvs[3].ToArray();
				}
#endif
				if (buildMesh.subMeshCount == 1) {
					var primitives = mesh.primitiveList[0];
					if (primitives is COLLADA.Triangles) {
						var triangles = primitives as COLLADA.Triangles;
						engineMesh.triangles = buildMesh.indices[0].ToArray();
						//						MaterialInfo materialInfo = bindMaterial.getMaterialInfo(triangles);
						//						if (materialInfo.name == null) {
						//							materialInfo.name = triangles.material;
						//						}
						//						UnityEngine.Material engineMaterial = getEngineMaterial(materialInfo.name);
						string materialName = bindMaterial.getMaterialName(0);
						if (materialName == null) {
							materialName = triangles.material;
						}
						UnityEngine.Material engineMaterial = getEngineMaterial(materialName);
						meshRenderer.material = engineMaterial;
					}
				} else if (buildMesh.subMeshCount > 1) {
					for (int i = 0; i < buildMesh.subMeshCount; i++) {
						var primitives = mesh.primitiveList[i];
						if (primitives is COLLADA.Triangles) {
							var triangles = primitives as COLLADA.Triangles;
							engineMesh.SetTriangles(buildMesh.indices[i].ToArray(), i);
							//							MaterialInfo materialInfo = bindMaterial.getMaterialInfo(triangles);
							//							if (materialInfo.name == null) {
							//								materialInfo.name = triangles.material;
							//							}
							//						UnityEngine.Material engineMaterial = getEngineMaterial(materialInfo.name);
							string materialName = bindMaterial.getMaterialName(i);
							if (materialName == null) {
								materialName = triangles.material;
							}
							UnityEngine.Material engineMaterial = getEngineMaterial(materialName);
							if (materials == null) {
								materials = new UnityEngine.Material[buildMesh.subMeshCount];
							}
							materials[i] = engineMaterial;
						}
					}
					meshRenderer.materials = materials;
				}
				MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
				meshFilter.mesh = engineMesh;
			}
			//return true;
			yield return null;
		}

		public bool BuildBindMaterial(COLLADA.BindMaterial bindMaterial) {
			if (bindMaterial == null) {
				return false;
			}
			var techniqueCommon = bindMaterial.technique_common;
			if (techniqueCommon == null) {
				return false;
			}
			var instance_materials = techniqueCommon.instance_materials;
			if (instance_materials == null || instance_materials.Count == 0) {
				return false;
			}
			foreach (var instanceMaterial in instance_materials) {
				string symbol = instanceMaterial.symbol;
				string target = instanceMaterial.target;
				if (symbol != null && target != null) {
					string materialName = target.Substring(1);
					buildMaterial(materialName, true);
				}
			}
			return true;
		}

		private bool buildMaterial(string materialName, bool reuseMaterial) {
			if (materialName == null) {
				return false;
			}
			if (reuseMaterial) {
				UnityEngine.Material engineMaterial = getEngineMaterial(materialName);
				if (engineMaterial != null) {
					return true;
				}
			}
			var material = mCOLLADA.getMaterial(materialName);
			if (material == null) {
				return false;
			}
			var instanceEffect = material.instance_effect;
			if (instanceEffect == null) {
				return false;
			}
			var instanceEffectUrl = instanceEffect.url;
			return buildEffect(instanceEffectUrl, materialName);
		}

		private bool buildEffect(string instanceEffectUrl, string materialName) {
            //没有用到
            //UnityEngine.Material engineMaterial = null;  
			if (instanceEffectUrl == null) {
				return false;
			}
			var effect = mCOLLADA.getEffect(instanceEffectUrl.Substring(1));
			if (effect == null) {
				return false;
			}

			// TODO more profile
			var profileCOMMON = effect.profile_COMMON;
			if (profileCOMMON == null) {
				return false;
			}
			var technique = profileCOMMON.technique;
			if (technique != null) {
				//				Phong phong = technique.type;
				//				if (phong != null) {
				//					return buildPhong(materialName, profileCOMMON, phong);
				//				}
				var techniqueType = technique.type;
				buildTechniqueType(materialName, profileCOMMON, techniqueType);
			}
			// TODO extra
			return false;
		}

		private bool buildTechniqueType(string materialName, COLLADA.ProfileCOMMON profileCOMMON, COLLADA.TechniqueType techniqueType) {
			if (techniqueType == null) {
				return false;
			}

			var shaderProperties = new Shaders.Properties();

			// ambient as _LightMap
			var commonColorOrTexture = techniqueType.ambient;
			if (commonColorOrTexture != null) {
				var texture = commonColorOrTexture.texture;
				if (texture != null) {
					shaderProperties.LightmapTexture = buildTexture(profileCOMMON, texture);
				} else {
					// TODO
				}
			}

			// diffuse as _MainTex
			commonColorOrTexture = techniqueType.diffuse;
			if (commonColorOrTexture != null) {
				var texture = commonColorOrTexture.texture;
				if (texture != null) {
					shaderProperties.DiffuseTexture = buildTexture(profileCOMMON, texture);
				} else {
					var color = commonColorOrTexture.color;
					if (color != null && color.value != null) {
						shaderProperties.DiffuseColor = ColorUtils.Deformat(color.value);
					}
				}
			}

			// transparent as _Transparent
			commonColorOrTexture = techniqueType.transparent;
			if (commonColorOrTexture != null) {
				var texture = commonColorOrTexture.texture;
				if (texture != null) {
					shaderProperties.TransparentTexture = buildTexture(profileCOMMON, texture);
				} else {
					// TODO
				}
			}
			// shader select
			Shader shader = null;
			UnityEngine.Material engineMaterial = null;
			shader = Shader.Find(shaderProperties.ToShader());
			engineMaterial = new UnityEngine.Material(shader);
			engineMaterial.name = materialName;
			shaderProperties.setupMaterial(engineMaterial);
			addEngineMaterial(engineMaterial);
			return true;
		}

		private UnityEngine.Texture buildTexture(COLLADA.ProfileCOMMON profileCOMMON, COLLADA.Texture texture) {
			UnityEngine.Texture engineTexture = null;
			string textureString = texture.texture;
			if (textureString != null) {
				var samplerNewparam = profileCOMMON.FindSamplerWithTexture(textureString);
				if (samplerNewparam != null) {
					// TODO more Sampler
					var sampler2D = samplerNewparam.sampler2D;
					if (sampler2D != null) {
						string samplerSource = sampler2D.source;
						if (samplerSource != null) {
							var surfaceNewparam = profileCOMMON.FindSurfaceWithSource(samplerSource);
							if (surfaceNewparam != null) {
								var surface = surfaceNewparam.surface;
								if (surface != null) {
									string init_from = surface.init_from;
									if (init_from != null) {
										engineTexture = buildImage(init_from);
									}
								}
							}
						}
					}
				}
			}
			return engineTexture;
		}

		private UnityEngine.Texture buildImage(string imageName) {
			UnityEngine.Texture engineTexture = null;
			var image = mCOLLADA.getImage(imageName);
			if (image != null) {
				string texturePath = image.init_from;
				if (texturePath != null) {
					uapp.IFile textureFile = getFileFromPath(texturePath);
					engineTexture = textureFile.GetContent<UnityEngine.Texture2D>();
					if (mListener != null) {
						mListener.OnResourceLoaded(engineTexture, textureFile);
					}
				}
			}
			return engineTexture;
		}

		private uapp.IFile getFileFromPath(string path) {
			//			if(relPath == null)
			//				return null;
			//
			//			// 解决一些制作工具导出相对路径时的问题
			//			if (relPath.StartsWith("file://")) {
			//				relPath = relPath.Substring(7);
			//			}
			//
			//			id<ICVFile> dir = mOriginFile.dir;
			//			if(dir == nil)
			//				return nil;
			//			return [dir relFileFromPath:relPath];

			if (path == null) {
				return null;
			}
			// external file
			//			uapp.IFile file = new uapp.File(FileType.Path, path);
			//			if (file.Exists) {
			//				return file;
			//			}
			uapp.IFile dir = mFile.Dir;
			if (dir == null) {
				return null;
			}
			uapp.IFile file = dir.RelFile(path);
			if (file.Exists) {
				return file;
			}
			string filename = Path.GetFileName(path);
			string newPath = Path.Combine(Path.Combine(dir.Path, "texture"), filename);
			file.Type = FileType.Path;
			file.Path = newPath;
			return file;
		}

		private UnityEngine.Material getEngineMaterial(string name) {
			if (mMaterials == null || !mMaterials.ContainsKey(name)) {
				return null;
			}
			return mMaterials[name];
		}

		private void addEngineMaterial(UnityEngine.Material material) {
			if (material == null) {
				return;
			}
			if (mMaterials == null) {
				mMaterials = new Dictionary<string, UnityEngine.Material>();
			}
			mMaterials[material.name] = material;
		}
	}
}
