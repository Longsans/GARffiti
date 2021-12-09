using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts
{
    public class SpaceDrawStrategy : BaseDrawStrategy
    {
        public override bool DrawStart(Vector2 cursorPos)
        {
            UpdateCursorPosition(cursorPos);

            GameObject newBrushInstance = GameObject.Instantiate(cursor.StrokePrefab, cursor.transform.position, Quaternion.identity);
            _stroke = new SpaceStroke(newBrushInstance);
            _stroke.StartDraw(cursor.transform.position);

            return base.DrawStart(cursorPos);
        }

        public override bool PlacingStarted(Vector2 cursorPos)
        {
            UpdateCursorPosition(cursorPos);

            base.PlacingStarted(cursorPos);
            _modelScript?.UseMidAnchor();

            return true;
        }

        protected override void UpdateCursorPosition(Vector2 position)
        {
            cursor.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(position.x, position.y, _distanceFromCamera));
        }

        public override void Dispose()
        {
            
        }

        public SpaceDrawStrategy(ARCursor arCursor) : base(arCursor)
        {
            ARCursor.FindObjectOfType<ARPlaneManager>().requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.None;
            _distanceFromCamera = 8.0f;
        }

        private float _distanceFromCamera;
    }
}
