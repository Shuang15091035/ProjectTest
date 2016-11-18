/**
 * @file Command.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class Command : ICommand {
		public virtual void Todo() {

		}

		public virtual void Redo() {
			Todo();
		}

		public virtual void Undo() {

		}

	}

}