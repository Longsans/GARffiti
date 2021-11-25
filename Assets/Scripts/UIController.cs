using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;

public class UIController : MonoBehaviour
{
    public Image DrawModeImage;

    private ARCursor.__DrawMode _drawMode;

    private void Start()
    {
        _drawMode = FindObjectOfType<ARCursor>().DrawMode;
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
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);

            var lineRend = FindObjectOfType<ARCursor>().LinePrefab.GetComponent<LineRenderer>();
            lineRend.sharedMaterial.mainTexture = texture;
        }, null, FileBrowser.PickMode.Files, false, null, null, "Load Image for Texture");
    }

    private IEnumerable ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, null, null, "Load Image for Texture");

        Debug.Log(FileBrowser.Success);

        if (FileBrowser.Success)
        {
            byte[] bytes = FileBrowserHelpers.ReadBytesFromFile(FileBrowser.Result[0]);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(bytes);

            var trailRend = FindObjectOfType<ARCursor>().StrokePrefab.GetComponent<TrailRenderer>();
            Material mat = new Material(trailRend.sharedMaterial)
            {
                mainTexture = texture
            };
            trailRend.material = mat;
        }
    }
}
