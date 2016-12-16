using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace uapp {

	public class ButtonEx : Button {

		protected override void Awake() {
			base.Awake();
			SelectableEx.CacheSelectableNormalSprites(gameObject);
		}

		protected override void DoStateTransition(SelectionState state, bool instant) {
//			base.DoStateTransition(state, instant);
//			SelectableEx.SetSelectableState(this, (SelectableEx.State)state);
			SelectableEx.SetSelectableState(this, (SelectableEx.State)state, true);
		}

	}

}
