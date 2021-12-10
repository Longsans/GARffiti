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

    [HideInInspector] public ModelScript model;

    protected override void Awake()
    {
        base.Awake();
        size.onValueChanged.AddListener(s => model.OnSizeChanged(s));
        rotationX.onValueChanged.AddListener(x => model.OnRotationXChanged(x));
        rotationY.onValueChanged.AddListener(y => model.OnRotationYChanged(y));
        rotationZ.onValueChanged.AddListener(z => model.OnRotationZChanged(z));
    }

    protected override void OtherButtonClicked(BtnBase clickingBtn)
    {
        gameObject.SetActive(false);
    }
}
