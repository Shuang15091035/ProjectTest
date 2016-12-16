/**
 * @file State.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {

	public class State : BaseBehaviour, IState {

		protected IStateMachine mParentMachine;
		protected IStateMachine mSubMachine;

		public virtual void OnCreate() {
			
		}

		public virtual void OnDelete() {
			
		}

		public string StateName {
			get {
				return gameObject.name;
			} set {
				gameObject.name = value;
			}
		}

        public IState ParentState {
            get {
				StateMachine parentMachine = ParentMachine as StateMachine;
                if (parentMachine != null) {
					var parentObject = parentMachine.gameObject;
					var parentState = parentObject.GetComponent<IState>();
					return parentState;
                }
                return null;
            }
        }

		public IStateMachine ParentMachine {
			get {
				return mParentMachine;
			} set {
				mParentMachine = value;
			}
		}

		public IStateMachine SubMachine {
			get {
				if (mSubMachine == null) {
					mSubMachine = gameObject.AddComponent(typeof(StateMachine)) as IStateMachine;
				}
				return mSubMachine;
			}
		}

		public virtual bool OnPreCondition() {
			return true;
		}

		public virtual void OnStateIn() {
			this.enabled = true;
			// NOTE 延时调用，由StateMachine调用
//			if (mSubMachine != null) {
//				var initState = mSubMachine.InitState;
//				if (initState != null) {
//					mSubMachine.ChangeState(initState);
//					mSubMachine.InitState = null;
//				}
//			}
		}

		public virtual void OnStateOut() {
			this.enabled = false;
		}

        public virtual bool OnPostCondition() {
            return true;
        }

		public void changeInitState() {
			if (mSubMachine != null) {
				var initState = mSubMachine.InitState;
				if (initState != null) {
					mSubMachine.ChangeState(initState);
					mSubMachine.InitState = null;
				}
			}
		}
	}
}
