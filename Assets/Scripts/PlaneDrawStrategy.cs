using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts
{
    public class PlaneDrawStrategy : BaseDrawStrategy
    {
        public override bool DrawStart(Vector2 cursorPos)
        {
            UpdateCursorPosition(cursorPos);
            if (!_planeDetected)
                return false;

            FocusOnPlane(_currentPlane);

            GameObject newBrushInstance = GameObject.Instantiate(cursor.StrokePrefab, cursor.transform.position, Quaternion.identity);
            _stroke = new PlaneStroke(newBrushInstance, (ARPlane)_currentPlane);
            _stroke.StartDraw(cursor.transform.position);

            return base.DrawStart(cursorPos);
        }

        public override void DrawEnd()
        {
            UpdateCursorPosition(Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)));
            FocusOnPlane(_currentPlane);

            base.DrawEnd();
        }

        public override bool PlacingStarted(Vector2 cursorPos)
        {
            UpdateCursorPosition(cursorPos);
            if (!_planeDetected)
                return false;

            base.PlacingStarted(cursorPos);
            _modelScript?.UseBottomAchor();

            return true;
        }

        protected override void UpdateCursorPosition(Vector2 touchPosition)
        {
            var hits = RaycastFromScreen(touchPosition);
            if (hits.Count > 0)
            {
                if (!_drawing)
                    _currentPlane = hits[0].trackable;
                
                foreach (var hit in hits)
                {
                    if (hit.trackable == _currentPlane)
                        cursor.transform.position = hit.pose.position;
                }
            }
            else
                _currentPlane = null;
        }

        private List<ARRaycastHit> RaycastFromScreen(Vector2 screenPoint)
        {
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            _raycastManager.Raycast(screenPoint, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            return hits;
        }

        private void FocusOnPlane(ARTrackable currentPlane)
        {
            foreach (var plane in _planeManager.trackables)
            {
                plane.gameObject.SetActive(plane == currentPlane);
            }
        }

        public override void Dispose()
        {
            foreach (var plane in _planeManager.trackables)
            {
                plane.gameObject.SetActive(false);
            }
        }

        public PlaneDrawStrategy(ARCursor arCursor) : base(arCursor)
        {
            _raycastManager = ARCursor.FindObjectOfType<ARRaycastManager>();
            _planeManager = ARCursor.FindObjectOfType<ARPlaneManager>();
            _planeManager.requestedDetectionMode = UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Horizontal | 
                UnityEngine.XR.ARSubsystems.PlaneDetectionMode.Vertical;
        }

        private ARTrackable _currentPlane;
        private ARRaycastManager _raycastManager;
        private ARPlaneManager _planeManager;
        private bool _planeDetected => _currentPlane != null;
        private bool _drawing => _stroke != null;
    }
}
