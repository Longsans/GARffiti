using HSVPicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBtnScript : BtnBase
{
    public const string ShowParam = "Show";
    public const string ChangeStateParam = "ChangeState";

    public Image BackImage;
    public ColorPicker Picker;

    private bool _isOpened = false;
    public bool IsOpened
    {
        get => _isOpened;
        private set
        {
            if (_isOpened != value)
            {
                _isOpened = value;
                Animator.SetBool(ShowParam, _isOpened);
                Animator.SetTrigger(ChangeStateParam);
            }
        }
    }

    private Animator Animator;

    protected override void Awake()
    {
        base.Awake();
        Animator = GetComponent<Animator>();
        BackImage.color = Picker.CurrentColor;
    }

    public void OnColorChanged(Color newColor)
    {
        BackImage.color = newColor;
    }

    private void OnEnable()
    {
        Picker.onValueChanged.AddListener(OnColorChanged);
    }
    private void OnDisable()
    {
        Picker.onValueChanged.RemoveListener(OnColorChanged);
    }

    public override void Clicked()
    {
        base.Clicked();
        IsOpened = !IsOpened;
    }

    protected override void OtherButtonClicked(BtnBase clickedBtn)
    {
        base.OtherButtonClicked(clickedBtn);
        IsOpened = false;
    }
}
