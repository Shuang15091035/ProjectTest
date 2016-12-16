/**
 * @file ObjectListItem.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-9-19
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class ObjectListItem : ListItem<object> {

		public override void SetRecord (object record) {
			throw new UnityException("ObjectListItem is a place holder, Please replace by your own ListItem.");
		}

	}
}
