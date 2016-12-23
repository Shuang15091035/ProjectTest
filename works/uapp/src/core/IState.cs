/**
 * @file IState.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public interface IState : IObject {

		string StateName { get; set; }

		bool OnPreCondition();

		/**
		* 状态进入时调用（避免与系统的OnStateEnter重名）
		*/
		void OnStateIn();

		/**
		* 状态退出时调用（避免与系统的OnStateLeave重名）
		*/
		void OnStateOut();

        bool OnPostCondition ();

        IState ParentState { get; }

        IStateMachine ParentMachine { get; }

        IStateMachine SubMachine { get; }
	}
}
