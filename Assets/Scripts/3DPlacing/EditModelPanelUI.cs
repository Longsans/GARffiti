using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditModelPanelUI : BtnBase
{
    [SerializeField] Slider size;
    [SerializeField] Slider rotationX;
    [SerializeField] Slider rotationY;
    [SerializeField] Slider rotationZ;

    private bool sizeChanging;
    private bool xChanging;
    private bool yChanging;
    private bool zChanging;

    private ModelScript _model = null;
    public ModelScript Model
    {
        get => _model;
        set
        {
            _model = value;
            if (_model == null)
                return;

            size.value = _model.SizeMultiplier;
            Vector3 rotation = _model.gameObject.transform.eulerAngles;
            rotationX.value = rotation.x;
            rotationY.value = rotation.y;
            rotationZ.value = rotation.z;
        }
    }

    protected override void Awake()
    {
        base.Awake();

        size.onValueChanged.AddListener(s =>
        {
            _model.OnSizeChanged(s);
        });
        rotationX.onValueChanged.AddListener(x =>
        {
            _model.OnRotationXChanged(x);
        });
        rotationY.onValueChanged.AddListener(y =>
        {
            _model.OnRotationYChanged(y);
        });
        rotationZ.onValueChanged.AddListener(z =>
        {
            _model.OnRotationZChanged(z);
        });
    }

    public void SizeChanging(BaseEventData data)
    {
        sizeChanging = true;
    }

    public void XChanging(BaseEventData data)
    {
        xChanging = true;
    }

    public void YChanging(BaseEventData data)
    {
        yChanging = true;
    }

    public void ZChanging(BaseEventData data)
    {
        zChanging = true;
    }

    public void SizeChanged(BaseEventData data)
    {
        if (sizeChanging)
        {
            History.AddAction(new EditModelAction(_model, _model.LastSizeMultiplier, _model.SizeMultiplier,
            _model.LastInstRotation, _model.InstanceRotation));
            _model.MarkLastSizeMultiplier();
        }
        sizeChanging = false;
    }

    public void XChanged(BaseEventData data)
    {
        if (xChanging)
        {
            History.AddAction(new EditModelAction(_model, _model.LastSizeMultiplier, _model.SizeMultiplier, 
                _model.LastInstRotation, _model.InstanceRotation));
            _model.MarkLastRotation();
        }
        xChanging = false;
    }

    public void YChanged(BaseEventData data)
    {
        if (yChanging)
        {
            History.AddAction(new EditModelAction(_model, _model.LastSizeMultiplier, _model.SizeMultiplier,
                _model.LastInstRotation, _model.InstanceRotation));
            _model.MarkLastRotation();
        }
        yChanging = false;
    }

    public void ZChanged(BaseEventData data)
    {
        if (zChanging)
        {
            History.AddAction(new EditModelAction(_model, _model.LastSizeMultiplier, _model.SizeMultiplier, 
                _model.LastInstRotation, _model.InstanceRotation));
            _model.MarkLastRotation();
        }
        zChanging = false;
    }

    public void UpdateState(float scaleFactor, float x, float y, float z)
    {
        size.value = scaleFactor;
        rotationX.value = x;
        rotationY.value = y;
        rotationZ.value = z;
        _model.MarkLastSizeMultiplier();
        _model.MarkLastRotation();
    }

    protected override void OtherButtonClicked(BtnBase clickingBtn)
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        if (EditModelCanvas.LastPanel != null && EditModelCanvas.LastPanel != this)
            EditModelCanvas.LastPanel.gameObject.SetActive(false);

        EditModelCanvas.LastPanel = this;
    }

    private void OnDisable()
    {
        if (EditModelCanvas.LastPanel == this)
            EditModelCanvas.LastPanel = null;
    }
}
