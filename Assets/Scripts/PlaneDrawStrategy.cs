using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;

namespace Assets.Scripts
{
    public class PlaneDrawStrategy : BaseDrawStrategy
    {
        public override void Draw()
        {
            if (_planeDetected && Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                // if player touched UI button then ignore
                if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                if (touch.phase == TouchPhase.Ended)
                {
                    if (Input.touchCount == 1) _currentLine = null;
                }
                else
                {
                    UpdateCursorPosition(touch.position);

                    if (_planeDetected)
                    {
                        if (touch.phase == TouchPhase.Began)
                        {
                            FocusOnPlane(_currentPlane);
                            _currentLine = new Line(cursor.LinePrefab, cursor.transform.position, (ARPlane)_currentPlane);
                        }
                        else
                            _currentLine.DrawTo(cursor.transform.position);
                    }
                }
            }
            else if (Input.touchCount < 1)
            {
                UpdateCursorPosition(Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f)));
                FocusOnPlane(_currentPlane);
            }
        }

        private void UpdateCursorPosition(Vector2 touchPosition)
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
        private Line _currentLine;
        private bool _planeDetected => _currentPlane != null;
        private bool _drawing => _currentLine != null;
    }
}
