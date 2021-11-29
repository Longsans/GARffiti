using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageBtnScript : BtnBase
{
    public Image buttonImage;
    public GameObject clearImageButton;

    private Color _originalColor;

    protected override void Awake()
    {
        base.Awake();
        Settings.onTextureChanged.AddListener(TextureChanged);
        SetClearButtonActivated();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        Settings.onTextureChanged.RemoveListener(TextureChanged);
    }

    private void TextureChanged(Texture2D texture)
    {
        if (texture != null)
        {
            _originalColor = buttonImage.color;
            buttonImage.color = new Color(1, 1, 1);
            buttonImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            SetClearButtonActivated();
            return;
        }

        buttonImage.color = _originalColor;
        buttonImage.sprite = Resources.Load<Sprite>("Icons/tiles");
        SetClearButtonActivated();
    }

    public void ClearImage()
    {
        Settings.Texture = null;
    }

    private void SetClearButtonActivated()
    {
        clearImageButton.SetActive(Settings.Texture != null);
    }
}
