using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public class ToggleEx : Toggle {

		protected override void Awake() {
			base.Awake();
			SelectableEx.CacheSelectableNormalSprites(gameObject);
		}

		protected override void DoStateTransition(SelectionState state, bool instant) {
			if (isOn && state == SelectionState.Normal) { // NOTE 先一刀切，屏蔽所有on状态时候转换为normal的情况，然后通过外部监听onValueChanged来设置状态
				return;
			}
//			base.DoStateTransition(state, instant);
//			SelectableEx.SetSelectableState(this, (SelectableEx.State)state);
			SelectableEx.SetSelectableState(this, (SelectableEx.State)state, true);
		}

		public static void SetToggleOnOff(Toggle toggle, bool onOff, bool includeSelf = true) {
			if (onOff) {
				SelectableEx.SetSelectableState(toggle, SelectableEx.State.Highlighted, includeSelf);
			} else {
				SelectableEx.SetSelectableState(toggle, SelectableEx.State.Normal, includeSelf);
			}
		}
	}

}
