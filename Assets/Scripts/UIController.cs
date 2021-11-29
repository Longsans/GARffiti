using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleFileBrowser;
using NativeGalleryNamespace;

public class UIController : MonoBehaviour
{
    public GameObject UIContainer;

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

            Settings.Texture = texture;
        }, null, FileBrowser.PickMode.Files, false, null, null, "Load Image for Texture");
    }

    public void OnStrokeWidthValueChange(float value)
    {
        Settings.BrushWidth = value;
    }

    public void OnStrokeColorChange(Color color)
    {
        Settings.BrushColor = color;
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
