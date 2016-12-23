using UnityEngine;
using System.Collections;

namespace uapp {
	
	public interface IObject {
	
		void OnCreate();
		void OnDelete(); // NOTE 避免与MonoBehaviour.OnDestroy重名
	}

	public class UObject : IObject {

		public virtual void OnCreate() {
			
		}

		public virtual void OnDelete() {
			
		}

	}
}
