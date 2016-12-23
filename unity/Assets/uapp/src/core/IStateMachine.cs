/**
 * @file IStateMachine.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public delegate void OnState(IState state);

	public interface IStateMachine {

		string Name { get; }
		string InitState { get; set; }
		void AddState(string name, IState state);
		IState RemoveState(string name);
		IState GetState(string name);
		IState CurrentState { get; }
		bool ChangeState(string name, bool pushState);
		bool ChangeState(string name);
		bool RevertState(bool pushState);
		bool RevertState();

		void EnumState(OnState onState);
	}

}
