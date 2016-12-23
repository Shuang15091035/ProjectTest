using UnityEngine;
using System.Collections;

namespace project {

	public enum EditPhase {
		Init,
		Design, // 设计阶段
		Edit,
	}

	public enum EditState {
		Normal,
		Highlighted,
		Selected,
	}
	
	public class EditItem : uapp.IObject {

		protected GameObject mGameObject;
		protected EditPhase mEditPhase = EditPhase.Design;
		protected EditState mEditState = EditState.Normal;

		protected EditItem(string name, int id, SKU sku, GameObject parent = null) {
			mGameObject = new GameObject(name + id);
			uapp.ObjectUtils.SetParent(mGameObject, parent);
			var item = Data.ItemFromObject(mGameObject);
			item.Id = id;
			item.SKU = sku;
		}

		public virtual void OnCreate() {

		}

		public virtual void OnDelete() {
			GameObject.Destroy(mGameObject);
			mGameObject = null;
		}

		public Item Item {
			get {
				return mGameObject.GetComponent<Item>();
			}
		}

		public virtual Area Area {
			get {
				var item = Data.ItemFromObject(mGameObject);
				return item.Area;
			} set {
				var item = Data.ItemFromObject(mGameObject);
				item.Area = value;
				item.AreaName = value.Name; // debug
			}
		}

		public virtual float ActualAreaSize {
			get {
				return 0.0f;
			}
		}

		public GameObject GameObject {
			get {
				return mGameObject;
			} set {
				mGameObject = value;
			}
		}

		public virtual EditState EditState {
			get {
				return mEditState;
			} set {
				mEditState = value;
			}
		}

		public virtual EditPhase EditPhase {
			get {
				return mEditPhase;
			} set {
				mEditPhase = value;
			}
		}

		public virtual bool IsPointInXZ(Vector3 point) {
			return false;
		}

	}
}
