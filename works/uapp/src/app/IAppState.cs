using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

namespace uapp {
	
	public interface IAppState : IState {

		void BroadcastMessage(int what, Type type, object data);
		void ReportMessage(int what, Type type, object data);
		void OnMessage(int what, Type type, object data);

	}

}
