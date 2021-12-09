using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelsBtnScript : BtnBase, IPointerUpHandler, IDragHandler
{
    public GameObject Center;
    public GameObject[] ModelPrefabs;

    public float PreviewRotationSpeed = 30;

    public Vector2 BoundingRect;
    public int UILayer = 5;

    private RectTransform _rectTransform;

    private bool _placingModel = false;
    private UIModelScript _previousModel = null;

    protected override void Awake()
    {
        base.Awake();
        _rectTransform = gameObject.GetComponent<RectTransform>();

        SelectModel(0);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!_placingModel)
        {
            Vector2 localPos;
            bool canCalculate = RectTransformUtility.ScreenPointToLocalPointInRectangle(_rectTransform, eventData.position, eventData.pressEventCamera, out localPos);

            if (canCalculate && !_rectTransform.rect.Contains(localPos))
            {
                _placingModel = ARCursor.Instance.DrawStrategy.PlacingStarted(eventData.position);
            }
            return;
        }

        ARCursor.Instance.DrawStrategy.PlacingMove(eventData.position);
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_placingModel)
        {
            ARCursor.Instance.DrawStrategy.PlacingEnded();
        }
        _placingModel = false;
    }

    public void SelectModel(int index)
    {
        if (_previousModel != null)
            GameObject.Destroy(_previousModel);

        GameObject model = Instantiate(ModelPrefabs[index], Center.gameObject.transform);
        model.transform.localPosition = new Vector3(0, 0, 0);

        UIModelScript modelScript = model.GetComponent<UIModelScript>();
        modelScript.Spinning = true;
        modelScript.RotationSpeed = PreviewRotationSpeed;
        modelScript.FitToSize(BoundingRect.x, BoundingRect.y);
        modelScript.UseMidAnchor();

        _previousModel = modelScript;
        Settings.Selected3DModel = modelScript.ModelPrefab;
    }
}
