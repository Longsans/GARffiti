using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using NativeGalleryNamespace;

public class UIController : MonoBehaviour
{
    public GameObject UIContainer;
    public Image DrawModeImage;
    public GameObject StrokeWidthSliderPanel;
    public Text StrokeWidthText;
    public GameObject StrokeColorPicker;
    public Image StrokeColorImage;

    private ARCursor.__DrawMode _drawMode;
    private LineRenderer lineRend;
    private TrailRenderer trailRend;

    private void Start()
    {
        _drawMode = FindObjectOfType<ARCursor>().DrawMode;
        lineRend = FindObjectOfType<ARCursor>().LinePrefab.GetComponent<LineRenderer>();
        trailRend = FindObjectOfType<ARCursor>().StrokePrefab.GetComponent<TrailRenderer>();
        StrokeColorImage.color = lineRend.sharedMaterial.color;
    }

    public void ChangeDrawModeIcon()
    {
        if (_drawMode == ARCursor.__DrawMode.SpaceOnly)
        {
            _drawMode = ARCursor.__DrawMode.PlanesOnly;
            DrawModeImage.sprite = Resources.Load<Sprite>("Icons/black-trap");
        }
        else
        {
            _drawMode = ARCursor.__DrawMode.SpaceOnly;
            DrawModeImage.sprite = Resources.Load<Sprite>("Icons/3d-cube");
        }
        FindObjectOfType<ARCursor>().DrawMode = _drawMode;
    }

    public void SelectTexture()
    {
        FileBrowser.SetFilters(false, new FileBrowser.Filter("Images", ".jpg", ".png"));
        FileBrowser.ShowLoadDialog(paths =>
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(paths[0]);
            Texture2D texture = new Texture2D(256, 256)
            {
                wrapMode = TextureWrapMode.Repeat
            };
            texture.LoadImage(bytes);

            
            lineRend.sharedMaterial = trailRend.sharedMaterial = new Material(Resources.Load<Material>("Materials/Stroke Std. Material"));
            lineRend.sharedMaterial.mainTexture = texture;

        }, null, FileBrowser.PickMode.Files, false, null, null, "Load Image for Texture");
    }

    public void ToggleStrokeWidthSlider()
    {
        StrokeWidthSliderPanel.SetActive(!StrokeWidthSliderPanel.activeInHierarchy);
    }

    public void OnStrokeWidthValueChange(float value)
    {
        lineRend.widthMultiplier = trailRend.widthMultiplier = value;
        StrokeWidthText.text = value.ToString("0.00");
    }

    public void ToggleStrokeColorPicker()
    {
        var colorPicker = StrokeColorPicker.GetComponent<HSVPicker.ColorPicker>();
        colorPicker.CurrentColor = lineRend.sharedMaterial.color;
        StrokeColorPicker.SetActive(!StrokeColorPicker.activeInHierarchy);
    }

    public void OnStrokeColorChange(Color color)
    {
        lineRend.sharedMaterial = trailRend.sharedMaterial = new Material(Resources.Load<Material>("Materials/Stroke Material"));
        lineRend.sharedMaterial.color = color;
        StrokeColorImage.color = color;
    }

    public void TakeSnapshot()
    {
        UIContainer.SetActive(false);
        StartCoroutine(Snap());
    }

    IEnumerator Snap()
    {
        yield return new WaitForEndOfFrame();

        var texture = ScreenCapture.CaptureScreenshotAsTexture();
        NativeGallery.SaveImageToGallery(texture, "GARffiti snapshots", "snapshot", (success, path) => {
            UIContainer.SetActive(true);
        });

        Object.Destroy(texture);
    }
}
