/**
 * @file FileTable.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace uapp {

	public class FileTable<T> : IFileTable<T> {

		public const string DefaultSeparator = "\t";
		/**
		 * NOTE 默认为读取microsoft office excel导出的unicode txt表格,使用Unicode（UTF-16）
		 */
		public static Encoding DefaultEncoding = Encoding.Unicode;
		public static T DefaultT = default(T);

		private string mSeparator = DefaultSeparator;
		private Encoding mEncoding = DefaultEncoding;
		private IFile mDir;
		private Dictionary<int, int> mColumnId2Index;

		public FileTable(IFile dir = null) {
			// 文件类型的属性值为字符串，可以设置dir通过相对路径的方式生成文件对象，也可重写getFile方法
			mDir = dir;
		}

		public string Separator {
			get {
				return mSeparator;
			} set {
				mSeparator = value;
			}
		}

		public Encoding Encoding {
			get {
				return mEncoding;
			} set {
				mEncoding = value;
			}
		}

		/**
		* 读取表格时,用于创建记录实例
		* @return
		*/
		protected virtual T newRecord() {
			return default(T);
		}

		/**
		* 读取表格时,用于获取列名对应的id(必须>=0),以便使用{@link #getColumnIndex(int)}来快速查找列id所对应的列索引
		* @param columnName
		* @return
		*/
		protected virtual int getColumnIdByName(string columnName) {
			return -1;
		}

		/**
		* 读取表格时,用于设置读取到的字符串到记录数据中
		* @param record
		* @param word
		* @param columnIndex
		*/
		protected virtual bool setRecord(T record, string word, int columnIndex) {
			return false;
		}

		/**
		* 读取表格时,用于解析文件对象的方法，需子类自行调用
		* @param record
		* @param word
		* @param columnIndex
		*/
		protected virtual IFile getFile(string word, int columnIndex) {
			if (mDir == null) {
				return null;
			}
			return mDir.RelFile(word);
		}
			
		/**
		 * 写入表格时,用于获取列数目
		 * @return
		 */
		protected virtual int getNumColumns() {
			return 0;
		}

		/**
		 * 写入表格时,用于获取记录中每个属性对应的字符串
		 * @param record
		 * @param columnIndex
		 * @return
		 */
		protected virtual string getRecord(T record, int columnIndex) {
			return null;
		}
		
		/**
		 * 写入表格时,用于获取列索引所对应的列名
		 * @param columnIndex
		 * @return
		 */
		protected virtual string getColumnNameByIndex(int columnIndex) {
			return null;
		}

		/**
		* 使用列id来获取列索引
		* @param columnId
		* @return
		*/
		protected int getColumnIndexById(int columnId) {
			if (mColumnId2Index == null) {
				return -1;
			}
			if (!mColumnId2Index.ContainsKey(columnId)) {
				return -1;
			}
			int columnIndex = mColumnId2Index[columnId];
			return columnIndex;
		}

		private Dictionary<int, int> ColumnId2Index {
			get {
				if (mColumnId2Index == null) {
					mColumnId2Index = new Dictionary<int, int>();
				}
				return mColumnId2Index;
			}
		}

        public IList<T> LoadFile(IFile file) {
			return LoadFile(file, false, null).syncResult;
		}

        public AsyncResult<IList<T>> LoadFile(IFile file, bool async, OnFileTableLoadListener<T> listener) {
            AsyncResult<IList<T>> result = new AsyncResult<IList<T>>();
			FileTableLoadTask<T> task = new FileTableLoadTask<T>(file, this, listener);
			task.execute(async);
			result.syncResult = task.Result;
			return result;
		}

		public Exception SaveFile(IFile file, IList<T> records) {
			if (file == null || records == null) {
				return new Exception("[" + GetType().Name + "] Null file or records");
			}

			int numColumns = this.getNumColumns();
			if (numColumns <= 0) {
				return null;
			}
			
			try
			{
				string text = "";
				
				// 写入属性列
				for (int i = 0; i < numColumns; i++) {
					string columnName = this.getColumnNameByIndex(i);
					if (columnName != null) {
						if (i == 0) {
							text += columnName;
						} else {
							text += this.mSeparator;
							text += columnName;
						}
					}
				}
				text += System.Environment.NewLine;
				
				// 写入记录数据
				foreach (T record in records) {
					for (int i = 0; i < numColumns; i++) {
						string word = this.getRecord(record, i);
						if (i == 0) {
							if (word != null) {
								text += word;
							}
						} else {
							if (word != null) {
								text += this.mSeparator;
								text += word;
							} else {
								text += this.mSeparator;
							}
						}
					}
					text += System.Environment.NewLine;
				}
                file.WriteTextWithEncoding(text, mEncoding);
			} catch(Exception e) {
				return e;
			}
			return null;
		}

		public AsyncResult<Exception> SaveFile(IFile file, IList<T> records, bool async, OnFileTableSaveListener<T> listener) {
			// TODO
			return null;
		}

		// 加载任务
		public sealed class FileTableLoadTask<K> : AsyncTask<IList<K>> {

			private IFile mFile;
			private FileTable<K> mFileTable;
			private OnFileTableLoadListener<K> mListener;
			public static K DefaultT = default(K);

			public FileTableLoadTask(IFile file, FileTable<K> fileTable, OnFileTableLoadListener<K> listener) {
				mFile = file;
				mFileTable = fileTable;
				mListener = listener;
			}

			public override void OnPreExecute () {
				base.OnPreExecute();
				if (mFile == null) {
					cancel();
				}
			}

			public override IEnumerator DoInBackground () {
				string text = mFile.TextWithEncoding(mFileTable.Encoding);
				if (text == null) {
					Return(null);
					yield break;
				}
				IList<K> records = new List<K>();
				string[] lines = Regex.Split(text, "\r\n|\r|\n");
				int row = 0;
				foreach (string line in lines) {
					string[] words = line.Split(new string[]{mFileTable.Separator}, System.StringSplitOptions.None);
					if (words.Length > 0) {
						int column = 0;
						if (row == 0) { // 处理属性列
							Dictionary<int, int> columnId2Index = mFileTable.ColumnId2Index;
							columnId2Index.Clear(); // 清除属性id对应关系,以免重复使用FileTable读取不同类型的文件id对应关系出现问题
							foreach (string word in words) {
								int columnId = mFileTable.getColumnIdByName(word);
								if (columnId < 0)
									columnId = column;
								columnId2Index[columnId] = column;
								column++;
							}
						} else { // 处理记录数据
							K record = DefaultT;
							bool recordOk = true;
							foreach (string word in words) {
								if (StringUtils.IsNullOrBlank(word)) {
									column++;
									continue;
								}
								if (object.Equals(record, DefaultT)) {
									record = mFileTable.newRecord();
									if (object.Equals(record, DefaultT)) {
										break;
									}
								}
								recordOk = mFileTable.setRecord(record, word, column);
								if (!recordOk) {
									break;
								}
								column++;
							}
							if (recordOk && !object.Equals(record, DefaultT)) {
								records.Add(record);
								if (mListener != null) {
									mListener.OnLoadRecord(record);
								}
								yield return null;
							}
						}
					}
					row++;
				}
				Return(records);
			}

			public override void OnPostExecute(IList<K> result) {
				base.OnPostExecute(result);
				if (mListener != null) {
					mListener.OnLoadFileTable(result);
				}
			}
		}
	}
}

