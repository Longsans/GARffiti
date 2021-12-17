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
    private string intName = "Showing";

    private int _shown = 0;

    protected override void Awake()
    {
        Image.sprite = HidedSprite;
        base.Awake();
    }

    public override void Clicked()
    {
        _shown = (_shown + 1) % 3;
        Animator.SetInteger(intName, _shown);
        Animator.SetTrigger(triggerName);
        if (_shown == 2)
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
