/**
 * @file AppState.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-6
 * @brief
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

namespace uapp {

	public class AppState : State, IAppState {

		protected App mApp;
		protected GameObject mView;

        private void setStateActive(bool active) {
            gameObject.SetActive (active);
            if (mView != null) {
                mView.SetActive (active);
            }
        }

		public sealed override void OnCreate() {
			base.OnCreate();
			GameObject parentView = null;
			var parentState = ParentState;
			if (parentState is AppState) {
				var parentAppState = parentState as AppState;
				parentView = parentAppState.View;
			}
			mView = OnCreateView(parentView);
            setStateActive (false);
        }

		public virtual GameObject OnCreateView(GameObject parentView) {
			// NOTE 一般使用gameObject作为一个state的view（gameObject应该是个Canvas或者空节点）
			return gameObject;
		}

		public override void OnStateIn() {
			base.OnStateIn();
            setStateActive (true);
        }

		public override void OnStateOut() {
            setStateActive (false);
            base.OnStateOut();
		}

		public GameObject View {
			get {
				return mView;
			}
		}

		protected App app {
			get {
				if (mApp == null) {
					GameObject appObject = GameObject.Find("App");
					if (appObject == null) {
						throw new UnityException("Cannot find GameObject Named \"App\".");
					}
					mApp = appObject.GetComponentInChildren<App>();
					if (mApp == null) {
						throw new UnityException("Cannot find App Component in App GameObject.");
					}
				}
				return mApp;
			}
		}

		protected T findViewById<T>(string viewId, T defaultValue = null, GameObject parent = null) where T : Component {
			if (parent == null) {
				parent = gameObject;
			}
			T view = UiUtils.FindViewById<T>(viewId, parent);
			if (view == null) {
				view = defaultValue;
			}
			return view;
		}

		public void BroadcastMessage(int what, Type type, object data) {
			if (mSubMachine == null) {
				return;
			}
			mSubMachine.EnumState((IState state) => {
				if (state is IAppState) {
					var appState = state as IAppState;
					appState.OnMessage(what, type, data);
				}
			});
		}

		public void ReportMessage(int what, Type type, object data) {
			var parentState = ParentState;
			if (parentState == null || !(parentState is IAppState)) {
				return;
			}
			var parentAppState = parentState as IAppState;
			parentAppState.OnMessage(what, type, data);
		}

		public virtual void OnMessage(int what, Type type, object data) {
			
		}

	}
}

