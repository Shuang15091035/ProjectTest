/**
 * @file IViewHolder.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public interface IViewHolder<T> {

		GameObject OnCreateView(int viewType, GameObject template, GameObject parent);
		void SetRecord(T record);

		ListView<T> ListView { get; set; }
		int Position { get; set; }
        ListAdapter<T>.OnItemClickDelegate OnItemClick { get; set; }
        ListAdapter<T>.OnItemEventDelegate OnItemEvent { get; set; }
		void OnSelect(bool selected);
	}
}