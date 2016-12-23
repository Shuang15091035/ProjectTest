/**
 * @file ListView.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-12
 * @brief
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public class ListView<T> : BaseBehaviour, IDataSetObserver {

        //  Unity Editor
        public List<GameObject> ItemTemplates;

		public Transform ContentView;
		protected ListAdapter<T> mAdapter;

		public ListAdapter<T> Adapter {
			get {
				return mAdapter;
			} set {
				if (mAdapter != null) {
					mAdapter.UnregisterDataSetObserver(this);
					mAdapter.ListView = null;
				}
				mAdapter = value;
				if (mAdapter != null) {
					mAdapter.RegisterDataSetObserver(this);
					mAdapter.ListView = this;
					mAdapter.ItemTemplates = ItemTemplates;
				}
			}
		}

		public virtual void OnDataSetChanged(IDataSet dataSet) {
			if (ContentView == null) { // 寻找content view
				ScrollRect scrollRect = gameObject.GetComponent<ScrollRect>();
				if (scrollRect != null) {
					ContentView = scrollRect.content;
				} else {
					ContentView = gameObject.GetComponent<RectTransform>();
				}
			}
			if (ContentView == null) {
				return;
			}
			ObjectUtils.Clear(ContentView.gameObject);
			int count = mAdapter.GetCount();
			for (int i = 0; i < count; i++) { // 添加item到content中
				GameObject item = mAdapter.GetViewAt(i, ContentView.gameObject);
				if (item != null) {
					// 设置worldPositionStays为false的目的在于避免系统在转换坐标系时，对该rect transform进行修改导致错误
					// 比如scale变成(0,0,0)
					item.transform.SetParent(ContentView, false);
				}
			}
		}
	}

}

