using HSVPicker;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CircularHueSlider : MonoBehaviour, IPointerClickHandler, IDragHandler
{
    public ColorPicker hsvpicker;
    public int radius;
    public GameObject thumb;

    private RectTransform _rectTransform;
    private RectTransform _thumbTransform;
    private Image _thumbImage;

    private const float segmentAngle = 60;

    // Update is called once per frame
    void Update()
    {
        //if (Input.touchCount == 0)
        //    return;
        //
        //Touch touch = Input.touches[0];
        //SetColorAndThumb(touch.position);
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetColorAndThumb(eventData.position, eventData.pressEventCamera);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SetColorAndThumb(eventData.position, eventData.pressEventCamera);
    }

    private void SetColorAndThumb(Vector2 screenPoint, Camera camera)
    {
        Vector2 localPos;
        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, screenPoint, camera, out localPos))
            return;

        Vector2 normVec = localPos.normalized;
        Vector2 xAxis = Vector2.right;
        float cosine = Vector2.Dot(xAxis, normVec) / (xAxis.magnitude * normVec.magnitude);
        float rad = Mathf.Acos(cosine);
        if (normVec.y < 0)
            rad = Mathf.PI * 2 - rad;

        float sliderValue = rad / (Mathf.PI * 2);

        hsvpicker.AssignColor(ColorValues.Hue, sliderValue);
    }

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _thumbTransform = thumb.GetComponent<RectTransform>();
        _thumbImage = thumb.GetComponent<Image>();
        hsvpicker.onHSVChanged.AddListener(ColorChanged);
    }
    private void OnDestroy()
    {
        hsvpicker.onHSVChanged.RemoveListener(ColorChanged);
    }

    private void ColorChanged(float hue, float saturation, float value)
    {
        // Convert hue (0 -> 1) to 360 degree hue to rgb without sat or val
        float hueDeg = hue * 360;
        int segment = (int)(hueDeg / segmentAngle);
        float segmentDeg = hueDeg % segmentAngle;

        Color color = new Color(0, 0, 0, 1);

        // If segment is odd that mean a value is reducing
        float colorValue = ((segment % 2 == 1) ? (segmentAngle - segmentDeg) : segmentDeg) / segmentAngle;

        switch (segment / 2)
        {
            case 0:
                color.r = color.g = 1;
                break;
            case 1:
                color.g = color.b = 1;
                break;
            case 2:
                color.b = color.r = 1;
                break;
        }

        int colorSeg = segment % 3;
        if (colorSeg == 1)
        {
            color.r = colorValue;
        }
        else if (colorSeg == 0)
        {
            color.g = colorValue;
        }
        else
        {
            color.b = colorValue;
        }

        _thumbImage.color = color;

        // Calculate the position of the thumb in the circle
        Vector2 normVec = Vector2.right;
        Vector2 rotatedNormVec = new Vector2();

        float hueRad = Mathf.Deg2Rad * hueDeg;
        float cosine = Mathf.Cos(hueRad);
        float sine = Mathf.Sin(hueRad);

        rotatedNormVec.x = normVec.x * cosine - normVec.y * sine;
        rotatedNormVec.y = normVec.y * cosine + normVec.x * sine;

        Vector2 thumbNewPos = rotatedNormVec * radius;
        _thumbTransform.localPosition = new Vector3(thumbNewPos.x, thumbNewPos.y, _thumbTransform.localPosition.z);

    }
}
