using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Assets.Scripts._3DPlacing;

public class EditModelBtnScipt : BtnBase
{
    [SerializeField]
    private Image Fill;

    [SerializeField]
    private Image Icon;

    [SerializeField]
    private TMP_Text Text;

    [SerializeField]
    private DrawingCanvas drawingCanvas;

    [SerializeField]
    private EditModelCanvas editCanvas;

    private static Color darkColor = new Color(38f/255f, 38f/255f, 38f/255f, 1f);

    public override void Clicked()
    {
        bool isChecked = gameObject.GetComponent<Toggle>().isOn;
        drawingCanvas.gameObject.SetActive(!isChecked);
        editCanvas.gameObject.SetActive(isChecked);
        Icon.color = Text.color = isChecked ? Color.white : darkColor;
        Fill.enabled = isChecked;
        base.Clicked();
    }
}
