using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts
{
    public class SpaceDrawStrategy : BaseDrawStrategy
    {
        public override void Draw()
        {
            if (Input.touchCount > 0)
            {
                var touch = Input.GetTouch(0);

                // if player touched UI button then ignore
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                if (touch.phase == TouchPhase.Ended)
                {
                    if (Input.touchCount == 1) stroke.transform.parent = null;
                }
                else
                {
                    cursor.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _distanceFromCamera));
                    if (touch.phase == TouchPhase.Began)
                        stroke = ARCursor.Instantiate(cursor.StrokePrefab, cursor.transform.position, Quaternion.identity, cursor.transform);
                }
            }
            else cursor.transform.position = Camera.main.transform.position + _distanceFromCamera * Camera.main.transform.forward;
        }

        public override void Dispose()
        {
            
        }

        public SpaceDrawStrategy(ARCursor arCursor) : base(arCursor)
        {
            ARCursor.FindObjectOfType<ARPlaneManager>().requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
            _distanceFromCamera = 2.0f;
        }

        private float _distanceFromCamera;
    }
}
