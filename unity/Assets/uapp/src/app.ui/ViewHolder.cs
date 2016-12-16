/**
 * @file ViewHolder.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class ViewHolder<T> : IViewHolder<T> {

		protected ListView<T> mListView;
		protected int mPosition;
        private ListAdapter<T>.OnItemClickDelegate mOnItemClick;
        private ListAdapter<T>.OnItemEventDelegate mOnItemEvent;
		private ListItem<T> mListItem;

		public virtual GameObject OnCreateView(int viewType, GameObject template, GameObject parent) {
			if (template != null) {
				GameObject view = GameObject.Instantiate(template) as GameObject;
				view.SetActive(true);
				mListItem = view.GetComponent(typeof(IListItem)) as ListItem<T>;
				if (mListItem != null) {
					mListItem.ListView = mListView;
					mListItem.OnItemClick = mOnItemClick;
					mListItem.OnItemEvent = mOnItemEvent;
				}
				return view;
			}
			return null;
		}

		public virtual void SetRecord(T record) {
			if (mListItem != null) { // 如果有ListItem，使用其来处理item操作
				mListItem.Position = mPosition;
				mListItem.SetRecord(record);
			}
		}

		public ListView<T> ListView {
			get {
				return mListView;
			} set {
				mListView = value;
			}
		}

		public int Position {
			get {
				return mPosition;
			} set {
				mPosition = value;
			}
		}

		public ListAdapter<T>.OnItemClickDelegate OnItemClick {
			get {
				return mOnItemClick;
			} set {
                mOnItemClick = value;
				if (mListItem != null) {
					mListItem.OnItemClick = mOnItemClick;
				}
			}
		}

		public ListAdapter<T>.OnItemEventDelegate OnItemEvent {
			get {
				return mOnItemEvent;
			} set {
                mOnItemEvent = value;
				if (mListItem != null) {
					mListItem.OnItemEvent = mOnItemEvent;
				}
			}
		}

		public void OnSelect(bool selected) {
			if (mListItem != null) {
				mListItem.OnSelect(selected);
			}
		}
	}
}