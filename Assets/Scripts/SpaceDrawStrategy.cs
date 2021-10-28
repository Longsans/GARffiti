using UnityEngine;
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

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        stroke = ARCursor.Instantiate(cursor.StrokePrefab, cursor.transform.position, Quaternion.identity, cursor.transform);
                        break;
                    case TouchPhase.Ended:
                        if (Input.touchCount == 1) stroke.transform.parent = null;
                        break;
                }
            }
        }

        public override void UpdateCursorPosition()
        {
            cursor.transform.position = Camera.main.transform.position + 2.0f * Camera.main.transform.forward;
        }

        public override void Dispose()
        {
            
        }

        public SpaceDrawStrategy(ARCursor arCursor) : base(arCursor)
        {
            ARCursor.FindObjectOfType<ARPlaneManager>().requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
        }
    }
}
