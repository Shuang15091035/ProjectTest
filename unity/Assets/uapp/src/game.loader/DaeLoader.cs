/**
 * @file DaeLoader.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-9-15
 * @brief
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace uapp {

	public class DaeLoadTask : AsyncTask<GameObject> {
		private IFile mFile;
		private GameObject mParent;
		private OnSceneLoadListener mListener;
		//private DaeLoader mDaeLoader;
		private float mStartTime;

		public DaeLoadTask(IFile file, GameObject parent, OnSceneLoadListener listener, ISceneLoader sceneLoader) {
			mFile = file;
			mParent = parent;
			mListener = listener;
			//mDaeLoader = sceneLoader as DaeLoader;
		}

		public override void OnPreExecute() {
			mStartTime = Time.time;
			if (mFile == null || !mFile.OpenInput()) {
				cancel();
			}
		}

		public override IEnumerator DoInBackground() {
			if (mListener != null) {
				mListener.OnSceneLoadingProgress(0.0f);
			}
			XmlSerializer dae = new XmlSerializer(typeof(COLLADA));
			FileStream fs = mFile.FileStream;
			COLLADA collada = dae.Deserialize(fs) as COLLADA;
			mFile.Close();
			COLLADAForLoad cl = new COLLADAForLoad(collada, mFile, mListener, this);
			if (Debug.isDebugBuild) {
				Debug.Log("Load File: " + mFile.RealPath);
				Debug.Log("XML Time: " + (Time.time - mStartTime));
				mStartTime = Time.time;
			}
			IEnumerator e = cl.Build(mParent);
			while (e.MoveNext()) {
				yield return e.Current;
			}
			if (Debug.isDebugBuild) {
				Debug.Log("Collada Time: " + (Time.time - mStartTime));
			}
			Return(cl.Object);
		}

		public override void OnPostExecute(GameObject result) {
			if (result != null) {
				if (mListener != null) {
					mListener.OnSceneLoadingProgress(1.0f);
					mListener.OnSceneFinishLoaded(result, mParent, mFile);
				}
			} else {
				if (mListener != null) {
					mListener.OnSceneFailToLoad(mParent, mFile, new Exception("DaeError"));
				}
			}
			base.OnPostExecute(result);
		}
	}

	public class DaeSaveTask : AsyncTask<bool> {
		private IFile mFile;
		private GameObject[] mObjects;
		private OnSceneSaveListener mListener;
		
		public DaeSaveTask(IFile file, GameObject[] objects, OnSceneSaveListener listener, ISceneLoader sceneLoader) {
			mFile = file;
			mObjects = objects;
			mListener = listener;
		}
		
		public override void OnPreExecute() {
			if (mFile == null || !mFile.OpenOutput()) {
				cancel();
			}
		}
		
		public override IEnumerator DoInBackground() {
			XmlSerializer dae = new XmlSerializer(typeof(COLLADA));
			FileStream fs = mFile.FileStream;
			COLLADAForSave cs = new COLLADAForSave(mFile, mListener, this);
			IEnumerator e = cs.Build(mObjects);
			while (e.MoveNext()) {
				yield return e.Current;
			}
			COLLADA collada = cs.Result;
			var sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
			dae.Serialize(sw, collada);
			mFile.Close();
			Return(collada != null);
		}
		
		public override void OnPostExecute(bool result) {
			if (result) {
				if (mListener != null) {
					mListener.OnSceneFinishSaved(mObjects, mFile);
				}
			} else {
				if (mListener != null) {
					mListener.OnSceneFailToSave(mObjects, mFile, new Exception("DaeError"));
				}
			}
			base.OnPostExecute(result);
		}
	}

	public class DaeExtra {
		public bool ExportBinormals = false;
		public IFile copyTextureToDir = null;
		public bool willDoPlatformOptimization = false;
		public RuntimePlatform platformOptimization = RuntimePlatform.IPhonePlayer;
		public bool Obfuscated = false;
	}

	public class DaeLoader : SceneLoader {

		public override string Pattern {
			get {
				return FilePatterns.DAE;
			}
		}

		protected override AsyncTask<GameObject> getLoadTask(IFile file, GameObject parent, OnSceneLoadListener listener, ISceneLoader sceneLoader) {
			return new DaeLoadTask(file, parent, listener, sceneLoader);
		}

		protected override AsyncTask<bool> getSaveTask(IFile file, GameObject[] objects, OnSceneSaveListener listener, ISceneLoader sceneLoader) {
			return new DaeSaveTask(file, objects, listener, sceneLoader);
		}
	}
}