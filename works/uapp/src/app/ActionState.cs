/**
 * @file ActionBehaviour.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public abstract class ActionState : AppState {

		public abstract void onAction();

		public sealed override void OnStateIn() {
			base.OnStateIn();
			onAction();
			app.AppMachine.RevertState();
		}
	}
}