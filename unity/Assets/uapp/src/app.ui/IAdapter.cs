/**
 * @file IAdapter.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public interface IAdapter<T> : IDataSet {

		int GetCount();
		T GetItemAt(int position);
		int GetViewTypeAt(int position);
		int GetViewTypeCount();
		GameObject GetViewAt(int position, GameObject parent);
	}
}