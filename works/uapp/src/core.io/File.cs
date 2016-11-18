/**
 * @file File.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace uapp {

	public class File : IFile {

		private FileType mType;
		private string mPath;
		private bool mIsDir;
		private FileStream mInputStream;
		private FileStream mOutputStream;
		private FileStream mFileStream;
		private object mExtra;

		public File(FileType type, string path, bool isDir = false) {
			mType = type;
			mPath = path;
			mIsDir = isDir;
		}

		public static IFile FilePath(string path, bool isDir = false) {
			return new File(FileType.Path, path, isDir);
		}

		public static IFile Resource(string path, bool isDir = false) {
			return new File(FileType.Resources, path, isDir);
		}

		public static IFile DataPath(string path, bool isDir = false) {
			return new File(FileType.Data, path, isDir);
		}

		public static IFile Scene(string name) {
			return new File(FileType.Scene, name);
		}

		public static IFile PersistentData(string path, bool isDir = false) {
			return new File(FileType.PersistentData, path, isDir);
		}

		public FileType Type {
			get {
				return mType;
			}
			set {
				mType = value;
			}
		}

		public string Path {
			get {
				return mPath;
			}
			set {
				mPath = value;
			}
		}

		public string RealPath {
			get {
				string realPath = null;
				switch (mType) {
					case FileType.Path:
						{
							realPath = mPath;
							break;
						}
					case FileType.Resources:
						{
							realPath = mPath;
							break;
						}
					case FileType.Data:
						{
							var dataPath = Application.dataPath;
							var combinePath = System.IO.Path.Combine(dataPath, mPath);
							realPath = System.IO.Path.GetFullPath(combinePath);
							break;
						}
					case FileType.PersistentData:
						{
							var persistentDataPath = Application.persistentDataPath;
							var combinePath = System.IO.Path.Combine(persistentDataPath, mPath);
							realPath = System.IO.Path.GetFullPath(combinePath);
							break;
						}
					default:
						break;
				}
				return realPath;
			}
		}

		public string FileName {
			get {
				string filename = null;
				switch (mType) {
					case FileType.Path:
					case FileType.Resources:
					case FileType.Data:
					case FileType.PersistentData:
						{
							filename = System.IO.Path.GetFileName(mPath);
							break;
						}
					default:
						break;
				}
				return filename;
			}
		}

		public string BaseName {
			get {
				string basename = null;
				switch (mType) {
					case FileType.Path:
					case FileType.Resources:
					case FileType.Data:
					case FileType.PersistentData:
						{
							basename = System.IO.Path.GetFileNameWithoutExtension(mPath);
							break;
						}
					default:
						break;
				}
				return basename;
			}
		}

		public string Ext {
			get {
				string ext = null;
				switch (mType) {
					case FileType.Path:
					case FileType.Resources:
					case FileType.Data:
					case FileType.PersistentData:
						{
							ext = System.IO.Path.GetExtension(mPath);
							break;
						}
					default:
						break;
				}
				return ext;
			}
		}

		public bool IsDir {
			get {
				if (Exists) {
					mIsDir = (System.IO.File.GetAttributes(RealPath) & System.IO.FileAttributes.Directory) == System.IO.FileAttributes.Directory;
				}
				return mIsDir;
			}
			set {
				mIsDir = value;
			}
		}

		public IFile Dir {
			get {
				IFile dir = null;
				switch (mType) {
					case FileType.Path:
					case FileType.Resources:
					case FileType.Data:
					case FileType.PersistentData:
						{
							dir = new File(mType, System.IO.Path.GetDirectoryName(mPath));
							break;
						}
					default:
						break;
				}
				return dir;
			}
		}

		public IFile RelFile(string relPath, bool isDir) {
			IFile relFile = null;
			switch (mType) {
				case FileType.Resources:
					{
						// TODO
						break;
					}
				case FileType.Path:
				case FileType.Data:
				case FileType.PersistentData:
					{
						string newPath = System.IO.Path.GetFullPath(System.IO.Path.Combine(RealPath, relPath));
						if (newPath != null) {
							// NOTE use Path for type
							relFile = new File(FileType.Path, newPath, isDir);
						}
						break;
					}
				default:
					break;
			}
			return relFile;
		}

		public bool Exists {
			get {
				bool exists = false;
				switch (mType) {
					case FileType.Resources:
						{
							// TODO how to check
							exists = true;
							break;
						}
					case FileType.Data:
					case FileType.PersistentData:
					case FileType.Path:
						{
							exists = System.IO.Directory.Exists(RealPath) || System.IO.File.Exists(RealPath);
							break;
						}
					default:
						break;
				}
				return exists;
			}
		}

		public bool Create(bool overrideIfExists) {
			if (Exists && !overrideIfExists) {
				return false;
			}
			bool b = false;
			switch (mType) {
				case FileType.Resources:
					{
						// NOTE unable to create
						break;
					}
				case FileType.Data:
				case FileType.PersistentData:
				case FileType.Path:
					{
						if (IsDir) {
							System.IO.Directory.CreateDirectory(RealPath);
							b = true;
						} else {
							System.IO.FileStream fileStream = System.IO.File.Create(RealPath);
							if (fileStream != null) {
								fileStream.Close();
								b = true;
							}
						}
						break;
					}
				default:
					break;
			}
			return b;
		}

		public bool Delete() {
			if (!Exists) {
				return false;
			}
			bool b = false;
			switch (mType) {
				case FileType.Resources:
					{
						// NOTE unable to create
						break;
					}
				case FileType.Data:
				case FileType.PersistentData:
				case FileType.Path:
					{
						if (IsDir) {
							System.IO.Directory.Delete(RealPath, true);
							b = true;
						} else {
							System.IO.File.Delete(RealPath);
							b = true;
						}
						break;
					}
				default:
					break;
			}
			return b;
		}

		public bool OpenInput() {
			if (mInputStream != null) {
				return true;
			}
			closeOutput();
			bool b = false;
			switch (mType) {
				case FileType.Resources:
				case FileType.Data:
				case FileType.Path:
				case FileType.PersistentData:
					{
						mInputStream = new FileStream(RealPath, FileMode.Open);
						if (mInputStream != null) {
							b = true;
						}
						break;
					}
				default:
					break;
			}
			mFileStream = mInputStream;
			return b;
		}

		public bool OpenOutput() {
			if (mOutputStream != null) {
				return true;
			}
			closeInput();
			bool b = false;
			switch (mType) {
				case FileType.Resources:
					{
						// unable to open
						break;
					}
				case FileType.Data:
				case FileType.Path:
				case FileType.PersistentData:
					{
						mOutputStream = new FileStream(RealPath, FileMode.Create);
						if (mOutputStream != null) {
							b = true;
						}
						break;
					}
				default:
					break;
			}
			mFileStream = mOutputStream;
			return b;
		}

		public void Close() {
			closeInput();
			closeOutput();
		}

		private void closeInput() {
			if (mInputStream != null) {
				mInputStream.Close();
				mInputStream = null;
			}
		}

		private void closeOutput() {
			if (mOutputStream != null) {
				mOutputStream.Close();
				mOutputStream = null;
			}
		}

		public FileStream FileStream {
			get {
				return mFileStream;
			}
		}

		public bool Copy(IFile src, bool overrideIfExists, string[] patterns, bool recursive) {
			if (src == null || (src.IsDir && !this.IsDir)) {
				return false;
			}
			if (StringUtils.Equals(src.RealPath, this.RealPath)) {
				return true;
			}
			if (this.IsDir) {
				if (!Create(overrideIfExists)) {
					return false;
				}
			}
			if (!src.IsDir) {
				if (this.IsDir) {
					var filename = src.FileName;
					var dstPath = System.IO.Path.Combine(RealPath, filename);
					System.IO.File.Copy(src.RealPath, dstPath, overrideIfExists);
				} else {
					System.IO.File.Copy(src.RealPath, RealPath, overrideIfExists);
				}
			} else {
				src.EnumFiles((IFile file) => {
					var newFilename = file.RealPath.Substring(src.RealPath.Length + 1);
					IFile newFile = RelFile(newFilename, false);
					newFile.Copy(file);
				}, patterns, recursive);
			}
			return true;
		}

		public bool EnumFiles(OnFileEnumDelegate onFileEnum, string[] patterns, bool recursive) {
			if (onFileEnum == null || !IsDir) {
				return false;
			}
			string dir = RealPath;
			try {
				var fileEntries = System.IO.Directory.GetFileSystemEntries(dir);
				if (fileEntries == null) {
					return false;
				}
				foreach (string fileEntry in fileEntries) {
					try {
						var filename = System.IO.Path.GetFileName(fileEntry);
						IFile file = new File(FileType.Path, System.IO.Path.Combine(dir, filename));
						if (!file.IsDir) {
							bool ok = true;
							if (patterns != null) {
								ok = false;
								foreach (string pattern in patterns) {
									if (Regex.IsMatch(filename, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
										ok = true;
									}
								}
							}
							if (ok) {
								onFileEnum(file);
							}
						} else {
							if (recursive) {
								file.EnumFiles(onFileEnum, patterns, recursive);
							}
						}
					} catch {
						// error
						return false;
					}
				}
			} catch {
				// error
				return false;
			}
			return true;
		}

		public IList<IFile> ListFiles(string[] patterns, bool recursive) {
			if (!Exists) {
				return null;
			}
			IList<IFile> files = null;
			switch (mType) {
				case FileType.Resources:
					{
						// unable to list
						break;
					}
				case FileType.Path:
				case FileType.Data:
				case FileType.PersistentData:
					{
						EnumFiles((IFile file) => {
							if (files == null) {
								files = new List<IFile>();
							}
							files.Add(file);
						}, patterns, recursive);
						break;
					}
				default:
					break;
			}
			return files;
		}

		//		public IList<IFile> ListDirFiles(string dir, string[] patterns = null, bool recursive = false) {
		//			List<IFile> files = null;
		//			try {
		//				var fileEntries = System.IO.Directory.GetFileSystemEntries(dir);
		//				if (fileEntries == null) {
		//					return files;
		//				}
		//				foreach (string fileEntry in fileEntries) {
		//					try {
		//						bool isFile = (System.IO.File.GetAttributes(fileEntry) & System.IO.FileAttributes.Directory) != System.IO.FileAttributes.Directory;
		//						if (isFile) {
		//							string filename = null;
		//							bool ok = false;
		//							if (patterns == null) {
		//								filename = System.IO.Path.GetFileName(fileEntry);
		//								ok = true;
		//							} else {
		//								foreach (string pattern in patterns) {
		//									filename = System.IO.Path.GetFileName(fileEntry);
		//									if (Regex.IsMatch(filename, pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline)) {
		//										ok = true;
		//									}
		//								}
		//							}
		//							if (ok) {
		//								if (files == null) {
		//									files = new List<IFile>();
		//								}
		//								files.Add(new File(FileType.Path, System.IO.Path.Combine(dir, filename)));
		//							}
		//						} else {
		//							if (recursive) {
		//								if (files == null) {
		//									files = new List<IFile>();
		//								}
		//								files.AddRange(ListDirFiles(fileEntry, patterns, recursive));
		//							}
		//						}
		//					} catch {
		//
		//					}
		//				}
		//			} catch {
		//
		//			}
		//			return files;
		//		}

		public string Text {
			get {
				return TextWithEncoding(null);
			}
		}

		public string TextWithEncoding(Encoding encoding) {
			if (!Exists) {
				return null;
			}
			if (encoding == null) {
				encoding = Encoding.UTF8;
			}
			string text = null;
			switch (mType) {
				case FileType.Data:
				case FileType.Path:
				case FileType.PersistentData:
					{
						text = System.IO.File.ReadAllText(RealPath);
						break;
					}
				case FileType.Resources:
					{
						TextAsset textAsset = Resources.Load(mPath, typeof(TextAsset)) as TextAsset;
						// NOTE Resources中的文件会按原来的编码方式读取出来，故无需编码转换
						//					if (textAsset != null) {
						//						text = encoding.GetString(textAsset.bytes);
						//					}
						if (textAsset != null) {
							text = textAsset.text;
						}
						break;
					}
				default:
					break;
			}
			return text;
		}

		public bool WriteText(string text) {
			return WriteTextWithEncoding(text, null);
		}

		public bool WriteTextWithEncoding(string text, Encoding encoding) {
			if (encoding == null) {
				encoding = Encoding.UTF8;
			}
			bool b = false;
			switch (mType) {
				case FileType.Resources:
					{
						break;
					}
				case FileType.Data:
				case FileType.PersistentData:
				case FileType.Path:
					{
						System.IO.File.WriteAllText(RealPath, text, encoding);
						b = true;
						break;
					}
				default:
					break;
			}
			return b;
		}

		public byte[] Bytes {
			get {
				byte[] bytes = null;
				switch (mType) {
					case FileType.Path:
					case FileType.Resources:
					case FileType.Data:
					case FileType.PersistentData:
						{
							bytes = System.IO.File.ReadAllBytes(RealPath);
							break;
						}
					default:
						break;
				}
				return bytes;
			}
			set {
				switch (mType) {
					case FileType.Path:
					case FileType.Data:
					case FileType.PersistentData:
						{
							System.IO.File.WriteAllBytes(RealPath, value);
							break;
						}
					default:
						break;
				}
			}
		}

		#if UNITY_EDITOR
		public bool Import() {
			bool b = false;
			switch (mType) {
				case FileType.Resources:
					{
						// TODO
						break;
					}
				case FileType.Path:
				case FileType.Data:
				case FileType.PersistentData:
					{
						var assetPath = PathUtils.GetProjectRelativePath(RealPath);
						if (!StringUtils.IsNullOrBlank(assetPath)) {
							AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceSynchronousImport);
							b = true;
						}
						break;
					}
				default:
					break;
			}
			return b;
		}
		#endif

		//		public UnityEngine.Object Content(Type type) {
		//			if (!Exists) {
		//				return null;
		//			}
		//			UnityEngine.Object content = null;
		//			switch (mType) {
		//				case FileType.Resources:
		//					{
		//						content = Resources.Load(mPath, type);
		//						break;
		//					}
		//				case FileType.Path:
		//				case FileType.Data:
		//				case FileType.PersistentData:
		//					{
		//						if (TypeUtils.Equals(type, typeof(Texture2D))) {
		//							Texture2D tex = texture2DFromPath(RealPath);
		//							content = tex;
		//						} else if (TypeUtils.Equals(type, typeof(Sprite))) {
		//							Texture2D tex = texture2DFromPath(RealPath);
		//							if (tex != null) {
		//								// 使用纹理创建Sprite，设置PixelsPerUnit = 100
		//								Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 100.0f);
		//								content = sprite;
		//							}
		//						}
		//						break;
		//					}
		//				default:
		//					break;
		//			}
		//			return content;
		//		}

		public T GetContent<T>(bool async, OnFileGetContentDelegate<T> onGetContent, OnFileGetContentProgressDelegate onProgress) where T : UnityEngine.Object {
			// TODO 异步实现
			if (!Exists) {
				return null;
			}
			T content = null;
			switch (mType) {
				case FileType.Resources:
					if (!async) {
						// NOTE 加载sprite的时候记得把资源转换为sprite
						content = Resources.Load<T>(mPath);
						if (content == null){
							return null;
						}
						if (TypeUtils.Equals<T, GameObject>()) {
							var gameObject = GameObject.Instantiate(content) as GameObject;
							gameObject.transform.position = Vector3.zero;
							content = gameObject as T;
						}
					} else {
						var asyncTaskCaller = AsyncTask<T>.GlobalCaller;
						asyncTaskCaller.StartCoroutine(LoadResourceAsync<T>(mPath, onGetContent, onProgress));
					}
					break;
				case FileType.Path:
				case FileType.Data:
				case FileType.PersistentData:
					if (TypeUtils.Equals<T, Texture2D>()) {
						var tex = texture2DFromPath(RealPath);
						content = tex as T;
					} else if (TypeUtils.Equals<T, Sprite>()) {
						var tex = texture2DFromPath(RealPath);
						if (tex != null) {
							// 使用纹理创建Sprite，设置PixelsPerUnit = 100
							Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), Vector2.zero, 100.0f);
							content = sprite as T;
						}
					}
					break;
				default:
					break;
			}
			if (!async && onGetContent != null) {
				onGetContent(content);
			}
			return content;
		}

		public Scene GetScene(bool async, OnFileGetSceneDelegate onGetScene, OnFileGetSceneProgressDelegate onProgress) {
			Scene scene = new Scene();
			if (mType != FileType.Scene) {
				return scene;
			}
			if (!async) {
				SceneManager.LoadScene(mPath, LoadSceneMode.Additive);
				scene = SceneManager.GetSceneByName(mPath);
				if (onGetScene != null) {
					onGetScene(scene);
				}
			} else {
				var asyncTaskCaller = AsyncTask<Scene>.GlobalCaller;
				asyncTaskCaller.StartCoroutine(LoadSceneAsync(mPath, onGetScene, onProgress));
			}
			return scene;
		}

		IEnumerator LoadResourceAsync<T>(string path, OnFileGetContentDelegate<T> onGetContent, OnFileGetContentProgressDelegate onProgress) where T : UnityEngine.Object {
			var result = Resources.LoadAsync<T>(mPath);
			while (!result.isDone) {
				yield return null;
				if (onProgress != null) {
					onProgress(result.progress); // 0.0 ~ 1.0
				}
			}
			yield return null;
			if (onGetContent != null) {
                if (result.asset == null) {
                    onGetContent (null);
                } else {
                    if (TypeUtils.Equals<T, GameObject> ()) {
						var gameObject = GameObject.Instantiate(result.asset) as GameObject;
						gameObject.transform.position = Vector3.zero;
						T content = gameObject as T;
                        onGetContent (content);
                    } else {
                        T content = result.asset as T;
                        onGetContent (content);
                    }
                }
			}
		}

		IEnumerator LoadSceneAsync(string name, OnFileGetSceneDelegate onGetScene, OnFileGetSceneProgressDelegate onProgress) {
			AsyncOperation result = null;
			try {
				result = SceneManager.LoadSceneAsync(name, LoadSceneMode.Additive);
			} catch (Exception) {
				if (onGetScene != null) {
					var scene = new Scene();
					onGetScene(scene);
				}
				yield break;
			}
			while (!result.isDone) {
				yield return null;
				if (onProgress != null) {
					onProgress(result.progress); // 0.0 ~ 1.0
				}
			}
			yield return null;
			if (onGetScene != null) {
				var scene = SceneManager.GetSceneByName(name);
				onGetScene(scene);
			}
		}

		private Texture2D texture2DFromPath(string path) {
#if UNITY_EDITOR
			path = PathUtils.GetProjectRelativePath(path);
			var tex = AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;
			if (tex == null) {
				tex = createTexture2D(path);
			}
			return tex;
#else
			return createTexture2D(path);
#endif
		}

		private Texture2D createTexture2D(string path) {
			byte[] fileData = System.IO.File.ReadAllBytes(RealPath);
			Texture2D tex = new Texture2D(2, 2); // 创建一个空纹理
			if (tex.LoadImage(fileData)) {
				return tex;
			} else {
				Debug.Log("[File] Cannot load a texture (" + RealPath + ").");
				GameObject.Destroy(tex);
			}
			return null;
		}

		//		public UnityEngine.AssetBundle AssetBundle {
		//			get {
		//				if (!Exists) {
		//					return null;
		//				}
		//				UnityEngine.AssetBundle assetBundle = null;
		//				switch (mType) {
		//					case FileType.Resources: {
		//						// TODO
		//						break;
		//					}
		//					case FileType.Path:
		//					case FileType.PersistentData: {
		//						assetBundle = AssetBundle.LoadFromFile(RealPath);
		//						break;
		//					}
		//					default:
		//						break;
		//				}
		//				return assetBundle;
		//			}
		//		}

		public object Extra {
			get {
				return mExtra;
			}
			set {
				mExtra = value;
			}
		}

		public IFile Instantiate() {
			var file = new File(mType, mPath, mIsDir);
			return file;
		}

		public override bool Equals(object obj) {
			if (obj == null) {
				return false;
			}
			File file = obj as File;
			if (mType != file.Type) {
				return false;
			}
			bool b = false;
			switch (mType) {
				case FileType.Data:
				case FileType.Path:
				case FileType.PersistentData:
				case FileType.Resources:
					{
						b = RealPath == file.RealPath;
						break;
					}
				default:
					break;
			}
			return b;
		}

		public override int GetHashCode() {
			// TODO
			return base.GetHashCode();
		}
	}
}