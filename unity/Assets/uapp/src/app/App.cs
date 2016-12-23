/**
 * @file App.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using System.Collections;

namespace uapp {
	/**
	* 全局App对象(单件),子类通过派生使用
	*/
	public class App : AppState {

		private static App instance;
		private ICommandMachine mCommandMachine;

		public static App Instance {
			get {
				return instance;
			}
		}

		public static GameObject Canvas {
			get {
				return instance.gameObject;
			}
		}

		public IStateMachine AppMachine {
			get {
				return SubMachine;
			}
		}

		public ICommandMachine CommandMachine {
			get {
				if (mCommandMachine == null) {
					mCommandMachine = GetComponent(typeof(ICommandMachine)) as ICommandMachine;
					if (mCommandMachine == null) {
						throw new UnityException("Cannot find CommandMachine in App, Please Add One.");
					}
				}
				return mCommandMachine;
			}
		}

		protected override void OnAwake() {
			base.OnAwake();
			instance = this;
			buildState(gameObject, AppMachine);
		}

		public override void OnStateIn() {
			base.OnStateIn();
			// NOTE 需要立即调用
			changeInitState();
		}

		private IState buildState(GameObject gameObject, IStateMachine stateMachine) {
			if (gameObject == null) {
				return null;
			}
			IState state = gameObject.GetComponent<State>();
//            if (state != null) {
//                if (gameObject == this.gameObject) {
//                    OnCreate ();
//                    OnStateIn ();
//                } else {
//                    var stateName = state.StateName;
//                    stateMachine.AddState (stateName, state);
//                    state.OnCreate ();
//                    Debug.Log ("State \"" + stateName + "\" in " + stateMachine.Name + " has Created.");
//                }
//            }
			if (state != null) {
				if (gameObject == this.gameObject) {
					OnCreate ();
				} else {
					var stateName = state.StateName;
					stateMachine.AddState (stateName, state);
					state.OnCreate ();
					Debug.Log ("State \"" + stateName + "\" in " + stateMachine.Name + " has Created.");
				}
			}

			string initState = null;
			var subStateMachine = state != null ? state.SubMachine : stateMachine;
			foreach (Transform childTransform in gameObject.transform) {
				GameObject childObject = childTransform.gameObject;
				var subState = buildState(childObject, subStateMachine);
				if (subState == null) {
					continue;
				}
				if (initState == null) {
					initState = subState.StateName;
				}
			}
			if (initState != null) {
//				subStateMachine.ChangeState(initState);
				subStateMachine.InitState = initState; // NOTE 延时转换
			}

			if (state != null) {
				if (gameObject == this.gameObject) {
					OnStateIn ();
				}
			}

			return state;
		}

//		protected override void OnAwake () {
//			base.OnAwake ();
//			string initStateName = null;
//			foreach (Transform child in transform) {
//				GameObject childObject = child.gameObject;
//				IState state = childObject.GetComponent<State>();
//				if (state != null) {
//					if (state is AppState) {
//						AppState s = (AppState)state;
//						s.OnCreate();
//						Debug.Log("State[" + state.StateName +"] Created.");
//						subMachine(s);
//					}
//					if (initStateName == null) {
//						initStateName = state.StateName;
//					}
//					AppMachine.AddState(state.StateName, state);
//				}
//			}
//			mView = OnCreateView();
//			if (initStateName == null) {
//				Debug.LogWarning("No States Found, Really?");
//			} else {
//				AppMachine.ChangeState(initStateName);
//			}
//		}
//
//		private void subMachine(AppState parentState) {
//			foreach (Transform child in parentState.transform) {
//				GameObject childObject = child.gameObject;
//				IState state = childObject.GetComponent<State>();
//				if (state != null) {
//					if (state is AppState) {
//						AppState s = (AppState)state;
//						s.OnCreate();
//						Debug.Log("State[" + state.StateName +"] Created.");
//					}
//					parentState.SubMachine.AddState(state.StateName, state);
//				}
//			}
//		}
	}
}