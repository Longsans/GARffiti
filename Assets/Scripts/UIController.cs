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
    private Slider _strwidthSlider;
    private VisualElement _strcolorContainer;
    private SliderInt _strcolorRedSlider;
    private SliderInt _strcolorGreenSlider;
    private SliderInt _strcolorBlueSlider;
    private SliderInt _strcolorAlphaSlider;

    private VisualElement[] _elements;

    private ARCursor _cursor;
    private Material _material;

    private bool _modesVisible;
    private bool _strwidthSliderVisible;
    private bool _strcolorContainerVisible;

    // Start is called before the first frame update
    void Start()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;
        _modeBtn = root.Q<Button>("mode-button");
        _planeModeBtn = root.Q<Button>("planemode-button");
        _spaceModeBtn = root.Q<Button>("spacemode-button");
        _strwidthBtn = root.Q<Button>("strwidth-button");
        _strcolorBtn = root.Q<Button>("strcolor-button");
        _strwidthContainer = root.Q<VisualElement>("strwidth-container");
        _strwidthSlider = _strwidthContainer.Q<Slider>("strwidth-slider");
        _strcolorContainer = root.Q<VisualElement>("strcolor-container");
        _strcolorRedSlider = _strcolorContainer.Q<SliderInt>("red-slider");
        _strcolorGreenSlider = _strcolorContainer.Q<SliderInt>("green-slider");
        _strcolorBlueSlider = _strcolorContainer.Q<SliderInt>("blue-slider");
        _strcolorAlphaSlider = _strcolorContainer.Q<SliderInt>("alpha-slider");

        _cursor = FindObjectOfType<ARCursor>();
        var materialColor = _cursor.StrokePrefab.GetComponent<TrailRenderer>().sharedMaterial.color;
        _strcolorRedSlider.value = (int)materialColor.r;
        _strcolorGreenSlider.value = (int)materialColor.g;
        _strcolorBlueSlider.value = (int)materialColor.b;
        _strcolorAlphaSlider.value = (int)materialColor.a;
        _strcolorBtn.Q<VisualElement>("icon").style.unityBackgroundImageTintColor = new StyleColor(materialColor);

        _modeBtn.clicked += ToggleDrawModesVisible;
        _planeModeBtn.clicked += SwitchToPlaneMode;
        _spaceModeBtn.clicked += SwitchToSpaceMode;
        _strwidthBtn.clicked += ToggleStrokeWidthSliderVisible;
        _strcolorBtn.clicked += ToggleStrokeColorContainerVisible;
        _strwidthSlider.RegisterValueChangedCallback(ChangeStrokeWidth);
        _strcolorRedSlider.RegisterValueChangedCallback(ChangeStrokeColor);
        _strcolorGreenSlider.RegisterValueChangedCallback(ChangeStrokeColor);
        _strcolorBlueSlider.RegisterValueChangedCallback(ChangeStrokeColor);
        _strcolorAlphaSlider.RegisterValueChangedCallback(ChangeStrokeColor);

        _planeModeBtn.visible = false;
        _spaceModeBtn.visible = false;
        _strwidthSlider.value = 0.4f;
        _strwidthContainer.visible = _strwidthSliderVisible = false;
        _strcolorContainer.visible = _strcolorContainerVisible = false;

        _elements = new VisualElement[]
        {
            _modeBtn,
            _planeModeBtn,
            _spaceModeBtn,
            _strwidthBtn,
            _strcolorBtn,
            _strwidthContainer,
            _strcolorContainer,
        };

        var safeArea = Screen.safeArea;

        foreach (var e in _elements)
            e.transform.position = new Vector3(safeArea.x, safeArea.y, e.transform.position.z);

        _modesVisible = false;
    }

    private void ToggleDrawModesVisible()
    {
        _planeModeBtn.visible = _spaceModeBtn.visible = _modesVisible = !_modesVisible;
    }

    private void ToggleStrokeWidthSliderVisible()
    {
        _strwidthContainer.visible = _strwidthSliderVisible = !_strwidthSliderVisible;
    }

    private void ToggleStrokeColorContainerVisible()
    {
        _strcolorContainer.visible = _strcolorContainerVisible = !_strcolorContainerVisible;
    }

    private void SwitchToPlaneMode()
    {
        _cursor.DrawMode = ARCursor.__DrawMode.PlanesOnly;
        _modeBtn.Q<VisualElement>("currentmode-icon").style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>("Icons/black-trap"));

        ToggleDrawModesVisible();
    }

    private void SwitchToSpaceMode()
    {
        _cursor.DrawMode = ARCursor.__DrawMode.SpaceOnly;
        _modeBtn.Q<VisualElement>("currentmode-icon").style.backgroundImage = new StyleBackground(Resources.Load<Texture2D>("Icons/3d-cube"));

        ToggleDrawModesVisible();
    }

    private void ChangeStrokeWidth(ChangeEvent<float> e)
    {
        _cursor.StrokePrefab.GetComponent<TrailRenderer>().widthMultiplier = e.newValue;
        _strwidthBtn.text = $"{e.newValue}x";
    }

    private void ChangeStrokeColor(ChangeEvent<int> e)
    {
        _material = new Material(_cursor.StrokePrefab.GetComponent<TrailRenderer>().sharedMaterial)
        {
            color = new Color(_strcolorRedSlider.value, _strcolorGreenSlider.value, _strcolorBlueSlider.value, _strcolorAlphaSlider.value)
        };
        _cursor.StrokePrefab.GetComponent<TrailRenderer>().sharedMaterial = _material;
        _strcolorBtn.Q<VisualElement>("icon").style.unityBackgroundImageTintColor = new StyleColor(_material.color);
    }
}
