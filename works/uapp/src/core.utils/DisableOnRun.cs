using UnityEngine;
using System.Collections;

namespace uapp {
	
	public class DisableOnRun : MonoBehaviour {

		void Start() {
			gameObject.SetActive(false);
		}

	}
}
