/**
 * @file CommandMachine.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public class CommandMachine : MonoBehaviour, ICommandMachine {
		public const int MaxCommandsDefault = 100;
		private IList<ICommand> mCommandStack;
		private int mCurrentCommand = -1;

		public int MaxCommands {
			get {
				if (mCommandStack == null) {
					return 0;
				}
				return mCommandStack.Count;
			}
			set {
				if (mCommandStack == null) {
					mCommandStack = new List<ICommand>(value);
					for (int i = 0; i < value; i++) {
						mCommandStack.Add(null);
					}
				} else {
					var newCommandStack = new List<ICommand>(value);
					int commandsToCopy = value < mCommandStack.Count ? value : mCommandStack.Count;
					for (int i = 0; i < commandsToCopy; i++) {
						newCommandStack.Add(mCommandStack[i]);
					}
					mCommandStack.Clear();
					mCommandStack = newCommandStack;
				}
			}
		}

		public bool Todo(ICommand command, bool canUndo) {
			if (command == null) {
				return false;
			}
			if (MaxCommands == 0) {
				MaxCommands = MaxCommandsDefault;
			}
			if (mCurrentCommand == mCommandStack.Count - 1) { // command stack full
				return false;
			}
			
			Debug.Log("todo command[" + command.GetType() + "].");
			command.Todo();

			if (canUndo) {
				mCurrentCommand++;
				mCommandStack[mCurrentCommand] = command;
			}
			for (int i = mCurrentCommand + 1; i < mCommandStack.Count; i++) {
				mCommandStack[i] = null;
			}
			return true;
		}

		public bool Todo(ICommand command) {
			return Todo(command, true);
		}

		public bool Undo() {
			if (MaxCommands == 0) {
				MaxCommands = MaxCommandsDefault;
			}
			if (!CanUndo) {
				return false;
			}

			ICommand command = mCommandStack[mCurrentCommand];
			Debug.Log("uodo command[" + command.GetType() + "].");
			command.Undo();
			mCurrentCommand--;
			return true;
		}

		public bool Redo() {
			if (MaxCommands == 0) {
				MaxCommands = MaxCommandsDefault;
			}
			if (!CanRedo) {
				return false;
			}

			mCurrentCommand++;
			ICommand command = mCommandStack[mCurrentCommand];
			Debug.Log("redo command[" + command.GetType() + "].");
			command.Redo();
			return true;
		}

		public bool CanUndo {
			get {
				return mCurrentCommand >= 0;
			}
		}

		public bool CanRedo {
			get {
				return mCurrentCommand < MaxCommands - 1;
			}
		}
	}

}
