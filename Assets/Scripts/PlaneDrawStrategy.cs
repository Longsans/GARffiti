using System.Collections.Generic;
using UnityEngine;
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
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        stroke = ARCursor.Instantiate(cursor.StrokePrefab, cursor.transform.position, Quaternion.identity, cursor.transform);
                        break;

                    // TODO: FIX FINGER MOVEMENT BUGS

                    //case TouchPhase.Moved:
                    //    var plane = _currentPlane as ARPlane;
                    //    var startPoint = Camera.main.ScreenToWorldPoint(touch.position - touch.deltaPosition);
                    //    var endPoint = Camera.main.ScreenToWorldPoint(touch.position);
                    //    _stroke.transform.position += Vector3.ProjectOnPlane(endPoint - startPoint, plane.normal);
                    //    break;
                    case TouchPhase.Ended:
                        if (Input.touchCount == 1) stroke.transform.parent = null;
                        break;
                }
            }
            else if (_drawing)
            {
                stroke.transform.parent = null;
            }
        }

        public override void UpdateCursorPosition()
        {
            DetectPlanes();
            cursor.transform.position = _currentCursorPosition;
        }

        private void DetectPlanes()
        {
            var screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            _raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

            _planeDetected = hits.Count > 0;
            if (_planeDetected)
            {
                if (!_drawing) _currentPlane = hits[0].trackable;

                foreach (var plane in _planeManager.trackables)
                {
                    plane.gameObject.SetActive(plane == _currentPlane);
                }

                for (int i = 0; i < hits.Count; i++)
                {
                    if (hits[i].trackable == _currentPlane)
                    {
                        _currentCursorPosition = hits[i].pose.position;
                        break;
                    }
                }
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
        private bool _planeDetected;
        private bool _drawing => stroke != null && stroke.transform.parent == cursor.transform;

        private Vector3 _currentCursorPosition;
    }
}
