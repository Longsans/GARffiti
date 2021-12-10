using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditModelPanelUI : BtnBase
{
    [SerializeField] Slider size;
    [SerializeField] Slider rotationX;
    [SerializeField] Slider rotationY;
    [SerializeField] Slider rotationZ;

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

        size.onValueChanged.AddListener(s => _model.OnSizeChanged(s));
        rotationX.onValueChanged.AddListener(x => _model.OnRotationXChanged(x));
        rotationY.onValueChanged.AddListener(y => _model.OnRotationYChanged(y));
        rotationZ.onValueChanged.AddListener(z => _model.OnRotationZChanged(z));
    }

    protected override void OtherButtonClicked(BtnBase clickingBtn)
    {
        gameObject.SetActive(false);
    }
}
