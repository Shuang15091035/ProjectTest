/**
 * @file COLLADA.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-9-15
 * @brief
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace uapp {

	[XmlRoot(ElementName = "COLLADA", Namespace = "http://www.collada.org/2005/11/COLLADASchema")]
	public class COLLADA {

		public class Versions {

			public static string _1_4_1 = "1.4.1";
		}

		[XmlAttribute("version")]
		public string version;

		[XmlElement("asset")]
		public Asset asset;

		[XmlArray("library_images")]
		[XmlArrayItem("image")]
		public List<Image> library_images;

		[XmlArray("library_effects")]
		[XmlArrayItem("effect")]
		public List<Effect> library_effects;

		[XmlArray("library_materials")]
		[XmlArrayItem("material")]
		public List<Material> library_materials;

		[XmlArray("library_geometries")]
		[XmlArrayItem("geometry")]
		public List<Geometry> library_geometries;

		[XmlArray("library_visual_scenes")]
		[XmlArrayItem("visual_scene")]
		public List<VisualScene> library_visual_scenes;

		[XmlArray("library_animations")]
		[XmlArrayItem("animation")]
		public List<Animation> library_animations;

		[XmlElement("scene")]
		public Scene scene;

		[XmlIgnore]
		public bool Obfuscated = false;

		public COLLADA() {
			this.version = Versions._1_4_1;
		}

		private Dictionary<string, Source> mSources;

		public Node FindNodeByPath(string path) {
			if (path == null) {
				return null;
			}
			var parts = path.Split('/');
			if (parts == null || parts.Length == 0) {
				return null;
			}
			string part0 = parts[0];
			Node node = FindNodeById(part0);
			if (node == null) {
				return null;
			}
			for (int i = 1; i < parts.Length; i++) {
				string part = parts[i];
				node = node.FindChildById(part);
				if (node == null) {
					return null;
				}
			}
			return node;
		}

		public Node FindNodeById(string id) {
			Node found = null;
			if (library_visual_scenes != null) {
				foreach (var visualScene in library_visual_scenes) {
					found = visualScene.FindNodeById(id);
				}
			}
			return found;
		}

		public void AddImage(Image image) {
			if (image == null) {
				return;
			}
			if (library_images == null) {
				library_images = new List<Image>();
			}
			library_images.Add(image);
		}

		public void AddEffect(Effect effect) {
			if (effect == null) {
				return;
			}
			if (library_effects == null) {
				library_effects = new List<Effect>();
			}
			library_effects.Add(effect);
		}

		public void AddMaterial(Material material) {
			if (material == null) {
				return;
			}
			if (library_materials == null) {
				library_materials = new List<Material>();
			}
			library_materials.Add(material);
		}

		public void AddGeometry(Geometry geometry) {
			if (geometry == null) {
				return;
			}
			if (library_geometries == null) {
				library_geometries = new List<Geometry>();
			}
			library_geometries.Add(geometry);
		}

		public class MaterialInfo {
			public string name;
			public int texcoordSet;
		}

		public class Asset {

			public static class UpAxis {
				public static string X_UP = "X_UP";
				public static string Y_UP = "Y_UP";
				public static string Z_UP = "Z_UP";
			}

			[XmlElement("contributor")]
			public Contributor contributor;

			[XmlElement("up_axis")]
			public string up_axis;
			[XmlElement("unit")]
			public Unit unit;
		}

		public class Contributor {

			[XmlElement("author")]
			public string author = "June.Winter";
			[XmlElement("authoring_tool")]
			public string authoring_tool = "June.Sourcer";
			[XmlElement("version")]
			public string version = "1.0.0";
			[XmlElement("hashcode")]
			public string hashcode;
		}

		public class Unit {
			[XmlAttribute("meter")]
			public float meter = 1.0f;
		}

		public class Image {
			[XmlAttribute("id")]
			public string id;
			[XmlElement("init_from")]
			public string init_from;
		}

		public class Effect {
			[XmlAttribute("id")]
			public string id;
			[XmlElement("profile_COMMON")]
			public ProfileCOMMON profile_COMMON; // TODO 1 or more
		}

		public class ProfileCOMMON {
			[XmlElement("newparam")]
			public List<Newparam> newparams;
			[XmlElement("technique")]
			public TechniqueFX technique;

			public void AddNewparam(Newparam newparam) {
				if (newparam == null) {
					return;
				}
				if (newparams == null) {
					newparams = new List<Newparam>();
				}
				newparams.Add(newparam);
			}

			public Newparam FindSurfaceWithSource(string source) {
				if (newparams == null) {
					return null;
				}
				foreach (Newparam newparam in newparams) {
					// TODO more sampler
					if (newparam.surface != null && StringUtils.Equals(newparam.sid, source)) {
						return newparam;
					}
				}
				return null;
			}

			public Newparam FindSamplerWithTexture(string texture) {
				if (newparams == null) {
					return null;
				}
				foreach (Newparam newparam in newparams) {
					// TODO more sampler
					if (newparam.sampler2D != null && StringUtils.Equals(newparam.sid, texture)) {
						return newparam;
					}
				}
				return null;
			}
		}

		public class Newparam {
			[XmlAttribute("sid")]
			public string sid;
			[XmlElement("surface")]
			public Surface surface;
			[XmlElement("sampler2D")]
			public Sampler2D sampler2D;
		}

		public class Surface {

			public static class Types {

				public static string _2D = "2D";
			}

			[XmlAttribute("type")]
			public string type;
			[XmlElement("init_from")]
			public string init_from;
		}

		public class Sampler2D {
			[XmlElement("source")]
			public string source;
		}

		public class Bump {
			[XmlElement("texture")]
			public Texture texture;
		}

		public class AO {
			[XmlElement("texture")]
			public Texture texture;
		}

		public class TechniqueCore {
			public class Profile {
				public static string UCOLLADA = "UCOLLADA";
				public static string FCOLLADA = "FCOLLADA";
			}

			public TechniqueCore() : this(Profile.UCOLLADA) {

			}

			public TechniqueCore(string profile) {
				this.profile = profile;
			}

			[XmlAttribute("profile")]
			public string profile = Profile.UCOLLADA;

			[XmlElement("bump")]
			public Bump bump;
			[XmlElement("ao")]
			public AO ao;

			[XmlElement("diffuse_st")]
			public string diffuse_st;
//			[XmlElement("lightmap_st")]
//			public string lightmap_st;
//			[XmlElement("reflective_st")]
//			public string reflective_st;
			[XmlElement("transparent_st")]
			public string transparent_st;
			[XmlElement("bump_st")]
			public string bump_st;
			[XmlElement("ao_st")]
			public string ao_st;

			public void SetTilingOffsetValue(string textureName, string value) {
				if (StringUtils.Equals(textureName, Shaders.Textures.Diffuse)) {
					diffuse_st = value;
//				} else if (StringUtils.Equals(textureName, Shaders.Textures.Lightmap)) {
//					lightmap_st = value;
//				} else if (StringUtils.Equals(textureName, Shaders.Textures.Reflective)) {
//					reflective_st = value;
				} else if (StringUtils.Equals(textureName, Shaders.Textures.Transparent)) {
					transparent_st = value;
				} else if (StringUtils.Equals(textureName, Shaders.Textures.Bump)) {
					bump_st = value;
				} else if (StringUtils.Equals(textureName, Shaders.Textures.AO)) {
					ao_st = value;
				}
			}

			[XmlElement("diffuse_color")]
			public Color diffuseColor;
			[XmlElement("refl_amount")]
			public float? reflAmount;
			[XmlElement("rim_color")]
			public Color rimColor;
			[XmlElement("rim_power")]
			public float? rimPower;
		}

		public class Extra {
			[XmlElement("technique")]
			public List<TechniqueCore> techniques;
			public void AddTechnique(TechniqueCore TechniqueCore) {
				if (techniques == null) {
					techniques = new List<TechniqueCore>();
				}
				techniques.Add(TechniqueCore);
			}
		}

		public class TechniqueFX {
			[XmlAttribute("sid")]
			public string sid;
			[XmlElement("phong", typeof(Phong))]
			[XmlElement("blinn", typeof(Blinn))]
			public TechniqueType type;
			[XmlElement("extra")]
			public Extra extra;
		}

		public class TechniqueType {
			[XmlElement("ambient")]
			public CommonColorOrTexture ambient;
			[XmlElement("diffuse")]
			public CommonColorOrTexture diffuse;
			[XmlElement("reflective")]
			public CommonColorOrTexture reflective;
			[XmlElement("transparent")]
			public CommonColorOrTexture transparent;
		}

		public class Phong : TechniqueType {

		}

		public class Blinn : TechniqueType {

		}

		public class CommonColorOrTexture {
			[XmlElement("texture")]
			public Texture texture;
			[XmlElement("color")]
			public Color color;
		}

		public class CommonFloatOrParam {

		}

		public class Texture {
			[XmlAttribute("texture")]
			public string texture;
			[XmlAttribute("texcoord")]
			public string texcoord;
		}

		public class Color {
			[XmlText]
			public string value;
		}

		public class Material {
			[XmlAttribute("id")]
			public string id;
			[XmlAttribute("name")]
			public string name;
			[XmlElement("instance_effect")]
			public InstanceEffect instance_effect;
		}

		public class InstanceEffect {
			[XmlAttribute("url")]
			public string url;
		}

		public class Geometry {
			[XmlAttribute("id")]
			public string id;
			[XmlAttribute("name")]
			public string name;
			[XmlElement("mesh")]
			public Mesh mesh;
		}

		public class Mesh {
			[XmlElement("source")]
			public List<Source> sources;
			[XmlElement("vertices")]
			public Vertices vertices;
			[XmlElement("triangles", typeof(Triangles))]
			public List<Primitives> primitiveList;

			public class BuildMesh {
				public List<Vector3> positions;
				public List<Vector2>[] uvs;
				public List<int>[] indices;
				public int subMeshCount;
			}
			public BuildMesh buildMesh;

			public void AddSource(Source source) {
				if (source == null) {
					return;
				}
				if (sources == null) {
					sources = new List<Source>();
				}
				sources.Add(source);
			}

			public void AddPrimitives(Primitives primitives) {
				if (primitives == null) {
					return;
				}
				if (primitiveList == null) {
					primitiveList = new List<Primitives>();
				}
				primitiveList.Add(primitives);
			}

			public IEnumerator Build(COLLADA collada) {
				if (primitiveList == null || primitiveList.Count == 0) {
					yield break;
				}
				if (buildMesh == null) {
					buildMesh = new BuildMesh();
				}
				buildMesh.subMeshCount = primitiveList.Count;
				int subMeshIndex = 0;
				foreach (Primitives primitives in primitiveList) {
					if (primitives is Triangles) {
						Triangles triangles = primitives as Triangles;
						//BuildTriangles(triangles, vertices, subMeshIndex, collada);
						IEnumerator e = BuildTriangles(triangles, vertices, subMeshIndex, collada);
						while (e.MoveNext()) {
							yield return e.Current;
						}
					}
					subMeshIndex++;
					yield return null;
				}
				//return true;
			}

			public IEnumerator BuildTriangles(Triangles triangles, Vertices vertices, int subMeshIndex, COLLADA collada) {
				if (vertices == null) {
					yield break;
				}
				if (triangles == null) {
					yield break;
				}
				//triangles.Build();
				IEnumerator e = triangles.Build();
				while (e.MoveNext()) {
					yield return e.Current;
				}
				if (triangles.indices == null) {
					yield break;
				}
				var verticesInputs = vertices.inputs;
				if (verticesInputs == null || verticesInputs.Count == 0) {
					yield break;
				}
				var primitivesInputs = triangles.inputs;
				if (primitivesInputs == null || primitivesInputs.Count == 0) {
					yield break;
				}
				List<Vector3> positionArray = null;
				int positionIndexOffset = 0;
				const int MaxTexcoords = 4;
				List<Vector2>[] texcoord2Arrays = null;
				List<Vector3>[] texcoord3Arrays = null;
				int[] texcoordIndexOffsets = null;
				int texcoordSet = 0;
				// TODO comfirm render operation
				foreach (var input in primitivesInputs) {
					if (StringUtils.Equals(input.semantic, Input.Semantic.VERTEX)) {
						foreach (var ci in verticesInputs) {
							if (StringUtils.Equals(ci.semantic, Input.Semantic.POSITION)) {
								// TODO 先简单通过判断名字来确定这个vertices就是我们要找的，其实应该按名字全局搜索
								string inputSource = input.source.Substring(1);
								if (StringUtils.Equals(inputSource, vertices.id)) {
									Source positionSource = collada.getSource(ci.source.Substring(1));
									if (positionSource == null) {
										yield break;
									}
									//positionSource.Build();
									e = positionSource.Build();
									while (e.MoveNext()) {
										yield return e.Current;
									}
									if (positionSource.type == SourceType.Vector3) {
										positionArray = positionSource.vector3s;
										positionIndexOffset = input.offset ?? 0;
									} else {
										yield break;
									}
								}
							}
						}
					} else if (StringUtils.Equals(input.semantic, Input.Semantic.NORMAL)) {
						// TODO
					} else if (StringUtils.Equals(input.semantic, Input.Semantic.TEXCOORD)) {
						Source texcoordSource = collada.getSource(input.source.Substring(1));
						if (texcoordSource == null) {
							break;
						}
						//texcoordSource.Build();
						e = texcoordSource.Build();
						while (e.MoveNext()) {
							yield return e.Current;
						}
						//texcoordSet = input.set;
						if (texcoordSource.type == SourceType.Vector2) {
							if (texcoord2Arrays == null) {
								texcoord2Arrays = new List<Vector2>[MaxTexcoords];
								texcoordIndexOffsets = new int[MaxTexcoords];
							}
							texcoord2Arrays[texcoordSet] = texcoordSource.vector2s;
							texcoordIndexOffsets[texcoordSet] = input.offset ?? 0;
						} else if (texcoordSource.type == SourceType.Vector3) {
							if (texcoord3Arrays == null) {
								texcoord3Arrays = new List<Vector3>[MaxTexcoords];
								texcoordIndexOffsets = new int[MaxTexcoords];
							}
							texcoord3Arrays[texcoordSet] = texcoordSource.vector3s;
							texcoordIndexOffsets[texcoordSet] = input.offset ?? 0;
						}
						texcoordSet++;
					}
					//yield return null;
				}

				// build data
				if (positionArray == null) {
					yield break;
				}
				int indexCount = triangles.indices.Count;
				int indexStride = triangles.GetVertexComponentCount();
				//
				List<Vector3> positions = new List<Vector3>();
				List<Vector2>[] texcoords = null;
				List<int> indices = new List<int>();
				int index = 0;
				if (buildMesh.positions != null) {
					// NOTE index move
					index = buildMesh.positions.Count;
				}
				for (int i = 0; i < indexCount; i += indexStride) {
					int positionIndex = triangles.indices[i + positionIndexOffset];
					if (collada.Obfuscated) {
						positionIndex--;
					}
					Vector3 position = positionArray[positionIndex];
					if (collada.Obfuscated) {
						position = Vector3Utils.Decode(position);
					}
					positions.Add(position);
					//positions.Add(new Vector3(position.x, position.y, -position.z));
					if (texcoord2Arrays != null) {
						texcoordSet = 0;
						foreach (List<Vector2> texcoordArray in texcoord2Arrays) {
							if (texcoordArray == null) {
								continue;
							}
							if (texcoords == null) {
								texcoords = new List<Vector2>[MaxTexcoords];
							}
							if (texcoords[texcoordSet] == null) {
								texcoords[texcoordSet] = new List<Vector2>();
							}
							int texcoordsIndex = triangles.indices[i + texcoordIndexOffsets[texcoordSet]];
							if (collada.Obfuscated) {
								texcoordsIndex--;
							}
							Vector2 uv = texcoordArray[texcoordsIndex];
							texcoords[texcoordSet].Add(uv);
							texcoordSet++;
						}
					} else if (texcoord3Arrays != null) {
						texcoordSet = 0;
						foreach (List<Vector3> texcoordArray in texcoord3Arrays) {
							if (texcoordArray == null) {
								continue;
							}
							if (texcoords == null) {
								texcoords = new List<Vector2>[MaxTexcoords];
							}
							if (texcoords[texcoordSet] == null) {
								texcoords[texcoordSet] = new List<Vector2>();
							}
							int texcoordsIndex = triangles.indices[i + texcoordIndexOffsets[texcoordSet]];
							if (collada.Obfuscated) {
								texcoordsIndex--;
							}
							Vector3 texcoord = texcoordArray[texcoordsIndex];
							Vector2 uv = new Vector2(texcoord.x, texcoord.y);
							texcoords[texcoordSet].Add(uv);
							texcoordSet++;
						}
					}
					indices.Add(index++);
					//yield return null;
				}
				if (buildMesh.positions == null) {
					buildMesh.positions = new List<Vector3>();
				}
				buildMesh.positions.AddRange(positions);
				//yield return null;
				if (buildMesh.uvs == null) {
					buildMesh.uvs = new List<Vector2>[MaxTexcoords];
				}
				if (texcoords != null) {
					for (int i = 0; i < MaxTexcoords; i++) {
						if (texcoords[i] != null) {
							if (buildMesh.uvs[i] == null) {
								buildMesh.uvs[i] = new List<Vector2>();
							}
							buildMesh.uvs[i].AddRange(texcoords[i]);
						}
						//yield return null;
					}
				}
				if (buildMesh.indices == null) {
					buildMesh.indices = new List<int>[buildMesh.subMeshCount];
				}
				buildMesh.indices[subMeshIndex] = indices;
				//return true;
			}
		}

		public enum SourceType {
			Unknown,
			Float,
			Vector2,
			Vector3,
			Color,
			Matrix4,
			JOINT,
		}

		public class Source {
			[XmlAttribute("id")]
			public string id;
			[XmlElement("float_array")]
			public FloatArray float_array;
			[XmlElement("Name_array")]
			public NameArray Name_array;
			[XmlElement("technique_common")]
			public TechniqueCommon technique_common;

			[XmlIgnore]
			public SourceType type;
			[XmlIgnore]
			public List<Vector2> vector2s;
			[XmlIgnore]
			public List<Vector3> vector3s;
			[XmlIgnore]
			public List<UnityEngine.Color> colors;

			public IEnumerator Build() {
				if (type != SourceType.Unknown) {
					yield break;
				}
				if (technique_common == null) {
					yield break;
				}
				Accessor accessor = technique_common.accessor;
				if (accessor == null) {
					yield break;
				}
				List<Param> parameters = accessor.parameters;
				if (parameters == null || parameters.Count == 0) {
					yield break;
				}
				// 分析类型
				type = SourceType.Unknown;
				int i = 0;
				switch (parameters.Count) {
					case 1: {
						Param param = parameters[0];
						string name = param.name;
						string paramType = param.type;
						if (StringUtils.Equals(name, "JOINT") && StringUtils.Equals(paramType, "Name")) {
							type = SourceType.JOINT;
						} else if (StringUtils.Equals(name, "TRANSFORM") && StringUtils.Equals(paramType, "float4x4")) {
							type = SourceType.Matrix4;
						} else if (StringUtils.Equals(name, "WEIGHT") && StringUtils.Equals(paramType, "float")) {
							type = SourceType.Float;
						}
						break;
					}
					case 2: {
						i = 0;
						foreach (Param param in parameters) {
							string name = param.name;
							string paramType = param.type;
							switch (i) {
								case 0: {
									if ((StringUtils.Equals(name, "X") || StringUtils.Equals(name, "S")) && StringUtils.Equals(paramType, "float")) {
										type = SourceType.Float;
									}
									break;
								}
								case 1: {
									if ((StringUtils.Equals(name, "Y") || StringUtils.Equals(name, "T")) && StringUtils.Equals(paramType, "float")) {
										type = SourceType.Vector2;
									}
									break;
								}
							}
							i++;
						}
						break;
					}
					case 3: {
						i = 0;
						foreach (Param param in parameters) {
							string name = param.name;
							string paramType = param.type;
							switch (i) {
								case 0: {
									if ((StringUtils.Equals(name, "X") || StringUtils.Equals(name, "S")) && StringUtils.Equals(paramType, "float")) {
										type = SourceType.Float;
									}
									break;
								}
								case 1: {
									if ((StringUtils.Equals(name, "Y") || StringUtils.Equals(name, "T")) && StringUtils.Equals(paramType, "float")) {
										type = SourceType.Vector2;
									}
									break;
								}
								case 2: {
									if ((StringUtils.Equals(name, "Z") || StringUtils.Equals(name, "P")) && StringUtils.Equals(paramType, "float")) {
										type = SourceType.Vector3;
									}
									break;
								}
							}
							i++;
							//yield return null;
						}
						break;
					}
				case 4: {
					i = 0;
					foreach (Param param in parameters) {
						string name = param.name;
						string paramType = param.type;
						type = SourceType.Unknown;
						switch (i) {
							case 0: {
								if (StringUtils.Equals(name, "R") && StringUtils.Equals(paramType, "float")) {
									type = SourceType.Color;
								}
								break;
							}
							case 1: {
								if (StringUtils.Equals(name, "G") && StringUtils.Equals(paramType, "float")) {
									type = SourceType.Color;
								}
								break;
							}
							case 2: {
								if (StringUtils.Equals(name, "B") && StringUtils.Equals(paramType, "float")) {
									type = SourceType.Color;
								}
								break;
							}
							case 3: {
								if (StringUtils.Equals(name, "A") && StringUtils.Equals(paramType, "float")) {
									type = SourceType.Color;
								}
								break;
							}
						}
						i++;
						//yield return null;
					}
					break;
				}
					default:
						break;
				}
				if (type == SourceType.Unknown) {
					yield break;
				}
//				if (type == SourceType.JOINT && colladaInstanceController == nil) {
//					return NO;
//				}
//				// 确认类型，解析数组
//				if (mValues == nil) {
//					mValues = [NSMutableArray array];
//				}
//				[mValues clear];
				switch (type) {
					case SourceType.Float: {
						// TODO
						break;
					}
					case SourceType.Vector2: {
						if (vector2s == null) {
							vector2s = new List<Vector2>();
						}
						vector2s.Clear();
						i = 0;
						string[] floatTexts = float_array.values.Split(new char[] {' '});
						Vector2 v2 = Vector2.zero;
						foreach (string floatText in floatTexts) {
							if (StringUtils.IsNullOrBlank(floatText)) {
								continue;
							}
							switch (i) {
								case 0: {
									v2.x = float.Parse(floatText);
									i++;
									break;
								}
								case 1: {
									v2.y = float.Parse(floatText);
									vector2s.Add(v2);
									i = 0;
									break;
								}
							}
						}
						//mCount = mValues.count;
						break;
					}
					case SourceType.Vector3: {
						if (vector3s == null) {
							vector3s = new List<Vector3>();
						}
						vector3s.Clear();
						i = 0;
						string[] floatTexts = float_array.values.Split(new char[] {' '});
						Vector3 v3 = Vector3.zero;
						//int ii = 0;
						foreach (string floatText in floatTexts) {
							if (StringUtils.IsNullOrBlank(floatText)) {
								continue;
							}
							switch (i) {
								case 0: {
									v3.x = float.Parse(floatText);
									i++;
									break;
								}
								case 1: {
									v3.y = float.Parse(floatText);
									i++;
									break;
								}
								case 2: {
									v3.z = float.Parse(floatText);
									vector3s.Add(v3);
									i = 0;
									break;
								}
							}
							//yield return null;
//							ii++;
//							if (ii % 500 == 0)
//								yield return null;
						}
						//mCount = mValues.count;
						break;
					}
				case SourceType.Color: {
					if (colors == null) {
						colors = new List<UnityEngine.Color>();
					}
					colors.Clear();
					i = 0;
					string[] floatTexts = float_array.values.Split(new char[] {' '});
					var c = UnityEngine.Color.white;
					//int ii = 0;
					foreach (string floatText in floatTexts) {
						if (StringUtils.IsNullOrBlank(floatText)) {
							continue;
						}
						switch (i) {
							case 0: {
								c.r = float.Parse(floatText);
								i++;
								break;
							}
							case 1: {
								c.g = float.Parse(floatText);
								i++;
								break;
							}
							case 2: {
								c.b = float.Parse(floatText);
								i++;
								break;
							}
							case 3: {
								c.a = float.Parse(floatText);
								colors.Add(c);
								i = 0;
								break;
							}
						}
					}
					break;
				}
					case SourceType.Matrix4: {
						// TODO
						break;
					}
					case SourceType.JOINT: {
						// TODO
						break;
					}
					default:
						break;
				}
				//return true;
			}
		}

		public class FloatArray {
			[XmlAttribute("id")]
			public string id;
			[XmlAttribute("count")]
			public int count;
			[XmlText]
			public string values;
		}

		public class NameArray {
			[XmlAttribute("id")]
			public string id;
			[XmlAttribute("count")]
			public int count;
			[XmlText]
			public string values;
		}

		public class TechniqueCommon {
			[XmlElement("accessor")]
			public Accessor accessor;
			[XmlElement("instance_material")]
			public List<InstanceMaterial> instance_materials;

			public void AddInstanceMaterial(InstanceMaterial instanceMaterial) {
				if (instanceMaterial == null) {
					return;
				}
				if (instance_materials == null) {
					instance_materials = new List<InstanceMaterial>();
				}
				instance_materials.Add(instanceMaterial);
			}
		}

		public class Accessor {
			[XmlAttribute("source")]
			public string source;
			[XmlAttribute("count")]
			public int count;
			[XmlAttribute("stride")]
			public int stride;
			[XmlElement("param")]
			public List<Param> parameters;

			public void AddParam(Param param) {
				if (param == null) {
					return;
				}
				if (parameters == null) {
					parameters = new List<Param>();
				}
				parameters.Add(param);
			}
		}

		public class Param {
			[XmlAttribute("name")]
			public string name;
			[XmlAttribute("type")]
			public string type;
		}

		public class Vertices {
			[XmlAttribute("id")]
			public string id;
			[XmlElement("input")]
			public List<Input> inputs;

			public void AddInput(Input input) {
				if (input == null) {
					return;
				}
				if (inputs == null) {
					inputs = new List<Input>();
				}
				inputs.Add(input);
			}
		}

		public class Input {

			public static class Semantic {
				public static string VERTEX = "VERTEX";
				public static string POSITION = "POSITION";
				public static string NORMAL = "NORMAL";
				public static string TANGENT = "TANGENT";
				public static string BINORMAL = "BINORMAL";
				public static string TEXCOORD = "TEXCOORD";
				public static string COLOR = "COLOR";
				public static string INPUT = "INPUT";
				public static string OUTPUT = "OUTPUT";
				public static string INTERPOLATION = "INTERPOLATION";
				public static string IN_TANGENT = "IN_TANGENT";
				public static string OUT_TANGENT = "OUT_TANGENT";
			}

			[XmlAttribute("semantic")]
			public string semantic;
			[XmlAttribute("source")]
			public string source;
			[XmlAttribute("offset")]
			public int? offset;
			[XmlAttribute("set")]
			public int? set;

		}

		public class Primitives {

		}

		public class Triangles : Primitives {
			[XmlAttribute("material")]
			public string material;
			[XmlAttribute("count")]
			public int count;
			[XmlElement("input")]
			public List<Input> inputs;
			[XmlElement("p")]
			public string p;

			public List<int> indices;

			public int GetVertexComponentCount() {
				if (inputs == null)
					return 0;
				// input当中不同offset的个数才是ComponentCount
				int count = 0;
				Flags f = new Flags();
				foreach (Input input in inputs) {
					if (!f.IsSet(input.offset ?? -1)) {
						f.Set(input.offset ?? -1, true);
						count++;
					}
				}
				return count;
			}

			public IEnumerator Build() {
				if (p == null) {
					yield break;
				}
				if (indices == null) {
					indices = new List<int>();
				}
				indices.Clear();
				string[] indexTexts = p.Split(new char[] {' '});
				foreach (string indexText in indexTexts) {
					if (StringUtils.IsNullOrBlank(indexText)) {
						continue;
					}
					int v = int.Parse(indexText);
					indices.Add(v);
					//yield return null;
				}
				count = indices.Count;
				//return true;
				yield return null;
			}
		}

		public class VisualScene {
			[XmlAttribute("id")]
			public string id;
			[XmlElement("node")]
			public List<Node> nodes;

			public void AddNode(Node node) {
				if (node == null) {
					return;
				}
				if (nodes == null) {
					nodes = new List<Node>();
				}
				nodes.Add(node);
			}

			public Node FindNodeById(string id) {
				if (nodes == null) {
					return null;
				}
				Node found = null;
				foreach (var node in nodes) {
					if (node.id == id) {
						found = node;
						break;
					}
					found = node.FindChildById(id);
					if (found != null) {
						return found;
					}
				}
				return found;
			}
		}
			
		public class Translate {
			[XmlAttribute("sid")]
			public string sid;
			[XmlText]
			public string value;
		}

		public class Rotate {
			[XmlAttribute("sid")]
			public string sid;
			[XmlText]
			public string value;
		}

		public class Node {
			[XmlAttribute("id")]
			public string id;
			[XmlAttribute("name")]
			public string name;
			[XmlAttribute("type")]
			public string type;

			[XmlElement("translate")]
			public List<Translate> translates;
			[XmlElement("rotate")]
			public List<Rotate> rotates;
			[XmlElement("scale")]
			public string scale;
			[XmlElement("matrix")]
			public string matrix;

			[XmlElement("instance_geometry")]
			public List<InstanceGeometry> instance_geometries;
			[XmlElement("node")]
			public List<Node> chlidren;

			[XmlIgnore]
			public GameObject gameObject;

			public void AddChild(Node child) {
				if (child == null) {
					return;
				}
				if (chlidren == null) {
					chlidren = new List<Node>();
				}
				chlidren.Add(child);
			}

			public Node FindChildById(string id) {
				if (chlidren == null) {
					return null;
				}
				foreach (var child in chlidren) {
					if (child.id == id) {
						return child;
					}
					Node found = child.FindChildById(id);
					if (found != null) {
						return found;
					}
				}
				return null;
			}

			public void AddTranslate(Translate translate) {
				if (translate == null) {
					return;
				}
				if (translates == null) {
					translates = new List<Translate>();
				}
				translates.Add(translate);
			}

			public void AddRotate(Rotate rotate) {
				if (rotate == null) {
					return;
				}
				if (rotates == null) {
					rotates = new List<Rotate>();
				}
				rotates.Add(rotate);
			}

			public void AddInstanceGeometry(InstanceGeometry instanceGeometry) {
				if (instanceGeometry == null) {
					return;
				}
				if (instance_geometries == null) {
					instance_geometries = new List<InstanceGeometry>();
				}
				instance_geometries.Add(instanceGeometry);
			}
		}

		public class InstanceGeometry {
			[XmlAttribute("url")]
			public string url;
			[XmlElement("bind_material")]
			public BindMaterial bind_material;
		}

		public class BindMaterial {
			[XmlElement("technique_common")]
			public TechniqueCommon technique_common;

			public string getMaterialName(int subMeshIndex) {
				if (technique_common == null) {
					return null;
				}
				var instanceMaterials = technique_common.instance_materials;
				if (instanceMaterials == null || instanceMaterials.Count == 0) {
					return null;
				}
				if (instanceMaterials.Count < subMeshIndex) {
					return null;
				}
				return instanceMaterials[subMeshIndex].target.Substring(1);
			}

			public MaterialInfo getMaterialInfo(Triangles triangles) { // TODO more primitives
				if (triangles == null) {
					return null;
				}
				var primitivesInputs = triangles.inputs;
				if (primitivesInputs == null || primitivesInputs.Count == 0) {
					return null;
				}

				string materialName = null;
				int texcoordSet = 0;

				if (technique_common != null) {
					var instanceMaterials = technique_common.instance_materials;
					if (instanceMaterials != null && instanceMaterials.Count > 0) {
						foreach (InstanceMaterial instanceMaterial in instanceMaterials) {
							var bindVertexInputs = instanceMaterial.bind_vertex_inputs;
							if (bindVertexInputs != null && bindVertexInputs.Count > 0) {
								foreach (Input input in primitivesInputs) {
									foreach (BindVertexInput bindVertexInput in bindVertexInputs) {
										if (StringUtils.Equals(input.semantic, Input.Semantic.TEXCOORD)) {
											if (StringUtils.Equals(input.semantic, bindVertexInput.input_semantic) && input.set == bindVertexInput.input_set) {
												texcoordSet = bindVertexInput.input_set - 1;  // collada input_set not 0 base
											}
										}
									}
								}
							}

							// TODO 暂时只支持一个instance_material
							materialName = instanceMaterial.target.Substring(1);
							break;
						}
					}
				}

				MaterialInfo materialInfo = new MaterialInfo();
				materialInfo.name = materialName;
				materialInfo.texcoordSet = texcoordSet;
				return materialInfo;
			}
		}

		public class InstanceMaterial {
			[XmlAttribute("symbol")]
			public string symbol;
			[XmlAttribute("target")]
			public string target;
			[XmlElement("bind_vertex_input")]
			public List<BindVertexInput> bind_vertex_inputs;

			public void AddBindVertexInput(BindVertexInput bindVertexInput) {
				if (bindVertexInput == null) {
					return;
				}
				if (bind_vertex_inputs == null) {
					bind_vertex_inputs = new List<BindVertexInput>();
				}
				bind_vertex_inputs.Add(bindVertexInput);
			}
		}

		public class BindVertexInput {
			[XmlAttribute("semantic")]
			public string semantic;
			[XmlAttribute("input_semantic")]
			public string input_semantic;
			[XmlAttribute("input_set")]
			public int input_set;
		}

		public class Scene {
			[XmlElement("instance_visual_scene")]
			public InstanceVisualScene instance_visual_scene;
		}

		public class InstanceVisualScene {
			[XmlAttribute("url")]
			public string url;
		}

		public class Animation {

			[XmlAttribute("id")]
			public string id;

			[XmlElement("source")]
			public List<Source> sources;

			[XmlElement("sampler")]
			public List<AnimationSampler> samplers;

			[XmlElement("channel")]
			public List<Channel> channels;

			public void AddSource(Source source) {
				if (source == null) {
					return;
				}
				if (sources == null) {
					sources = new List<Source>();
				}
				sources.Add(source);
			}

			public void AddSampler(AnimationSampler sampler) {
				if (sampler == null) {
					return;
				}
				if (samplers == null) {
					samplers = new List<AnimationSampler>();
				}
				samplers.Add(sampler);
			}

			public void AddChannel(Channel channel) {
				if (channel == null) {
					return;
				}
				if (channels == null) {
					channels = new List<Channel>();
				}
				channels.Add(channel);
			}
		}

		public class AnimationSampler {
			[XmlAttribute("id")]
			public string id;

			[XmlElement("input")]
			public List<Input> inputs;

			public void AddInput(Input input) {
				if (input == null) {
					return;
				}
				if (inputs == null) {
					inputs = new List<Input>();
				}
				inputs.Add(input);
			}
		}

		public class Channel {
			[XmlAttribute("source")]
			public string source;

			[XmlAttribute("target")]
			public string target;
		}

		public VisualScene getVisualScene(string id) {
			if (library_visual_scenes == null || library_visual_scenes.Count == 0) {
				return null;
			}
			foreach (VisualScene scene in library_visual_scenes) {
				if (StringUtils.Equals(scene.id, id)) {
					return scene;
				}
			}
			return null;
		}

		public Geometry getGeometry(string id) {
			if (library_geometries == null || library_geometries.Count == 0) {
				return null;
			}
			foreach (Geometry geometry in library_geometries) {
				if (StringUtils.Equals(geometry.id, id)) {
					return geometry;
				}
			}
			return null;
		}

		public Material getMaterial(string id) {
			if (library_materials == null || library_materials.Count == 0) {
				return null;
			}
			foreach (Material material in library_materials) {
				if (StringUtils.Equals(material.id, id)) {
					return material;
				}
			}
			return null;
		}

		public Effect getEffect(string id) {
			if (library_effects == null || library_effects.Count == 0) {
				return null;
			}
			foreach (Effect effect in library_effects) {
				if (StringUtils.Equals(effect.id, id)) {
					return effect;
				}
			}
			return null;
		}

		public Image getImage(string id) {
			if (library_images == null || library_images.Count == 0) {
				return null;
			}
			foreach (Image image in library_images) {
				if (StringUtils.Equals(image.id, id)) {
					return image;
				}
			}
			return null;
		}

		public Source getSource(string id) {
			if (library_geometries == null || library_geometries.Count == 0) {
				return null;
			}
			if (mSources == null) {
				mSources = new Dictionary<string, Source>();
				foreach (Geometry geometry in library_geometries) {
					Mesh mesh = geometry.mesh;
					if (mesh == null || mesh.sources == null || mesh.sources.Count == 0) {
						continue;
					}
					foreach (var source in mesh.sources) {
						mSources[source.id] = source;
					}
				}
			}
			if (!mSources.ContainsKey(id)) {
				return null;
			}
			return mSources[id];
		}
	}
}
