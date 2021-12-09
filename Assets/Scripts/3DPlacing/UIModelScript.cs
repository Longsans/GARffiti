using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModelScript : ModelScript
{
    public float RotationSpeed = 30;
    public GameObject ModelPrefab;

    public bool Spinning = false;

    protected override void Awake()
    {
        base.Awake();
        gameObject.AddComponent<RectTransform>();
        MidAnchor.AddComponent<RectTransform>();
        BottomAnchor.AddComponent<RectTransform>();
    }

    private void Update()
    {
        if (Spinning)
            Rotation += RotationSpeed * Time.deltaTime;
    }

    public void FitToSize(float width, float height)
    {
        float ratioW = Size.x * 100 / width;
        float ratioH = Size.y * 100 / height;

        float ratio = ratioH > ratioW ? ratioH : ratioW;
        SizeMultiplier = 1 / ratio;
    }

    public override void UseMidAnchor()
    {
        base.UseMidAnchor();
        gameObject.transform.localPosition = new Vector3(0, -MidPoint.y, 0);
    }
}
