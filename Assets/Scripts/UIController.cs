using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private Button _modeBtn;
    private Button _planeModeBtn;
    private Button _spaceModeBtn;
    private Button _strwidthBtn;
    private Button _strcolorBtn;
    private VisualElement _strwidthContainer;
    private SliderInt _strwidthSlider;

    private bool _modesVisible;
    private bool _strwidthSliderVisible;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _modeBtn = root.Q<Button>("mode-button");
        _planeModeBtn = root.Q<Button>("planemode-button");
        _spaceModeBtn = root.Q<Button>("spacemode-button");
        _strwidthBtn = root.Q<Button>("strwidth-button");
        _strcolorBtn = root.Q<Button>("strcolor-button");
        _strwidthContainer = root.Q<VisualElement>("strwidth-slidercontainer");
        _strwidthSlider = _strwidthContainer.Q<SliderInt>("strwidth-slider");

        _modeBtn.clicked += ToggleDrawModesVisible;
        _planeModeBtn.clicked += SwitchToPlaneMode;
        _spaceModeBtn.clicked += SwitchToSpaceMode;
        _strwidthBtn.clicked += ToggleStrokeWidthSliderVisible;
        _strwidthSlider.RegisterValueChangedCallback(ChangeStrokeWidth);

        _planeModeBtn.visible = false;
        _spaceModeBtn.visible = false;
        _strwidthSlider.value = 12;
        _strwidthContainer.visible = false;

        _modesVisible = false;
    }

    private void Update()
    {
        
    }

    private void ToggleDrawModesVisible()
    {
        if (_modesVisible = !_modesVisible)
        {
            _planeModeBtn.visible = true;
            _spaceModeBtn.visible = true;
        }
        else
        {
            _planeModeBtn.visible = false;
            _spaceModeBtn.visible = false;
        }
    }

    private void ToggleStrokeWidthSliderVisible()
    {
        if (_strwidthSliderVisible = !_strwidthSliderVisible)
            _strwidthContainer.visible = true;
        else 
            _strwidthContainer.visible = false;
    }

    private void SwitchToPlaneMode()
    {
        var cursor = FindObjectOfType<ARCursor>();
        cursor.DrawMode = ARCursor.__DrawMode.PlanesOnly;
        // remove current visual element and add new one
        //

        ToggleDrawModesVisible();
    }

    private void SwitchToSpaceMode()
    {
        var cursor = FindObjectOfType<ARCursor>();
        cursor.DrawMode = ARCursor.__DrawMode.SpaceOnly;
        // remove current visual element and add new one
        //
        
        ToggleDrawModesVisible();
    }

    private void ChangeStrokeWidth(ChangeEvent<int> e)
    {
        //change stroke width
        //

        _strwidthBtn.text = $"{e.newValue}px";
    }
}
