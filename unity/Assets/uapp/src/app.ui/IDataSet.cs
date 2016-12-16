/**
 * @file IDataSet.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-11
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public interface IDataSet {

		void RegisterDataSetObserver(IDataSetObserver observer);
		void UnregisterDataSetObserver(IDataSetObserver observer);
		void NotifyDataSetChanged();
	}

	public interface IDataSetObserver {

		void OnDataSetChanged(IDataSet dataSet);
	}
}