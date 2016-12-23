using UnityEngine;
using System.Collections;

namespace uapp {

	public interface IArchElement : IObject {

		IArchitecture Arch { get; set; }
		IRoom Room { get; set; }

	}

	public class ArchElement : UObject, IArchElement {

		protected IArchitecture mArch;
		protected IRoom mRoom;

		public IArchitecture Arch {
			get {
				return mArch;
			}
			set {
				mArch = value;
			}
		}

		public IRoom Room {
			get {
				return mRoom;
			}
			set {
				mRoom = value;
			}
		}
		
	}
}
