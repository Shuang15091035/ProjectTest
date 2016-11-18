using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public interface IArchitecture : IObject {

		float Scale { get; set; } // NOTE 比例尺，代表物件放到此场景中需要缩放的数值
		IEnumerable<IRoom> Rooms { get; }
		GameObject Root { get; }
	}

	public class Architecture : UObject, IArchitecture {

		private float mScale = 1.0f;
		private List<IRoom> mRooms;
		private GameObject mRoot;

		public Architecture() {
			mRoot = new GameObject("Arch");
		}

		public float Scale {
			get {
				return mScale;
			}
			set {
				mScale = value;
			}
		}

		public IEnumerable<IRoom> Rooms {
			get {
				return mRooms;
			}
		}

		public GameObject Root {
			get {
				return mRoot;
			}
		}

		public void AddRoom(IRoom room) {
			if (room == null) {
				return;
			}
			if (mRooms == null) {
				mRooms = new List<IRoom>();
			}
			mRooms.Add(room);
		}
	}
}
