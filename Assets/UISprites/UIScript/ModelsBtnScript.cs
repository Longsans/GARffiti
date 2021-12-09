using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelsBtnScript : BtnBase, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public GameObject Canvas;
    public GameObject[] ModelPrefabs;

    private RectTransform _rectTransform;
    private RectTransform _canvasTransform;

    private bool _placingModel = false;
    private Vector2 _startLocation;
    private const float _startPlacingMag = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        _canvasTransform = Canvas.GetComponent<RectTransform>();
        _rectTransform = gameObject.GetComponent<RectTransform>();

        Settings.Selected3DModel = ModelPrefabs[0];
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_placingModel)
        {
            Vector2 localPos;
            bool canCalculate = RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out localPos);

            if (canCalculate && !_rectTransform.rect.Contains(localPos))
            {
                _placingModel = true;
                ARCursor.Instance.DrawStrategy.PlacingStarted(eventData.position);
            }
            return;
        }

        ARCursor.Instance.DrawStrategy.PlacingMove(eventData.position);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        _startLocation = eventData.position;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_placingModel)
        {
            ARCursor.Instance.DrawStrategy.PlacingEnded();
        }
        _placingModel = false;
    }
}
