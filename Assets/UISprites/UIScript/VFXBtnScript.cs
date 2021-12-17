using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VFXBtnScript : MonoBehaviour
{
    public Sprite SnowSprite;
    public Sprite RainSprite;
    public Sprite NoneSprite;

    public GameObject PartSystem;
    public Image Icon;
    public Image BorderFill;
    public TMP_Text Text;

    private int _effectIndex = 0;

    private void Start()
    {
        SwitchEffect();
    }

    public void SwitchEffect()
    {
        switch (_effectIndex)
        {
            case 0:
                Icon.sprite = NoneSprite;
                BorderFill.color = Color.black;
                Text.text = "None";
                PartSystem.SetActive(false);
                break;
            case 1:
                Icon.sprite = RainSprite;
                BorderFill.color = new Color(3f/255f, 169f/255f, 252f/255f);
                Text.text = "Rain";
                PartSystem.GetComponentInChildren<RainController>().masterIntensity = 1;
                PartSystem.GetComponentInChildren<SnowController>().masterIntensity = 0;
                PartSystem.SetActive(true);
                break;
            case 2:
                Icon.sprite = SnowSprite;
                BorderFill.color = new Color(2f/255f, 9f/255f, 204f/255f);
                Text.text = "Snow";
                PartSystem.GetComponentInChildren<RainController>().masterIntensity = 0;
                PartSystem.GetComponentInChildren<SnowController>().masterIntensity = 1;
                PartSystem.SetActive(true);
                break;
        }
    }

    public void NextEffect()
    {
        _effectIndex = ((_effectIndex + 1) % 3 + 3) % 3;
        SwitchEffect();
    }

    public void PreviousEffect()
    {
        _effectIndex = ((_effectIndex - 1) % 3 + 3) % 3;
        SwitchEffect();
    }
}
