using HSVPicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorBtnScript : MonoBehaviour
{
    public Image BackImage;
    public ColorPicker Picker;

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
}
