/**
 * @file IFile.cs
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
using System.IO;

namespace uapp {

	public enum FileType {
		Unknown = 0,
		Path, //
		Resources, // Editor中Assets内名为Resources的文件夹内的文件，使用Resources.Load方式加载，路径需要忽略文件后缀名
		Data, // 程序的数据文件所在文件夹的路径，例如在Editor中就是Assets了
		Scene, // unity的场景文件
		StreamingAssets, // 流数据的缓存目录，返回路径为相对路径，适合设置一些外部数据文件的路径
		PersistentData, // 一个持久化数据存储目录的路径，可以在此路径下存储一些持久化的数据文件
		TemporaryCache, // 一个临时数据的缓存目录
		Url, // Url
	};

	public delegate void OnFileEnumDelegate(IFile file);
	public delegate void OnFileGetContentDelegate<T>(T content) where T : UnityEngine.Object;
	public delegate void OnFileGetContentProgressDelegate(float progress);
	public delegate void OnFileGetSceneDelegate(Scene scene);
	public delegate void OnFileGetSceneProgressDelegate(float progress);

	public interface IFile {
		// 文件类型
		FileType Type { get; set; }

		// 路径相关
		string Path { get; set; }
		string RealPath { get; }
		string FileName { get; }
		string BaseName { get; }
		string Ext { get; }

		// 属性相关
		bool IsDir { get; set; }
		IFile Dir { get; }

		// 子文件
		IFile RelFile(string relPath, bool isDir = false);

		// 文件操作相关
		bool Exists { get; }
		bool Create(bool overrideIfExists = true);
        bool Delete ();
		bool OpenInput();
		bool OpenOutput();
		void Close();
		FileStream FileStream { get; }
		bool Copy(IFile src, bool overrideIfExists = true, string[] patterns = null, bool recursive = false);

		// 文件内容相关
		bool EnumFiles(OnFileEnumDelegate onFileEnum, string[] patterns = null, bool recursive = false);
		IList<IFile> ListFiles(string[] patterns = null, bool recursive = false);
		string Text { get; }
		string TextWithEncoding(Encoding encoding);
		bool WriteText(string text);
		bool WriteTextWithEncoding(string text, Encoding encoding);
		byte[] Bytes { get; set; }

		// 获取内容
		T GetContent<T>(bool async = false, OnFileGetContentDelegate<T> onGetContent = null, OnFileGetContentProgressDelegate onProgress = null) where T : UnityEngine.Object;
		Scene GetScene(bool async = false, OnFileGetSceneDelegate onGetScene = null, OnFileGetSceneProgressDelegate onProgress = null);

		// Unity Editor相关
		#if UNITY_EDITOR
		bool Import();
		//UnityEngine.AssetBundle AssetBundle { get; }
		#endif

		// 附加数据
		object Extra { get; set; }

		// 复制实例
		IFile Instantiate();
	}
}