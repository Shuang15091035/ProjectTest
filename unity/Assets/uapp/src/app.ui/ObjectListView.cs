/**
 * @file ObjectListView.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-9-19
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class ObjectListView : ListView<object> {

		public override void OnDataSetChanged (IDataSet dataSet) {
			throw new UnityException("ObjectListView is a place holder, Please replace by your own ListView.");
		}
	}
}