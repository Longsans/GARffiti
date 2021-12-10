using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowModelPanelBtn : BtnBase
{
    public Sprite ShowedSprite;
    public Sprite HidedSprite;
    public Image Image;
    public Animator Animator;

    private string triggerName = "BoolChanged";
    private string boolName = "Showing";

    private bool _showed = false;

    protected override void Awake()
    {
        Image.sprite = HidedSprite;
        base.Awake();
    }

    public override void Clicked()
    {
        _showed = !_showed;
        Animator.SetBool(boolName, _showed);
        Animator.SetTrigger(triggerName);
        if (_showed)
        {
            Image.sprite = ShowedSprite;
        }
        else
        {
            Image.sprite = HidedSprite;
        }

        base.Clicked();
    }
}
