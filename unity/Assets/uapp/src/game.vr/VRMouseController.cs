using UnityEngine;
using System;
using System.Collections;

namespace uapp {
    public abstract class VRMouseController : MonoBehaviour {

        public float MouseSpeed = 0.05f;
        public GameObject Eye;
        public float EyeDistance = -1.0f;
        public bool AutoStickObjectWhenTouch = true; // 当鼠标触摸到物件时，自动贴合到物件表面

        private GameObject mHitObject = null;

        void Start() {
            if (Eye == null) {
                Eye = gameObject.transform.parent.gameObject;
            }
            if (Eye == null) {
                throw new Exception("Eye Object is null, please add one.");
            }
            if (EyeDistance < 0.0f) {
                EyeDistance = gameObject.transform.localPosition.z;
            }
        }

        protected abstract void OnObjectTouch(GameObject obj); // 当物件被触摸到时触发，当没有触摸到物件时，obj==null

        protected virtual Vector2 GetMouseDeltaPosition() {
            return new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        }

        void Update() {
            // 鼠标移动
            Vector2 deltaMousePosition = GetMouseDeltaPosition() * MouseSpeed;
            gameObject.transform.Translate(deltaMousePosition.x, deltaMousePosition.y, 0.0f, Space.Self);

            // 鼠标触摸物件
            Vector3 origin = Eye.transform.position;
            Vector3 target = gameObject.transform.position;
            Vector3 direction = Vector3.Normalize(target - origin);
            RaycastHit hitInfo;
            if (Physics.Raycast(origin, direction, out hitInfo)) {
                GameObject hitObject = hitInfo.collider.gameObject;
                if (mHitObject != hitObject) {
                    OnObjectTouch(hitObject);
                }
                if (AutoStickObjectWhenTouch) {
                    Vector3 hitPoint = origin + direction * hitInfo.distance;
                    gameObject.transform.position = hitPoint;
                }
                mHitObject = hitObject;
            } else {
                if (mHitObject != null) {
                    OnObjectTouch(null);
                }
                gameObject.transform.Translate(gameObject.transform.position.x, gameObject.transform.position.y, EyeDistance, Space.Self);
                mHitObject = null;
            }
        }
    }
}
