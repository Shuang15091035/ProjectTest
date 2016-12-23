/**
 * @file ListItem.cs
 * @author Gavin Lo <luojunwen123@gmail.com>
 * @version
 * @date 2015-8-13
 * @brief
 */
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace uapp {

	public interface IListItem {
	}

	/**
	 * 用于托管ViewHolder的操作和操作ListView中的Item
	 */
	public abstract class ListItem<T> : BaseBehaviour, IListItem {

		protected ListView<T> mListView;
		protected int mPosition;
        private ListAdapter<T>.OnItemClickDelegate mOnItemClick;
        private ListAdapter<T>.OnItemEventDelegate mOnItemEvent;

        public abstract void SetRecord(T record);
		public virtual void OnSelect(bool selected) {}

		protected void listenOnClick(Button button, T record) {
			if (button == null) {
				return;
			}
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => {
				if (mOnItemClick != null) {
                    mOnItemClick(record, mPosition, gameObject, mListView);
				}
			});
		}

		protected void listenOnClick(Toggle toggle, T record) {
			if (toggle == null) {
				return;
			}
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener((bool onOff) => {
				if (onOff) {
                    if (mOnItemClick != null) {
                        mOnItemClick (record, mPosition, gameObject, mListView);
                    }
				}
			});
		}

		protected void listenOnClick(ToggleEx toggle, T record) {
			if (toggle == null) {
				return;
			}
			toggle.onValueChanged.RemoveAllListeners();
			toggle.onValueChanged.AddListener((bool onOff) => {
				ToggleEx.SetToggleOnOff(toggle, onOff);
				if (onOff) {
					if (mOnItemClick != null) {
						mOnItemClick (record, mPosition, gameObject, mListView);
					}
				}
			});
		}

		protected void sendEvent(int what, T record) {
			if (mOnItemEvent != null) {
                mOnItemEvent(what, record, mPosition, gameObject, mListView);
			}
		}

		protected void sendEventOnClick(int what, T record, Button button) {
			if (button == null) {
				return;
			}
			button.onClick.RemoveAllListeners();
			button.onClick.AddListener(() => {
				sendEvent(what, record);
			});
		}

		public ListView<T> ListView {
			get {
				return mListView;
			} set {
				mListView = value;
			}
		}

		public int Position {
			get {
				return mPosition;
			} set {
				mPosition = value;
			}
		}

        public ListAdapter<T>.OnItemClickDelegate OnItemClick {
            get {
                return mOnItemClick;
            }
            set {
                mOnItemClick = value;
            }
        }

        public ListAdapter<T>.OnItemEventDelegate OnItemEvent {
            get {
                return mOnItemEvent;
            }
            set {
                mOnItemEvent = value;
            }
        }
    }
}

