using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ARCursor : MonoBehaviour
{
    public GameObject StrokePrefab;

    private GameObject _stroke;
    private ARTrackable _currentPlane;
    private List<ARTrackable> _drawnPlanes;
    private ARRaycastManager _raycastManager;
    private ARPlaneManager _planeManager;
    private bool _planeDetected;

    void Start()
    {
        _raycastManager = FindObjectOfType<ARRaycastManager>();
        _planeManager = FindObjectOfType<ARPlaneManager>();
        _drawnPlanes = new List<ARTrackable>();
        _currentPlane = null;
    }

    void Update()
    {
        UpdateCursorPose();
        Draw();
    }

    private void UpdateCursorPose()
    {
        var screenPosition = Camera.main.ViewportToScreenPoint(new Vector2(0.5f, 0.5f));
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        _raycastManager.Raycast(screenPosition, hits, UnityEngine.XR.ARSubsystems.TrackableType.Planes);

        _planeDetected = hits.Count > 0;
        if (_planeDetected)
        {
            _currentPlane = hits[0].trackable;
            foreach (var plane in _planeManager.trackables)
            {
                if (plane != _currentPlane && !_drawnPlanes.Contains(plane)) plane.gameObject.SetActive(false);
                else plane.gameObject.SetActive(true);
            }

            transform.position = hits[0].pose.position;
            transform.rotation = hits[0].pose.rotation;
        }
    }

    private void Draw()
    {
        if (_planeDetected && Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _stroke = Instantiate(StrokePrefab, transform.position, Quaternion.identity, transform);
                    _drawnPlanes.Add(_currentPlane);
                    break;

                // TODO: FIX FINGER MOVEMENT BUGS

                //case TouchPhase.Moved:
                //    var plane = _currentPlane as ARPlane;
                //    var startPoint = Camera.main.ScreenToWorldPoint(touch.position - touch.deltaPosition);
                //    var endPoint = Camera.main.ScreenToWorldPoint(touch.position);
                //    _stroke.transform.position += Vector3.ProjectOnPlane(endPoint - startPoint, plane.normal);
                //    break;
                case TouchPhase.Ended:
                case TouchPhase.Canceled:
                    if (Input.touchCount == 1) _stroke.transform.parent = null;
                    break;
            }
        }
        else
            _stroke = null;
    }
}
