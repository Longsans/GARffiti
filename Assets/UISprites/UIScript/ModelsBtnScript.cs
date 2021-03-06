using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ModelsBtnScript : BtnBase, IPointerUpHandler, IDragHandler, IPointerDownHandler
{
    public GameObject Center;
    public GameObject[] ModelPrefabs;

    public float PreviewRotationSpeed = 30;

    public Vector2 BoundingRect;
    public int UILayer = 5;

    private RectTransform _rectTransform;

    private bool _placingModel = false;
    private UIModelScript _previousModel = null;

    private int _currentIndex = 0;

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
                Debug.Log(eventData.position);
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
        _previousModel?.Destroy();

        GameObject model = Instantiate(ModelPrefabs[index], Center.gameObject.transform);

        UIModelScript modelScript = model.GetComponent<UIModelScript>();
        modelScript.Spinning = true;
        modelScript.RotationSpeed = PreviewRotationSpeed;
        modelScript.FitToSize(BoundingRect.x, BoundingRect.y);
        modelScript.UseMidAnchor();
        modelScript.MoveToLocal(new Vector3(0, 0, -Mathf.Max(modelScript.Size.x, modelScript.Size.z) * modelScript.SizeMultiplier));

        _previousModel = modelScript;
        Settings.Selected3DModel = modelScript.ModelPrefab;
    }

    public void SelectNextModel()
    {
        _currentIndex++;
        _currentIndex %= ModelPrefabs.Length;

        SelectModel(_currentIndex);
    }

    public void SelectPreviousModel()
    {
        _currentIndex--;
        if (_currentIndex < 0)
            _currentIndex = ModelPrefabs.Length - 1;

        SelectModel(_currentIndex);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // this function is needed even if it's empty
    }
}
