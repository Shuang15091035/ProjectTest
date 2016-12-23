/**
 * @file ICommandMachine.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public interface ICommandMachine {
		int MaxCommands { get; set; }
		bool Todo(ICommand command, bool canUndo);
		bool Todo(ICommand command);
		bool Undo();
		bool Redo();
		bool CanUndo { get; }
		bool CanRedo { get; }
	}

}
