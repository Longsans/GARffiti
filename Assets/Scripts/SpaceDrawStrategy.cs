using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts
{
    public class SpaceDrawStrategy : BaseDrawStrategy
    {
        public override void Draw()
        {
            if (Input.touchCount == 0)
                return;

            var touch = Input.GetTouch(0);

            // if player touched UI button then ignore
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (touch.phase == TouchPhase.Ended)
            {
                if (Input.touchCount == 1)
                {
                    _stroke.Finished();
                    _stroke = null;
                }
            }
            else
            {
                cursor.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, _distanceFromCamera));
                if (touch.phase == TouchPhase.Began)
                {
                    GameObject newBrushInstance = GameObject.Instantiate(cursor.StrokePrefab, cursor.transform.position, Quaternion.identity);
                    _stroke = new SpaceStroke(newBrushInstance);
                    _stroke.DrawTo(cursor.transform.position);

                    // Send the newly created stroke down the event
                    DrawPhaseStarted.Invoke(_stroke);
                }
                else
                {
                    _stroke.DrawTo(cursor.transform.position);
                }
            }
        }

        public override void Dispose()
        {
            
        }

        public SpaceDrawStrategy(ARCursor arCursor) : base(arCursor)
        {
            ARCursor.FindObjectOfType<ARPlaneManager>().requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
            _distanceFromCamera = 4.0f;
        }

        private float _distanceFromCamera;
    }
}
