using UnityEngine;
using System;
using System.Collections;

namespace uapp {

	public delegate void OnTimerUpdateDelegate(ITimer timer);
	public delegate void OnTimerDateDelegate(ITimer timer);

	public interface ITimer {

		long Cycle { get; set; }
		DateTime Date{ get; set; }
		void Start();
		void Pause();
		void FixedUpdate(); // call in MonoBehaviour::FixedUpdate

		OnTimerUpdateDelegate OnUpdate{ get; set; }
		OnTimerDateDelegate OnDate{ get; set; }
	}
}