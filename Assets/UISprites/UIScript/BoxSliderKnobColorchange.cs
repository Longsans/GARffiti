using HSVPicker;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SVBoxSlider))]
public class BoxSliderKnobColorchange : MonoBehaviour
{
    public GameObject thumb;
    private Image _thumbImage;
    private ColorPicker _picker;

    void Awake()
    {
        _thumbImage = thumb.GetComponent<Image>();
        _picker = gameObject.GetComponent<SVBoxSlider>().picker;
    }

    private void OnDisable()
    {
        _picker.onValueChanged.RemoveListener(ColorChanged);
    }

    private void OnEnable()
    {
        _picker.onValueChanged.AddListener(ColorChanged);
    }

    private void ColorChanged(Color newColor)
    {
        _thumbImage.color = newColor;
    }
}
