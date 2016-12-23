/**
 * @file ICommand.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public interface ICommand {
		void Todo();
		void Redo();
		void Undo();
	}

}
