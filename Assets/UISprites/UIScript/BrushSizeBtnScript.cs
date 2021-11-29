using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrushSizeBtnScript : BtnBase
{
    public const string ShowParam = "Show";
    public const string ChangeStateParam = "ChangeState";

    private Animator _animator;
    public Slider slider;

    private bool _isOpened = false;
    public bool IsOpened
    {
        get => _isOpened;
        private set
        {
            if (_isOpened != value)
            {
                _isOpened = value;
                _animator.SetBool(ShowParam, _isOpened);
                _animator.SetTrigger(ChangeStateParam);
                if (_isOpened)
                {
                    slider.value = Settings.BrushWidth;
                }
            }
        }
    }

    protected override void Awake()
    {
        base.Awake();
        slider.onValueChanged.AddListener(SliderValueChanged);
        _animator = GetComponent<Animator>();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        slider.onValueChanged.RemoveListener(SliderValueChanged);
    }

    private void SliderValueChanged(float value)
    {
        Settings.BrushWidth = value;
    }

    public override void Clicked()
    {
        base.Clicked();
        IsOpened = !IsOpened;
    }

    protected override void OtherButtonClicked(BtnBase clickingBtn)
    {
        base.OtherButtonClicked(clickingBtn);
        IsOpened = false;
    }
}
