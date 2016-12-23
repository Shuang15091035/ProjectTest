using UnityEngine;
using System;
using System.Collections;

namespace uapp {
	
	public class FixKeyLicenseTest : MonoBehaviour {

		void Start () {
			var license = GetComponent<FixKeyLicense>();
			license.AddKey("a", DateTime.Now + new TimeSpan(0, 0, 10));
			license.StartOnDate(DateTime.Now);
		}

	}

}
