using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace uapp {
	
	public abstract class RayPointerInputModule : BaseInputModule {

		private int mPointerCount;

		#region subclass override
		/// <summary>
		/// 是否为调试状态，调试状态可显示射线
		/// </summary>
		/// <returns><c>true</c>, if debug was ised, <c>false</c> otherwise.</returns>
		protected virtual bool isDebug() {
			return true;
		}

		/// <summary>
		/// 返回可用的Pointer数量
		/// </summary>
		/// <returns>可用的Pointer数量.</returns>
		protected abstract int getNumPointers();

		/// <summary>
		/// 获取代表Pointer的射线
		/// </summary>
		/// <returns><c>true</c>, 表示获取成功，射线会用于控件求交, <c>false</c> 否则.</returns>
		/// <param name="pointerId">Pointer ID.</param>
		/// <param name="ray">射线.</param>
		protected abstract bool getPointerRay(int pointerId, out Ray ray);

		/// <summary>
		/// 显示射线，调试用
		/// </summary>
		/// <param name="pointerId">Pointer ID.</param>
		/// <param name="ray">射线.</param>
		protected abstract void showPointerRay(int pointerId, Ray ray);

		/// <summary>
		/// 如何判断Pointer为Down状态
		/// </summary>
		/// <returns><c>true</c>, 表示Pointer为Down状态, <c>false</c> 否则.</returns>
		/// <param name="pointerId">Pointer ID.</param>
		protected abstract bool isPointerDown(int pointerId);

		/// <summary>
		/// 如何判断Pointer为Move状态
		/// </summary>
		/// <returns><c>true</c>, 表示Pointer为Move状态, <c>false</c> 否则.</returns>
		/// <param name="pointerId">Pointer ID.</param>
		protected abstract bool isPointerMove(int pointerId);

		/// <summary>
		/// 如何判断Pointer为Up状态
		/// </summary>
		/// <returns><c>true</c>, 表示Pointer为Up状态, <c>false</c> 否则.</returns>
		/// <param name="pointerId">Pointer ID.</param>
		protected abstract bool isPointerUp(int pointerId);
		#endregion

		#region RayPointerEventData pool
		protected Dictionary<int, RayPointerEventData> mRayPointerEventDatas = new Dictionary<int, RayPointerEventData>();
		protected bool getRayPointerEventData(int pointerId, out RayPointerEventData data, bool createIfNotExists) {
			if (!mRayPointerEventDatas.TryGetValue(pointerId, out data) && createIfNotExists) {
				data = new RayPointerEventData(eventSystem) {
					pointerId = pointerId,
					pointerPress = null,
				};
				mRayPointerEventDatas.Add(pointerId, data);
				return true;
			}
			return false;
		}
		#endregion

		#region InputModule Implementation
		private RaycastResult findTopmostRaycast(List<RaycastResult> results) {
			if (results == null) {
				return Constants.RaycastResultInvalid;
			}
			var maxDepthResult = Constants.RaycastResultInvalid;
			foreach (var result in results) {
				if (result.depth > maxDepthResult.depth) {
					maxDepthResult = result;
				}
			}
			return maxDepthResult;
		}

		private void handlePointerExitAndEnter(RayPointerEventData currentPointerData, GameObject newEnterTarget) {
			if (currentPointerData.pointerEnter == newEnterTarget) {
				return;
			}
			// NOTE 跟HandlePointerExitAndEnter处理方式大致相同
			ExecuteEvents.ExecuteHierarchy(currentPointerData.pointerEnter, currentPointerData, ExecuteEvents.pointerExitHandler);
			currentPointerData.pointerEnter = newEnterTarget;
			ExecuteEvents.ExecuteHierarchy(currentPointerData.pointerEnter, currentPointerData, ExecuteEvents.pointerEnterHandler);
			if (newEnterTarget == null) { // NOTE 清除选中状态
				eventSystem.SetSelectedGameObject(null, GetBaseEventData());
			}
		}

		protected void clearSelection() {
			foreach (var pointerEventData in mRayPointerEventDatas.Values) {
				HandlePointerExitAndEnter(pointerEventData, null);
			}
			mRayPointerEventDatas.Clear();
			eventSystem.SetSelectedGameObject(null, GetBaseEventData());
		}

		public override void DeactivateModule() {
			base.DeactivateModule();
			clearSelection();
		}

		public override void Process() {
			var numPointers = getNumPointers();
			for (var pointerId = 0; pointerId < numPointers; pointerId++) {
				// 获取PointerEventData
				RayPointerEventData rayPointerEventData;
				getRayPointerEventData(pointerId, out rayPointerEventData, true);
				// 获取射线
				Ray ray;
				if (getPointerRay(pointerId, out ray)) {
					if (isDebug()) { // 调试状态显示射线
						showPointerRay(pointerId, ray);
					}
					rayPointerEventData.Ray = ray;
					m_RaycastResultCache.Clear();
					// 调用Raycaster跟控件求交，以便下面根据Pointer状态进行事件发送
					eventSystem.RaycastAll(rayPointerEventData, m_RaycastResultCache);

					// NOTE Button必须收到Enter和Down事件后，Click事件才会出现显示效果的变化
					if (m_RaycastResultCache.Count == 0) { // 没有求交到控件时离开当前控件
						handlePointerExitAndEnter(rayPointerEventData, null);
					}
					// 让最上面的控件以及其父控件处理enter exit事件
					var pointerEnterResult = findTopmostRaycast(m_RaycastResultCache);
					var pointerEnter = pointerEnterResult.gameObject;
					handlePointerExitAndEnter(rayPointerEventData, pointerEnter);
                    rayPointerEventData.pointerCurrentRaycast = pointerEnterResult;

					// Pointer Down处理
					if (isPointerDown(pointerId)) {
						foreach (var result in m_RaycastResultCache) {
							rayPointerEventData.position = result.screenPosition;
							rayPointerEventData.pressPosition = rayPointerEventData.position;
							rayPointerEventData.eligibleForClick = true;
							rayPointerEventData.delta = Vector2.zero;
							rayPointerEventData.dragging = false;
							rayPointerEventData.useDragThreshold = false;
							var b = ExecuteEvents.Execute(result.gameObject, rayPointerEventData, ExecuteEvents.pointerDownHandler);
							if (b && rayPointerEventData.pointerPress == null) { // 找到第一个处理Down事件的控件
								rayPointerEventData.pointerPress = result.gameObject;
							}
							b = ExecuteEvents.Execute(result.gameObject, rayPointerEventData, ExecuteEvents.initializePotentialDrag);
							if (b && rayPointerEventData.pointerDrag == null) { // 找到第一个处理Drag事件的控件，并初始化
								rayPointerEventData.pointerDrag = result.gameObject;
							}
						}
					}
					// Pointer Move处理
					if (isPointerMove(pointerId) && rayPointerEventData.pointerDrag != null) {
						// 获取当前canvas坐标点
						Vector2 currentScreenPosition = Vector2.zero;
						var foundCurrentScreenPosition = false;
						foreach (var result in m_RaycastResultCache) {
							if (result.depth < 0) {
								currentScreenPosition = result.screenPosition;
								foundCurrentScreenPosition = true;
								break;
							}
						}

                        rayPointerEventData.position = currentScreenPosition;
                        if (!foundCurrentScreenPosition) { // 不在canvas内，结束拖动
							var pointerDrag = rayPointerEventData.pointerDrag;
							ExecuteEvents.Execute(pointerDrag, rayPointerEventData, ExecuteEvents.endDragHandler);
							rayPointerEventData.pointerDrag = null;
						} else { // 在canvas内，开始拖动
							var pointerDrag = rayPointerEventData.pointerDrag;
							if (!rayPointerEventData.dragging) {
								ExecuteEvents.Execute(pointerDrag, rayPointerEventData, ExecuteEvents.beginDragHandler);
								rayPointerEventData.dragging = true;
							}
							if (rayPointerEventData.dragging) {
								var d = currentScreenPosition - rayPointerEventData.position;
								d.y = -d.y; // 反向以显示正确
								rayPointerEventData.scrollDelta = d;
								ExecuteEvents.Execute(pointerDrag, rayPointerEventData, ExecuteEvents.dragHandler);
//							ExecuteEvents.Execute(pointerDrag, rayPointerEventData, ExecuteEvents.scrollHandler);
							}
						}
					}
					// Pointer Up处理
					if (isPointerUp(pointerId)) {
						if (rayPointerEventData.pointerPress != null && rayPointerEventData.eligibleForClick) {
							var pointerPress = rayPointerEventData.pointerPress;
                            var sendUpAndClick = false;
                            foreach (var result in m_RaycastResultCache) {
                                if (result.gameObject == pointerPress) {
                                    sendUpAndClick = true;
                                    break;
                                }
                            }
                            if (sendUpAndClick) {
                                ExecuteEvents.Execute (pointerPress, rayPointerEventData, ExecuteEvents.pointerUpHandler);
                                ExecuteEvents.Execute (pointerPress, rayPointerEventData, ExecuteEvents.pointerClickHandler);
                            }
							rayPointerEventData.eligibleForClick = false;
							rayPointerEventData.pointerPress = null;

							if (rayPointerEventData.pointerDrag != null && rayPointerEventData.dragging) {
								var pointerDrag = rayPointerEventData.pointerDrag;
								ExecuteEvents.Execute(pointerDrag, rayPointerEventData, ExecuteEvents.endDragHandler);
								rayPointerEventData.pointerDrag = null;
							}
						}
					}
				}
			}
		}
		#endregion

	}
}
