/**
 * @file IFileTable.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace uapp {

	public interface OnFileTableLoadListener<T> {
		void OnLoadRecord(T record);
		void OnLoadFileTable(IList<T> records);
	}

	public interface OnFileTableSaveListener<T> {
		void OnSaveRecord(T record);
		void OnSaveFileTable(Exception e);
	}

	public interface IFileTable<T> {
		string Separator { get; set; }
		Encoding Encoding { get; set; }

        IList<T> LoadFile(IFile file);
        AsyncResult<IList<T>> LoadFile(IFile file, bool async, OnFileTableLoadListener<T> listener);
		Exception SaveFile(IFile file, IList<T> records);
		AsyncResult<Exception> SaveFile(IFile file, IList<T> records, bool async, OnFileTableSaveListener<T> listener);
	}

	public class BaseOnFileTableLoadListener<T> : OnFileTableLoadListener<T> {
		public delegate void OnLoadRecordDelegate(T record);
        public delegate void OnLoadFileTableDelegate(IList<T> records);

		private OnLoadRecordDelegate mOnLoadRecordEvent;
		private OnLoadFileTableDelegate mOnLoadFileTableEvent;

		public OnLoadRecordDelegate OnLoadRecordEvent {
			get {
				return mOnLoadRecordEvent;
			} set {
				mOnLoadRecordEvent = value;
			}
		}

		public OnLoadFileTableDelegate OnLoadFileTableEvent {
			get {
				return mOnLoadFileTableEvent;
			} set {
				mOnLoadFileTableEvent = value;
			}
		}

		public void OnLoadRecord(T record) {
			if (mOnLoadRecordEvent != null) {
				mOnLoadRecordEvent(record);
			}
		}

		public void OnLoadFileTable(IList<T> records) {
			if (mOnLoadFileTableEvent != null) {
				mOnLoadFileTableEvent(records);
			}
		}
	}
}

