using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public class SelectableEx : Selectable {

		private static Dictionary<Selectable, UnityEngine.UI.ColorBlock> mSelectableNormalColors;
		private static Dictionary<Selectable, Sprite> mSelectableNormalSprites;
	
		public enum State {
			Normal = SelectionState.Normal,
			Highlighted = SelectionState.Highlighted,
			Pressed = SelectionState.Pressed,
			Disabled = SelectionState.Disabled,
		}

		public static void CacheSelectableNormalSprites(GameObject gameObject) {
			var selectables = gameObject.GetComponentsInChildren<Selectable>();
			foreach (var selectable in selectables) {
				if (selectable.transition == Selectable.Transition.SpriteSwap) {
					if (mSelectableNormalSprites == null) {
						mSelectableNormalSprites = new Dictionary<Selectable, Sprite>();
					}
					if (!mSelectableNormalSprites.ContainsKey(selectable)) {
						mSelectableNormalSprites[selectable] = selectable.image.sprite;
					}
				}
			}
		}

		public static void SetSelectableState(Selectable selectable, SelectableEx.State state, bool includeSelf = false) {
			if (selectable == null) {
				return;
			}
			var selectables = selectable.gameObject.GetComponentsInChildren<Selectable>();
			foreach (var sel in selectables) {
				if (!includeSelf && sel == selectable) {
					continue;
				}
				switch (sel.transition) {
					case Selectable.Transition.ColorTint:
						switch (state) {
							case SelectableEx.State.Normal:
								sel.targetGraphic.color = sel.colors.normalColor;
								break;
							case SelectableEx.State.Highlighted:
								sel.targetGraphic.color = sel.colors.highlightedColor;
								break;
							case SelectableEx.State.Pressed:
								sel.targetGraphic.color = sel.colors.pressedColor;
								break;
							case SelectableEx.State.Disabled:
								sel.targetGraphic.color = sel.colors.disabledColor;
								break;
						}
						break;
					case Selectable.Transition.SpriteSwap:
						switch (state) {
							case SelectableEx.State.Normal:
								if (mSelectableNormalSprites == null) {
									mSelectableNormalSprites = new Dictionary<Selectable, Sprite>();
								}
								if (!mSelectableNormalSprites.ContainsKey(sel)) {
									mSelectableNormalSprites[sel] = sel.image.sprite;
								}
								sel.image.overrideSprite = mSelectableNormalSprites[sel];
								break;
							case SelectableEx.State.Highlighted:
								sel.image.overrideSprite = sel.spriteState.highlightedSprite;
								break;
							case SelectableEx.State.Pressed:
								sel.image.overrideSprite = sel.spriteState.pressedSprite;
								break;
							case SelectableEx.State.Disabled:
								sel.image.overrideSprite = sel.spriteState.disabledSprite;
								break;
						}
						break;
				}
			}

			var textStates = selectable.gameObject.GetComponentsInChildren<TextState>();
			foreach (var textState in textStates) {
				switch (state) {
					case SelectableEx.State.Normal:
						textState.Target.text = textState.Normal;
						break;
					case SelectableEx.State.Highlighted:
						textState.Target.text = textState.Highlighted != null ? textState.Highlighted : textState.Normal;
						break;
					case SelectableEx.State.Pressed:
						textState.Target.text = textState.Pressed != null ? textState.Pressed : textState.Normal;
						break;
					case SelectableEx.State.Disabled:
						textState.Target.text = textState.Disabled != null ? textState.Disabled : textState.Normal;
						break;
				}
			}

			var graphicStates = selectable.gameObject.GetComponentsInChildren<GraphicState>();
			foreach (var graphicState in graphicStates) {
				switch (state) {
					case SelectableEx.State.Normal:
//						activeState.gameObject.SetActive(activeState.Normal);
						graphicState.Target.enabled = graphicState.Normal;
						break;
					case SelectableEx.State.Highlighted:
//						activeState.gameObject.SetActive(activeState.Highlighted);
						graphicState.Target.enabled = graphicState.Highlighted;
						break;
					case SelectableEx.State.Pressed:
//						activeState.gameObject.SetActive(activeState.Pressed);
						graphicState.Target.enabled = graphicState.Pressed;
						break;
					case SelectableEx.State.Disabled:
//						activeState.gameObject.SetActive(activeState.Disabled);
						graphicState.Target.enabled = graphicState.Disabled;
						break;
				}
			}
		}

	}
}
