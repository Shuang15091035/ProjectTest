/**
 * @file ListAdapter.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public class ListAdapter<T> : Adapter<T> {

        public delegate void OnItemClickDelegate(T item, int position, GameObject view, ListView<T> listView);
        public delegate void OnItemEventDelegate(int what, T item, int position, GameObject view, ListView<T> listView);

        protected ListView<T> mListView;
		protected IList<T> mData;
		protected IList<IViewHolder<T>> mHolders;
		protected IList<GameObject> mItemTemplates;
		protected OnItemClickDelegate mOnItemClick;
		protected OnItemEventDelegate mOnItemEvent;

		protected virtual IViewHolder<T> OnCreateViewHolder() {
			return new ViewHolder<T>();
		}

		public IList<T> Data {
			get {
				return mData;
			} set {
				mData = value;
			}
		}

		public IList<GameObject> ItemTemplates {
			get {
				return mItemTemplates;
			} set {
				mItemTemplates = value;
				if (mItemTemplates != null) { // 隐藏模板
					foreach (var template in mItemTemplates) {
						template.SetActive(false);
					}
				}
			}
		}

		public override int GetCount () {
			if (mData == null) {
				return 0;
			}
			return mData.Count;
		}

		public override T GetItemAt (int position) {
			if (mData == null) {
				return default(T);
			}
			return mData[position];
		}

		public override int GetViewTypeCount () {
			if (mItemTemplates == null) {
				return base.GetViewTypeCount();
			}
			return mItemTemplates.Count;
		}

		protected GameObject getItemTemplateByViewType(int viewType) {
			if (mItemTemplates == null) {
				return null;
			}
			return mItemTemplates[viewType];
		}

		public override GameObject GetViewAt (int position, GameObject parent) {
			GameObject view = null;
			IViewHolder<T> holder = null;
			// TODO reuse view
			{
				holder = getViewHolderAt(position);
				int viewType = GetViewTypeAt(position);
				GameObject template = getItemTemplateByViewType(viewType);
				holder.ListView = mListView;
				holder.OnItemClick = mOnItemClick;
				holder.OnItemEvent = mOnItemEvent;
				view = holder.OnCreateView(viewType, template, parent);
			}

			T record = GetItemAt(position);
			if (!Object.Equals(record, default(T))) {
				holder.SetRecord(record);
			}
			return view;
		}

		protected IViewHolder<T> getViewHolderAt(int position) {
			if (mHolders == null) {
				mHolders = new List<IViewHolder<T>>();
			}
			IViewHolder<T> found = null;
			foreach(var holder in mHolders) {
				if (holder.Position == position) {
					found = holder;
					break;
				}
			}
			if (found != null) {
				return found;
			}
			found = OnCreateViewHolder();
			found.Position = position;
			mHolders.Add(found);
			return found;
		}

		public ListView<T> ListView {
			get {
				return mListView;
			} set {
				mListView = value;
			}
		}

		public override void NotifyDataSetChanged () {
			// TODO
			base.NotifyDataSetChanged ();
		}

		public OnItemClickDelegate OnItemClick {
			get {
				return mOnItemClick;
			} set {
                mOnItemClick = value;
				if (mHolders != null) {
					foreach(var holder in mHolders) {
						holder.OnItemClick = mOnItemClick;
					}
				}
			}
		}

		public OnItemEventDelegate OnItemEvent {
			get {
				return mOnItemEvent;
			} set {
				mOnItemEvent = value;
				if (mHolders != null) {
					foreach(var holder in mHolders) {
						holder.OnItemEvent = mOnItemEvent;
					}
				}
			}
		}

		public void SetItemSelected(int position) {
			if (mHolders == null) {
				return;
			}
			for (int i = 0; i < mHolders.Count; i++) {
				var holder = mHolders[i];
				if (i == position) {
					holder.OnSelect(true);
				} else {
					holder.OnSelect(false);
				}
			}
		}
	}
}