/**
 * @file StateMachine.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public class StateMachine : MonoBehaviour, IStateMachine {

		private Dictionary<string, IState> mStates;
		private IState mCurrentState;
		private IList<IState> mStateStack;
		private string mInitState;

		~StateMachine() {
			if (mStates != null) {
				mStates.Clear();
				mStates = null;
			}
		}

		public string Name {
			get {
				return "SubMachine@" + gameObject.name;
			}
		}

		public string InitState {
			get {
				return mInitState;
			}
			set {
				mInitState = value;
			}
		}

		public void AddState(string name, IState state) {
			if (mStates == null) {
				mStates = new Dictionary<string, IState>();
			}
			if (mStates.ContainsKey(name)) {
				return;
			}
			State s = (State)state;
			s.ParentMachine = this;
			mStates.Add(name, state);
		}

		public IState RemoveState(string name) {
			if (mStates == null)
				return null;

			IState state = mStates[name];
			if (state == null)
				return null;

			mStates.Remove(name);
			if (mCurrentState == state)
				mCurrentState = null;
			State s = (State)state;
			s.ParentMachine = null;
			return state;
		}

		public IState GetState(string name) {
			if (mStates == null)
				return null;
			if (!mStates.ContainsKey(name))
				return null;
			return mStates[name];
		}

		public IState CurrentState {
			get {
				return mCurrentState;
			}
		}

		public bool ChangeState(string name, bool pushState) {
			if (mStates == null)
				return false;
			IState from = mCurrentState;
			IState to = null;
			if (name != null) {
				if (!mStates.ContainsKey(name)) {
					Debug.LogWarning("There is no such state Named \"" + name + "\", Please Add One.");
					return false;
				}
				to = mStates[name];
			}
			return changeStateImpl(from, to, pushState);
		}

		public bool ChangeState(string name) {
			return ChangeState(name, true);
		}

		private bool changeStateImpl(IState fromState, IState toState, bool pushState) {
			if (fromState == toState) {
				return false;
			}

			string fromStateName = (fromState == null ? "null" : fromState.StateName);
            if (fromState != null && !fromState.OnPostCondition()) {
                Debug.Log ("PostCondition failed on " + fromStateName + ", Stay in " + fromStateName + ".");
                return false;
            }
			if (toState != null && !toState.OnPreCondition()) {
				Debug.Log("PreCondition failed on " + toState.StateName + ", Stay in " + fromStateName + ".");
				return false;
			}

			if (fromState != null) {
				fromState.OnStateOut();
//				if (pushState) {
//					if (mStateStack == null) {
//						mStateStack = new List<IState>();
//					}
//					mStateStack.Add(fromState);
//				}
			}
			// NOTE 允许将null状态放入历史状态
			if (pushState) {
				if (mStateStack == null) {
					mStateStack = new List<IState>();
				}
				mStateStack.Add(fromState);
			}

			mCurrentState = toState;

			string toStateName = (toState == null ? "null" : toState.StateName);
			Debug.Log("[" + Name + "] Change state from " + fromStateName + " to " + toStateName + ".");

			if (toState != null) {
				toState.OnStateIn();
				var ts = toState as State;
				ts.changeInitState();
			}

			return true;
		}

		public bool RevertState(bool pushState) {
			return revertStateImpl(1, pushState);
		}

		public bool RevertState() {
			return revertStateImpl(1, false);
		}

		private bool revertStateImpl(int step, bool pushState) {
			if (mStateStack == null || mStateStack.Count == 0) {
				return false;
			}

			IState fromState = mCurrentState;
			IState toState = null;
			int i = 0;
			while (i < step) {
				int li = mStateStack.Count - 1;
				toState = (IState)mStateStack[li];
				mStateStack.RemoveAt(li);
				i++;
			}
			return changeStateImpl(fromState, toState, pushState);
		}

		public void EnumState(OnState onState) {
			if (mStates == null || onState == null) {
				return;
			}
			foreach (var state in mStates.Values) {
				onState(state);
			}
		}

		//	protected void Start()
		//	{
		//		Component[] states = GetComponents(typeof(State));
		//		foreach(Component comp in states)
		//		{
		//			State state = (State)comp;
		//			state.enabled = false;
		//			state.onCreate();
		//			addState(state.stateName, state);
		//			if(initialState == null || initialState.Length == 0)
		//				initialState = state.stateName;
		//		}
		//		changeState(initialState);
		//	}

	}

}